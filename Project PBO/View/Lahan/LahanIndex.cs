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
    public partial class LahanIndex : UserControl
    {
        public LahanIndex()
        {
            InitializeComponent();
            ShowUserInfo();
        }

        private void ShowUserInfo()
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.Text = $"Lahan Management - User: {UserSession.Username}";
            }
        }

        private void LoadLahanData()
        {
            try
            {
                List<LahanModel> lahanList = LahanController.GetAllLahan();
                dgLahan.Grid.DataSource = lahanList;
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

        private void LahanIndex_Load(object sender, EventArgs e)
        {

            dgLahan.Grid.AutoGenerateColumns = false;
            dgLahan.Grid.Columns.Clear();

            var colIdLahan = new DataGridViewTextBoxColumn();
            colIdLahan.Name = "IdLahan";
            colIdLahan.HeaderText = "ID Lahan";
            colIdLahan.DataPropertyName = "IdLahan";
            dgLahan.Grid.Columns.Add(colIdLahan);

            var colNama = new DataGridViewTextBoxColumn();
            colNama.Name = "Nama";
            colNama.HeaderText = "Nama Lahan";
            colNama.DataPropertyName = "Nama";
            dgLahan.Grid.Columns.Add(colNama);

            var colLuas = new DataGridViewTextBoxColumn();
            colLuas.Name = "Luas";
            colLuas.HeaderText = "Luas (m²)";
            colLuas.DataPropertyName = "Luas";
            dgLahan.Grid.Columns.Add(colLuas);

            var colLokasi = new DataGridViewTextBoxColumn();
            colLokasi.Name = "Lokasi";
            colLokasi.HeaderText = "Lokasi";
            colLokasi.DataPropertyName = "Lokasi";
            dgLahan.Grid.Columns.Add(colLokasi);

            var colIsActive = new DataGridViewTextBoxColumn();
            colIsActive.Name = "IsActive";
            colIsActive.HeaderText = "Aktif";
            colIsActive.DataPropertyName = "IsActive";
            dgLahan.Grid.Columns.Add(colIsActive);

            // Kolom button Edit
            var btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dgLahan.Grid.Columns.Add(btnEdit);

            // Kolom button Hapus
            var btnHapus = new DataGridViewButtonColumn();
            btnHapus.Name = "btnHapus";
            btnHapus.HeaderText = "";
            btnHapus.Text = "Hapus";
            btnHapus.UseColumnTextForButtonValue = true;
            dgLahan.Grid.Columns.Add(btnHapus);

            dgLahan.Grid.CellContentClick += dgLahan_CellContentClick;

            LoadLahanData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (var tambahLahanForm = new LahanTambah())
            {
                ViewHelper.ShowDialogWithFormOverlay(this, tambahLahanForm);
                if (tambahLahanForm.DialogResult == DialogResult.OK)
                {
                    LoadLahanData();
                }
            }
        }

        private void dgLahan_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgLahan.Grid.Rows.Count)
                return;

            DataGridViewColumn? columnEdit = dgLahan.Grid.Columns["btnEdit"];
            DataGridViewColumn? columnHapus = dgLahan.Grid.Columns["btnHapus"];

            if (columnEdit != null && e.ColumnIndex == columnEdit.Index)
            {
                if (
                    dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"] != null
                    && dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"].Value != null
                )
                {
                    int idLahan = Convert.ToInt32(
                        dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"].Value
                    );
                    LahanModel? lahan = LahanController.GetLahanById(idLahan);
                    if (lahan != null)
                    {
                        using (var editLahanForm = new LahanEdit(lahan))
                        {
                            ViewHelper.ShowDialogWithFormOverlay(this, editLahanForm);
                            if (editLahanForm.DialogResult == DialogResult.OK)
                            {
                                LoadLahanData();
                            }
                        }
                    }
                }
            }
            else if (columnHapus != null && e.ColumnIndex == columnHapus.Index)
            {
                if (
                    dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"] != null
                    && dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"].Value != null
                )
                {
                    int idLahan = Convert.ToInt32(
                        dgLahan.Grid.Rows[e.RowIndex].Cells["IdLahan"].Value
                    );
                    LahanModel? lahan = LahanController.GetLahanById(idLahan);
                    if (
                        lahan != null
                        && MessageBox.Show(
                            $"Apakah yakin ingin menghapus lahan '{lahan.Nama}'?",
                            "Konfirmasi Hapus",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        ) == DialogResult.Yes
                    )
                    {
                        if (LahanController.DeleteLahan(idLahan))
                        {
                            LoadLahanData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Gagal menghapus lahan.",
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
