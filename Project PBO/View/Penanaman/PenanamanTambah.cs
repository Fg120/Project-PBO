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
    public partial class PenanamanTambah : Form
    {
        public PenanamanTambah()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(255, 242, 225);
            this.TransparencyKey = Color.FromArgb(255, 242, 225);
            this.Size = new Size(818, 600);
        }

        private void PenanamanTambah_Load(object? sender, EventArgs e)
        {
            var lahanList = LahanController.GetAllAvailableLahan();
            comboLahan.DisplayMember = "Nama";
            comboLahan.ValueMember = "IdLahan";
            comboLahan.DataSource = lahanList;
            if (comboLahan.Items.Count > 0)
                comboLahan.SelectedIndex = 0;

            var tanamanList = TanamanController.GetAllActiveTanaman();
            comboTanaman.DisplayMember = "Nama";
            comboTanaman.ValueMember = "IdTanaman";
            comboTanaman.DataSource = tanamanList;
            if (comboTanaman.Items.Count > 0)
                comboTanaman.SelectedIndex = 0;

            dateTanggalTanam.Enabled = false;
            dateTanggalTanam.Value = DateTime.Today;

            comboStatus.Items.Clear();
            comboStatus.Items.Add("Direncanakan");
            comboStatus.Items.Add("Aktif");
            //comboStatus.SelectedIndex = 0;
        }

        private void comboStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboStatus.SelectedItem?.ToString() == "Direncanakan")
            {
                dateTanggalTanam.Enabled = true;
                dateTanggalTanam.MinDate = DateTime.Today;
                dateTanggalTanam.MaxDate = DateTime.Today.AddDays(14);
            }
            else if (comboStatus.SelectedItem?.ToString() == "Aktif")
            {
                dateTanggalTanam.Enabled |= true;
                dateTanggalTanam.MaxDate = DateTime.Today;
                dateTanggalTanam.MinDate = DateTime.Today.AddDays(-14);
            }
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

                string? catatan = string.IsNullOrWhiteSpace(txtCatatan.Text)
                    ? null
                    : txtCatatan.Text.Trim();

                string status = comboStatus.SelectedItem?.ToString() ?? "Direncanakan";

                bool isAdded = PenanamanController.AddPenanaman(
                    new PenanamanModel
                    {
                        IdLahan = idLahan,
                        IdTanaman = idTanaman,
                        TanggalTanam = tanggalTanam,
                        TanggalPanen = null,
                        HasilPanen = null,
                        Catatan = catatan ?? string.Empty,
                        Status = status,
                    }
                );
                if (isAdded)
                {
                    var penanaman = PenanamanController.GetAllPenanaman().LastOrDefault();
                    if (penanaman != null)
                    {
                        penanaman.Status = status;
                        PenanamanController.UpdatePenanaman(penanaman);
                    }
                }

                if (isAdded)
                {
                    MessageBox.Show(
                        "Penanaman berhasil ditambahkan.",
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
                        "Gagal menambahkan penanaman. Silakan coba lagi.",
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
