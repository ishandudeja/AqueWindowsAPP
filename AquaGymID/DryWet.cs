using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using DBUtil;


namespace AquaGymID
{
    public partial class DryWet : UserControl
    {

        private string _productSlicer;
        private bool _monthChange;

        public DryWet()
        {
            InitializeComponent();

            PrepareData();
            _productSlicer = "-- ALL --";
            CreatePieCharts();


        }

        public void PrepareData()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;

            conn.Open();

            try
            {

                string sql = "select * from v_AllMonths order by 2";

                DataSet ds = new DataSet();

                ds = DBUtils.GetSQLDataSet(sql, conn);

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "tmonth";
                comboBox1.SelectedIndex = 0;

                _productSlicer = comboBox1.Text;
                _monthChange = true;

                comboBox1.Show();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }

            conn.Close();



        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_productSlicer != comboBox1.Text)
            {
                _monthChange = true;
            }
            else
            {
                _monthChange = false;
            }
            _productSlicer = comboBox1.Text;
            CreatePieCharts();


        }

        private void CreatePieCharts()
        {

            string sql;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;
            conn.Open();

            pieChart1.InnerRadius = 50;

            try
            {
                DataSet ds = new DataSet();
                _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);
                //string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = 'Chocolade'";



                sql = "SELECT * FROM V_WETDRYPARTICIPANTS WHERE tmonth= '" + _productSlicer + "' ";


                ds = DBUtils.GetSQLDataSet(sql, conn);

                pieChart1 = DBUtils.InitialisePieChart(pieChart1);
                pieChart1 = DBUtils.GetPieChartDataSeries(pieChart1, 0, 1, 60, ds, "*");

                pieChart2 = DBUtils.InitialisePieChart(pieChart2);
                pieChart2 = DBUtils.GetPieChartDataSeries(pieChart2, 0, 2, 60, ds, "*");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conn.Close();

        }


    }
}

