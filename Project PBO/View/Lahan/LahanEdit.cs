using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_PBO.Controller;
using Project_PBO.Models;

namespace Project_PBO.View
{
    public partial class LahanEdit : Form
    {
        private int id_lahan;
        private bool initialIsActive;

        public LahanEdit(LahanModel lahan)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 340);

            id_lahan = lahan.IdLahan;
            inputNama.Text = lahan.Nama;
            inputLuas.Text = lahan.Luas.ToString();
            inputLokasi.Text = lahan.Lokasi;

            initialIsActive = lahan.IsActive;
        }

        private void LahanEdit_Load(object sender, EventArgs e)
        {
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");

            inputActive.SelectedItem = initialIsActive ? "Ya" : "Tidak";
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string nama = inputNama.Text.Trim();
            string luasText = inputLuas.Text.Trim();
            string lokasi = inputLokasi.Text.Trim();

            if (
                string.IsNullOrEmpty(nama)
                || string.IsNullOrEmpty(luasText)
                || string.IsNullOrEmpty(lokasi)
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

            // Konversi luas dari string ke int
            if (!int.TryParse(luasText, out int luas) || luas <= 0)
            {
                MessageBox.Show(
                    "Luas harus berupa angka yang valid dan lebih dari 0.",
                    "Validasi Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }
            try
            {
                bool isUpdated = LahanController.UpdateLahan(
                    new LahanModel
                    {
                        IdLahan = id_lahan,
                        Nama = nama,
                        Luas = luas,
                        Lokasi = lokasi,
                        IsActive = inputActive.SelectedItem?.ToString() == "Ya" ? true : false,
                    }
                );
                if (isUpdated)
                {
                    MessageBox.Show(
                        "Lahan berhasil diperbarui.",
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
                        "Gagal memperbarui lahan",
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
