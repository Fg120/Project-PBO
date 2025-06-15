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
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(818, 600);

            id_penanaman = penanaman.IdPenanaman;
            originalPenanaman = penanaman;
        }

        private void dateTanggalTanam_ValueChanged(object? sender, EventArgs e)
        {
            dateTanggalPanen.MinDate = dateTanggalTanam.Value;

            if (dateTanggalPanen.Checked && dateTanggalPanen.Value < dateTanggalTanam.Value)
            {
                dateTanggalPanen.Value = dateTanggalTanam.Value.AddMonths(1);
            }
        }

        private void PenanamanEdit_Load(object? sender, EventArgs e)
        {
            var lahanList = LahanController.GetAllLahan();
            comboLahan.DisplayMember = "Nama";
            comboLahan.ValueMember = "IdLahan";
            comboLahan.DataSource = lahanList;
            comboLahan.SelectedValue = originalPenanaman.IdLahan;

            var tanamanList = TanamanController.GetAllTanaman();
            comboTanaman.DisplayMember = "Nama";
            comboTanaman.ValueMember = "IdTanaman";
            comboTanaman.DataSource = tanamanList;
            comboTanaman.SelectedValue = originalPenanaman.IdTanaman;

            dateTanggalTanam.MinDate = new DateTime(1753, 1, 1);
            dateTanggalTanam.MaxDate = new DateTime(9998, 12, 31);
            dateTanggalTanam.Value = originalPenanaman.TanggalTanam.ToDateTime(TimeOnly.MinValue);

            dateTanggalPanen.MinDate = dateTanggalTanam.Value;

            if (originalPenanaman.TanggalPanen.HasValue)
            {
                dateTanggalPanen.Checked = true;
                if (
                    originalPenanaman.TanggalPanen.Value.ToDateTime(TimeOnly.MinValue)
                    < dateTanggalTanam.Value
                )
                {
                    dateTanggalPanen.Value = dateTanggalTanam.Value.AddMonths(1);
                }
                else
                {
                    dateTanggalPanen.Value = originalPenanaman.TanggalPanen.Value.ToDateTime(
                        TimeOnly.MinValue
                    );
                }
            }
            else
            {
                dateTanggalPanen.Checked = false;
                dateTanggalPanen.Value = dateTanggalTanam.Value.AddMonths(1);
            }

            txtHasilPanen.Text = originalPenanaman.HasilPanen?.ToString() ?? "";
            txtCatatan.Text = originalPenanaman.Catatan ?? "";
            comboStatus.Items.Clear();
            comboStatus.Items.Add("Direncanakan");
            comboStatus.Items.Add("Aktif");
            comboStatus.Items.Add("Dipanen");
            comboStatus.Items.Add("Selesai");
            comboStatus.Items.Add("Dibatalkan");
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
