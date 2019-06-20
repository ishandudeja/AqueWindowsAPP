using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using DBUtil;


namespace AquaGymID
{
    public partial class Classes : UserControl
    {
        string _productSlicer;

        public Classes()
        {
            InitializeComponent();
            PrepareData();
            _productSlicer = "-- ALL --";
            CreateLineCharts();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            _productSlicer = comboBox1.Text;
            CreateLineCharts();

        }

        public void PrepareData()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;

            conn.Open();

            try
            {

                string sql = "select * from v_categories";

                DataSet ds = new DataSet();

                ds = DBUtils.GetSQLDataSet(sql, conn);

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "cc_name";
                comboBox1.SelectedIndex = 0;

                _productSlicer = comboBox1.Text;

                comboBox1.Show();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }

            conn.Close();



        }


        private void CreateLineCharts()
        {

            string sql;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = Variables.connString;
            conn.Open();

            try
            {
                DataSet ds = new DataSet();
                _productSlicer = DBUtils.GetSQLQuoteInputFilter(_productSlicer);

                sql = "select * from V_ACTIVHOUR where activity = '" + _productSlicer + "' order by ac_time";

                ds = DBUtils.GetSQLDataSet(sql, conn);

                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
                cartesianChart1 = DBUtils.GetLineChartSingleDataSeries(cartesianChart1, 0, "Participants", 1, ds, "*");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conn.Close();



        }


    }
}
