

PRAGMA foreign_keys = ON;


-- ============================================================
-- Users
-- ============================================================
CREATE TABLE IF NOT EXISTS Users (
    UserID       INTEGER PRIMARY KEY AUTOINCREMENT,
    FullName     TEXT NOT NULL,
    Username     TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    IsActive	 INTEGER NOT NULL DEFAULT 1
);

-- ============================================================
-- Settings
-- ============================================================
CREATE TABLE IF NOT EXISTS Settings (
    SettingID           INTEGER PRIMARY KEY AUTOINCREMENT,
    AcademicYear        TEXT NOT NULL,              -- 2025/2026
    Semester            TEXT NOT NULL,              -- الأول / الثاني
    SemesterType        TEXT NOT NULL,        -- منتصف الفصل / نهاية الفصل
    DefaultAnswerForms  TEXT NOT NULL DEFAULT 'أ / ب'
);

-- ============================================================
-- Halls
-- ============================================================
CREATE TABLE IF NOT EXISTS Halls (
    HallID          INTEGER PRIMARY KEY AUTOINCREMENT,
    HallName        TEXT NOT NULL UNIQUE,           -- مدرج 1 / قاعة 7
    HallType        TEXT NOT NULL,                  -- مدرج / قاعة/ معمل
    DefaultCapacity INTEGER NOT NULL DEFAULT 30,
    IsActive        INTEGER NOT NULL DEFAULT 1       -- 0/1 بديل BIT
);

-- ============================================================
-- Subjects
-- ============================================================
CREATE TABLE IF NOT EXISTS Subjects (
    SubjectID       INTEGER PRIMARY KEY AUTOINCREMENT,
    SubjectName     TEXT NOT NULL UNIQUE
);
-- ============================================================
-- Levels
-- ============================================================
CREATE TABLE IF NOT EXISTS Levels (
    LevelID         INTEGER PRIMARY KEY AUTOINCREMENT,
    LevelName       TEXT NOT NULL UNIQUE            -- الثالثة / الرابعة
);

-- ============================================================
-- Departments
-- ============================================================

CREATE TABLE IF NOT EXISTS Departments (
    DepartmentID    INTEGER PRIMARY KEY AUTOINCREMENT,
    DepartmentName  TEXT NOT NULL UNIQUE,           -- نظم المعلومات / محاسبة / ادارة الاعمال
    Description     TEXT                            -- وصف اختياري للقسم، يسمح NULL تلقائياً
);


-- ============================================================
-- Dcotors
-- ============================================================
CREATE TABLE IF NOT EXISTS Doctors (
    DoctorID        INTEGER PRIMARY KEY AUTOINCREMENT,
    DoctorName      TEXT NOT NULL UNIQUE,
    DepartmentID    INTEGER NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID)
);

-- ============================================================
-- SubjectOfferings  (معدّل — إضافة عمود Semester)
--    بيحدد المقرر متاح لأي فرقة/قسم في أي ترم (الأول / الثاني / سمر)
-- ============================================================
CREATE TABLE IF NOT EXISTS SubjectOfferings (
    OfferingID      INTEGER PRIMARY KEY AUTOINCREMENT,
    SubjectID       INTEGER NOT NULL,
    LevelID         INTEGER NOT NULL,
    DepartmentID    INTEGER NOT NULL,
    Semester        TEXT NOT NULL,                  -- الأول / الثاني / سمر
    FOREIGN KEY (SubjectID) REFERENCES Subjects(SubjectID),
    FOREIGN KEY (LevelID) REFERENCES Levels(LevelID),
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    UNIQUE (SubjectID, LevelID, DepartmentID, Semester)
);
 
-- ============================================================
-- SubjectDoctors  (Subject Offering تعيين الاساتذه لكل)
-- ============================================================

CREATE TABLE IF NOT EXISTS SubjectDoctors (
    SubjectDoctorID INTEGER PRIMARY KEY AUTOINCREMENT,
    OfferingID      INTEGER NOT NULL UNIQUE,        -- one assignment record per offering
    DoctorID        INTEGER NOT NULL,               -- الدكتور الأول (إلزامي)
    DoctorID2       INTEGER,                        -- الدكتور الثاني (اختياري)
    FOREIGN KEY (OfferingID) REFERENCES SubjectOfferings(OfferingID),
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID),
    FOREIGN KEY (DoctorID2) REFERENCES Doctors(DoctorID)
);


