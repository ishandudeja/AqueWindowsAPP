using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using LiveCharts;
using LiveCharts.Defaults; //Contains the already defined types
using LiveCharts.WinForms; //the WinForm wrappers
using DBUtil;
namespace WindowsFormsAppLeb
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            angularGauge1.Hide();
            comboBox1.Hide();
        }

        private string _productSlicer;
        private bool _productsChange = false;
        //grobal connection
        private OracleConnection conn;
        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
                                    "(HOST=info706.cwwvo42siq12.ap-southeast-2.rds.amazonaws.com)(PORT=1521))(CONNECT_DATA=" +
                                    "(SERVER=DEDICATED)(SERVICE_NAME=ORCL)));Password=" +
                                textBox2.Text + ";User ID=" + textBox1.Text + ";";
                 conn = new OracleConnection();
                conn.ConnectionString = connString;
                conn.Open(); // check server connection
                             // MessageBox.Show("Connected...");
                label5.Text = "Connected";
                conn.Close();
                tabControl1.Visible = true;
                FillComboProductBox();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from CUSTOMERS";
                //OracleCommand cmd = new OracleCommand(sql, conn);
                //OracleDataAdapter pAdap = new OracleDataAdapter();
               // pAdap.SelectCommand = cmd;
                DataSet ds = new DataSet("dsCust");
                ds = DBUtils.GetSQLDataSet(sql, conn);

               // pAdap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
                cartesianChart1 = DBUtils.GetLineChartSingleDataSeries(cartesianChart1,1,"IDs",0,ds,"5");
                //conn.Clone();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "select * from  PRODUCTS";
                OracleCommand cmd = new OracleCommand(sql, conn);
                OracleDataAdapter pAdap = new OracleDataAdapter();
                pAdap.SelectCommand = cmd;
                DataSet ds = new DataSet("dsCust");
                pAdap.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Clone();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void createPaiChart() {
            try
            {
                DataSet ds = new DataSet();
                string sql = "SELECT UNIQUE country, COUNT(country) FROM customers GROUP BY country ORDER BY COUNT(COUNTRY) DESC";
                string amt = "5";
                ds = DBUtils.GetSQLDataSet(sql, conn);
                pieChart1 = DBUtils.InitialisePieChart(pieChart1);
                pieChart1 = DBUtils.GetPieChartDataSeries(pieChart1, 0, 1, 20, ds, amt);
                pieChart1.Show();
                lblPieChart.Visible = true;
                lblPieChart.Text = "Top " + amt + " country by per customer";

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            createPaiChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FillComboProductBox() {
            try
            {
                string sql = "SELECT product_name FROM lkup_product_by_name ORDER BY product_name";
                DataSet ds = new DataSet();
                ds = DBUtils.GetSQLDataSet(sql, conn);
                dataGridView1.DataSource = ds.Tables[0];
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "PRODUCT_NAME";
                comboBox1.SelectedIndex = 0;
                _productSlicer = comboBox1.Text;
                _productsChange = true;
                comboBox1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show( "ERROR: While filling Product Combo Box");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_productsChange) {
                _productSlicer = comboBox1.Text;
                CreateAngularProductsChart();
            }
        }

        private void CreateAngularProductsChart()
        {
            try
            {
                DataSet ds = new DataSet();
                _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);
                //string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = 'Chocolade'";
                string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = '" + _productSlicer + "'";
                ds = DBUtils.GetSQLDataSet(sql, conn);
                dataGridView1.DataSource = ds.Tables[0];

                // Get the value to graph from the data set
                DataTable dt = ds.Tables[0];
                int inStock = int.Parse(dt.Rows[0][0].ToString()); // Row 0, Col 0
                int onOrder = int.Parse(dt.Rows[0][1].ToString()); // Row 0, Col 1

                angularGauge1 = DBUtils.SetAngularChart(angularGauge1, inStock, 100);
                //angularGauge1 = DBUtils.SetAngularChart(angularGauge1, onOrder, 100);
                angularGauge1.Show();
                //lblStock.Text = lblStock.Text + "Chocolade";
                lblStock.Text = "Products In Stock: " + _productSlicer;
                //angularGauge1.Show();

            }
            catch (Exception ex)
            {
               MessageBox.Show("ERROR: SQL Query did not Execute...");
            }
            conn.Close();
        }

    }
}
