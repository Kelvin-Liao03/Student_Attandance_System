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
    public partial class Form6 : Form
    {
        public string memberID;
        public Form6(string memberID)
        {
            InitializeComponent();
            this.memberID = memberID;
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\Documents\K_Foundation.mdf;Integrated Security=True;Connect Timeout=30");



        private void Form6_Load(object sender, EventArgs e)
        {
            autoEnrollID();
            cboCID.SelectedIndex = -1;
            cboSID.SelectedIndex = -1;
            cboCIDSelect();
            cboSIDSelect();
            lblID.Text = memberID;
        }
        private void cboCIDSelect()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Class_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Class WHERE Class_List.CID = Remove_Class.CID)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            cboCID.DataSource = ds.Tables[0];
            cboCID.DisplayMember = "CID";
            cboCID.SelectedIndex = -1;
            cboCID.Text = "Select";
        }
        private void cboSIDSelect()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Student_List WHERE NOT EXISTS ( SELECT 1 FROM Remove_Student WHERE Student_List.SID = Remove_Student.SID)", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteNonQuery();
            con.Close();
            cboSID.DataSource = ds.Tables[0];
            cboSID.DisplayMember = "SID";
            cboSID.SelectedIndex = -1;
            cboSID.Text = "Select";
        }
        public void autoEnrollID()
        {
            int r = 000;
            con.Open();
            SqlCommand cmd = new SqlCommand("select max(EID) from Enrollment", con);
            String var = Convert.ToString(cmd.ExecuteScalar());
            con.Close();
            string re = @"[0-9]+";
            Regex regex = new Regex(re);
            MatchCollection match = regex.Matches(var);
            for (int count = 0; count < match.Count; count++)
            {
                r = Convert.ToInt32(match[count].Value) + 1;
                txtEID.Text = "Enroll-" + r.ToString("000");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void Refresh()
        {
            autoEnrollID();
            cboSID.Text="Select";
            cboCID.Text="Select";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(cboCID.SelectedIndex==-1)
            {
                MessageBox.Show("Please select Class ID.");
            }
            else if(cboSID.SelectedIndex==-1)
            {
                MessageBox.Show("Please select Student ID.");
            }
            else
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Enrollment where SID = '" + cboSID.Text.Trim() + "' and CID= '" + cboCID.Text.Trim() + "' ";
                cmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                con.Close();
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("This student has already enrolled.");
                    Refresh();
                }
                else
                {
                    con.Open();
                    cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into Enrollment(EID,SID,CID)values('" + txtEID.Text + "','" + cboSID.Text + "','" + cboCID.Text + "')";

                    cmd.ExecuteNonQuery();
                    con.Close();
                    Refresh();
                    autoEnrollID();

                    MessageBox.Show("New Student Enrollment Added Successful");



                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var f5=new Form5(memberID);
            this.Hide();
            f5.Show();
        }
    }
}
