using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AmazonManagementSystem.DBFunctionLibrary;
using AmazonManagementSystem.DBFunctionLibrary.SalesRecordTableAdapters;

namespace AmazonManagementSystem.NetProfitBreakdown
{
    public partial class Daily : System.Web.UI.Page
    {
        private String _queryDay;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            
            _queryDay = Request.QueryString["day"];
            if (String.IsNullOrEmpty(_queryDay))
            {
                _queryDay = DateTime.Now.ToString("yyyyMMdd");
            }

            QueriesTableAdapter qtAdapter = new QueriesTableAdapter();
            //GridView1.DataSource = qtAdapter.NetProfitQuery();
            //GridView1.DataBind();
            DetailsView1.DataSource = qtAdapter.CurrencyRateQuery();
            DetailsView1.DataBind();
        }

        protected void tmrUpdate_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        }

        protected void ListView1_PreRender(object sender, EventArgs e)
        {
            
        }

        protected void ListView1tiesChanged(object sender, EventArgs e)
        {

        }

        protected void ListView1_ItemCommand1(object sender, ListViewCommandEventArgs e)
        {
            
        }

        protected void ListView1_Sorting(object sender, ListViewSortEventArgs e)
        {
            //this cannot be deleted , to handle the sorting event
        }

        protected void DataPager1_PreRender(object sender, EventArgs e)
        {
            this.ListView1.DataSourceID = null;
            this.ListView1.DataSource = this.ObjectDataSource2;
            this.ListView1.DataBind();
        }

        protected void MaxiumRecordTextBox_OnTextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            try
            {
                this.DataPager1.PageSize = Convert.ToInt32(t.Text);
            }
            catch (Exception ex)
            {
                this.DataPager1.PageSize = 10;
            }
        }

        public DataSet GetNetProfitData()
        {
            return GetNetProfitData("A2TYS339AQK0NU");
        }

        public DataSet GetNetProfitData(string sMerchantID)
        {
            //QueriesTableAdapter qtAdapter = new QueriesTableAdapter();
            String _connectionString = "Data Source=192.168.103.150\\INFLOWSQL;User ID=mws;Password=p@ssw0rd";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetNetProfitDataByDate", con);
                DateTime dt1 = DateTime.Today;
                dt1.AddDays(-5);
                cmd.Parameters.AddWithValue("@STARTDATE", dt1);
                DateTime dt2 = DateTime.Today;
                cmd.Parameters.AddWithValue("@ENDDATE", dt2);
                cmd.Parameters.AddWithValue("@MERCHANT_ID", sMerchantID);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {   
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }
    }
}