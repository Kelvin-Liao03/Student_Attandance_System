using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COS_209_Project
{
    public partial class Form15 : Form
    {
        string memberID;
        public Form15(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");



        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern int SendMessage(IntPtr hWd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 1 && e.Y <= this.Height && e.Y >= 0)
                {
                    ReleaseCapture();
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }

            }
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            dgv1.Hide();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (cboView.SelectedIndex > -1)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                if (cboView.Text == "Student")
                {
                    cmd.CommandText = "SELECT * FROM Remove_Student";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();


                }
                else if (cboView.Text == "Teacher")
                {
                    cmd.CommandText = "SELECT * FROM Remove_Teacher";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();


                }
                else if (cboView.Text == "StudentService_Member")
                {
                    cmd.CommandText = "SELECT * FROM Remove_SSMember";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();

                }
                else if (cboView.Text == "Class")
                {
                    cmd.CommandText = "SELECT * FROM Remove_Class";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();

                    con.Close();

                }
                else if (cboView.Text == "Enrollment")
                {
                    cmd.CommandText = "SELECT * FROM Remove_Enrollment";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();

                    con.Close();

                }
            }
            else
            {
                MessageBox.Show("Please select one table.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f7 = new Form7(memberID);
            this.Hide();
            f7.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}