using System;
using System.Windows.Forms;
using Project_PBO.Controller;
using Project_PBO.Database;
using Project_PBO.Helpers;
using Project_PBO.Models;

namespace Project_PBO.View
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(
                    "Username dan password tidak boleh kosong.",
                    "Login Gagal",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                AkunModel? user = AkunController.Login(username.Trim(), password.Trim());

                if (user != null && user.IsActive)
                {
                    UserSession.CurrentUser = user;

                    MessageBox.Show(
                        $"Welcome {user.Username}! Role: {user.Role}",
                        "Login Successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    if (user.Role == "Admin")
                    {
                        ContainerAdmin containerForm = new ContainerAdmin();
                        this.Hide();
                        containerForm.ShowDialog();
                        this.Close();
                    }
                    else if (user.Role == "Petani")
                    {
                        ContainerPetani containerPetaniForm = new ContainerPetani();
                        this.Hide();
                        containerPetaniForm.ShowDialog();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Username atau password salah, atau akun tidak aktif.",
                        "Login Gagal",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Login error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
