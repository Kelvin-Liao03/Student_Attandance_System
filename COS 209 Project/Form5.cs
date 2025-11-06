using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COS_209_Project
{
    public partial class Form5 : Form
    {
        public string memberID;
        public Form5(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }


        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");


        private void label5_Click(object sender, EventArgs e)
        {

        }
        public void clearStudent()
        {
            autoStudentID();
            txtSName.Text = "";
            txtAge.Text = "";
            rdbMale.Checked = false;
            rdbFemale.Checked = false;
            txtEmail.Text = "";
            txtAddress.Text = "";
            pictureBox1.Image = null;

        }
        public void autoStudentID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(SID) from Student_List", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                txtSID.Text = "KF-S" + r.ToString("000");
            }
            
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            clearStudent();
        }

        private void AddPanel_Paint(object sender, PaintEventArgs e)
        {
            autoStudentID();
        }

        private void btnUplaod_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Select image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                if (IsValidFilePath(filePath))
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            pictureBox1.Image = Image.FromFile(filePath);
                        }
                        else
                        {
                            MessageBox.Show("Selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid file path format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private bool IsValidFilePath(string path)
        {
            return Path.IsPathRooted(path);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSName.Text))
            {
                MessageBox.Show("Please fill your Name!!");
            }
            else if (string.IsNullOrEmpty(txtAge.Text))
            {
                MessageBox.Show("Please fill your Age!!");
            }

            else if (string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Please fill your Email!!");
            }
            else if (string.IsNullOrEmpty(txtAddress.Text))
            {
                MessageBox.Show("Please fill your Address!!");
            }
            else if (rdbMale.Checked == false & rdbFemale.Checked == false)
            {
                MessageBox.Show("Please select your Gender!!");
            }

            else
            {
                try
                {


                    string Gender;
                    if (rdbMale.Checked == true)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }
                    
                    if (!int.TryParse(txtAge.Text, out int age))
                    {
                        MessageBox.Show("Please enter a valid age.");
                        return; // Exit the method if age is not a valid integer.
                    }
                    if (age < 15)
                    {
                        MessageBox.Show("Sorry, you are too young!!");
                        return; // Exit the method if the age is less than 15.
                    }


                    else
                    {

                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from Student_List where SName = '" + txtSName.Text.Trim() + "' and SAge= '" + txtAge.Text.Trim() + "' and SEmail= '" + txtEmail.Text.Trim() + "' and SAddress='" + txtAddress.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count == 1)
                        {
                            MessageBox.Show("This student has already registered.");
                            clearClass();
                        }
                        else
                        {
                            byte[] imageData = ImageToByteArray(pictureBox1.Image);

                            con.Open();
                            cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into Student_List(SID,SName,SAge,SEmail,SAddress,Gender,Photo)values('" + txtSID.Text + "','" + txtSName.Text + "','" + txtAge.Text + "','" + txtEmail.Text + "','" + txtAddress.Text + "','" + Gender + "',@Image)";
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            clearStudent();

                            MessageBox.Show("Student Added Successful");
                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }


            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null; // Return null if the input image is null
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int radius = 50;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);

            this.Region = new Region(path);
            AddStudentPanel.Hide();
            AddClassPanel.Hide();
            AddTPanel.Hide();
            AddSSPanel.Hide();
            cboSelect();
            lblID.Text = memberID;
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

        private void btnContinue_Click(object sender, EventArgs e)
        {
            if (cboView.Text == "Student")
            {
                AddStudentPanel.Show();
                AddSSPanel.Hide();
                AddTPanel.Hide();
                AddClassPanel.Hide();

            }
            else if (cboView.Text == "Class")
            {
                AddClassPanel.Show();
                AddSSPanel.Hide();
                AddTPanel.Hide();
                AddStudentPanel.Hide();

            }
            else if(cboView.Text.Trim()=="StudentService_Member")
            {
                AddSSPanel.Show();
                AddClassPanel.Hide();
                AddStudentPanel.Hide();
                AddTPanel.Hide();
            }
            else if(cboView.Text=="Teacher")
            {
                AddTPanel.Show();
                AddClassPanel.Hide();
                AddSSPanel.Hide();
                AddStudentPanel.Hide();
            }
        }

        private void cboTeacherID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void cboSelect()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Teacher_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Teacher WHERE Teacher_List.TID = Remove_Teacher.TID)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            cboTeacherID.DataSource = ds.Tables[0];
            cboTeacherID.DisplayMember = "TID";
        }
        public void clearClass()
        {
            autoClassID();
            txtCID.Text = "";
            txtCName.Text = "";
            cboTeacherID.SelectedIndex = 0;
            txtDuration.Text = "";
            txtDay.Text = "";
            txtTime.Text = "";
        }
        public void autoClassID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(CID) from Class_List", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                txtCID.Text = "KF-C" + r.ToString("000");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearClass();
        }

        private void btnClassSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCName.Text))
            {
                MessageBox.Show("Please fill Class Name!!");
            }
            else if (cboTeacherID.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Teacher ID!!");
            }

            else if (string.IsNullOrEmpty(txtDuration.Text))
            {
                MessageBox.Show("Please fill Class Duration!!");
            }
            else if (string.IsNullOrEmpty(txtDay.Text))
            {
                MessageBox.Show("Please fill Class Days!!");
            }
            else if (string.IsNullOrEmpty(txtTime.Text))
            {
                MessageBox.Show("Please fill Class Time!!");
            }


            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Class_List where CName = '" + txtCName.Text.Trim() + "' and TID= '" + cboTeacherID.Text.Trim() + "' and Duration= '" + txtDuration.Text.Trim() + "' and Day='" + txtDay.Text.Trim() + "' and Time = '" + txtTime.Text.Trim() + "' ";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("This class has already registered.");
                    clearClass();
                }
                else
                {
                    try
                    {

                        con.Open();
                        cmd.CommandText = "insert into Class_List(CID,CName,TID,Duration,Day,Time)values('" + txtCID.Text + "','" + txtCName.Text + "','" + cboTeacherID.Text + "','" + txtDuration.Text + "','" + txtDay.Text + "','" + txtTime.Text + "')";

                        cmd.ExecuteNonQuery();
                        con.Close();
                        clearClass();

                        MessageBox.Show("New Class Added Successfully");
                    }





                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                        }
                    }
                }



            }
        }

        private void AddClassPanel_Paint(object sender, PaintEventArgs e)
        {
            autoClassID();
        }

        private void btnSSUplaod_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Select image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;

                if (IsValidFilePath(filePath))
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            pictureBox2.Image = Image.FromFile(filePath);
                        }
                        else
                        {
                            MessageBox.Show("Selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid file path format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnSSRefresh_Click(object sender, EventArgs e)
        {
            clearSS();
        }
        public void clearSS()
        {
            autoSSID();
            txtSSName.Text = "";
            txtSSAge.Text = "";
            rdbSSMale.Checked = false;
            rdbSSFemale.Checked = false;
            txtSSEmail.Text = "";
            txtSSAddress.Text = "";
            pictureBox2.Image = null;

        }
        public void autoSSID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(SSID) from StudentService_List", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                txtSSID.Text = "KF-SS" + r.ToString("000");
            }

        }

        private void btnSSSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSSName.Text))
            {
                MessageBox.Show("Please fill your Name!!");
            }
            else if (string.IsNullOrEmpty(txtSSAge.Text))
            {
                MessageBox.Show("Please fill your Age!!");
            }

            else if (string.IsNullOrEmpty(txtSSEmail.Text))
            {
                MessageBox.Show("Please fill your Email!!");
            }
            else if (string.IsNullOrEmpty(txtSSAddress.Text))
            {
                MessageBox.Show("Please fill your Address!!");
            }
            else if (rdbSSMale.Checked == false & rdbSSFemale.Checked == false)
            {
                MessageBox.Show("Please select your Gender!!");
            }

            else
            {
                try
                {


                    string Gender;
                    if (rdbSSMale.Checked == true)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }

                    if (!int.TryParse(txtSSAge.Text, out int age))
                    {
                        MessageBox.Show("Please enter a valid age.");
                        return; // Exit the method if age is not a valid integer.
                    }
                    if (age < 15)
                    {
                        MessageBox.Show("Sorry, you are too young!!");
                        return; // Exit the method if the age is less than 15.
                    }


                    else
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from StudentService_List where SSName = '" + txtSSName.Text.Trim() + "' and SSAge= '" + txtSSAge.Text.Trim() + "' and SSEmail= '" + txtSSEmail.Text.Trim() + "' and SSAddress='" + txtSSAddress.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count == 1)
                        {
                            MessageBox.Show("This member has already registered.");
                            clearClass();
                        }
                        else
                        {
                            byte[] imageData = ImageToByteArray(pictureBox2.Image);

                            con.Open();
                            cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into StudentService_List(SSID,SSName,SSAge,SSEmail,SSAddress,Gender,SSPhoto)values('" + txtSSID.Text + "','" + txtSSName.Text + "','" + txtSSAge.Text + "','" + txtSSEmail.Text + "','" + txtSSAddress.Text + "','" + Gender + "',@Image)";
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            clearSS();

                            MessageBox.Show("New Student Service Member Added Successful");

                        }

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }


            }
        }

        private void btnTUplaod_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog2 = new OpenFileDialog();

            openFileDialog2.Filter = "Select image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            DialogResult dr = openFileDialog2.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string filePath = openFileDialog2.FileName;

                if (IsValidFilePath(filePath))
                {
                    try
                    {
                        if (File.Exists(filePath))
                        {
                            pictureBox3.Image = Image.FromFile(filePath);
                        }
                        else
                        {
                            MessageBox.Show("Selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid file path format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnTRefresh_Click(object sender, EventArgs e)
        {
            clearTeacher();
        }
        public void clearTeacher()
        {
            autoTID();
            txtTName.Text = "";
            txtTAge.Text = "";
            rdbTMale.Checked = false;
            rdbTFemale.Checked = false;
            txtTEmail.Text = "";
            txtTAddress.Text = "";
            pictureBox3.Image = null;

        }
        public void autoTID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(TID) from Teacher_List", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                txtTID.Text = "KF-T" + r.ToString("000");
            }

        }

        private void btnTSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTName.Text))
            {
                MessageBox.Show("Please fill your Name!!");
            }
            else if (string.IsNullOrEmpty(txtTAge.Text))
            {
                MessageBox.Show("Please fill your Age!!");
            }

            else if (string.IsNullOrEmpty(txtTEmail.Text))
            {
                MessageBox.Show("Please fill your Email!!");
            }
            else if (string.IsNullOrEmpty(txtTAddress.Text))
            {
                MessageBox.Show("Please fill your Address!!");
            }
            else if (rdbTMale.Checked == false & rdbTFemale.Checked == false)
            {
                MessageBox.Show("Please select your Gender!!");
            }

            else
            {
                try
                {


                    string Gender;
                    if (rdbTMale.Checked == true)
                    {
                        Gender = "Male";
                    }
                    else
                    {
                        Gender = "Female";
                    }

                    if (!int.TryParse(txtTAge.Text, out int age))
                    {
                        MessageBox.Show("Please enter a valid age.");
                        return; // Exit the method if age is not a valid integer.
                    }
                    if (age < 15)
                    {
                        MessageBox.Show("Sorry, you are too young!!");
                        return; // Exit the method if the age is less than 15.
                    }


                    else
                    {
                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from Teacher_List where TName = '" + txtTName.Text.Trim() + "' and TAge= '" + txtTAge.Text.Trim() + "' and TEmail= '" + txtTEmail.Text.Trim() + "' and TAddress='" + txtTAddress.Text.Trim() + "'";
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        con.Close();
                        if (dt.Rows.Count == 1)
                        {
                            MessageBox.Show("This member has already registered.");
                            clearClass();
                        }
                        else
                        {

                            byte[] imageData = ImageToByteArray(pictureBox3.Image);

                            con.Open();
                            cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "insert into Teacher_List(TID,TName,TAge,TEmail,TAddress,TGender,TPhoto)values('" + txtTID.Text + "','" + txtTName.Text + "','" + txtTAge.Text + "','" + txtTEmail.Text + "','" + txtTAddress.Text + "','" + Gender + "',@Image)";
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            clearTeacher();

                            MessageBox.Show("New Teacher Added Successful");
                        }


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }


            }

        }

        private void AddSSPanel_Paint(object sender, PaintEventArgs e)
        {
            autoSSID();
        }

        private void AddTPanel_Paint(object sender, PaintEventArgs e)
        {
            autoTID();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f6 = new Form6(memberID);
            this.Hide();
            f6.Show();
        }
    }
    
}