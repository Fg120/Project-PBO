using System;
using System.Drawing;
using System.Windows.Forms;

namespace Project_PBO.Component
{
    public partial class CustomTable : UserControl
    {
        public CustomTable()
        {
            InitializeComponent();
            InitStyle();
        }

        private void InitStyle()
        {
            // Tidak bisa diseleksi
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1
                .DefaultCellStyle
                .BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1
                .DefaultCellStyle
                .ForeColor;
            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();
            dataGridView1.MultiSelect = false;

            // Nonaktifkan fitur yang tidak diperlukan
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;

            // Tampilan umum
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.GridColor = Color.LightGray;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Header
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(255, 242, 225);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(
                "Poppins",
                10,
                FontStyle.Bold
            );
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.ColumnHeadersHeight = 40;
            dataGridView1.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Sel
            dataGridView1.DefaultCellStyle.Font = new Font("Poppins", 10);
            dataGridView1.DefaultCellStyle.Padding = new Padding(5);

            // Tinggi baris
            dataGridView1.RowTemplate.Height = 40;

            // Stripe baris
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            // Kolom otomatis merata
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // Akses langsung dari luar
        public DataGridView Grid => dataGridView1;
    }
}
