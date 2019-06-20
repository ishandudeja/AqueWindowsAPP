using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using DBUtil;


namespace AquaGymID
{
    public partial class Form1 : Form
    {
        public string sqlquery;

        public Form1()
        {
            InitializeComponent();

            LoadMainData();
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            topSessions1.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button2.Height;
            SidePanel.Top = button2.Top;
            attendance1.BringToFront();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            topSessions1.BringToFront();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button3.Height;
            SidePanel.Top = button3.Top;
            dryWet1.BringToFront();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button4.Height;
            SidePanel.Top = button4.Top;
            classes1.BringToFront();

        }

        private void LoadMainData()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                conn.Open();
                DataSet ds = new DataSet();
                DataTable dt;
                sqlquery = "select count(distinct activity) activities from CLASS_TIMETABLED ct JOIN AQUA_CLASS AC ON ct.CT_AC_ID = AC.AC_ID WHERE CT_DATE >= to_date('04-08-2017','DD-MM-YYYY')"; 


                ds = DBUtils.GetSQLDataSet(sqlquery, conn);

                dt = ds.Tables[0];

                label4.Text = dt.Rows[0][0].ToString();
                conn.Close();

                sqlquery = "SELECT * FROM V_LASTWEEK";

                conn.Open();
                ds = DBUtils.GetSQLDataSet(sqlquery, conn);
                dt = ds.Tables[0];

                label2.Text = dt.Rows[0][0].ToString() + "%";
                label1.Text = dt.Rows[0][1].ToString();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }

            conn.Close();
            Cursor.Current = Cursors.Arrow;

        }

    }
}
