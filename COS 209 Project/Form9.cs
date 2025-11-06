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
    public partial class Form9 : Form
    {
        public string memberID;
        public string id;
        bool updateClicked = false;
        public Form9(string id,string memberID)
        {
            InitializeComponent();
            this.id = id;
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
        private void cboSelect()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Teacher_List", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            cboTeacherID.DataSource = ds.Tables[0];
            cboTeacherID.DisplayMember = "TID";
            cboTeacherID.SelectedIndex = -1;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            if (id.Trim().StartsWith("KF-SS"))
            {
                UpdateStudentPanel.Hide();
                UpdateClassPanel.Hide();
                UpdateTPanel.Hide();
                UpdateSSPanel.Show();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from StudentService_List where SSID = '" + id + "'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                txtSSID.Text = dt.Rows[0]["SSID"].ToString();
                txtSSName.Text = dt.Rows[0]["SSName"].ToString();
                txtSSAge.Text = dt.Rows[0]["SSAge"].ToString();
                txtSSEmail.Text = dt.Rows[0]["SSEmail"].ToString();
                txtSSAddress.Text = dt.Rows[0]["SSAddress"].ToString();
                if (dt.Rows[0]["Gender"].ToString() == "Male")
                {
                    rdbSSMale.Checked = true;
                }
                else
                {
                    rdbSSFemale.Checked = true;
                }

                byte[] imageData = (byte[])dt.Rows[0]["SSPhoto"];
                DisplayImage(imageData, pictureBox2);

            }
            else if (id.Trim().StartsWith("KF-S"))
            {
                UpdateStudentPanel.Show();
                UpdateClassPanel.Hide();
                UpdateSSPanel.Hide();
                UpdateTPanel.Hide();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Student_List where SID = '"+id+"'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                txtSID.Text = dt.Rows[0]["SID"].ToString();
                txtSName.Text = dt.Rows[0]["SName"].ToString();
                txtAge.Text = dt.Rows[0]["SAge"].ToString();
                txtEmail.Text = dt.Rows[0]["SEmail"].ToString();
                txtAddress.Text = dt.Rows[0]["SAddress"].ToString();
                if (dt.Rows[0]["Gender"].ToString() == "Male")
                {
                    rdbMale.Checked = true;
                }
                else
                {
                    rdbFemale.Checked = true;
                }
                
                byte[] imageData = (byte[])dt.Rows[0]["Photo"];
                DisplayImage(imageData, pictureBox1);

            }
            else if(id.Trim().StartsWith("KF-C"))
            {
                UpdateStudentPanel.Hide();
                UpdateClassPanel.Show();
                UpdateTPanel.Hide();
                UpdateSSPanel.Hide();
                cboSelect();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Class_List where CID='" + id + "'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                txtCID.Text = dt.Rows[0]["CID"].ToString();
                txtCName.Text = dt.Rows[0]["CName"].ToString();
                cboTeacherID.Text = dt.Rows[0]["TID"].ToString();
                txtDuration.Text = dt.Rows[0]["Duration"].ToString();
                txtDay.Text = dt.Rows[0]["Day"].ToString();
                txtTime.Text = dt.Rows[0]["Time"].ToString();
            }
            else if (id.Trim().StartsWith("KF-T"))
            {
                UpdateStudentPanel.Hide();
                UpdateClassPanel.Hide();
                UpdateTPanel.Show();
                UpdateSSPanel.Hide();
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Teacher_List where TID = '" + id + "'";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                txtTID.Text = dt.Rows[0]["TID"].ToString();
                txtTName.Text = dt.Rows[0]["TName"].ToString();
                txtTAge.Text = dt.Rows[0]["TAge"].ToString();
                txtTEmail.Text = dt.Rows[0]["TEmail"].ToString();
                txtTAddress.Text = dt.Rows[0]["TAddress"].ToString();
                if (dt.Rows[0]["TGender"].ToString() == "Male")
                {
                    rdbTMale.Checked = true;
                }
                else
                {
                    rdbTFemale.Checked = true;
                }

                byte[] imageData = (byte[])dt.Rows[0]["TPhoto"];
                DisplayImage(imageData, pictureBox3);

            }
            

        }
        private void DisplayImage(byte[] imageData, PictureBox pictureBox)
        {
            try
            {
                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f8 = new Form8(memberID);
            this.Hide();
            f8.Show();
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
                        using (Image newImage = Image.FromFile(filePath))
                        {
                            pictureBox1.Image = new Bitmap(newImage); // Create a new bitmap to avoid GDI+ errors
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    updateClicked = true;
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
                        if(updateClicked)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                if (pictureBox1.Image != null)
                                {
                                    using (Image clonedImage = (Image)pictureBox1.Image.Clone())
                                    {
                                        clonedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        byte[] imageData = ms.ToArray();

                                        con.Open();
                                        SqlCommand cmd = con.CreateCommand();
                                        cmd.CommandType = CommandType.Text;
                                        cmd.CommandText = "update Student_List set SName = '" + txtSName.Text + "',SAge = '" + txtAge.Text + "',SEmail = '" + txtEmail.Text + "',SAddress='" + txtAddress.Text + "',Gender='" + Gender + "',Photo = @Image where SID = '" + txtSID.Text + "'";
                                        cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                        MessageBox.Show("Student " + txtSID.Text + " Info is updated succedssfully.");

                                    }

                                }

                            }
                        }
                        else
                        {
                            MessageBox.Show("Please upload photo. If you don't want to change new photo. you can insert the old one.");
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
               
                    try
                    {

                        con.Open();
                        SqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "update Class_List set CID='"+txtCID.Text+"',CName='"+txtCName.Text+"',TID='"+cboTeacherID.Text+"',Duration='"+txtDuration.Text+"',Day='"+txtDay.Text+"',Time='"+txtTime.Text+"' where CID='"+txtCID.Text+"'";

                        cmd.ExecuteNonQuery();
                        con.Close();
                        

                        MessageBox.Show("Class "+ txtCID.Text+ "  Info Updated Successfully");
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
                    updateClicked = true;
                }
                else
                {
                    MessageBox.Show("Invalid file path format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                        if(updateClicked)
                        {
                            byte[] imageData = ImageToByteArray(pictureBox3.Image);

                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "update Teacher_List set TName = '" + txtTName.Text + "',TAge = '" + txtTAge.Text + "',TEmail = '" + txtTEmail.Text + "',TAddress='" + txtTAddress.Text + "',TGender='" + Gender + "',TPhoto = @Image where TID = '" + txtTID.Text + "'";
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Teacher " + txtTID.Text + " Info is updated succedssfully.");
                        }

                        else
                        {
                            MessageBox.Show("Please upload photo. If you don't want to change new photo. you can insert the old one.");

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
                    updateClicked = true;
                }
                else
                {
                    MessageBox.Show("Invalid file path format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                        if(updateClicked)
                        {

                            byte[] imageData = ImageToByteArray(pictureBox2.Image);

                            con.Open();
                            SqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "update StudentService_List set SSName = '" + txtSSName.Text + "',SSAge = '" + txtSSAge.Text + "',SSEmail = '" + txtSSEmail.Text + "',SSAddress='" + txtSSAddress.Text + "',Gender='" + Gender + "',SSPhoto = @Image where SSID = '" + txtSSID.Text + "'";
                            cmd.Parameters.Add(new SqlParameter("@Image", SqlDbType.VarBinary)).Value = imageData;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Student Service Member " + txtSSID.Text + " Info is updated succedssfully.");
                        }
                        else
                        {

                            MessageBox.Show("Please upload photo. If you don't want to change new photo. you can insert the old one.");

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

        private void UpdateClassPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
