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
using Project_PBO.View.Penanaman;

namespace Project_PBO.View
{
    public partial class PenanamanDetail : UserControl
    {
        private int id_penanaman;
        private string statusPenanaman;
        public event EventHandler? KembaliClicked;
        private string AkunRole = UserSession.Role;

        private void PenanamanDetail_Load(object sender, EventArgs e)
        {
            dgAktivitas.Grid.AutoGenerateColumns = false;
            dgAktivitas.Grid.Columns.Clear();

            var colIdAktivitas = new DataGridViewTextBoxColumn();
            colIdAktivitas.Name = "IdAktivitas";
            colIdAktivitas.HeaderText = "ID Aktivitas";
            colIdAktivitas.DataPropertyName = "IdAktivitas";
            dgAktivitas.Grid.Columns.Add(colIdAktivitas);

            var colIdAkun = new DataGridViewTextBoxColumn();
            colIdAkun.Name = "IdAkun";
            colIdAkun.HeaderText = "ID Akun";
            colIdAkun.DataPropertyName = "IdAkun";
            colIdAkun.Visible = false; // Sembunyikan kolom ID Akun
            dgAktivitas.Grid.Columns.Add(colIdAkun);

            var colNamaJenisAktivitas = new DataGridViewTextBoxColumn();
            colNamaJenisAktivitas.Name = "NamaJenisAktivitas";
            colNamaJenisAktivitas.HeaderText = "Nama Jenis Aktivitas";
            colNamaJenisAktivitas.DataPropertyName = "NamaJenisAktivitas";
            dgAktivitas.Grid.Columns.Add(colNamaJenisAktivitas);

            var colNamaAkun = new DataGridViewTextBoxColumn();
            colNamaAkun.Name = "NamaAkun";
            colNamaAkun.HeaderText = "Nama Akun";
            colNamaAkun.DataPropertyName = "NamaAkun";
            dgAktivitas.Grid.Columns.Add(colNamaAkun);

            var colWaktu = new DataGridViewTextBoxColumn();
            colWaktu.Name = "Waktu";
            colWaktu.HeaderText = "Waktu";
            colWaktu.DataPropertyName = "Waktu";
            colWaktu.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgAktivitas.Grid.Columns.Add(colWaktu);

            var colCatatan = new DataGridViewTextBoxColumn();
            colCatatan.Name = "Catatan";
            colCatatan.HeaderText = "Catatan";
            colCatatan.DataPropertyName = "Catatan";
            dgAktivitas.Grid.Columns.Add(colCatatan);

            // Kolom button Edit
            var btnEdit = new DataGridViewButtonColumn();
            btnEdit.Name = "btnEdit";
            btnEdit.HeaderText = "";
            btnEdit.Text = "Edit";
            btnEdit.UseColumnTextForButtonValue = true;
            dgAktivitas.Grid.Columns.Add(btnEdit);

            // Kolom button Hapus
            if (AkunRole == "Admin")
            {
                var btnHapus = new DataGridViewButtonColumn();
                btnHapus.Name = "btnHapus";
                btnHapus.HeaderText = "";
                btnHapus.Text = "Hapus";
                btnHapus.UseColumnTextForButtonValue = true;
                dgAktivitas.Grid.Columns.Add(btnHapus);
            }
            else
            {
                btnEditPenanaman.Visible = false;
            }

            // Hubungkan event
            dgAktivitas.Grid.CellContentClick += dgAktivitas_CellContentClick;
            LoadAktivitasData();
        }

        public PenanamanDetail(PenanamanModel penanaman)
        {
            InitializeComponent();
            id_penanaman = penanaman.IdPenanaman;
            LoadDetailPenanaman(id_penanaman);

            btnKembali.Click += (s, e) => KembaliClicked?.Invoke(this, EventArgs.Empty);
        }

        public void btnEditPenanaman_Click(object sender, EventArgs e)
        {
            PenanamanModel? penanaman = PenanamanController.GetPenanamanById(id_penanaman);
            if (penanaman != null)
            {
                using (var editPenanamanForm = new PenanamanEdit(penanaman))
                {
                    ViewHelper.ShowDialogWithFormOverlay(this, editPenanamanForm);

                    if (editPenanamanForm.DialogResult == DialogResult.OK)
                    {
                        LoadDetailPenanaman(id_penanaman);
                    }
                }
            }
        }

