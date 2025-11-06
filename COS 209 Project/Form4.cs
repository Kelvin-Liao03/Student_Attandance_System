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
    public partial class Form4 : Form
    {
        public string memberID;

        private bool isStudentLogInClicked = false;
        private bool isStudentServiceLogInClicked = false;
        private bool isTeacherLogInClicked = false;

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");


        public Form4(string memberID)
        {
            InitializeComponent();
            SidePanel.Height = btnHome.Height;
            SidePanel.Top = btnHome.Top;
            this.memberID = memberID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            SidePanel.Height = btnHome.Height;
            SidePanel.Top = btnHome.Top;
            
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            if (isStudentLogInClicked)
            {
                SidePanel.Height = btnAttendance.Height;
                SidePanel.Top = btnAttendance.Top;
                var f11 = new Form11(memberID);
                this.Hide();
                f11.Show();

            }
            else if (isStudentServiceLogInClicked)
            {
                SidePanel.Height = btnAttendance.Height;
                SidePanel.Top = btnAttendance.Top;
                var f11 = new Form11(memberID);
                this.Hide();
                f11.Show();

            }
            else if(isTeacherLogInClicked)
            {
                SidePanel.Height = btnAttendance.Height;
                SidePanel.Top = btnAttendance.Top;
                var f10 = new Form10(memberID);
                this.Hide();
                f10.Show();


            }

            
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (isStudentServiceLogInClicked)
            {
                SidePanel.Height = btnView.Height;
                SidePanel.Top = btnView.Top;
                var f7 = new Form7(memberID);
                this.Hide();
                f7.Show();
            }
            else if (isTeacherLogInClicked)
            {
                SidePanel.Height = btnAttendance.Height;
                SidePanel.Top = btnAttendance.Top;
                var f12 = new Form12(memberID);
                this.Hide();
                f12.Show();

            }
           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
            if (isStudentServiceLogInClicked)
            {
                SidePanel.Height = btnAdd.Height;
                SidePanel.Top = btnAdd.Top;
                var f5 = new Form5(memberID);
                this.Hide();
                f5.Show();
            }
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (isStudentServiceLogInClicked)
            {
                SidePanel.Height = btnUpdate.Height;
                SidePanel.Top = btnUpdate.Top;
                var f8 = new Form8(memberID);
                this.Hide();
                f8.Show();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var f1 = new Form1();
            this.Hide();
            f1.Show();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int radius = 50;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);

            this.Region = new Region(path);


            if(string.IsNullOrEmpty(memberID))
            {
                btnAttendance.Visible = false;
                btnView.Visible = false;
                btnAdd.Visible = false;
                btnUpdate.Visible = false;
            }
            else
            {
                if(memberID.Trim().StartsWith("KF-T"))
                {
                    isTeacherLogInClicked = true;
                    btnAttendance.Visible = true;
                    btnView.Visible = true;
                }
                else if(memberID.Trim().StartsWith("KF-SS"))
                {
                    isStudentServiceLogInClicked = true;
                    btnAttendance.Visible = true;
                    btnView.Visible = true;
                    btnAdd.Visible = true;
                    btnUpdate.Visible = true;
                }
                else if(memberID.Trim().StartsWith("KF-S"))
                {
                    isStudentLogInClicked = true;

                    btnAttendance.Visible = true;

                }



              
            }
        }
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern int SendMessage(IntPtr hWd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void btnSLogIn_Click(object sender, EventArgs e)
        {
            


            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Student_List where SID='" + txtSID.Text + "' and SEmail='" + txtSEmail.Text.Trim() + "'";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            con.Close();
            if (dt.Rows.Count == 1)
            {
                MessageBox.Show("Log In Success!!\nYou can view your attendance record in Attendance Field.");

                btnAttendance.Visible = true;
                isStudentLogInClicked = true;
                memberID = dt.Rows[0]["SID"].ToString();
            }
            else
            {
                MessageBox.Show("Invalid ID or Email.Please Try Again!!");
            }

        }

        private void btnStaffLogIn_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            if (txtMemberID.Text.Trim().StartsWith("KF-T"))
            {
                cmd.CommandText = "select * from Teacher_List where TID='" + txtMemberID.Text + "' and TEmail='" + txtMemberEmail.Text.Trim() + "'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Log In Success!!\nYou can give the marks for students' attendance.");

                    btnView.Visible = true;
                    isTeacherLogInClicked = true;
                    btnAttendance.Visible = true;
                    memberID = dt.Rows[0]["TID"].ToString();

                }
                else
                {
                    MessageBox.Show("Invalid ID or Email.Please Try Again!!");
                }
            }
            else if (txtMemberID.Text.Trim().StartsWith("KF-SS"))
            {
                cmd.CommandText = "select * from StudentService_List where SSID='" + txtMemberID.Text + "' and SSEmail='" + txtMemberEmail.Text.Trim() + "'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Log In Success!!\nYou can add,update,delete and view the students' information.");

                    btnAttendance.Visible = true;
                    btnView.Visible = true;
                    btnAdd.Visible = true;
                    btnUpdate.Visible = true;

                    isStudentServiceLogInClicked = true;
                    memberID = dt.Rows[0]["SSID"].ToString();
                }
                else
                {
                    MessageBox.Show("Invalid ID or Email.Please Try Again!!");
                }
            }







        }

        private void txtMemberID_TextChanged(object sender, EventArgs e)
        {
            txtSID.Text = "";
            txtSEmail.Text = "";
        }

        private void txtSID_TextChanged(object sender, EventArgs e)
        {
            txtMemberID.Text = "";
            txtMemberEmail.Text = "";
            btnUpdate.Visible = false;
            btnAdd.Visible = false;
            btnView.Visible = false;
            btnAttendance.Visible = false;
        }
    }
}
