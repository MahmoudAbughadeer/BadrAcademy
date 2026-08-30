using BLL;
using BadrAcademy.Helpers;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Levels.Controls
{
    public partial class ctrLevelsPage : UserControl
    {
        //Private fields
        private BindingSource _bindingSource = new BindingSource();


        //Constructors
        public ctrLevelsPage()
        {
            InitializeComponent();
        }


        //DataGridView Events
        private void dgvLevels_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvLevels.Rows[e.RowIndex].Cells["RowNumber"].Value = e.RowIndex + 1;
        }

        //Events
        private async void ctrSettingsPage_Load(object sender, EventArgs e)
        {
            InitializeDataGrideViewColumns();//Here I add the columns manually
            dgvLevels.AutoGenerateColumns = false;//this is adding columnss/property name that come from database

            //change the font size and adjust row height
            dgvLevels.Font = new Font("Arial", 14);
            dgvLevels.RowTemplate.Height = 35;
            dgvLevels.DefaultCellStyle.Padding = new Padding(5);

            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadLevels);
        }


        //Private Methods
        private async Task LoadLevels()
        {
            _bindingSource.DataSource = null;

            DataTable levels = await clsLevel.GetAllAsync();
            _bindingSource.DataSource = levels;
            dgvLevels.DataSource = _bindingSource;

            clsButtonHelper.SetEditDelteButtonsState(dgvLevels, btnEdit, btnDelete);
        }
 
        private void InitializeDataGrideViewColumns()
        {
            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RowNumber",
                DataPropertyName = "RowNumber",
                HeaderText = "م",
                FillWeight = 10,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelID",
                DataPropertyName = "LevelID",
                HeaderText = "المعرف",
                FillWeight = 25
            });

            dgvLevels.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "LevelName",
                DataPropertyName = "LevelName",
                HeaderText = "المستوي",
                FillWeight = 65
            });
        }
        
      
        private async Task AddEditLevel_SaveCompletedAsync()
        {
            await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadLevels);
        }



        //Buttons Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddEditLevel frm = new frmAddEditLevel();
            frm.SaveCompleted += AddEditLevel_SaveCompletedAsync; //will not be block you event is Task and awaitable
            frm.ShowDialog();

            frm.SaveCompleted -= AddEditLevel_SaveCompletedAsync;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int currentLevelID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvLevels, "LevelID");
            frmAddEditLevel frm = new frmAddEditLevel(currentLevelID);

            frm.SaveCompleted += AddEditLevel_SaveCompletedAsync;//will not be block you event is Task and awaitable
            frm.ShowDialog();
            frm.SaveCompleted -= AddEditLevel_SaveCompletedAsync;
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            int currentLevelID = clsDataGridViewHelper.GetCellValueAsInt(this.dgvLevels, "LevelID");

            if (clsMessageBoxHelper.ShowConfirmation("هل أنت متأكد من حذف هذه المستوي؟") == DialogResult.Cancel)
                return;

            if (await clsAsyncMethodExecutor.RunWithWaitAsync<bool>(ctrWait1, () => clsLevel.DeleteAsync(currentLevelID)))
            {
                await clsAsyncMethodExecutor.RunWithWaitAsync(ctrWait1, LoadLevels);
                clsMessageBoxHelper.ShowInfo("تم حذف بيانات المستوي");         
            }
            else
                clsMessageBoxHelper.ShowError("حدث خطأ عند محاولة حذف بيانات المستوي");
        }    
    }
}
