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
using Project_PBO.Helpers;
using Project_PBO.Models;

namespace Project_PBO.View
{
    public partial class LahanTambah : Form
    {
        public LahanTambah()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 340);

            SetUserInfo();
        }

        private void SetUserInfo()
        {
            this.Text = $"Add Lahan - User: {UserSession.Username}";
        }

        private void LahanTambah_Load(object sender, EventArgs e)
        {
            inputActive.Items.Add("Ya");
            inputActive.Items.Add("Tidak");
            inputActive.SelectedIndex = 0;
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
                bool isAdded = LahanController.AddLahan(
                    new LahanModel
                    {
                        Nama = nama,
                        Luas = luas,
                        Lokasi = lokasi,
                        IsActive = inputActive.SelectedItem?.ToString() == "Ya" ? true : false,
                    }
                );
                if (isAdded)
                {
                    MessageBox.Show(
                        "Lahan berhasil ditambahkan.",
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
                        "Gagal menambahkan lahan. Silakan coba lagi.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
