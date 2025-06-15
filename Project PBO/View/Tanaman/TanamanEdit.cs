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
    public partial class TanamanEdit : Form
    {
        private int id_tanaman;
        private bool initialIsActive;

        public TanamanEdit(TanamanModel tanaman)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None; // Remove window header
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225); // Match LahanEdit
            this.TransparencyKey = Color.FromArgb(255, 242, 225); // Match LahanEdit
            this.Size = new Size(820, 280); // Match TanamanTambah

            id_tanaman = tanaman.IdTanaman;
            inputNama.Text = tanaman.Nama;
            inputMasaTanam.Text = tanaman.MasaTanam.ToString();
            initialIsActive = tanaman.IsActive;
        }

        private void TanamanEdit_Load(object? sender, EventArgs e) // Changed signature
        {
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");

            inputActive.SelectedItem = initialIsActive ? "Ya" : "Tidak";
        }

        private void btnBatal_Click(object? sender, EventArgs e) // Changed signature
        {
            this.DialogResult = DialogResult.Cancel; // Changed from DialogResult
            this.Close(); // Changed from DialogResult
        }

        private void btnSimpan_Click(object? sender, EventArgs e) // Changed signature
        {
            string nama = inputNama.Text.Trim();
            string masaTanamText = inputMasaTanam.Text.Trim();

            if (string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(masaTanamText))
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
                bool isUpdated = TanamanController.UpdateTanaman(
                    new TanamanModel
                    {
                        IdTanaman = id_tanaman,
                        Nama = nama,
                        MasaTanam = masaTanam,
                        IsActive = inputActive.SelectedItem?.ToString() == "Ya" ? true : false,
                    }
                );
                if (isUpdated)
                {
                    MessageBox.Show(
                        "Tanaman berhasil diperbarui.",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    this.DialogResult = DialogResult.OK; // Changed from DialogResult
                    this.Close(); // Changed from DialogResult
                }
                else
                {
                    MessageBox.Show(
                        "Gagal memperbarui tanaman.", // Corrected typo
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
