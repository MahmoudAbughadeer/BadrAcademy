using BadrAcademy.Helpers;
using BLL;
using BLL.Core;
using BLL.Validation;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Levels
{
    public partial class frmAddEditLevel : Form
    {
        
        enMode _mode;
        int _levelID;
        clsLevel _level;

        public frmAddEditLevel()
        {
            InitializeComponent();
            _mode = enMode.Add;
            _level = null;
        }

        public frmAddEditLevel(int levelID)
        {
            InitializeComponent();
            _mode = enMode.Update;
            _levelID = levelID;
        }



        //Events

        public delegate Task SaveCompletedAsync();

        public event SaveCompletedAsync SaveCompleted;

        private async void frmAddEditLevel_Load(object sender, EventArgs e)
        {

            //Set combo boxeds
            cbLevelName.SelectedIndex = 0;

            //Handel update mode
            if(_mode == enMode.Update)
            {
                _level = await clsAsyncMethodExecutor.RunWithWaitAsync<clsLevel>(ctrWait1, () => clsLevel.FindAsync(_levelID));
                ChangeFormTitleToUpdate();
                DiplayLevelData();
            }       
        }



        //Private Methods

        private void LoadSettingInfoFromFormToSLevelObject()
        {

            string levelName = cbLevelName.Text.Trim();

            if (_mode == enMode.Add)
            {
                _level = new clsLevel(levelName);
                return;
            }

            _level.LevelName = levelName;
        }

        private void DiplayLevelData()
        {
            lblLevelID.Text = _level.LevelID.ToString();
            cbLevelName.Text = _level.LevelName.Trim();
        }

        private void ChangeFormTitleToUpdate()
        {
            lblOperationTitle.Text = "تعديل اعدادات المستوي";
        }

        private bool IsLevelDataChanged()
        {
            return _level.LevelName.Trim() != cbLevelName.Text.Trim();
        }



        //Buttons Events
        private async void btnSave_Click(object sender, EventArgs e)
        {

            if (_mode == enMode.Update && !IsLevelDataChanged())
            {
                clsMessageBoxHelper.ShowWarning("لم يتم إجراء اى تعديلات");
                return;
            }



            LoadSettingInfoFromFormToSLevelObject();
            if (!await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, () => _level.SaveAsync()))
            {

                clsMessageBoxHelper.ShowWarning("حدث خطأ أثناء محاولة حفظ بيانات المستوي");
                return;
            }

            lblLevelID.Text = _level.LevelID.ToString();

            await SaveCompleted?.Invoke();

            if (_mode == enMode.Add)
            {
                _mode = enMode.Update;
                ChangeFormTitleToUpdate();
            }

            clsMessageBoxHelper.ShowInfo("تم حفظ البيانات بنجاح");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}







