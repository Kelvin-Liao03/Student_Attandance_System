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
    public partial class Form14 : Form
    {
        string memberID;
        public Form14(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");



        private void label3_Click(object sender, EventArgs e)
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

        private void Form14_Load(object sender, EventArgs e)
        {
            lblID.Text = memberID;
            txtID.Text = memberID;
            autoSendReportID();
            lblSendReportID.Hide();
            txtEmailFrom.Text = "studentserviceteamkf@gmail.com";
            cboSelect();
        }
        public void autoSendReportID()
        {
            try
            {
                int r = 000;
                con.Open();
                SqlCommand cmd = new SqlCommand("select max(SRID) from SendReport", con);
                String var = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                string re = @"[0-9]+";
                Regex regex = new Regex(re);
                MatchCollection match = regex.Matches(var);
                for (int count = 0; count < match.Count; count++)
                {
                    r = Convert.ToInt32(match[count].Value) + 1;
                    lblSendReportID.Text = "ID-" + r.ToString("000");

                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            
        
           finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
    
        private void fillEmail()
        {
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Student_List where SID = '" + txtID.Text + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count == 1)
                {
                    txtEmailFrom.Text = dt.Rows[0]["SEmail"].ToString();
                }
                else
                {
                    MessageBox.Show("Invalid ID Number.");
                }
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
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

        private void txtEmailFrom_TextChanged(object sender, EventArgs e)
        {
            fillEmail();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtID.Text != null & txtEmailTo.Text != null & txtEmailFrom.Text != null & txtRemark.Text != null)
            {

                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "insert into SendReport(SRID,SID,CName,Reason)values('" + lblSendReportID.Text + "','" + txtID.Text + "','" + cboView.Text + "','" + txtRemark.Text + "')";
                Refresh();
                cmd.ExecuteNonQuery();
                con.Close();


                MessageBox.Show("Sent Email Successful");
            }
            else
            {
                MessageBox.Show("Please fill all fields!!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f11 = new Form11(memberID);
            this.Hide();
            f11.Show();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Refresh()
        {
           // autoSendReportID();
            txtRemark.Text = "";
            cboView.Text="Select";
        }
    }
}