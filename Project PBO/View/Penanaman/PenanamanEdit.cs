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
    public partial class PenanamanEdit : Form
    {
        private int id_penanaman;
        private PenanamanModel originalPenanaman;

        public PenanamanEdit(PenanamanModel penanaman)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(818, 600);

            id_penanaman = penanaman.IdPenanaman;
            originalPenanaman = penanaman;
        }

        //private void dateTanggalTanam_ValueChanged(object? sender, EventArgs e)
        //{
        //    dateTanggalPanen.MinDate = dateTanggalTanam.Value;

        //    if (dateTanggalPanen.Checked && dateTanggalPanen.Value < dateTanggalTanam.Value)
        //    {
        //        dateTanggalPanen.Value = dateTanggalTanam.Value.AddMonths(1);
        //    }
        //}

        private void PenanamanEdit_Load(object? sender, EventArgs e)
        {
            var lahanList = LahanController.GetAllAvailableLahan();
            var currentLahan = LahanController.GetLahanById(originalPenanaman.IdLahan);

            if (currentLahan != null && !lahanList.Any(l => l.IdLahan == currentLahan.IdLahan))
            {
                lahanList.Insert(0, currentLahan);
            }

            comboLahan.DisplayMember = "Nama";
            comboLahan.ValueMember = "IdLahan";
            comboLahan.DataSource = lahanList;
            comboLahan.SelectedValue = originalPenanaman.IdLahan;
            comboLahan.DisplayMember = "Nama";
            comboLahan.ValueMember = "IdLahan";
            comboLahan.DataSource = lahanList;
            comboLahan.SelectedValue = originalPenanaman.IdLahan;

            var tanamanList = TanamanController.GetAllTanaman();
            comboTanaman.DisplayMember = "Nama";
            comboTanaman.ValueMember = "IdTanaman";
            comboTanaman.DataSource = tanamanList;
            comboTanaman.SelectedValue = originalPenanaman.IdTanaman;

            dateTanggalTanam.Value = originalPenanaman.TanggalTanam.ToDateTime(TimeOnly.MinValue);

            txtHasilPanen.Text = originalPenanaman.HasilPanen?.ToString() ?? "";
            txtCatatan.Text = originalPenanaman.Catatan ?? "";

            if (originalPenanaman.TanggalPanen.HasValue)
            {
                dateTanggalPanen.Value = originalPenanaman.TanggalPanen.Value.ToDateTime(TimeOnly.MinValue);
            }

            // Atur status default kontrol, sebagian besar dinonaktifkan.
            dateTanggalTanam.Enabled = false;
            dateTanggalPanen.Enabled = false;
            txtHasilPanen.Enabled = false;
            comboLahan.Enabled = false;
            comboTanaman.Enabled = false;
            comboStatus.Enabled = true;
            comboStatus.Items.Clear();

            // Konfigurasi UI berdasarkan status penanaman saat ini.
            switch (originalPenanaman.Status)
            {
                case "Direncanakan":
                    dateTanggalTanam.Enabled = true;
                    comboLahan.Enabled = true;
                    comboTanaman.Enabled = true;
                    dateTanggalTanam.MinDate = DateTime.Today.AddDays(-30);
                    dateTanggalTanam.MaxDate = DateTime.Today.AddDays(30);
                    comboStatus.Items.AddRange(new[] { "Direncanakan", "Aktif", "Dibatalkan" });
                    break;

                case "Aktif":
                    comboStatus.Items.AddRange(new[] { "Aktif", "Dipanen", "Gagal" });
                    break;

                case "Dipanen":
                    txtHasilPanen.Enabled = true;
                    dateTanggalPanen.Enabled = true;
                    dateTanggalPanen.MinDate = originalPenanaman.TanggalTanam.ToDateTime(TimeOnly.MinValue);
                    dateTanggalPanen.MaxDate = DateTime.Today.AddDays(14);
                    comboStatus.Items.AddRange(new[] { "Dipanen", "Selesai" });
                    break;

                case "Selesai":
                case "Dibatalkan":
                case "Gagal":
                    comboStatus.Items.Add(originalPenanaman.Status);
                    comboStatus.Enabled = false;
                    break;
            }

            //comboStatus.Items.Clear();
            //comboStatus.Items.Add("Direncanakan");
            //comboStatus.Items.Add("Aktif");
            //comboStatus.Items.Add("Dipanen");
            //comboStatus.Items.Add("Selesai");
            //comboStatus.Items.Add("Dibatalkan");
            comboStatus.SelectedItem = originalPenanaman.Status;
        }

        private void btnBatal_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSimpan_Click(object? sender, EventArgs e)
        {
            if (
                comboLahan.SelectedItem == null
                || comboTanaman.SelectedItem == null
                || comboStatus.SelectedItem == null
            )
            {
                MessageBox.Show(
                    "Mohon pilih Lahan, Tanaman, dan Status.",
                    "Validasi Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                if (comboLahan.SelectedValue == null || comboTanaman.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Nilai terpilih untuk Lahan atau Tanaman tidak valid.",
                        "Validasi Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
                int idLahan = (int)comboLahan.SelectedValue;
                int idTanaman = (int)comboTanaman.SelectedValue;

                DateOnly tanggalTanam = DateOnly.FromDateTime(dateTanggalTanam.Value);
                DateOnly? tanggalPanen = dateTanggalPanen.Checked
                    ? DateOnly.FromDateTime(dateTanggalPanen.Value)
                    : null;

                decimal? hasilPanen = null;
                if (!string.IsNullOrWhiteSpace(txtHasilPanen.Text))
                {
                    if (decimal.TryParse(txtHasilPanen.Text, out decimal hasil) && hasil > 0)
                    {
                        hasilPanen = hasil;
                    }
                    else
                    {
                        MessageBox.Show(
                            "Hasil panen harus angka positif jika diisi.",
                            "Validasi Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }

                string? catatan = string.IsNullOrWhiteSpace(txtCatatan.Text)
                    ? null
                    : txtCatatan.Text.Trim();
                string statusString =
                    comboStatus.SelectedItem?.ToString() ?? originalPenanaman.Status;

                if (originalPenanaman.Status == "Dipanen")
                {
                    if (!tanggalPanen.HasValue)
                    {
                        MessageBox.Show(
                            "Tanggal panen harus diisi untuk status 'Dipanen' atau 'Selesai'.",
                            "Validasi Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                    if (!hasilPanen.HasValue)
                    {
                        MessageBox.Show(
                            "Hasil panen harus diisi untuk status 'Dipanen' atau 'Selesai'.",
                            "Validasi Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }
                else
                {
                    tanggalPanen = null;
                    hasilPanen = null;
                }


                originalPenanaman.IdLahan = idLahan;
                originalPenanaman.IdTanaman = idTanaman;
                originalPenanaman.TanggalTanam = tanggalTanam;
                originalPenanaman.TanggalPanen = tanggalPanen;
                originalPenanaman.HasilPanen = hasilPanen;
                originalPenanaman.Catatan = catatan;
                originalPenanaman.Status = statusString;
                bool isUpdated = PenanamanController.UpdatePenanaman(originalPenanaman);

                if (isUpdated)
                {
                    MessageBox.Show(
                        "Penanaman berhasil diperbarui.",
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
                        "Gagal memperbarui penanaman",
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
