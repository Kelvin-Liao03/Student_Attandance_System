using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COS_209_Project
{
    public partial class Form7 : Form
    {
        public string memberID;
        
        public Form7(string memberID)
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

        private void btnContinue_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            if (cboView.SelectedIndex > -1)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;

                if (cboView.Text == "Student")
                {
                    cmd.CommandText = "SELECT * FROM Student_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Student WHERE Student_List.SID = Remove_Student.SID)";
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
                    cmd.CommandText = "SELECT * FROM Teacher_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Teacher WHERE Teacher_List.TID = Remove_Teacher.TID)";
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
                    cmd.CommandText = "SELECT * FROM StudentService_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_SSMember WHERE StudentService_List.SSID = Remove_SSMember.SSID)";
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
                    cmd.CommandText = "SELECT * FROM Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)";
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
                    /*cmd.CommandText = "select * from Enrollment";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();

                    con.Close();
                    */






                    cmd.CommandText = "SELECT EID, Enrollment.SID, SName, CName " +
                                        "FROM Student_List " +
                                        "JOIN Enrollment ON Student_List.SID = Enrollment.SID " +
                                         "JOIN Class_List ON Enrollment.CID = Class_List.CID " +
                                         "WHERE NOT EXISTS (SELECT 1 FROM Remove_Enrollment WHERE Enrollment.EID = Remove_Enrollment.EID)";
                   
                    cmd.ExecuteNonQuery();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    con.Close();

                    dgv1.DataSource = dt;
                    dgv1.Show();



                }
                else if (cboView.Text == "Reports")
                {
                    cmd.CommandText = "select * from SendReport";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();

                    con.Close();

                }
                else if (cboView.Text == "Sent_Emails")
                {
                    cmd.CommandText = "select * from SendEmail";
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
                MessageBox.Show("Please Select one Table..");
            }
        }

        
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30"))
                {
                   
                    if (cboView.Text == "StudentService_Member")
                    {
                        if (txtID.Text.Trim().StartsWith("KF-SS"))
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select * from Remove_SSMember where SSID = '" + txtID.Text + "'";
                            cmd.ExecuteNonQuery();
                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);
                            con.Close();
                            if (dt1.Rows.Count == 0)
                            {
                                con.Open();
                                cmd.CommandText = "select * from StudentService_List where SSID ='" + txtID.Text + "'";
                                cmd.ExecuteNonQuery();

                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                adp.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count == 1)
                                {
                                    dgv1.DataSource = dt;
                                    dgv1.Show();
                                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;

                                }
                                else
                                {
                                    MessageBox.Show("Invalid ID Number.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This ID has been removed.");
                            }
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    else if (cboView.Text == "Student")
                    {
                        if (txtID.Text.Trim().StartsWith("KF-S"))
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select * from Remove_Student where SID ='" + txtID.Text + "'";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);

                            if (dt1.Rows.Count == 0)
                            {
                                con.Open();
                                cmd.CommandText = "select * from Student_List where SID ='" + txtID.Text + "'";
                                cmd.ExecuteNonQuery();

                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                adp.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count == 1)
                                {
                                    dgv1.DataSource = dt;
                                    dgv1.Show();
                                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid ID Number.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This ID has been removed.");
                            }
                            con.Close();
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    else if (cboView.Text == "Class")
                    {
                        if (txtID.Text.Trim().StartsWith("KF-C"))
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select * from Remove_Class where CID = '" + txtID.Text + "'";
                            cmd.ExecuteNonQuery();
                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);
                            con.Close();
                            if (dt1.Rows.Count == 0)
                            {
                                con.Open();
                                cmd.CommandText = "select * from Class_List where CID='" + txtID.Text + "'";
                                cmd.ExecuteNonQuery();

                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                adp.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count == 1)
                                {
                                    dgv1.DataSource = dt;
                                    dgv1.Show();
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Invalid ID Number.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This ID has been removed.");
                            }

                            con.Close();

                        }
                        if(con.State == ConnectionState.Open)
                                        con.Close();

                    }
                    else if (cboView.Text == "Teacher")
                    {
                        if (txtID.Text.Trim().StartsWith("KF-T"))
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select * from Remove_Teacher where TID = '" + txtID.Text + "'";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);
                            con.Close();
                            if (dt1.Rows.Count == 0)
                            {
                                con.Open();
                                cmd.CommandText = "select * from Teacher_List where TID ='" + txtID.Text + "'";
                                cmd.ExecuteNonQuery();
                                con.Close();
                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                adp.Fill(dt);

                                if (dt.Rows.Count == 1)
                                {
                                    dgv1.DataSource = dt;
                                    dgv1.Show();
                                    DataGridViewImageColumn dGVimageColumn = (DataGridViewImageColumn)dgv1.Columns[6];
                                    dGVimageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
                                    con.Close();
                                }
                                else
                                {
                                    con.Close();
                                    MessageBox.Show("Invalid ID Number.");
                                }
                                con.Close();
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            else
                            {
                                MessageBox.Show("This ID has been removed.");
                            }
                            con.Close();
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }

                    else if (cboView.Text == "Enrollment")
                    {
                        if (txtID.Text.Trim().StartsWith("Enroll-"))
                        {
                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "select * from Remove_Enrollment where EID ='" + txtID.Text + "'";
                            cmd.ExecuteNonQuery();
                            con.Close();
                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);

                            if (dt1.Rows.Count == 0)
                            {
                                con.Open();
                                cmd.CommandText = "select * from Enrollment where EID ='" + txtID.Text + "'";
                                cmd.ExecuteNonQuery();

                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                DataTable dt = new DataTable();
                                adp.Fill(dt);
                                con.Close();
                                if (dt.Rows.Count == 1)
                                {
                                    dgv1.DataSource = dt;
                                    dgv1.Show();
                                   
                                }
                                else
                                {
                                    MessageBox.Show("Invalid ID Number.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This ID has been removed.");
                            }
                            con.Close();
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    else if(cboView.Text=="Reports")
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from SendReport where SRID ='" + txtID.Text + "'";
                        cmd.ExecuteNonQuery();

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count == 1)
                        {
                            dgv1.DataSource = dt;
                            dgv1.Show();

                        }
                        else
                        {
                            MessageBox.Show("Invalid ID Number.");
                        }
                    }
                    else if (cboView.Text == "Sent_Emails")
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from SendEmail where ID ='" + txtID.Text + "'";
                        cmd.ExecuteNonQuery();

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count == 1)
                        {
                            dgv1.DataSource = dt;
                            dgv1.Show();

                        }
                        else
                        {
                            MessageBox.Show("Invalid ID Number.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select one table to search!!!");
                    }
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }
                  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Ensure the connection is closed in case of an exception
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
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

        private void Form7_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            dgv1.Hide();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dgv1.Hide();
            cboView.Text="Select";
            txtID.Text = null;
        }

        private void lblID_Click(object sender, EventArgs e)
        {

        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgv1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }
        

        private void dgv1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

           

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f15 = new Form15(memberID);
            this.Hide();
            f15.Show();
        }
    }
}
