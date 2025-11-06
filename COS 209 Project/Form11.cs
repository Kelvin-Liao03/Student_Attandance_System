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
    public partial class Form11 : Form
    {
        string memberID;
        public Form11(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }



        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");


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
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern int SendMessage(IntPtr hWd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void Form11_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            cboSelect();
            dgv1.Hide();
            cboView.SelectedIndex = -1;
            btnReport.Hide();
            btnEmail.Hide();
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
           
            if (cboView.SelectedIndex > -1)
            {
                dgv1.Show();
                if (memberID.Trim().StartsWith("KF-SS"))
                {
                    btnEmail.Show();
                }
                else if (memberID.Trim().StartsWith("KF-S"))
                {
                    btnReport.Show();
                }
                con.Open();
                
                dgv1.Rows.Clear();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT s1.SID, s1.SName, AVG(a.Attendance_Mark) AS AvgAttendance FROM Student_List s1 " +
                                  "JOIN Enrollment e ON s1.SID = e.SID " +
                                  "JOIN Attendance a ON e.EID = a.EID " +
                                  "JOIN Class_List c1 ON e.CID = c1.CID " +
                                  "LEFT OUTER JOIN Remove_Student rs ON s1.SID = rs.SID " +
                                  "WHERE NOT EXISTS (SELECT 1 FROM Remove_Student WHERE s1.SID = Remove_Student.SID) " +
                                  "AND c1.CName='" + cboView.Text + "' GROUP BY s1.SID, s1.SName";
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                foreach (DataRow item in dt.Rows)
                {
                    int n = dgv1.Rows.Add();
                    dgv1.Rows[n].Cells[0].Value = item["SID"].ToString();
                    dgv1.Rows[n].Cells[1].Value = item["SName"].ToString();
                    dgv1.Rows[n].Cells[2].Value = item["AvgAttendance"].ToString();
                    dgv1.Rows[n].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
            }
            else
            {
                MessageBox.Show("Please select one class.");
            }
        }
        private void cboSelect()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            cboView.DataSource = ds.Tables[0];
            cboView.DisplayMember = "CName";
            cboView.SelectedIndex = -1;
            cboView.Text = "Select";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f4 = new Form4(memberID);
            this.Hide();
            f4.Show();
        }

        private void dgv1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int limit = 10;

            // Check if the value is below the limit and the current cell is in the desired column
            if (e.ColumnIndex == 2 && Convert.ToInt32(e.Value) < limit)
            {
                // Set the background color to red
                e.CellStyle.BackColor = Color.Red;
                e.CellStyle.ForeColor = Color.White; // Optionally set text color for better visibility
            }
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            var f13 = new Form13(memberID);
            this.Hide();
            f13.Show();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            var f14 = new Form14(memberID);
            this.Hide();
            f14.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cboView.Text="Select";
            dgv1.Hide();
            btnEmail.Hide();
            btnReport.Hide();
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void gbView_Enter(object sender, EventArgs e)
        {

        }
    }
}
