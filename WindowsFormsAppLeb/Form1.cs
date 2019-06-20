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
using LiveCharts.Wpf;

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
                string sql = "select * from line_participent where  cc_name='Aqua Class'and  year_id=2017 and ac_name='Aqua Fit' and week_day_short='MON'";
            //OracleCommand cmd = new OracleCommand(sql, conn);
            //OracleDataAdapter pAdap = new OracleDataAdapter();
            // pAdap.SelectCommand = cmd;
            DataSet ds = new DataSet("dsCust");
            ds = DBUtils.GetSQLDataSet(sql, conn);

            // pAdap.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            // cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
           // cartesianChart1 = DBUtils.GetLineChartSingleDataSeries(cartesianChart1,1,"IDs",0,ds,"5");
            //conn.Clone();


            DataTable dt = ds.Tables[0];
            var x = (from r in dt.AsEnumerable()
                     select r["class_time"]).Distinct().ToList();
            var collection = new SeriesCollection();

            for (int r = 0; r < x.Count; r++)
            {
                var series = new LineSeries();
                series.Title =x[ r].ToString();

                var chartvalues = new ChartValues<double>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {


                    if (x[r].Equals(ds.Tables[0].Rows[i]["class_time"].ToString()))
                    {
                        chartvalues.Add(Convert.ToDouble( ds.Tables[0].Rows[i]["paarticipent"]));

                    }

                }
                series.Values = chartvalues;
                collection.Add(series);
                 

            }
            

            //    cartesianChart1.Series = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> {4, 6, 5, 2, 7}
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 2",
            //        Values = new ChartValues<double> {6, 7, 3, 4, 6},
            //        PointGeometry = null
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 2",
            //        Values = new ChartValues<double> {5, 2, 8, 3},
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    }
            //};
            var AxisX = new Axis();
            AxisX.Title = "Month";

                var m = (from r in dt.AsEnumerable()
                         select r["month_short"]).Distinct().ToList();
                List<string> mName = new List<string>();
                for (int i = 0; i < m.Count; i++)
                {
                    mName.Add(m[i].ToString());


                }
                AxisX.Labels = mName;
                cartesianChart1.AxisX.Add(AxisX);
                //cartesianChart1.AxisX.Add(new Axis
                //{
                //    Title = "Month",
                //    Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" ,"Jun","Jul","Aug"}
                //});

                cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Number of Participents",
              // LabelFormatter = value => value.ToString("N")
            });

            cartesianChart1.LegendLocation = LegendLocation.Right;
            cartesianChart1.Series = collection;
                
                //modifying the series collection will animate and update the chart
                //cartesianChart1.Series.Add(new LineSeries
                //{
                //    Values = new ChartValues<double> {1,2,3},
                //    LineSmoothness = 0, //straight lines, 1 really smooth lines
                //                        //  PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                //    PointGeometrySize = 10,
                //    //   PointForeground = Brushes.Gray
                //});

                //modifying any series values will also animate and update the chart
              //  cartesianChart1.Series[2].Values.Add(5d);


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
        public void createPaiChart()
        {
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
            catch (Exception ex)
            {
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

        private void FillComboProductBox()
        {
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
                MessageBox.Show("ERROR: While filling Product Combo Box");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_productsChange)
            {
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

        private void button6_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
          //  _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);
            //string sql = "SELECT units_in_stock, units_on_order FROM products WHERE product_name = 'Chocolade'";
            string sql = "select cc_name,Month_short,sum(ct_amount)Revenue from v_aqua_date_dim  where year_id = 2017  group by cc_name,year_id, month_short";
            ds = DBUtils.GetSQLDataSet(sql, conn);
            //dataGridView1.DataSource = ds.Tables[0];

            DataTable dt = ds.Tables[0];
            var x = (from r in dt.AsEnumerable()
                     select r["cc_name"]).Distinct().ToList();
            var collection = new SeriesCollection();

            for (int r = 0; r < x.Count; r++)
            {
                var series = new ColumnSeries();
                series.Title = x[r].ToString();

                var chartvalues = new ChartValues<double>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {


                    if (x[r].Equals(ds.Tables[0].Rows[i]["cc_name"].ToString()))
                    {
                        chartvalues.Add(Convert.ToDouble(ds.Tables[0].Rows[i]["Revenue"]));

                    }

                }
                series.Values = chartvalues;
                collection.Add(series);


            }

            cartesianChart2.Series = collection;


            //cartesianChart2.Series = new SeriesCollection
            //{
            //    new ColumnSeries
            //    {
            //        Title = "2015",
            //        Values = new ChartValues<double> { 10, 50, 39, 50 }
            //    }
            //};

            ////adding series will update and animate the chart automatically
            //cartesianChart2.Series.Add(new ColumnSeries
            //{
            //    Title = "2016",
            //    Values = new ChartValues<double> { 11, 56, 42 }
            //});

            //also adding values updates and animates the chart automatically
            cartesianChart2.Series[1].Values.Add(48d);

            var AxisX = new Axis();
            AxisX.Title = "Month";

            var m = (from r in dt.AsEnumerable()
                     select r["month_short"]).Distinct().ToList();
            List<string> mName = new List<string>();
            for (int i = 0; i < m.Count; i++)
            {
                mName.Add(m[i].ToString());


            }
            AxisX.Labels = mName;
            cartesianChart2.AxisX.Add(AxisX);

            //cartesianChart2.AxisX.Add(new Axis
            //{
            //    Title = "Sales Man",
            //    Labels = new[] { "Maria", "Susan", "Charles", "Frida" }
            //});

            cartesianChart2.AxisY.Add(new Axis
            {
                Title = "Revanue",
                LabelFormatter = value => value.ToString("C")
            });
        }
    }
}
