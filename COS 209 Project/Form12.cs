using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COS_209_Project
{
    public partial class Form12 : Form
    {
        public string memberID;
        public Form12(string memberID)
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

        private void Form12_Load(object sender, EventArgs e)
        {
            
            dgv2.Hide();

            lblID.Text = memberID;

        }

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

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Image image = Image.FromStream(ms);
                return image;
            }
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
                    cmd.CommandText = "select SID,SName,SEmail,Photo from Student_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Student WHERE Student_List.SID = Remove_Student.SID)";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv2.DataSource = dt;
                    dgv2.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv2.Columns[3];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();


                }

                // Add rows to DataGridView
                else if (cboView.Text == "StudentService_Member")
                {
                    cmd.CommandText = "select SSID,SSName,SSEmail,SSPhoto from StudentService_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_SSMember WHERE StudentService_List.SSID = Remove_SSMember.SSID)";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv2.DataSource = dt;
                    dgv2.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv2.Columns[3];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();


                }
                if (cboView.Text == "Teacher")
                {
                    cmd.CommandText = "select Teacher_List.TID,TName,TEmail,TPhoto from Teacher_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Teacher WHERE Teacher_List.TID = Remove_Teacher.TID)";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv2.DataSource = dt;
                    dgv2.Show();
                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv2.Columns[3];
                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    con.Close();


                }
                else if (cboView.Text == "Class")
                {

                    cmd.CommandText = "select * from Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv2.DataSource = dt;
                    dgv2.Show();

                    con.Close();

                }

                con.Close();
            }
            else
            {
                MessageBox.Show("Please Select one Table..");
            }
        }
        
        private void dgv1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*
            if (e.ColumnIndex == 3 && e.Value != null)
            {
                e.CellStyle.NullValue = null;
                e.CellStyle = new DataGridViewCellStyle
                {
                    Stretch = DataGridViewImageCellLayout.Stretch
                };
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f4 = new Form4(memberID);
            this.Hide();
            f4.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cboView.Text="Select";
            dgv2.Hide();
        }
    }
}
