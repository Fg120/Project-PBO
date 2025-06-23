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

            btnDashboard_Click(null, EventArgs.Empty);

            cardApp.Padding = new Padding(0);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void CheckUserSession()
        {
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

        private void Container_Load(object sender, EventArgs e)
        {
            UpdateMenuAccess();
            SetGreeting();

            btnDashboard_Click(null, EventArgs.Empty);
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SidebarActive.MenuActive = "Dashboard";
            CheckActiveSidebar();

            panelContent.Controls.Clear();
            DashboardAdmin dashboard = new DashboardAdmin();
            dashboard.Dock = DockStyle.Fill;
            panelContent.Controls.Add(dashboard);
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
            SidebarActive.MenuActive = "Akun";
            CheckActiveSidebar();

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
            SidebarActive.MenuActive = "Lahan";
            CheckActiveSidebar();

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
            SidebarActive.MenuActive = "Tanaman";
            CheckActiveSidebar();

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
            SidebarActive.MenuActive = "JenisAktivitas";
            CheckActiveSidebar();

            panelContent.Controls.Clear();
            JenisAktivitasIndex jenisAktivitas = new JenisAktivitasIndex();
            jenisAktivitas.Dock = DockStyle.Fill;
            panelContent.Controls.Add(jenisAktivitas);
        }

        private void btnPenanaman_Click(object sender, EventArgs e)
        {
            SidebarActive.MenuActive = "Penanaman";
            CheckActiveSidebar();

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

        private void CheckActiveSidebar()
        {
            // Reset all buttons to default state first
            ResetAllButtonsToDefault();

            string activeMenu = SidebarActive.MenuActive;
            Color activeColor = Color.DodgerBlue;

            // Set the active button based on current menu
            if (activeMenu == "Dashboard")
            {
                btnDashboard.BackColor = activeColor;
                btnDashboard.HoverColor = activeColor;
            }
            else if (activeMenu == "Akun")
            {
                btnAkun.BackColor = activeColor;
                btnAkun.HoverColor = activeColor;
            }
            else if (activeMenu == "Lahan")
            {
                btnLahan.BackColor = activeColor;
                btnLahan.HoverColor = activeColor;
            }
            else if (activeMenu == "Tanaman")
            {
                btnTanaman.BackColor = activeColor;
                btnTanaman.HoverColor = activeColor;
            }
            else if (activeMenu == "JenisAktivitas")
            {
                btnJenisAktivitas.BackColor = activeColor;
                btnJenisAktivitas.HoverColor = activeColor;
            }
            else if (activeMenu == "Penanaman")
            {
                btnPenanaman.BackColor = activeColor;
                btnPenanaman.HoverColor = activeColor;
            }
        }

        private void ResetAllButtonsToDefault()
        {
            Color defaultBackColor = Color.FromArgb(64, 64, 64); // or whatever your default color is
            Color defaultHoverColor = Color.DodgerBlue; // or whatever your default hover color is

            btnDashboard.BackColor = defaultBackColor;
            btnDashboard.HoverColor = defaultHoverColor;

            btnAkun.BackColor = defaultBackColor;
            btnAkun.HoverColor = defaultHoverColor;

            btnLahan.BackColor = defaultBackColor;
            btnLahan.HoverColor = defaultHoverColor;

            btnTanaman.BackColor = defaultBackColor;
            btnTanaman.HoverColor = defaultHoverColor;

            btnJenisAktivitas.BackColor = defaultBackColor;
            btnJenisAktivitas.HoverColor = defaultHoverColor;

            btnPenanaman.BackColor = defaultBackColor;
            btnPenanaman.HoverColor = defaultHoverColor;
        }

        private void lblUsername_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
