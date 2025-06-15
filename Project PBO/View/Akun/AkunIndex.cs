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
    public partial class AkunIndex : UserControl
    {
        public AkunIndex()
        {
            InitializeComponent();
            CheckAccess();
        }

        private void CheckAccess()
        {
            // Check if current user has permission to access this page
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show(
                    "You must login first!",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            if (!UserSession.IsAdmin())
            {
                MessageBox.Show(
                    "Access denied! Only admin can access account management.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                this.Enabled = false;
                return;
            }
        }

        private void LoadAkunData()
        {
            try
            {
                List<AkunModel> akunList = AkunController.GetAllAkun();
                dgAkun.Grid.DataSource = akunList;
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

        private void AkunIndex_Load(object sender, EventArgs e)
        {
            dgAkun.Grid.AutoGenerateColumns = false;

            // Tambah kolom dengan DataPropertyName
            var colIdAkun = new DataGridViewTextBoxColumn();
            colIdAkun.Name = "IdAkun";
            colIdAkun.HeaderText = "ID Akun";
            colIdAkun.DataPropertyName = "IdAkun";
            dgAkun.Grid.Columns.Add(colIdAkun);

            var colUsername = new DataGridViewTextBoxColumn();
            colUsername.Name = "Username";
            colUsername.HeaderText = "Username";
            colUsername.DataPropertyName = "Username";
            dgAkun.Grid.Columns.Add(colUsername);

            var colRole = new DataGridViewTextBoxColumn();
            colRole.Name = "Role";
            colRole.HeaderText = "Role";
            colRole.DataPropertyName = "Role";
            dgAkun.Grid.Columns.Add(colRole);

            var colIsActive = new DataGridViewTextBoxColumn();
            colIsActive.Name = "IsActive";
            colIsActive.HeaderText = "Aktif";
            colIsActive.DataPropertyName = "IsActive";
            dgAkun.Grid.Columns.Add(colIsActive);

            // Kolom button Edit
            var btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dgAkun.Grid.Columns.Add(btnEdit);

            // Kolom button Hapus
            var btnHapus = new DataGridViewButtonColumn();
            btnHapus.Name = "btnHapus";
            btnHapus.HeaderText = "";
            btnHapus.Text = "Hapus";
            btnHapus.UseColumnTextForButtonValue = true;
            dgAkun.Grid.Columns.Add(btnHapus);

            // Hubungkan event
            dgAkun.Grid.CellContentClick += dgAkun_CellContentClick;

            LoadAkunData();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (var tambahAkunForm = new AkunTambah())
            {
                ViewHelper.ShowDialogWithFormOverlay(this, tambahAkunForm);
                if (tambahAkunForm.DialogResult == DialogResult.OK)
                {
                    LoadAkunData();
                }
            }
        }

        private void dgAkun_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgAkun.Grid.Rows.Count)
                return;

            if (e.ColumnIndex == dgAkun.Grid.Columns["btnEdit"].Index)
            {
                int idAkun = Convert.ToInt32(dgAkun.Grid.Rows[e.RowIndex].Cells["IdAkun"].Value);
                AkunModel? akun = AkunController.GetAkunById(idAkun);
                if (akun != null)
                {
                    using (var editAkunForm = new AkunEdit(akun))
                    {
                        ViewHelper.ShowDialogWithFormOverlay(this, editAkunForm);
                        if (editAkunForm.DialogResult == DialogResult.OK)
                        {
                            LoadAkunData();
                        }
                    }
                }
            }
            else if (e.ColumnIndex == dgAkun.Grid.Columns["btnHapus"].Index)
            {
                int idAkun = Convert.ToInt32(dgAkun.Grid.Rows[e.RowIndex].Cells["IdAkun"].Value);
                AkunModel? akun = AkunController.GetAkunById(idAkun);
                if (
                    akun != null
                    && MessageBox.Show(
                        $"Apakah yakin ingin menghapus akun '{akun.Username}'?",
                        "Konfirmasi Hapus",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    ) == DialogResult.Yes
                )
                {
                    if (AkunController.DeleteAkun(idAkun))
                    {
                        LoadAkunData();
                    }
                    else
                    {
                        MessageBox.Show(
                            "Gagal menghapus akun.",
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
