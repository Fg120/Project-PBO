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
using Project_PBO.Helpers;
using Project_PBO.Models;

namespace Project_PBO.View
{
    public partial class PenanamanIndex : UserControl
    {
        public PenanamanIndex()
        {
            InitializeComponent();
        }

        private void LoadPenanamanData()
        {
            try
            {
                List<PenanamanModel> penanamanList = PenanamanController.GetAllPenanaman();
                dgPenanaman.Grid.DataSource = penanamanList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading data: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void PenanamanIndex_Load(object sender, EventArgs e)
        {
            dgPenanaman.Grid.AutoGenerateColumns = false;
            dgPenanaman.Grid.Columns.Clear();

            var colIdPenanaman = new DataGridViewTextBoxColumn();
            colIdPenanaman.Name = "IdPenanaman";
            colIdPenanaman.HeaderText = "ID Penanaman";
            colIdPenanaman.DataPropertyName = "IdPenanaman";
            dgPenanaman.Grid.Columns.Add(colIdPenanaman);

            var colNamaTanaman = new DataGridViewTextBoxColumn();
            colNamaTanaman.Name = "NamaTanaman";
            colNamaTanaman.HeaderText = "Nama Tanaman";
            colNamaTanaman.DataPropertyName = "NamaTanaman";
            dgPenanaman.Grid.Columns.Add(colNamaTanaman);

            var colNamaLahan = new DataGridViewTextBoxColumn();
            colNamaLahan.Name = "NamaLahan";
            colNamaLahan.HeaderText = "Nama Lahan";
            colNamaLahan.DataPropertyName = "NamaLahan";
            dgPenanaman.Grid.Columns.Add(colNamaLahan);

            var colTanggalTanam = new DataGridViewTextBoxColumn();
            colTanggalTanam.Name = "TanggalTanam";
            colTanggalTanam.HeaderText = "Tanggal Tanam";
            colTanggalTanam.DataPropertyName = "TanggalTanam";
            colTanggalTanam.DefaultCellStyle.Format = "dd/MM/yyyy";
            dgPenanaman.Grid.Columns.Add(colTanggalTanam);

            var colHasilPanen = new DataGridViewTextBoxColumn();
            colHasilPanen.Name = "HasilPanen";
            colHasilPanen.HeaderText = "Hasil Panen";
            colHasilPanen.DataPropertyName = "HasilPanen";
            dgPenanaman.Grid.Columns.Add(colHasilPanen);

            var colStatus = new DataGridViewTextBoxColumn();
            colStatus.Name = "Status";
            colStatus.HeaderText = "Status";
            colStatus.DataPropertyName = "Status";
            dgPenanaman.Grid.Columns.Add(colStatus);

            var btnDetail = new DataGridViewButtonColumn();
            btnDetail.Name = "btnDetail";
            btnDetail.HeaderText = "";
            btnDetail.Text = "Detail";
            btnDetail.UseColumnTextForButtonValue = true;
            dgPenanaman.Grid.Columns.Add(btnDetail);

            dgPenanaman.Grid.CellContentClick += dgPenanaman_CellContentClick;

            LoadPenanamanData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (var tambahPenanamanForm = new PenanamanTambah())
            {
                ViewHelper.ShowDialogWithFormOverlay(this, tambahPenanamanForm);

                if (tambahPenanamanForm.DialogResult == DialogResult.OK)
                {
                    LoadPenanamanData();
                }
            }
        }

        private void dgPenanaman_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgPenanaman.Grid.Rows.Count)
                return;

            DataGridViewColumn? columnDetail = dgPenanaman.Grid.Columns["btnDetail"];

            if (columnDetail != null && e.ColumnIndex == columnDetail.Index)
            {
                if (
                    dgPenanaman.Grid.Rows[e.RowIndex].Cells["IdPenanaman"] != null
                    && dgPenanaman.Grid.Rows[e.RowIndex].Cells["IdPenanaman"].Value != null
                )
                {
                    int idPenanaman = Convert.ToInt32(
                        dgPenanaman.Grid.Rows[e.RowIndex].Cells["IdPenanaman"].Value
                    );
                    PenanamanModel? penanaman = PenanamanController.GetPenanamanById(idPenanaman);
                    if (penanaman is not null)
                    {
                        var parentControl = this.Parent;
                        if (parentControl != null)
                        {
                            PenanamanDetail detailView = new PenanamanDetail(penanaman);
                            detailView.Dock = DockStyle.Fill;
                            detailView.KembaliClicked += (s, ev) =>
                            {
                                parentControl.Controls.Remove(detailView);
                                detailView.Dispose();
                                this.Show();
                                this.BringToFront();
                            };
                            parentControl.Controls.Add(detailView);
                            detailView.BringToFront();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Tidak dapat menampilkan detail, kontrol induk tidak ditemukan.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
        }
    }
}
