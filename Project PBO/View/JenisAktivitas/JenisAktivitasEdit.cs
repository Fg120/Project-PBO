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
    public partial class JenisAktivitasEdit : Form
    {
        private int id_jenis_aktivitas;

        public JenisAktivitasEdit(JenisAktivitasModel jenisAktivitas)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(820, 230);

            id_jenis_aktivitas = jenisAktivitas.IdJenisAktivitas;
            inputNama.Text = jenisAktivitas.Nama;
        }

        private void JenisAktivitasEdit_Load(object? sender, EventArgs e) { }

        private void btnBatal_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSimpan_Click(object? sender, EventArgs e)
        {
            string nama = inputNama.Text.Trim();

            if (string.IsNullOrEmpty(nama))
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
                bool isUpdated = JenisAktivitasController.UpdateJenisAktivitas(
                    new JenisAktivitasModel { IdJenisAktivitas = id_jenis_aktivitas, Nama = nama }
                );
                if (isUpdated)
                {
                    MessageBox.Show(
                        "JenisAktivitas berhasil diperbarui.",
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
                        "Gagal memperbarui jenisAktivitas",
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
