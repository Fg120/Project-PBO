using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_PBO.Helpers;
using ReaLTaiizor.Forms;

namespace Project_PBO.View
{
    public partial class ContainerAdmin : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        public ContainerAdmin()
        {
            InitializeComponent();
            CheckUserSession();
            UpdateUserInfo();

            if (UserSession.IsLoggedIn)
            {
                UpdateMenuAccess();
                SetGreeting();
            }
        }

        private void CheckUserSession()
        {
            // Check if user is logged in
            if (!UserSession.IsLoggedIn)
            {
                MessageBox.Show(
                    "Anda harus login terlebih dahulu!",
                    "Akses Ditolak",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                this.Close();
                return;
            }
        }

        private void UpdateUserInfo()
        {
            this.Text =
                $"Aplikasi Manajemen Pertanian - Selamat Datang {UserSession.Username} ({UserSession.Role})";
        }

        private void UpdateMenuAccess()
        {
            bool isAdmin = UserSession.IsAdmin();
            btnAkun.Visible = isAdmin;
            btnLahan.Visible = isAdmin;
            btnTanaman.Visible = isAdmin;
            btnJenisAktivitas.Visible = isAdmin;
        }

        private void btnAkun_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show(
                    "Hanya admin yang boleh mengakses halaman ini.",
                    "Akses Ditolak",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            panelContent.Controls.Clear();
            AkunIndex akun = new AkunIndex();
            akun.Dock = DockStyle.Fill;
            panelContent.Controls.Add(akun);
        }

        private void btnLahan_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show(
                    "Hanya admin yang boleh mengakses halaman ini.",
                    "Akses Ditolak",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            panelContent.Controls.Clear();
            LahanIndex lahan = new LahanIndex();
            lahan.Dock = DockStyle.Fill;
            panelContent.Controls.Add(lahan);
        }

        private void btnTanaman_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show(
                    "Hanya admin yang boleh mengakses halaman ini.",
                    "Akses Ditolak",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            panelContent.Controls.Clear();
            TanamanIndex tanaman = new TanamanIndex();
            tanaman.Dock = DockStyle.Fill;
            panelContent.Controls.Add(tanaman);
        }

        private void btnJenisAktivitas_Click(object sender, EventArgs e)
        {
            if (!UserSession.IsAdmin())
            {
                MessageBox.Show(
                    "Hanya admin yang boleh mengakses halaman ini.",
                    "Akses Ditolak",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            panelContent.Controls.Clear();
            JenisAktivitasIndex jenisAktivitas = new JenisAktivitasIndex();
            jenisAktivitas.Dock = DockStyle.Fill;
            panelContent.Controls.Add(jenisAktivitas);
        }

        private void btnPenanaman_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            PenanamanIndex penanaman = new PenanamanIndex();
            penanaman.Dock = DockStyle.Fill;
            panelContent.Controls.Add(penanaman);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Apakah Anda yakin ingin logout?",
                "Konfirmasi Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                UserSession.Logout();
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void Container_Load(object sender, EventArgs e)
        {
            UpdateMenuAccess();
            SetGreeting();

            panelContent.Controls.Clear();
            DashboardAdmin dashboard = new DashboardAdmin();
            dashboard.Dock = DockStyle.Fill;
            panelContent.Controls.Add(dashboard);
        }

        private void SetGreeting()
        {
            string username = UserSession.Username;
            string timeOfDayGreeting;
            int currentHour = DateTime.Now.Hour;

            if (currentHour >= 0 && currentHour < 12) // Pagi dari jam 00:00 hingga 11:59
            {
                timeOfDayGreeting = "Pagi";
            }
            else if (currentHour >= 12 && currentHour < 15) // Siang dari jam 12:00 hingga 14:59
            {
                timeOfDayGreeting = "Siang";
            }
            else if (currentHour >= 15 && currentHour < 19) // Sore dari jam 15:00 hingga 18:59
            {
                timeOfDayGreeting = "Sore";
            }
            else
            {
                timeOfDayGreeting = "Malam";
            }

            lblGreeting.Text = $"Selamat {timeOfDayGreeting}, {username}!";
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            DashboardAdmin dashboard = new DashboardAdmin();
            dashboard.Dock = DockStyle.Fill;
            panelContent.Controls.Add(dashboard);
        }
    }
}
