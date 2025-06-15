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

namespace Project_PBO.View
{
    public partial class DashboardAdmin : UserControl
    {
        public DashboardAdmin()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            dgTopPenanaman.Grid.AutoGenerateColumns = false;
            dgTopPenanaman.Grid.Columns.Clear();

            var colIdPenanaman = new DataGridViewTextBoxColumn();
            colIdPenanaman.Name = "IdPenanaman";
            colIdPenanaman.HeaderText = "ID Penanaman";
            colIdPenanaman.DataPropertyName = "IdPenanaman";
            dgTopPenanaman.Grid.Columns.Add(colIdPenanaman);

            var colNamaTanaman = new DataGridViewTextBoxColumn();
            colNamaTanaman.Name = "NamaTanaman";
            colNamaTanaman.HeaderText = "Nama Tanaman";
            colNamaTanaman.DataPropertyName = "NamaTanaman";
            dgTopPenanaman.Grid.Columns.Add(colNamaTanaman);

            var colNamaLahan = new DataGridViewTextBoxColumn();
            colNamaLahan.Name = "NamaLahan";
            colNamaLahan.HeaderText = "Nama Lahan";
            colNamaLahan.DataPropertyName = "NamaLahan";
            dgTopPenanaman.Grid.Columns.Add(colNamaLahan);

            var colTanggalTanam = new DataGridViewTextBoxColumn();
            colTanggalTanam.Name = "TanggalTanam";
            colTanggalTanam.HeaderText = "Tanggal Tanam";
            colTanggalTanam.DataPropertyName = "TanggalTanam";
            colTanggalTanam.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgTopPenanaman.Grid.Columns.Add(colTanggalTanam);

            var colTanggalPanen = new DataGridViewTextBoxColumn();
            colTanggalPanen.Name = "TanggalPanen";
            colTanggalPanen.HeaderText = "Tanggal Panen";
            colTanggalPanen.DataPropertyName = "TanggalPanen";
            colTanggalPanen.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgTopPenanaman.Grid.Columns.Add(colTanggalPanen);

            var colHasilPanen = new DataGridViewTextBoxColumn();
            colHasilPanen.Name = "HasilPanen";
            colHasilPanen.HeaderText = "Hasil Panen";
            colHasilPanen.DataPropertyName = "HasilPanen";
            dgTopPenanaman.Grid.Columns.Add(colHasilPanen);

            LoadDataCounts();
            LoadTopPenanaman();
        }

        private void LoadDataCounts()
        {
            try
            {
                lblCountLahan.Text = LahanController.CountAllLahan().ToString();
                lblCountTanaman.Text = TanamanController.CountAllTanaman().ToString();
                lblCountJenisAktivitas.Text = JenisAktivitasController.CountAllJenisAktivitas().ToString();
                lblCountPenanaman.Text = PenanamanController.CountAllPenanaman().ToString();
                lblCountAkun.Text = AkunController.CountAllAkun().ToString();
                lblCountAktivitas.Text = AktivitasController.CountAllAktivitas().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error memuat data dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTopPenanaman()
        {
            try
            {
                var topPenanaman = PenanamanController.GetTopFive();
                dgTopPenanaman.Grid.DataSource = topPenanaman;

                int totalHeight = dgTopPenanaman.Grid.ColumnHeadersHeight;
                foreach (DataGridViewRow row in dgTopPenanaman.Grid.Rows)
                {
                    totalHeight += row.Height;
                }

                if (dgTopPenanaman.Grid.Rows.Count > 0)
                {
                    dgTopPenanaman.Height = totalHeight + dgTopPenanaman.Grid.Margin.Vertical;
                    dgTopPenanaman.Grid.ScrollBars = ScrollBars.None;
                }
                else
                {
                    dgTopPenanaman.Height = dgTopPenanaman.Grid.ColumnHeadersHeight + dgTopPenanaman.Grid.Margin.Vertical + 5;
                    dgTopPenanaman.Grid.ScrollBars = ScrollBars.None;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error memuat data top penanaman: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
