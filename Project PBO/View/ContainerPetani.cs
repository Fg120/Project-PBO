using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project_PBO.Helpers;

namespace Project_PBO.View
{
    public partial class ContainerPetani : Form
    {
        public ContainerPetani()
        {
            InitializeComponent();
            CheckUserSession();
            UpdateUserInfo();

            if (UserSession.IsLoggedIn)
            {
                SetGreeting();
            }
        }

        private void ContainerPetani_Load(object sender, EventArgs e)
        {
            panelContent.Controls.Clear();
            DashboardPetani dashboardPetani = new DashboardPetani();
            dashboardPetani.Dock = DockStyle.Fill;
            panelContent.Controls.Add(dashboardPetani);
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
    }
}
