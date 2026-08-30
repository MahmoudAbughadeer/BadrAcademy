
using System.Drawing;
using System.Windows.Forms;

namespace BadrAcademy.Helpers
{
    internal static class clsOpenFormHelper
    {
        public static void ShowDialogCenteredInContainer(Form frm, Control Container)
        {
            // Calculate screen coordinates for centering
            Point pagePlaceScreenLocation = Container.PointToScreen(Point.Empty);

            frm.StartPosition = FormStartPosition.Manual;//Must make it manual
            frm.Location = new Point(
                pagePlaceScreenLocation.X + (Container.ClientSize.Width - frm.Width) / 2,
                pagePlaceScreenLocation.Y + (Container.ClientSize.Height - frm.Height) / 2
            );

            frm.ShowDialog();//Mus be after the location cause showDialog wait for close the form so the location won't set
        }
    }
}
