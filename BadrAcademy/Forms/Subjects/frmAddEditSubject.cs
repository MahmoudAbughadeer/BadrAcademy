using BLL;
using BLL.Core;
using BLL.Validation;
using BadrAcademy.Helpers;
using BadrAcademy.Global;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BLL.Utilities;
using System.Data;

namespace BadrAcademy.Forms.Subjects
{
    public partial class frmAddEditSubject : Form
    {
        
        enMode _mode;
        int _subjectID;
        clsSubject _subject;
        clsTextBoxValidator _formValidator;


        public frmAddEditSubject()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _subject = null;
        }

        public frmAddEditSubject(int subjectID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _subjectID = subjectID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditSubject_Load(object sender, EventArgs e)
        {
            _formValidator = new clsTextBoxValidator(epSubject);
            SetupValidation();

            if(_mode == enMode.Update)
            {
                _subject = await clsAsyncMethodExecutor.RunWithWaitAsync<clsSubject>(ctrWait1, () => clsSubject.FindAsync(_subjectID));
                ChangeFormTitleToUpdate();
                DiplaySubjectData();
            }       
        }



        //Private Methods
        private void SetupValidation()
        {
            _formValidator.Clear();
            _formValidator.Add(txtSubjectName, new clsRequiredValidator());
        }

        private void LoadUserInfoFromFormToSubjectObject()
        {
            string subjectName = txtSubjectName.Text.Trim();
            if (_mode == enMode.Add)
            {
                _subject = new clsSubject(subjectName);
                return;
            }

            _subject.SubjectName = subjectName;
        }

        private void DiplaySubjectData()
        {
            txtSubjectName.Text = _subject.SubjectName.Trim();
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل بيانات المقرر";
        }

        private bool IsUserDataChanged()
        {
            return _subject.SubjectName.Trim() != txtSubjectName.Text.Trim();
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (!_formValidator.AreAllFieldsValid())
            {
                clsMessageBoxHelper.ShowWarning("بعض الحقول غير صحيحة، ضع مؤشر الماوس فوق الأيقونات الحمراء لمعرفة الخطأ");
                return;
            }

            if (_mode == enMode.Update && !IsUserDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadUserInfoFromFormToSubjectObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _subject.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات المقرر");
                return;
            }

            lblSubjectID.Text = _subject.SubjectID.ToString();

            await SaveCompleted?.Invoke();

            if (_mode == enMode.Add)
            {
                _mode = enMode.Update;
                ChangeFormTitleToUpdate();
                SetupValidation();
            }

            clsMessageBoxHelper.ShowInfo("تم حفظ البيانات بنجاح");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    
    }
}







