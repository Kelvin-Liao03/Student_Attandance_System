using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COS_209_Project
{
    public partial class Form10 : Form
    {
        public string memberID;
        public Form10(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
            dgv1.CellContentClick += dgv1_CellContentClick;
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");

        private void Form10_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            cboSelect();
            dgv1.Hide();
            txtDate.Text = "Today Date : " + DateTime.Now.ToString("dd-MM-yyy");
            btnSave.Hide();
            lblAID.Hide();
            cboView.SelectedIndex = -1;
            
        }
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]

        private static extern int SendMessage(IntPtr hWd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void label1_Click(object sender, EventArgs e)
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
        public void autoAID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(AID) from Attendance", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                lblAID.Text = "KF-A" + r.ToString("000");
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

        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if(cboView.SelectedIndex >-1)
            {
                dgv1.Show();
                con.Open();
                btnSave.Show();
                dgv1.Rows.Clear();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select Student_List.SName from Enrollment " +
                                    "inner join Student_List on Student_List.SID=Enrollment.SID " +
                                    "left outer join Remove_Student on Student_List.SID=Remove_Student.SID where NOT EXISTS ( SELECT 1 FROM Remove_Student WHERE Student_List.SID = Remove_Student.SID)" +
                                    "and Enrollment.CID=(select CID from Class_List where CName='" + cboView.Text + "')";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                foreach(DataRow item in dt.Rows)
                {
                    int n = dgv1.Rows.Add();
                    dgv1.Rows[n].Cells[0].Value = item["SName"].ToString();
                    
                }
            }
            else
            {
                MessageBox.Show("Please select one class.");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f4 = new Form4(memberID);
            this.Hide();
            f4.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Present means a student can take the lass fully.\nLate is registered for students who are not in class withiin 15 mins of the class started.\nLeave is for students who taking leave for some reason.\nAbsent is for reasonless students without leave.");
        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboView.SelectedIndex == -1)
            {
                cboView.Text = "Select";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dgv1.Rows)
            {
                autoAID();

                bool present = Convert.ToBoolean(row.Cells["Present"].Value);
                bool late = Convert.ToBoolean(row.Cells["Late"].Value);
                bool leave = Convert.ToBoolean(row.Cells["Leave"].Value);
                bool absent = Convert.ToBoolean(row.Cells["Absent"].Value);


                if (present == true || late == true || leave == true || absent == true)
                {
                   
                    if (present)
                    {

                       
                        string dataGridViewValue = row.Cells["StudentName"].Value.ToString();
                        string ClassName = cboView.Text;

                        string p = "20";
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select EID from Enrollment where SID=(Select SID from Student_List where SName='" + dataGridViewValue + "') and CID=(Select CID from Class_List where CName='" + ClassName + "')";

                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);

                        cmd.CommandText = "insert into Attendance(AID,EID,Attendance_Mark,Date)values('"+lblAID.Text+"','" + dt.Rows[0]["EID"] + "','" + p + "',GETDATE())";
                        cmd.ExecuteNonQuery();
                        con.Close();
                       
                    }
                    else if (late)
                    {

                        
                        string dataGridViewValue = row.Cells["StudentName"].Value.ToString();
                        string ClassName = cboView.Text;

                        string p = "15";
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                       cmd.CommandText = "select EID from Enrollment where SID=(Select SID from Student_List where SName='" + dataGridViewValue + "') and CID=(Select CID from Class_List where CName='" + ClassName + "')";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);

                        cmd.CommandText = "insert into Attendance(AID,EID,Attendance_Mark,Date)values('" + lblAID.Text + "','" + dt.Rows[0]["EID"] + "','" + p + "',GETDATE())";
                        cmd.ExecuteNonQuery();
                        con.Close();
                       
                    }
                    else if (leave)
                    {

                        
                        string dataGridViewValue = row.Cells["StudentName"].Value.ToString();
                        string ClassName = cboView.Text;

                        string p = "5";
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select EID from Enrollment where SID=(Select SID from Student_List where SName='" + dataGridViewValue + "') and CID=(Select CID from Class_List where CName='" + ClassName + "')";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);


                        cmd.CommandText = "insert into Attendance(AID,EID,Attendance_Mark,Date)values('" + lblAID.Text + "','" + dt.Rows[0]["EID"] + "','" + p + "',GETDATE())";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        

                    }
                    else if (absent)
                    {

                      
                        string dataGridViewValue = row.Cells["StudentName"].Value.ToString();
                        string ClassName = cboView.Text;

                        string p = "0";
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select EID from Enrollment where SID=(Select SID from Student_List where SName='" + dataGridViewValue + "') and CID=(Select CID from Class_List where CName='" + ClassName + "')";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);


                        cmd.CommandText = "insert into Attendance(AID,EID,Attendance_Mark,Date)values('" + lblAID.Text + "','" + dt.Rows[0]["EID"] + "','" + p + "',GETDATE())";
                        cmd.ExecuteNonQuery();
                        con.Close();
                       

                    }
                    

                }
                else
                {
                    MessageBox.Show("Please give attendance for all students.");
                }

            }
            MessageBox.Show("Successfully Recorded.");
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {



            if (e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell chkCell1 = dgv1.Rows[e.RowIndex].Cells["Present"] as DataGridViewCheckBoxCell;
                DataGridViewCheckBoxCell chkCell2 = dgv1.Rows[e.RowIndex].Cells["Late"] as DataGridViewCheckBoxCell;
                DataGridViewCheckBoxCell chkCell3 = dgv1.Rows[e.RowIndex].Cells["Leave"] as DataGridViewCheckBoxCell;
                DataGridViewCheckBoxCell chkCell4 = dgv1.Rows[e.RowIndex].Cells["Absent"] as DataGridViewCheckBoxCell;
                if (chkCell1 != null)
                {
                    // Uncheck all other checkboxes in the same row
                    foreach (DataGridViewCell cell in dgv1.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != e.ColumnIndex && cell is DataGridViewCheckBoxCell)
                        {
                            dgv1[cell.ColumnIndex, e.RowIndex].Value = false;
                        }
                    }
                }
                else if (e.ColumnIndex == chkCell2.ColumnIndex)
                {
                    foreach (DataGridViewCell cell in dgv1.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != e.ColumnIndex && cell is DataGridViewCheckBoxCell)
                        {
                            dgv1[cell.ColumnIndex, e.RowIndex].Value = false;
                        }
                    }
                }
                else if (e.ColumnIndex == chkCell3.ColumnIndex)
                {
                    foreach (DataGridViewCell cell in dgv1.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != e.ColumnIndex && cell is DataGridViewCheckBoxCell)
                        {
                            dgv1[cell.ColumnIndex, e.RowIndex].Value = false;
                        }
                    }
                }
                else if (e.ColumnIndex == chkCell4.ColumnIndex)
                {
                    foreach (DataGridViewCell cell in dgv1.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != e.ColumnIndex && cell is DataGridViewCheckBoxCell)
                        {
                            dgv1[cell.ColumnIndex, e.RowIndex].Value = false;
                        }
                    }
                }

            }
        }
    }
}
