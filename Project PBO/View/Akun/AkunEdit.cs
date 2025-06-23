using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_PBO.Controller;
using Project_PBO.Models;

namespace Project_PBO.View
{
    public partial class AkunEdit : Form
    {
        private int id_akun;
        private string initialRole;
        private bool initialIsActive;

        public AkunEdit(AkunModel akun)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 280);

            id_akun = akun.IdAkun;
            inputUsername.Text = akun.Username;
            inputPassword.Text = akun.Password;

            initialRole = akun.Role;
            initialIsActive = akun.IsActive;
        }

        private void AkunEdit_Load(object? sender, EventArgs e)
        {
            inputRole.Items.Add("Admin");
            inputRole.Items.Add("Petani");
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");

            inputRole.SelectedItem = initialRole;
            inputActive.SelectedItem = initialIsActive ? "Ya" : "Tidak";
        }

        private void btnBatal_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Ensure form closes
        }

        private void btnSimpan_Click(object? sender, EventArgs e)
        {
            string username = inputUsername.Text.Trim();
            string password = inputPassword.Text?.Trim() ?? string.Empty;
            string? role = inputRole.SelectedItem?.ToString();
            string? activeStatus = inputActive.SelectedItem?.ToString();

            if (
                string.IsNullOrEmpty(username)
                ||
                string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(role)
                || string.IsNullOrEmpty(activeStatus)
            )
            {
                MessageBox.Show(
                    "Mohon isi semua input.",
                    "Validasi Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            try
            {
                bool isUpdated = AkunController.UpdateAkun(
                    new AkunModel
                    {
                        IdAkun = id_akun,
                        Username = username,
                        Password = password,
                        Role = role,
                        IsActive = activeStatus == "Ya" ? true : false,
                    }
                );
                if (isUpdated)
                {
                    MessageBox.Show(
                        "Akun berhasil diperbarui.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Gagal memperbarui akun",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error terjadi: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
