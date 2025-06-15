using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_PBO.Helpers
{
    public static class ViewHelper
    {
        public static void ShowDialogWithFormOverlay(Control ownerControl, Form dialogForm)
        {
            if (ownerControl == null) throw new ArgumentNullException(nameof(ownerControl));
            if (dialogForm == null) throw new ArgumentNullException(nameof(dialogForm));

            Form parentForm = ownerControl.FindForm();
            if (parentForm == null)
            {
                // Jika tidak ada parent form, gunakan ownerControl sebagai form atau pusatkan dialog
                if (ownerControl is Form formAsOwner)
                {
                    parentForm = formAsOwner;
                }
                else
                {
                    dialogForm.StartPosition = FormStartPosition.CenterScreen;
                    dialogForm.ShowDialog();
                    return;
                }
            }

            Form overlayForm = null;

            try
            {
                // Buat form overlay
                overlayForm = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    BackColor = Color.Black,
                    Opacity = 0.8,
                    StartPosition = FormStartPosition.Manual,
                    ShowInTaskbar = false,
                    TopMost = false,
                    WindowState = FormWindowState.Normal
                };

                // Sesuaikan ukuran dan posisi overlay dengan parent form
                overlayForm.Bounds = parentForm.Bounds;

                // Tampilkan overlay
                overlayForm.Show(parentForm);

                // Konfigurasi dan tampilkan dialog
                dialogForm.StartPosition = FormStartPosition.CenterParent;
                dialogForm.TopMost = false;
                dialogForm.ShowDialog(parentForm);
            }
            finally
            {
                // Hapus overlay
                if (overlayForm != null)
                {
                    overlayForm.Close();
                    overlayForm.Dispose();
                }
            }
        }
    }
}