-- ============================================================
-- 4. ExamSchedule
--    بدل ما يحمل SubjectID + LevelID + DepartmentID لوحدهم (وممكن يتعارضوا
--    مع SubjectOfferings)، بقى يشاور على OfferingID اللي فيه السياق كامل
-- ============================================================
CREATE TABLE IF NOT EXISTS ExamSchedule (
    ExamID          INTEGER PRIMARY KEY AUTOINCREMENT,
    OfferingID      INTEGER NOT NULL,
    ExamDayName     TEXT NOT NULL,                  -- الاحد / الاثنين ...
    ExamDate        TEXT NOT NULL,                  -- 'YYYY-MM-DD'
    TimeFrom        INTEGER NOT NULL,
    TimeTo          INTEGER NOT NULL,
    Period          TEXT NOT NULL,                  -- الأولى / الثانية
    SettingID       INTEGER NOT NULL,
    FOREIGN KEY (OfferingID) REFERENCES SubjectOfferings(OfferingID),
    FOREIGN KEY (SettingID) REFERENCES Settings(SettingID)
);

-- ============================================================
-- 5. DistributionTemplate
-- ============================================================
CREATE TABLE IF NOT EXISTS DistributionTemplate (
    TemplateID      INTEGER PRIMARY KEY AUTOINCREMENT,

    LevelID         INTEGER NOT NULL,
    DepartmentID    INTEGER NOT NULL,

    CommitteeNo     INTEGER NOT NULL,
    StudentsCount   INTEGER NOT NULL,
    SettingID       INTEGER NOT NULL,
    HallID          INTEGER NOT NULL,
    FOREIGN KEY (LevelID) REFERENCES Levels(LevelID),
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (SettingID) REFERENCES Settings(SettingID),
    FOREIGN KEY (HallID) REFERENCES Halls(HallID),
    UNIQUE (SettingID, LevelID, DepartmentID, CommitteeNo)
);

-- ============================================================
-- فهارس مساعدة (تسريع الاستعلامات)
-- ============================================================
CREATE INDEX IF NOT EXISTS idx_exam_setting    ON ExamSchedule(SettingID);
CREATE INDEX IF NOT EXISTS idx_exam_offering   ON ExamSchedule(OfferingID);
CREATE INDEX IF NOT EXISTS idx_offering_subject ON SubjectOfferings(SubjectID);
CREATE INDEX IF NOT EXISTS idx_offering_level   ON SubjectOfferings(LevelID);
CREATE INDEX IF NOT EXISTS idx_offering_department ON SubjectOfferings(DepartmentID);
CREATE INDEX IF NOT EXISTS idx_dist_setting    ON DistributionTemplate(SettingID);
CREATE INDEX IF NOT EXISTS idx_dist_hall       ON DistributionTemplate(HallID);
CREATE INDEX IF NOT EXISTS idx_dist_level      ON DistributionTemplate(LevelID);
CREATE INDEX IF NOT EXISTS idx_dist_department ON DistributionTemplate(DepartmentID);

-- ============================================================
-- Views (تعمل في SQLite بدون مشاكل — بدل الـ Stored Procedures)
-- ============================================================

-- *** مراية الأسئلة *** : صف واحد لكل مدرج/قاعة/ معمل لكل امتحان
DROP VIEW IF EXISTS vw_QuestionEnvelopes;
CREATE VIEW vw_QuestionEnvelopes AS
WITH HallGroups AS (
    SELECT
        dt.SettingID, dt.LevelID, dt.DepartmentID, dt.HallID,
        MIN(dt.CommitteeNo)   AS CommitteeFrom,
        MAX(dt.CommitteeNo)   AS CommitteeTo,
        COUNT(*)              AS CommitteesCount,
        SUM(dt.StudentsCount) AS TotalStudents,
        DENSE_RANK() OVER (
            PARTITION BY dt.SettingID, dt.LevelID, dt.DepartmentID
            ORDER BY MIN(dt.CommitteeNo)
        ) AS EnvelopeOrder
    FROM DistributionTemplate dt
    GROUP BY dt.SettingID, dt.LevelID, dt.DepartmentID, dt.HallID
),
EnvelopeTotals AS (
    SELECT SettingID, LevelID, DepartmentID, COUNT(*) AS TotalEnvelopes
    FROM HallGroups
    GROUP BY SettingID, LevelID, DepartmentID
)
SELECT
    e.ExamID,
    s.SubjectName                                        AS SubjectName,
    lv.LevelName                                          AS Level,
    dp.DepartmentName                                     AS Department,
    e.ExamDayName                                         AS ExamDayName,
    e.ExamDate                                            AS ExamDate,
    e.TimeFrom                                            AS TimeFrom,
    e.TimeTo                                              AS TimeTo,
    e.Period                                              AS Period,
    h.HallName                                            AS HallName,
    hg.CommitteeFrom AS CommitteeFrom,
    hg.CommitteeTo   AS CommitteeTo,
    hg.TotalStudents                                      AS QuestionSheets,
    hg.CommitteesCount                                    AS CommitteesCount,
    hg.EnvelopeOrder                                      AS EnvelopeOrder,
    et.TotalEnvelopes                                     AS TotalEnvelopes
