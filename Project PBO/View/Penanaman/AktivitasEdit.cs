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

namespace Project_PBO.View.Penanaman
{
    public partial class AktivitasEdit : Form
    {
        private int id_aktivitas;

        public AktivitasEdit(int id_aktivitas)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(818, 600);
            this.id_aktivitas = id_aktivitas;
        }

        private void AktivitasEdit_Load(object sender, EventArgs e)
        {
            var JenisAktivitasList = JenisAktivitasController.GetAllJenisAktivitas();
            inputJenisAktivitas.DisplayMember = "Nama";
            inputJenisAktivitas.ValueMember = "IdJenisAktivitas";
            inputJenisAktivitas.DataSource = JenisAktivitasList;
            if (inputJenisAktivitas.Items.Count > 0)
                inputJenisAktivitas.SelectedIndex = 0;
            var Aktivitas = AktivitasController.GetAktivitasById(id_aktivitas);
            if (Aktivitas != null)
            {
                if (JenisAktivitasList.Any(ja => ja.IdJenisAktivitas == Aktivitas.IdJenisAktivitas))
                {
                    inputJenisAktivitas.SelectedValue = Aktivitas.IdJenisAktivitas;
                }
                inputCatatan.Text = Aktivitas.Catatan ?? "";
                dateWaktu.Value = Aktivitas.Waktu;
            }
            else
            {
                MessageBox.Show(
                    "Aktivitas tidak ditemukan.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.Close();
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (inputJenisAktivitas.SelectedItem == null)
            {
                MessageBox.Show(
                    "Mohon pilih Jenis Aktivitas.",
                    "Validasi Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                if (inputJenisAktivitas.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Nilai terpilih untuk Lahan atau Tanaman tidak valid.",
                        "Validasi Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                int idJenisAktivitas = (int)inputJenisAktivitas.SelectedValue;

                DateTime Waktu = dateWaktu.Value;

                string? catatan = string.IsNullOrWhiteSpace(inputCatatan.Text)
                    ? null
                    : inputCatatan.Text.Trim();

                bool isUpdated = AktivitasController.UpdateAktivitas(
                    new AktivitasModel
                    {
                        IdAktivitas = id_aktivitas,
                        IdJenisAktivitas = idJenisAktivitas,
                        Waktu = Waktu,
                        Catatan = catatan ?? string.Empty,
                    }
                );
                if (isUpdated)
                {
                    MessageBox.Show(
                        "Aktivitas berhasil diperbarui.",
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
                        "Gagal memperbarui Aktivitas. Silakan coba lagi.",
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

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
