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
    public partial class TanamanTambah : Form
    {
        public TanamanTambah()
        {
            InitializeComponent();

            // Apply styling similar to LahanTambah
            this.FormBorderStyle = FormBorderStyle.None; // Remove window header
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225); // Common background color
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 280); // Consistent size with other new forms
        }

        private void TanamanTambah_Load(object? sender, EventArgs e) // sender can be null
        {
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");
            inputActive.SelectedIndex = 0;
        }

        private void btnBatal_Click(object? sender, EventArgs e) // sender can be null
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close(); // Ensure form closes
        }

        private void btnSimpan_Click(object? sender, EventArgs e) // sender can be null
        {
            string nama = inputNama.Text.Trim();
            string masaTanamText = inputMasaTanam.Text.Trim();
            string? activeStatus = inputActive.SelectedItem?.ToString(); // activeStatus can be null

            if (
                string.IsNullOrEmpty(nama)
                || string.IsNullOrEmpty(masaTanamText)
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

            if (!int.TryParse(masaTanamText, out int masaTanam) || masaTanam <= 0)
            {
                MessageBox.Show(
                    "Masa Tanam harus berupa angka yang valid dan lebih dari 0.",
                    "Validasi Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                bool isAdded = TanamanController.AddTanaman(
                    new TanamanModel
                    {
                        Nama = nama,
                        MasaTanam = masaTanam,
                        IsActive = activeStatus == "Ya",
                    }
                );
                if (isAdded)
                {
                    MessageBox.Show(
                        "Tanaman berhasil ditambahkan.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.OK;
                    this.Close(); // Ensure form closes
                }
                else
                {
                    MessageBox.Show(
                        "Gagal menambahkan tanaman. Silakan coba lagi.",
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
