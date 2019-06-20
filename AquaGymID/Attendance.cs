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
    public partial class Attendance : UserControl
    {
        private string _productSlicer;
        private string _productSlicer2;

        public Attendance()
        {
            InitializeComponent();

            PrepareData();
            _productSlicer = "-- ALL --";
            _productSlicer2 = "-- ALL --";
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

                string sql = "select * from v_AllActivities order by activity ";

                DataSet ds = new DataSet();

                ds = DBUtils.GetSQLDataSet(sql, conn);

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.DisplayMember = "Activity";
                comboBox1.SelectedIndex = 0;

                _productSlicer = comboBox1.Text;

                comboBox1.Show();


            }

            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }

            conn.Close();

            conn.Open();

            try
            {

                string sql = "select * from v_AllActivities order by activity ";

                DataSet ds = new DataSet();

                ds = DBUtils.GetSQLDataSet(sql, conn);

                comboBox2.DataSource = ds.Tables[0];
                comboBox2.DisplayMember = "Activity";
                comboBox2.SelectedIndex = 0;

                comboBox2.Show();


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
                _productSlicer2 = DBUtils.GetSQLQuoteInputFilter(_productSlicer2);



                sql = "select tm, nm,nvl((select att from ATTE a where nmo = nm and aty = '" + _productSlicer + "'),0)at1,nvl((select att from  ATTE b where nmo = nm and aty = '" + _productSlicer2 + "'),0)at2 from tm t order by 2";


                ds = DBUtils.GetSQLDataSet(sql, conn);

                cartesianChart1 = DBUtils.InitialiseCartesianChart(cartesianChart1);
                cartesianChart1 = DBUtils.GetLineChartDoubleDataSeries(cartesianChart1, 0, 2, 3, "% Attendance " + _productSlicer, "% Attendance " + _productSlicer2, ds, "12");

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
            conn.Close();



        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _productSlicer2 = comboBox2.Text;
            CreateLineCharts();
        }
    }
}
