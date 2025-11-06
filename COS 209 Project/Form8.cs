using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Diagnostics.Eventing.Reader;

namespace COS_209_Project
{
    public partial class Form8 : Form
    {
        public string id = "";
        public string name = "";
        public string age = "";
        public string email ="";
        public string address = "";
        public string gender = "";
        public string teacherID = "";
        public string duration = "";
        public string day = "";
        public string time = "";
        public string sid = "";
        public string cid = "";
        byte[] imagePhoto = null;
        
        public string memberID;
        public Form8(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");

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

        private void Form8_Load(object sender, EventArgs e)
        {
            dgv1.Hide();
            lblID.Text = memberID;
            btnUpdate.Hide();
            btnDelete.Hide();
            btnRemove.Hide();
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
                    btnUpdate.Show();
                    btnRemove.Show();
                    btnDelete.Hide();
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
                    btnUpdate.Show();
                    btnRemove.Show();
                    btnDelete.Hide();
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
                    btnUpdate.Show();
                    btnRemove.Show();
                    btnDelete.Hide();
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
                    btnUpdate.Show();
                    btnRemove.Show();
                    btnDelete.Hide();
                    cmd.CommandText = "SELECT * FROM Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();
                    con.Close();
                }
                else if (cboView.Text == "Reports")
                {
                    btnDelete.Show();
                    btnUpdate.Hide();
                    btnRemove.Hide();
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
                    btnDelete.Show();
                    btnUpdate.Hide();
                    btnRemove.Hide();
                    cmd.CommandText = "select * from SendEmail";
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    dgv1.DataSource = dt;
                    dgv1.Show();
                    con.Close();
                }
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            else
            {
                MessageBox.Show("Please Select one Table..");
            }
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {

                DataGridViewRow index = dgv1.Rows[e.RowIndex];

                if(cboView.Text == "Student" || cboView.Text == "Teacher" || cboView.Text == "StudentService_Member"  )
                {
                    id = index.Cells[0].Value.ToString();
                    name = index.Cells[1].Value.ToString();
                    age = index.Cells[2].Value.ToString();
                    email = index.Cells[3].Value.ToString();
                    address = index.Cells[4].Value.ToString();
                    gender = index.Cells[5].Value.ToString();
                    imagePhoto =(byte[]) index.Cells[6].Value;



                }
                else if(cboView.Text == "Class")
                {
                    id = index.Cells[0].Value.ToString();
                    name = index.Cells[1].Value.ToString();
                    teacherID = index.Cells[2].Value.ToString();
                    duration = index.Cells[3].Value.ToString();
                    day = index.Cells[4].Value.ToString();
                    time = index.Cells[5].Value.ToString();
                }
                
                else if(cboView.Text == "Reports" || cboView.Text=="Sent_Emails")
                {
                    id = index.Cells[0].Value.ToString();
                }
                
            }
        }
        private Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return null; // Return null if the input byte array is null or empty
            }

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                Image image2 = Image.FromStream(ms);
                return image2;
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Please select a row in data grid view.");
            }
            else
            {
                var f9 = new Form9(id, memberID);
                this.Hide();
                f9.Show();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cboView.Text="Select";
            dgv1.Hide();
            btnRemove.Hide();
            btnUpdate.Hide();
            btnDelete.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var DialogResult = MessageBox.Show("Are you sure you want to Delete?", "Are you sure?", MessageBoxButtons.YesNoCancel);
            if (DialogResult == DialogResult.Yes)
            {

                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                if (cboView.Text == "Reports")
                {

                    cmd.CommandText = "delete from SendReport where SRID='" + id + "'";
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Deleted Successfully!!");
                    ReportDisplay();



                }
                else if (cboView.Text == "Sent_Emails")
                {

                    cmd.CommandText = "delete from SendEmail where ID='" + id + "'";
                    cmd.ExecuteNonQuery();

                    con.Close();
                    MessageBox.Show("Deleted Successfully!!");

                    SentEmailsDisplay();
                }


            }
            else if (DialogResult == DialogResult.No)
            {
                var f8 = new Form8(memberID);
                this.Hide();
                f8.Show();
            }

        }
        public void ReportDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from SendReport";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }
        public void SentEmailsDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from SendEmail";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }


        



        private void StudentDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Student_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Student WHERE Student_List.SID = Remove_Student.SID)";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }
        private void SSMemberDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM StudentService_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_SSMember WHERE StudentService_List.SSID = Remove_SSMember.SSID)";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }
        private void TeacherDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Teacher_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Teacher WHERE Teacher_List.TID = Remove_Teacher.TID)";
             cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }
        private void ClassDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dgv1.DataSource = dt;
            con.Close();
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {

            var DialogResult = MessageBox.Show("Are you sure you want to Remove?", "Are you sure?", MessageBoxButtons.YesNoCancel);

            if (DialogResult == DialogResult.Yes)
            {
                if (cboView.Text == "Student" || cboView.Text == "Teacher" || cboView.Text == "StudentService_Member")
                {
                    if (imagePhoto != null)
                        {
                           if (cboView.Text == "Student")
                                {

                                    con.Open();
                                    SqlCommand cmd = con.CreateCommand();
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "insert into Remove_Student(SID,SName,SAge,SEmail,SAddress,Gender,Photo)values(@ID, @Name, @Age, @Email, @Address, @Gender, @Image)";
                                    cmd.Parameters.AddWithValue("@ID", id);
                                    cmd.Parameters.AddWithValue("@Name", name);
                                    cmd.Parameters.AddWithValue("@Age", age);
                                    cmd.Parameters.AddWithValue("@Email", email);
                                    cmd.Parameters.AddWithValue("@Address", address);
                                    cmd.Parameters.AddWithValue("@Gender", gender);
                                    cmd.Parameters.Add("@Image", imagePhoto);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MessageBox.Show("Data Removed Successfully!!");
                                    StudentDisplay();

                                    con.Open();
                                    SqlCommand cmd1 = con.CreateCommand();
                                    cmd1.CommandType = CommandType.Text;
                                    cmd1.CommandText = "select * from Enrollment where SID = '" + id + "'";
                                    cmd1.ExecuteNonQuery();
                                    con.Close();
                                    SqlDataAdapter adp = new SqlDataAdapter(cmd1);
                                    DataTable dt = new DataTable();
                                    adp.Fill(dt);
                                    

                                    //MessageBox.Show("The student id is " + id);
                                    if(dt.Rows.Count>0)
                                    {
                                        for(int i=0; i <dt.Rows.Count; i++)
                                        {
                                            string eid = dt.Rows[i]["EID"].ToString();
                                            string sid = dt.Rows[i]["SID"].ToString();
                                            string cid = dt.Rows[i]["CID"].ToString();

                                            con.Open();
                                            SqlCommand cmd2 = con.CreateCommand();
                                            cmd2.CommandType = CommandType.Text;
                                            cmd2.CommandText = "select * from Remove_Enrollment where EID = '" + eid + "' and SID='" + sid + "'";
                                            cmd2.ExecuteNonQuery();
                                            con.Close();

                                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd2);
                                            DataTable dt1 = new DataTable();
                                            adp1.Fill(dt1);
                                            if (dt1.Rows.Count == 0)
                                            {

                                                con.Open();
                                                SqlCommand cmd3 = con.CreateCommand();
                                                cmd3.CommandType = CommandType.Text;
                                                cmd3.CommandText = "insert into Remove_Enrollment(EID,SID,CID)values('" + eid + "','" + sid + "','" + cid + "')";
                                                cmd3.ExecuteNonQuery();
                                                con.Close();



                                            }
                                  
                                        }
                                     }
                                        
                                    
                                    
                            


                                }
                                else if (cboView.Text == "StudentService_Member")
                                {

                                    con.Open();
                                    SqlCommand cmd = con.CreateCommand();
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "insert into Remove_SSMember(SSID,SSName,SSAge,SSEmail,SSAddress,Gender,SSPhoto)values(@ID, @Name, @Age, @Email, @Address, @Gender, @Image)";
                                    cmd.Parameters.AddWithValue("@ID", id);
                                    cmd.Parameters.AddWithValue("@Name", name);
                                    cmd.Parameters.AddWithValue("@Age", age);
                                    cmd.Parameters.AddWithValue("@Email", email);
                                    cmd.Parameters.AddWithValue("@Address", address);
                                    cmd.Parameters.AddWithValue("@Gender", gender);
                                    cmd.Parameters.Add("@Image", imagePhoto);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MessageBox.Show("Data Removed Successfully!!");
                                    SSMemberDisplay();
                                }
                                else if (cboView.Text == "Teacher")
                                {

                                    con.Open();
                                    SqlCommand cmd = con.CreateCommand();
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "insert into Remove_Teacher(TID,TName,TAge,TEmail,TAddress,TGender,TPhoto)values(@ID, @Name, @Age, @Email, @Address, @Gender, @Image)";
                                    cmd.Parameters.AddWithValue("@ID", id);
                                    cmd.Parameters.AddWithValue("@Name", name);
                                    cmd.Parameters.AddWithValue("@Age", age);
                                    cmd.Parameters.AddWithValue("@Email", email);
                                    cmd.Parameters.AddWithValue("@Address", address);
                                    cmd.Parameters.AddWithValue("@Gender", gender);
                                    cmd.Parameters.Add("@Image", imagePhoto);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MessageBox.Show("Data Removed Successfully!!");
                                    TeacherDisplay();


                                    con.Open();
                                    SqlCommand cmd3 = con.CreateCommand();
                                    cmd3.CommandType = CommandType.Text;
                                    cmd3.CommandText = "select * from Class_List where TID = '" + id + "'";
                                    cmd3.ExecuteNonQuery();
                                    con.Close();
                                    SqlDataAdapter adp1 = new SqlDataAdapter(cmd3);
                                    DataTable dt = new DataTable();
                                    adp1.Fill(dt);
                                    if(dt.Rows.Count > 0)
                                    {
                                        MessageBox.Show("This teacher teaches the class in this orgnization. Please update the class info with the new teacher.");
                                    }
                        }
                        

                    }
                    else
                    {
                        MessageBox.Show("Image is null. Cannot proceed with removal.");
                    }



                }
                else if (cboView.Text == "Class")
                {
                    con.Open();
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "insert into Remove_Class(CID,CName,TID,Duration,Day,Time)values(@ID, @Name, @TeacherID, @Duration, @Day, @Time)";
                    // Add other parameters as needed
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@TeacherID", teacherID);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@Day", day);
                    cmd.Parameters.AddWithValue("@Time", time);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data Removed Successfully!!");
                    ClassDisplay();

                    con.Open();
                    SqlCommand cmd1 = con.CreateCommand();
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "select * from Enrollment where CID = '" + id + "'";
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd1);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);


                    //MessageBox.Show("The student id is " + id);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string eid = dt.Rows[i]["EID"].ToString();
                            string sid = dt.Rows[i]["SID"].ToString();
                            string cid = dt.Rows[i]["CID"].ToString();

                            con.Open();
                            SqlCommand cmd2 = con.CreateCommand();
                            cmd2.CommandType = CommandType.Text;
                            cmd2.CommandText = "select * from Remove_Enrollment where EID = '" + eid + "' and CID='" + cid + "'";
                            cmd2.ExecuteNonQuery();
                            con.Close();

                            SqlDataAdapter adp1 = new SqlDataAdapter(cmd2);
                            DataTable dt1 = new DataTable();
                            adp1.Fill(dt1);
                            if (dt1.Rows.Count == 0)
                            {

                                con.Open();
                                SqlCommand cmd3 = con.CreateCommand();
                                cmd3.CommandType = CommandType.Text;
                                cmd3.CommandText = "insert into Remove_Enrollment(EID,SID,CID)values('" + eid + "','" + sid + "','" + cid + "')";
                                cmd3.ExecuteNonQuery();
                                con.Close();



                            }

                        }
                    }

                }
                
                con.Close();
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            else if (DialogResult == DialogResult.No)
            {
                var f8 = new Form8(memberID);
                this.Hide();
                f8.Show();
            }
        }
       





    }
}
