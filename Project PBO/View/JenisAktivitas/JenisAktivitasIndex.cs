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
    public partial class JenisAktivitasIndex : UserControl
    {
        public JenisAktivitasIndex()
        {
            InitializeComponent();
        }

        private void LoadJenisAktivitasData()
        {
            try
            {
                List<JenisAktivitasModel> jenisAktivitasList =
                    JenisAktivitasController.GetAllJenisAktivitas();
                dgJenisAktivitas.Grid.DataSource = jenisAktivitasList;
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

        private void JenisAktivitasIndex_Load(object sender, EventArgs e)
        {
            dgJenisAktivitas.Grid.AutoGenerateColumns = false;

            var colIdJenisAktivitas = new DataGridViewTextBoxColumn();
            colIdJenisAktivitas.Name = "IdJenisAktivitas";
            colIdJenisAktivitas.HeaderText = "ID Jenis Aktivitas";
            colIdJenisAktivitas.DataPropertyName = "IdJenisAktivitas";
            dgJenisAktivitas.Grid.Columns.Add(colIdJenisAktivitas);

            var colNama = new DataGridViewTextBoxColumn();
            colNama.Name = "Nama";
            colNama.HeaderText = "Nama Jenis Aktivitas";
            colNama.DataPropertyName = "Nama";
            dgJenisAktivitas.Grid.Columns.Add(colNama);

            // Kolom button Edit
            var btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dgJenisAktivitas.Grid.Columns.Add(btnEdit);

            // Kolom button Hapus
            var btnHapus = new DataGridViewButtonColumn();
            btnHapus.Name = "btnHapus";
            btnHapus.HeaderText = "";
            btnHapus.Text = "Hapus";
            btnHapus.UseColumnTextForButtonValue = true;
            dgJenisAktivitas.Grid.Columns.Add(btnHapus);

            dgJenisAktivitas.Grid.CellContentClick += dgJenisAktivitas_CellContentClick;

            LoadJenisAktivitasData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (var tambahJenisAktivitasForm = new JenisAktivitasTambah())
            {
                ViewHelper.ShowDialogWithFormOverlay(this, tambahJenisAktivitasForm);
                if (tambahJenisAktivitasForm.DialogResult == DialogResult.OK)
                {
                    LoadJenisAktivitasData();
                }
            }
        }

        private void dgJenisAktivitas_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgJenisAktivitas.Grid.Rows.Count) // Periksa batas baris
                return;

            DataGridViewColumn? columnEdit = dgJenisAktivitas.Grid.Columns["btnEdit"];
            DataGridViewColumn? columnHapus = dgJenisAktivitas.Grid.Columns["btnHapus"];

            if (columnEdit != null && e.ColumnIndex == columnEdit.Index) // Periksa apakah kolom Edit ada dan diklik
            {
                if (
                    dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"] != null
                    && dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"].Value
                        != null
                )
                {
                    int idJenisAktivitas = Convert.ToInt32(
                        dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"].Value
                    );
                    JenisAktivitasModel? jenisAktivitas =
                        JenisAktivitasController.GetJenisAktivitasById(idJenisAktivitas);
                    if (jenisAktivitas != null)
                    {
                        using (var editJenisAktivitasForm = new JenisAktivitasEdit(jenisAktivitas))
                        {
                            ViewHelper.ShowDialogWithFormOverlay(this, editJenisAktivitasForm);
                            if (editJenisAktivitasForm.DialogResult == DialogResult.OK)
                            {
                                LoadJenisAktivitasData();
                            }
                        }
                    }
                }
            }
            else if (columnHapus != null && e.ColumnIndex == columnHapus.Index) // Periksa apakah kolom Hapus ada dan diklik
            {
                if (
                    dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"] != null
                    && dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"].Value
                        != null
                )
                {
                    int idJenisAktivitas = Convert.ToInt32(
                        dgJenisAktivitas.Grid.Rows[e.RowIndex].Cells["IdJenisAktivitas"].Value
                    );
                    JenisAktivitasModel? jenisAktivitas =
                        JenisAktivitasController.GetJenisAktivitasById(idJenisAktivitas);
                    if (
                        jenisAktivitas != null
                        && MessageBox.Show(
                            $"Apakah yakin ingin menghapus jenis aktivitas '{jenisAktivitas.Nama}'?",
                            "Konfirmasi Hapus",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        ) == DialogResult.Yes
                    )
                    {
                        if (JenisAktivitasController.DeleteJenisAktivitas(idJenisAktivitas))
                        {
                            LoadJenisAktivitasData();
                        }
                        else
                        {
                            MessageBox.Show(
                                "Gagal menghapus jenis aktivitas.",
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
