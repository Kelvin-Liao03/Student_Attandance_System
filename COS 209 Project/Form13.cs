using System;
using System.CodeDom;
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
    public partial class Form13 : Form
    {
        string memberID;
        public Form13(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");


        private void button2_Click(object sender, EventArgs e)
        {

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
        private void fillEmail()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Student_List where SID = '" + txtID.Text + "'";
            cmd.ExecuteNonQuery();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            con.Close();
            if(dt.Rows.Count==1)
            {
                   txtEmail.Text = dt.Rows[0]["SEmail"].ToString();
                
            }
          
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(txtID.Text != null & txtEmail.Text !=null & txtRemark.Text != null)
            {

                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into SendEmail(ID,SID,SEmail,Remark,MemberID)values('"+lblSendEmailID.Text+"','" + txtID.Text + "','" + txtEmail.Text + "','" + txtRemark.Text + "','"+lblID.Text+"')";
                
                cmd.ExecuteNonQuery();
                con.Close();
                

                MessageBox.Show("Sent Email Successful");
                autoSendEmailID();
                Refresh();
            }
            else
            {
                MessageBox.Show("Please fill all fields!!");
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void autoSendEmailID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(ID) from SendEmail", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                lblSendEmailID.Text = "ID-" + r.ToString("000");
            }
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            autoSendEmailID();
            lblID.Text = memberID;
            lblSendEmailID.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f11 = new Form11(memberID);
            this.Hide();
            f11.Show();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

            fillEmail();
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void Refresh()
        {
            
            txtID.Text = null;
            txtEmail.Text = null;
            txtRemark.Text = null;
        }
    }
}
