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
    public partial class TopSessions : UserControl
    {

        private string _productSlicer;
        private bool _monthChange;

        public TopSessions()
        {
            InitializeComponent();
            PrepareData();
            _productSlicer = "-- ALL --";
            CreateBarCharts();
            CreateBarCharts2();

        }

        public void PrepareData() {
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
            else {
                _monthChange = false;
            }
            _productSlicer = comboBox1.Text;
            CreateBarCharts();
            CreateBarCharts2();

        }

        private void CreateBarCharts()
        {

            string sql;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;
            conn.Open();

            try
            {
                DataSet ds = new DataSet();
                _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);
                //string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = 'Chocolade'";



                if (_productSlicer == "-- ALL --")
                {
                    sql = "SELECT sum(amount) amount, ac_name FROM V_TOPCLASSES group by ac_name  order by amount desc";
                }
                else {
                    sql = "SELECT* FROM V_TOPCLASSES WHERE tmonth= '" + _productSlicer + "' order by amount desc";
                }



                ds = DBUtils.GetSQLDataSet(sql, conn);

                
                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
                cartesianChart1 = DBUtils.GetColumnChartSingleDataSeries(cartesianChart1, 1, "Sessions", 0, ds, "5");



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conn.Close();

        }


        private void CreateBarCharts2()
        {

            string sql;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;
            conn.Open();

            try
            {
                DataSet ds = new DataSet();
                _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);
                //string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = 'Chocolade'";



                if (_productSlicer == "-- ALL --")
                {
                    sql = "SELECT sum(amount) amount, ac_name FROM V_TOPCLASSES group by ac_name  order by amount ";
                }
                else
                {
                    sql = "SELECT * FROM V_TOPCLASSES WHERE tmonth= '" + _productSlicer + "' order by amount ";
                }



                ds = DBUtils.GetSQLDataSet(sql, conn);

                cartesianChart2 = DBUtils.InitialiseCartesianChart(cartesianChart2);
                cartesianChart2 = DBUtils.GetColumnChartSingleDataSeries(cartesianChart2, 1, "Participants", 0, ds, "5");



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conn.Close();

        }

        
    }
}

