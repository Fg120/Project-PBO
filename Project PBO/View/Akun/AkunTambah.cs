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
    public partial class AkunTambah : Form
    {
        public AkunTambah()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 280);
        }

        private void AkunTambah_Load(object? sender, EventArgs e)
        {
            inputRole.Items.Add("Admin");
            inputRole.Items.Add("Petani");
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");
            inputActive.SelectedIndex = 0; // Default "Ya"
            inputRole.SelectedIndex = 0; // Default "Admin"
        }

        private void btnBatal_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSimpan_Click(object? sender, EventArgs e)
        {
            string username = inputUsername.Text.Trim();
            string password = inputPassword.Text?.Trim() ?? string.Empty;
            string? role = inputRole.SelectedItem?.ToString();
            string? activeStatus = inputActive.SelectedItem?.ToString();

            if (
                string.IsNullOrEmpty(username)
                || string.IsNullOrEmpty(password)
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
                bool isAdded = AkunController.AddAkun(
                    new AkunModel
                    {
                        Username = username,
                        Password = password,
                        Role = role,
                        IsActive = activeStatus == "Ya",
                    }
                );

                if (isAdded)
                {
                    MessageBox.Show(
                        "Akun berhasil ditambahkan.",
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
                        "Gagal menambahkan akun. Silakan coba lagi.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Terjadi kesalahan: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
