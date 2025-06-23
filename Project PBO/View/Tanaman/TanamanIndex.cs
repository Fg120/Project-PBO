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
    public partial class TanamanIndex : UserControl
    {
        public TanamanIndex()
        {
            InitializeComponent();
        }

        private void LoadTanamanData()
        {
            try
            {
                List<TanamanModel> tanamanList = TanamanController.GetAllTanaman();
                dgTanaman.Grid.DataSource = tanamanList;
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

        private void TanamanIndex_Load(object? sender, EventArgs e) // Changed sender to nullable
        {
            dgTanaman.Grid.AutoGenerateColumns = false;
            dgTanaman.Grid.Columns.Clear();

            var colIdTanaman = new DataGridViewTextBoxColumn();
            colIdTanaman.Name = "IdTanaman";
            colIdTanaman.HeaderText = "ID Tanaman";
            colIdTanaman.DataPropertyName = "IdTanaman";
            dgTanaman.Grid.Columns.Add(colIdTanaman);

            var colNama = new DataGridViewTextBoxColumn();
            colNama.Name = "Nama";
            colNama.HeaderText = "Nama Tanaman";
            colNama.DataPropertyName = "Nama";
            dgTanaman.Grid.Columns.Add(colNama);

            var colMasaTanam = new DataGridViewTextBoxColumn();
            colMasaTanam.Name = "MasaTanam";
            colMasaTanam.HeaderText = "Masa Tanam (Hari)";
            colMasaTanam.DataPropertyName = "MasaTanam";
            dgTanaman.Grid.Columns.Add(colMasaTanam);

            var colIsActive = new DataGridViewTextBoxColumn();
            colIsActive.Name = "IsActive";
            colIsActive.HeaderText = "Aktif";
            colIsActive.DataPropertyName = "IsActive";
            dgTanaman.Grid.Columns.Add(colIsActive);

            var btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dgTanaman.Grid.Columns.Add(btnEdit);

            var btnHapus = new DataGridViewButtonColumn();
            btnHapus.Name = "btnHapus";
            btnHapus.HeaderText = "";
            btnHapus.Text = "Hapus";
            btnHapus.UseColumnTextForButtonValue = true;
            dgTanaman.Grid.Columns.Add(btnHapus);

            dgTanaman.Grid.CellContentClick += dgTanaman_CellContentClick;
            btnTambah.Click += btnTambah_Click;

            LoadTanamanData();
        }

        private void btnTambah_Click(object? sender, EventArgs e)
        {
            using (var tambahTanamanForm = new TanamanTambah())
            {
                ViewHelper.ShowDialogWithFormOverlay(this, tambahTanamanForm);
                if (tambahTanamanForm.DialogResult == DialogResult.OK)
                {
                    LoadTanamanData();
                }
            }
        }

        private void dgTanaman_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgTanaman.Grid.Rows.Count)
                return;

            DataGridViewColumn? columnEdit = dgTanaman.Grid.Columns["btnEdit"];
            DataGridViewColumn? columnHapus = dgTanaman.Grid.Columns["btnHapus"];

            if (columnEdit != null && e.ColumnIndex == columnEdit.Index)
            {
                if (
                    dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"] != null
                    && dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"].Value != null
                )
                {
                    int idTanaman = Convert.ToInt32(
                        dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"].Value
                    );
                    TanamanModel? tanaman = TanamanController.GetTanamanById(idTanaman);
                    if (tanaman != null)
                    {
                        using (var editTanamanForm = new TanamanEdit(tanaman))
                        {
                            ViewHelper.ShowDialogWithFormOverlay(this, editTanamanForm);
                            if (editTanamanForm.DialogResult == DialogResult.OK)
                            {
                                LoadTanamanData();
                            }
                        }
                    }
                }
            }
            else if (columnHapus != null && e.ColumnIndex == columnHapus.Index)
            {
                if (
                    dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"] != null
                    && dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"].Value != null
                )
                {
                    int idTanaman = Convert.ToInt32(
                        dgTanaman.Grid.Rows[e.RowIndex].Cells["IdTanaman"].Value
                    );
                    TanamanModel? tanaman = TanamanController.GetTanamanById(idTanaman);
                    if (
                        tanaman != null
                        && MessageBox.Show(
                            $"Apakah yakin ingin menghapus tanaman '{tanaman.Nama}'?",
                            "Konfirmasi Hapus",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        ) == DialogResult.Yes
                    )
                    {
                        if (TanamanController.DeleteTanaman(idTanaman))
                        {
                            LoadTanamanData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Gagal menghapus tanaman.",
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