FROM ExamSchedule e
JOIN SubjectOfferings so ON so.OfferingID = e.OfferingID
JOIN Subjects s ON s.SubjectID = so.SubjectID
JOIN Levels lv ON lv.LevelID = so.LevelID
JOIN Departments dp ON dp.DepartmentID = so.DepartmentID
JOIN HallGroups hg
    ON hg.SettingID    = e.SettingID
   AND hg.LevelID      = so.LevelID
   AND hg.DepartmentID = so.DepartmentID
JOIN EnvelopeTotals et
    ON et.SettingID    = e.SettingID
   AND et.LevelID      = so.LevelID
   AND et.DepartmentID = so.DepartmentID
JOIN Halls h ON h.HallID = hg.HallID;


-- *** مراية الأجابه *** : أنشئ صفًا واحدًا لكل لجنة حتى تتمكن من طباعة مظروف إجابة لكل لجنة.
DROP VIEW IF EXISTS vw_AnswerEnvelopes;
CREATE VIEW vw_AnswerEnvelopes AS
WITH Numbered AS (
    SELECT
        dt.SettingID, dt.LevelID, dt.DepartmentID, dt.HallID,
        dt.CommitteeNo, dt.StudentsCount,
        ROW_NUMBER() OVER (
            PARTITION BY dt.SettingID, dt.LevelID, dt.DepartmentID
            ORDER BY dt.CommitteeNo
        ) AS EnvelopeSeq,
        COUNT(*) OVER (
            PARTITION BY dt.SettingID, dt.LevelID, dt.DepartmentID
        ) AS TotalCommittees
    FROM DistributionTemplate dt
)
SELECT
    e.ExamID,
    s.SubjectName         AS SubjectName,
    lv.LevelName           AS Level,
    dp.DepartmentName       AS Department,
    e.ExamDayName         AS ExamDayName,
    e.ExamDate            AS ExamDate,
    e.TimeFrom            AS TimeFrom,
    e.TimeTo              AS TimeTo,
    e.Period              AS Period,
    n.CommitteeNo         AS CommitteeNo,
    h.HallName            AS HallName,
    n.StudentsCount       AS StudentsCount,
    n.EnvelopeSeq         AS EnvelopeSeq,
    n.TotalCommittees     AS TotalCommittees
FROM ExamSchedule e
JOIN SubjectOfferings so ON so.OfferingID = e.OfferingID
JOIN Subjects s ON s.SubjectID = so.SubjectID
JOIN Levels lv ON lv.LevelID = so.LevelID
JOIN Departments dp ON dp.DepartmentID = so.DepartmentID
JOIN Numbered n
    ON n.SettingID     = e.SettingID
   AND n.LevelID       = so.LevelID
   AND n.DepartmentID  = so.DepartmentID
JOIN Halls h ON h.HallID = n.HallID;



-- *** مرايات الأيام / تعليق/ عام لباقى الحجات *** : صف واحد لكل امتحان
DROP VIEW IF EXISTS vw_DayGeneral;
CREATE VIEW vw_DayGeneral AS
SELECT
    e.ExamID,
    s.SubjectName       AS SubjectName,
    d.DoctorName        AS DoctorName,
    lv.LevelName        AS Level,
    dp.DepartmentName   AS Department,
    e.ExamDayName       AS ExamDayName,
    e.ExamDate          AS ExamDate,
    e.TimeFrom          AS TimeFrom,
    e.TimeTo            AS TimeTo,
    e.Period            AS Period
FROM ExamSchedule e
JOIN SubjectOfferings so ON so.OfferingID = e.OfferingID
JOIN Subjects s ON s.SubjectID = so.SubjectID
JOIN SubjectDoctors sd ON sd.OfferingID = e.OfferingID
JOIN Doctors d ON d.DoctorID = sd.DoctorID
JOIN Levels lv ON lv.LevelID = so.LevelID
JOIN Departments dp ON dp.DepartmentID = so.DepartmentID;