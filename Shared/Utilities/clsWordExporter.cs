using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Shared.Utilities
{
    public static class clsWordExporter
    {
        private static readonly Regex _placeholder = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);

        public static Task GenerateEnvelopesAsync(string templatePath, string outputPath, DataTable rows)
        {
            return Task.Run(() => GenerateEnvelopes(templatePath, outputPath, rows));
        }

        // One copy of the template page per row. Placeholders must match the DataTable column names exactly.
        public static void GenerateEnvelopes(string templatePath, string outputPath, DataTable rows)
        {
            File.Copy(templatePath, outputPath, true);

            using (var doc = WordprocessingDocument.Open(outputPath, true))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                SectionProperties sectPr = body.Elements<SectionProperties>().LastOrDefault();

                // keep a copy of the template page, then clear the body
                var templateElements = body.Elements()
                    .Where(e => !(e is SectionProperties))
                    .Select(e => e.CloneNode(true))
                    .ToList();

                foreach (var el in body.Elements().Where(e => !(e is SectionProperties)).ToList())
                    el.Remove();

                for (int i = 0; i < rows.Rows.Count; i++)
                {
                    DataRow row = rows.Rows[i];
                    bool first = true;

                    foreach (var tpl in templateElements)
                    {
                        OpenXmlElement clone = tpl.CloneNode(true);
                        ReplacePlaceholders(clone, row);

                        // page break on the first paragraph of every copy except the first:
                        // no extra empty paragraph, so no blank pages
                        if (first && i > 0)
                            SetPageBreakBefore(clone);
                        first = false;

                        Append(body, clone, sectPr);
                    }
                }

                doc.MainDocumentPart.Document.Save();
            }

            // the document is closed now: fix the IDs that were copied along with the template
            MakeIdsUnique(outputPath);
        }

        // Copying the template page repeats the IDs of its text boxes. Word refuses to open a file with
        // duplicate drawing IDs ("found unreadable content"), so give every one a unique value.
        private static void MakeIdsUnique(string docxPath)
        {
            using (var doc = WordprocessingDocument.Open(docxPath, true, new OpenSettings { AutoSave = false }))
            {
                OpenXmlPart part = doc.MainDocumentPart;

                string xml;
                using (var sr = new StreamReader(part.GetStream(FileMode.Open, FileAccess.Read), Encoding.UTF8))
                    xml = sr.ReadToEnd();

                // drawing ids must be unique across the whole document
                int drawingId = 1;
                xml = Regex.Replace(xml,
                    @"(<(?:wp:docPr|wps:cNvPr)\b[^>]*?\bid="")\d+("")",
                    m => m.Groups[1].Value + (drawingId++) + m.Groups[2].Value);

                // optional tracking ids: removing them is safe, Word recreates them when needed
                xml = Regex.Replace(xml,
                    @"\s(?:wp14:anchorId|wp14:editId|w14:paraId|w14:textId)=""[0-9A-Fa-f]+""", "");

                // legacy (VML) copy of each text box
                int shapeId = 1;
                xml = Regex.Replace(xml,
                    @"(<v:shape\b[^>]*?\bid="")([^""]+)("")",
                    m => m.Groups[1].Value + m.Groups[2].Value + "_" + (shapeId++) + m.Groups[3].Value);
                xml = Regex.Replace(xml,
                    @"(\bo:spid="")[^""]+("")",
                    m => m.Groups[1].Value + "_x0000_s" + (2000 + shapeId++) + m.Groups[2].Value);

                using (var sw = new StreamWriter(part.GetStream(FileMode.Create, FileAccess.Write), new UTF8Encoding(false)))
                    sw.Write(xml);
            }
        }

        private static void Append(Body body, OpenXmlElement el, SectionProperties sectPr)
        {
            if (sectPr != null)
                body.InsertBefore(el, sectPr);
            else
                body.AppendChild(el);
        }

        private static void SetPageBreakBefore(OpenXmlElement element)
        {
            Paragraph p = element as Paragraph ?? element.Descendants<Paragraph>().FirstOrDefault();
            if (p == null) return;

            p.ParagraphProperties = p.ParagraphProperties ?? new ParagraphProperties();
            p.ParagraphProperties.PageBreakBefore = new PageBreakBefore();
        }

        private static void ReplacePlaceholders(OpenXmlElement element, DataRow row)
        {
            // the element itself (if it is a paragraph) plus every paragraph nested inside it,
            // including paragraphs inside table cells and text boxes
            var paragraphs = element.Descendants<Paragraph>().ToList();
            if (element is Paragraph self)
                paragraphs.Insert(0, self);

            foreach (var para in paragraphs)
            {
                // only the Text elements that belong directly to this paragraph,
                // not to a paragraph nested in a text box
                var texts = para.Descendants<Text>()
                    .Where(t => t.Ancestors<Paragraph>().First() == para)
                    .ToList();
                if (texts.Count == 0) continue;

                // start offset of each Text inside the paragraph's full string
                var starts = new int[texts.Count];
                int pos = 0;
                for (int i = 0; i < texts.Count; i++)
                {
                    starts[i] = pos;
                    pos += texts[i].Text.Length;
                }

                string full = string.Concat(texts.Select(t => t.Text));
                var matches = _placeholder.Matches(full).Cast<Match>().ToList();
                if (matches.Count == 0) continue;

                // go from last match to first so earlier offsets stay valid
                for (int m = matches.Count - 1; m >= 0; m--)
                {
                    Match match = matches[m];
                    string colName = match.Groups[1].Value;

                    if (!row.Table.Columns.Contains(colName))
                        continue;                               // unknown placeholder: leave it visible

                    string value = row[colName] == DBNull.Value ? "" : row[colName].ToString();

                    int s = match.Index;
                    int e = match.Index + match.Length;

                    int a = LastIndexStartingAtOrBefore(starts, s);
                    int b = LastIndexStartingAtOrBefore(starts, e - 1);

                    string ta = texts[a].Text;
                    int localStart = s - starts[a];

                    if (a == b)
                    {
                        int localEnd = e - starts[a];
                        texts[a].Text = ta.Substring(0, localStart) + value + ta.Substring(localEnd);
                    }
                    else
                    {
                        texts[a].Text = ta.Substring(0, localStart) + value;

                        for (int k = a + 1; k < b; k++)
                            texts[k].Text = "";

                        string tb = texts[b].Text;
                        int localEnd = e - starts[b];
                        texts[b].Text = tb.Substring(localEnd);
                    }

                    texts[a].Space = SpaceProcessingModeValues.Preserve;
                    texts[b].Space = SpaceProcessingModeValues.Preserve;

                    if (para.ParagraphProperties != null && para.ParagraphProperties.BiDi != null)
                    {
                        foreach (var t in new[] { texts[a], texts[b] }.Distinct())
                        {
                            Run run = t.Ancestors<Run>().FirstOrDefault();
                            if (run == null) continue;

                            run.RunProperties = run.RunProperties ?? new RunProperties();
                            run.RunProperties.RightToLeftText = new RightToLeftText();
                        }
                    }
                }
            }
        }

        private static int LastIndexStartingAtOrBefore(int[] starts, int charIndex)
        {
            int idx = 0;
            for (int i = 0; i < starts.Length; i++)
                if (starts[i] <= charIndex) idx = i;
            return idx;
        }



        //Sepecialized Generator
        //===================================================================================


        // Text placed between the departments when one subject is examined for several departments
        public const string DepartmentSeparator = " - ";

        public static Task GenerateExamsPerDayReportsAsync(string templatePath, string outputPath, DataTable rows)
        {
            return Task.Run(() => GenerateExamsPerDayReports(templatePath, outputPath, rows));
        }


        // One page per exam day (and period), with a subject table that grows by row.
        //
        // Expected columns in rows: ExamDate (yyyy-MM-dd), ExamDayName, Period, Level, Department, SubjectName.
        // Optional column LevelID: when present, the subjects are ordered by level (1, 2, 3, 4).
        //
        // Template: the header table uses {{ExamDayName}} {{ExamDate}} {{Period}};
        // the subject table has a header row and ONE model row with {{Level}} {{Department}} {{SubjectName}}.
        // The model row is repeated once per subject. A subject taken by several departments of the same
        // level on the same day is written once, with its departments joined in the Department cell.
        public static void GenerateExamsPerDayReports(string templatePath, string outputPath, DataTable rows)
        {
            if (rows == null || rows.Rows.Count == 0)
                throw new InvalidOperationException("No data to export.");

            File.Copy(templatePath, outputPath, true);

            using (var doc = WordprocessingDocument.Open(outputPath, true))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                SectionProperties sectPr = body.Elements<SectionProperties>().LastOrDefault();

                // keep a copy of the template page, then clear the body
                var templateElements = body.Elements()
                    .Where(e => !(e is SectionProperties))
                    .Select(e => e.CloneNode(true))
                    .ToList();

                foreach (var el in body.Elements().Where(e => !(e is SectionProperties)).ToList())
                    el.Remove();

                bool hasLevelId = rows.Columns.Contains("LevelID");

                // one page per day and period, in the order the rows arrive (date, then time)
                var days = rows.Rows.Cast<DataRow>()
                    .GroupBy(r => new { Date = r["ExamDate"].ToString(), Period = r["Period"].ToString() })
                    .ToList();

                for (int d = 0; d < days.Count; d++)
                {
                    List<DataRow> dayRows = days[d].ToList();
                    DataRow firstRow = dayRows[0];

                    // one line per (level, subject); the departments of that subject are joined
                    var items = dayRows
                        .GroupBy(r => new { Level = r["Level"].ToString(), Subject = r["SubjectName"].ToString() })
                        .Select(g => new
                        {
                            Level = g.Key.Level,
                            Subject = g.Key.Subject,
                            LevelOrder = hasLevelId ? Convert.ToInt32(g.First()["LevelID"]) : 0,
                            Departments = string.Join(DepartmentSeparator,
                                g.Select(x => x["Department"].ToString()).Distinct())
                        })
                        .OrderBy(x => x.LevelOrder)          // OrderBy is stable: same level keeps its order
                        .Select(x => new Dictionary<string, string>
                        {
                            ["Level"] = x.Level,
                            ["Department"] = x.Departments,
                            ["SubjectName"] = x.Subject
                        })
                        .ToList();

                    var header = new Dictionary<string, string>
                    {
                        ["ExamDayName"] = firstRow["ExamDayName"].ToString(),
                        ["ExamDate"] = FormatDate(firstRow["ExamDate"].ToString()),
                        ["Period"] = firstRow["Period"].ToString()
                    };

                    bool first = true;
                    foreach (var tpl in templateElements)
                    {
                        OpenXmlElement clone = tpl.CloneNode(true);

                        FillSubjectTable(clone, items);
                        ReplacePlaceholders(clone, name => header.TryGetValue(name, out string v) ? v : null);

                        if (first && d > 0)
                            SetPageBreakBefore(clone);
                        first = false;

                        Append(body, clone, sectPr);
                    }
                }

                doc.MainDocumentPart.Document.Save();
            }

            MakeIdsUnique(outputPath);
        }

        // Finds the table that holds {{SubjectName}} and repeats its model row once per item.
        private static void FillSubjectTable(OpenXmlElement clone, List<Dictionary<string, string>> items)
        {
            IEnumerable<Table> tables = clone is Table self
                ? new[] { self }
                : clone.Descendants<Table>();

            Table table = tables.FirstOrDefault(t => t.InnerText.Contains("{{SubjectName}}"));
            if (table == null)
                return;

            // the model row is the last row that still contains a placeholder
            TableRow model = table.Elements<TableRow>().LastOrDefault(r => r.InnerText.Contains("{{"));
            if (model == null)
                return;

            foreach (var item in items)
            {
                var newRow = (TableRow)model.CloneNode(true);
                ReplacePlaceholders(newRow, name => item.TryGetValue(name, out string v) ? v : null);
                table.InsertBefore(newRow, model);
            }

            model.Remove();
        }

        private static string FormatDate(string text)
        {
            DateTime date;
            if (DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date.ToString("d/M/yyyy", CultureInfo.InvariantCulture);

            return text;
        }

        // lookup returns the text for a placeholder name, or null for an unknown name
        // (an unknown placeholder is left as {{...}} so a typo in the template is easy to spot).
        private static void ReplacePlaceholders(OpenXmlElement element, Func<string, string> lookup)
        {
            // the element itself (if it is a paragraph) plus every paragraph nested inside it,
            // including paragraphs inside table cells and text boxes
            var paragraphs = element.Descendants<Paragraph>().ToList();
            if (element is Paragraph self)
                paragraphs.Insert(0, self);

            foreach (var para in paragraphs)
            {
                // only the Text elements that belong directly to this paragraph,
                // not to a paragraph nested in a text box
                var texts = para.Descendants<Text>()
                    .Where(t => t.Ancestors<Paragraph>().First() == para)
                    .ToList();
                if (texts.Count == 0) continue;

                // start offset of each Text inside the paragraph's full string
                var starts = new int[texts.Count];
                int pos = 0;
                for (int i = 0; i < texts.Count; i++)
                {
                    starts[i] = pos;
                    pos += texts[i].Text.Length;
                }

                string full = string.Concat(texts.Select(t => t.Text));
                var matches = _placeholder.Matches(full).Cast<Match>().ToList();
                if (matches.Count == 0) continue;

                // go from last match to first so earlier offsets stay valid
                for (int m = matches.Count - 1; m >= 0; m--)
                {
                    Match match = matches[m];
                    string colName = match.Groups[1].Value;

                    string value = lookup(colName);
                    if (value == null)
                        continue;                               // unknown placeholder: leave it visible

                    int s = match.Index;
                    int e = match.Index + match.Length;

                    int a = LastIndexStartingAtOrBefore(starts, s);
                    int b = LastIndexStartingAtOrBefore(starts, e - 1);

                    string ta = texts[a].Text;
                    int localStart = s - starts[a];

                    if (a == b)
                    {
                        int localEnd = e - starts[a];
                        texts[a].Text = ta.Substring(0, localStart) + value + ta.Substring(localEnd);
                    }
                    else
                    {
                        texts[a].Text = ta.Substring(0, localStart) + value;

                        for (int k = a + 1; k < b; k++)
                            texts[k].Text = "";

                        string tb = texts[b].Text;
                        int localEnd = e - starts[b];
                        texts[b].Text = tb.Substring(localEnd);
                    }

                    texts[a].Space = SpaceProcessingModeValues.Preserve;
                    texts[b].Space = SpaceProcessingModeValues.Preserve;

                    if (para.ParagraphProperties != null && para.ParagraphProperties.BiDi != null)
                    {
                        foreach (var t in new[] { texts[a], texts[b] }.Distinct())
                        {
                            Run run = t.Ancestors<Run>().FirstOrDefault();
                            if (run == null) continue;

                            run.RunProperties = run.RunProperties ?? new RunProperties();
                            run.RunProperties.RightToLeftText = new RightToLeftText();
                        }
                    }
                }
            }
        }

    }
}