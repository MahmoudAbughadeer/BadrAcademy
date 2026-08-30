using BadrAcademy.Properties;
using System.ComponentModel;
using System.Windows.Forms;

namespace BadrAcademy.Forms.Misc.Controls
{
    public partial class ctrWait : UserControl
    {
        //Enums
        public enum enSpinnerColor { Black, Green };


        //Backing Fields
        private enSpinnerColor _spinnerColor = enSpinnerColor.Black;


        //Properties
        [Category("Spinner Appearance")]
        [Description("Spinner color")]
        public enSpinnerColor SpinnerColor
        { 
            get => _spinnerColor;
            set
            {
                if(value == _spinnerColor)
                    return;

                _spinnerColor = value;
                SetSpinnerColor();
            }
        }


        //Constructors
        public ctrWait()
        {
            InitializeComponent();
        }


        //Private Methods
        private void SetSpinnerColor()
        {
            switch(_spinnerColor)
            {
                case enSpinnerColor.Black:
                    pbSpinner.Image = Resources.Spinner100;
                    break;
                case enSpinnerColor.Green:
                    pbSpinner.Image = Resources.GreenSpinner92;
                    break;
            }
        }
    }
}