        public void LoadBtn(string Status)
        {
            if (Status == "Aktif" || Status == "Dipanen")
            {
                btnTambahAktivitas.Enabled = true;
            }
            else
            {
                btnTambahAktivitas.Enabled = false;
                btnTambahAktivitas.Click += null;
                btnTambahAktivitas.BackColor = Color.FromArgb(64, 64, 64);
                btnTambahAktivitas.HoverColor = Color.FromArgb(64, 64, 64);
            }
        }

        public void LoadDetailPenanaman(int idPenanaman)
        {
            PenanamanModel? penanaman = PenanamanController.GetPenanamanById(idPenanaman);
            if (penanaman != null)
            {
                dtlNamaTanaman.Text = penanaman.NamaTanaman ?? "Tidak diketahui";
                dtlNamaLahan.Text = penanaman.NamaLahan ?? "Tidak diketahui";
                dtlTanggalTanam.Text = penanaman.TanggalTanam.ToString("dd/MM/yyyy");
                dtlTanggalPanen.Text = penanaman.TanggalPanen.HasValue
                    ? penanaman.TanggalPanen.Value.ToString("dd/MM/yyyy")
                    : "Belum dipanen";
                dtlHasilPanen.Text = penanaman.HasilPanen.HasValue
                    ? penanaman.HasilPanen.Value.ToString("N2")
                    : "Belum ada hasil";
                dtlCatatan.Text = string.IsNullOrEmpty(penanaman.Catatan)
                    ? "Tidak ada catatan"
                    : penanaman.Catatan;
                dtlStatus.Text = penanaman.Status;
                LoadBtn(penanaman.Status);
                statusPenanaman = penanaman.Status;
            }
        }

        private void LoadAktivitasData()
        {
            try
            {
                List<AktivitasModel> aktivitasList = AktivitasController.GetAllByIdPenanaman(
                    id_penanaman
                );
                dgAktivitas.Grid.DataSource = aktivitasList;
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

        private void btnTambahAktivitas_Click(object sender, EventArgs e)
        {
            using (var TambahAktivitasform = new AktivitasTambah(id_penanaman))
            {
                ViewHelper.ShowDialogWithFormOverlay(this, TambahAktivitasform);

                if (TambahAktivitasform.DialogResult == DialogResult.OK)
                {
                    LoadAktivitasData();
                }
            }
        }

        private void dgAktivitas_Load(object sender, EventArgs e) { }

        private void dgAktivitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewColumn? columnEdit = dgAktivitas.Grid.Columns["btnEdit"];
            if (columnEdit != null && e.ColumnIndex == columnEdit.Index)
            {
                if (
                    dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"] != null
                    && dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"].Value != null
                )
                {
                    if (UserSession.CurrentUser.IdAkun != Convert.ToInt32(dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAkun"].Value))
                    {
                        MessageBox.Show(
                            "Anda tidak bisa mengedit aktivitas ini karena bukan milik Anda.",
                            "Akses Ditolak",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                    int idAktivitas = Convert.ToInt32(
                        dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"].Value
                    );
                    AktivitasModel? aktivitas = AktivitasController.GetAktivitasById(idAktivitas);
                    if (aktivitas != null)
                    {
                        using (var editAktivitasForm = new AktivitasEdit(idAktivitas))
                        {
                            ViewHelper.ShowDialogWithFormOverlay(this, editAktivitasForm);
                            if (editAktivitasForm.DialogResult == DialogResult.OK)
                            {
                                LoadAktivitasData();
                                LoadBtn(statusPenanaman);
                            }
                        }
                    }
                }
            }

            if (AkunRole == "Admin")
            {
                DataGridViewColumn? columnHapus = dgAktivitas.Grid.Columns["btnHapus"];
                if (columnHapus != null && e.ColumnIndex == columnHapus.Index)
                {
                    if (
                        dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"] != null
                        && dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"].Value != null
                    )
                    {
                        int idAktivitas = Convert.ToInt32(
                            dgAktivitas.Grid.Rows[e.RowIndex].Cells["IdAktivitas"].Value
                        );
                        AktivitasModel? Aktivitas = AktivitasController.GetAktivitasById(idAktivitas);
                        if (
                            Aktivitas != null
                            && MessageBox.Show(
                                $"Apakah yakin ingin menghapus Aktivitas '{Aktivitas.NamaJenisAktivitas}'?",
                                "Konfirmasi Hapus",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning
                            ) == DialogResult.Yes
                        )
                        {
                            if (AktivitasController.DeleteAktivitas(idAktivitas))
                            {
                                LoadAktivitasData();
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

        private void materialCard1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
