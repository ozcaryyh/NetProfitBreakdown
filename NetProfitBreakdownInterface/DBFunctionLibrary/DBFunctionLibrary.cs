using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace AmazonManagementSystem.DBFunctionLibrary
{
    public class DBFunctionLibrary
    {
        public DBFunctionLibrary()
        {
            _connectionString = "Data Source=192.168.103.150\\INFLOWSQL;User ID=mws;Password=p@ssw0rd";
        }

        private String _connectionString;
        public String ConnectionString
        {
            set
            {
                _connectionString = value;
            }
            get
            {
                return _connectionString;
            }
        }

        public DataSet GetAllData(string query)
        {
            SqlCommand cmd = new SqlCommand(query);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        public DataSet GetDataByCriteria(string query, string value)
        {
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@parameter", value);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        return ds;
                    }
                }
            }
        }

        private DataSet AddCalculatedColumn(DataSet originalDataSet, string columnName, string expression)
        {
            originalDataSet.Tables[0].Columns.Add(columnName, typeof(double), expression);

            return originalDataSet;
        }

        private DataSet FilterSortData(DataSet originalDataSet, string filter, string sortExpression)
        {
            DataSet sortDS = originalDataSet.Clone();
            DataRow[] drs = originalDataSet.Tables[0].Select(filter, sortExpression);

            foreach (DataRow dr in drs)
            {
                sortDS.Tables[0].ImportRow(dr);
            }

            return sortDS;
        }

        public DataSet GetAllSortedData(string sMerchantID)
        {
            return GetAllSortedDataByMerchantID("", sMerchantID);
        }

        public DataSet GetAllSortedDataByMerchantID(string sortExpression, string sMerchantID)
        {
            string sSelectQuery = "SELECT [MWS].[dbo].[ProductAvailability].[FNSKU] AS " + '"' + "FNSKU" + '"'
                 + ",[inFlow].[dbo].[BASE_Product].[Name] AS " + '"' + "SellerSKU" + '"'
                 + ",[inFlow].[dbo].[BASE_Product].[Description] AS " + '"' + "ProductTitle" + '"'
                 + ",ISNULL([inFlow].[dbo].[Base_InventoryCost].[AverageCost],0) AS " + '"' + "Cost" + '"'
                 + ",[MWS].[dbo].[ProductAvailability].[Inbound] AS " + '"' + "Inbound" + '"'
                 + ",[MWS].[dbo].[ProductAvailability].[Fulfillable] AS " + '"' + "Fulfillable" + '"'
                 + ",[MWS].[dbo].[ProductAvailability].[MerchantID]"
                 + "FROM [inFlow].[dbo].[BASE_Product] "
                 + "INNER JOIN [inFlow].[dbo].[BASE_InventoryCost] ON [inFlow].[dbo].[BASE_Product].[ProdId]=[inFlow].[dbo].[BASE_InventoryCost].[ProdId] "
                 + "INNER JOIN [MWS].[dbo].[ProductAvailability] ON [MWS].[dbo].[ProductAvailability].SellerSKU = [inFlow].[dbo].[BASE_Product].[Name] "
                 + "WHERE [inFlow].[dbo].[BASE_InventoryCost].[CurrencyId] = '8'"
                 + "AND ( MerchantID =" + "'" + sMerchantID + "')";

            DataSet ds = GetAllData(sSelectQuery);
            ds = AddCalculatedColumn(ds, "InboundTotal", "Inbound * Cost");
            ds = AddCalculatedColumn(ds, "FulfillableTotal", "Fulfillable * Cost");
            ds = AddCalculatedColumn(ds, "SubTotal", "InboundTotal + FulfillableTotal");

            if (!String.IsNullOrEmpty(sortExpression.Trim()))
            {
                ds = FilterSortData(ds, "", sortExpression);
            }

            return ds;
        }

        public DataSet GetDataByFilter(string parameter, string value)
        {
            string query = "SELECT [MWS].[dbo].[ProductAvailability].[FNSKU] AS " + '"' + "FNSKU" + '"'
                + ",[inFlow].[dbo].[BASE_Product].[Name] AS " + '"' + "SellerSKU" + '"'
                + ",[inFlow].[dbo].[BASE_Product].[Description] AS " + '"' + "" + '"'
                + ",ISNULL([inFlow].[dbo].[Base_InventoryCost].[AverageCost],0) AS " + '"' + "Cost" + '"'
                + ",[MWS].[dbo].[ProductAvailability].[Inbound] AS " + '"' + "Inbound" + '"'
                + ",[MWS].[dbo].[ProductAvailability].[Fulfillable] AS " + '"' + "Fulfillable" + '"'
                + "FROM [inFlow].[dbo].[BASE_Product] "
                + "INNER JOIN [inFlow].[dbo].[BASE_InventoryCost] ON [inFlow].[dbo].[BASE_Product].[ProdId]=[inFlow].[dbo].[BASE_InventoryCost].[ProdId] "
                + "INNER JOIN [MWS].[dbo].[ProductAvailability] ON [MWS].[dbo].[ProductAvailability].SellerSKU = [inFlow].[dbo].[BASE_Product].[Name] "
                + "WHERE [inFlow].[dbo].[BASE_InventoryCost].[CurrencyId] = '8'"
                + "AND (" + parameter + " LIKE '%'+@parameter+'%')";

            DataSet ds = GetDataByCriteria(query, value);
            ds = AddCalculatedColumn(ds, "InboundTotal", "Inbound * Cost");
            ds = AddCalculatedColumn(ds, "FulfillableTotal", "Fulfillable * Cost");
            ds = AddCalculatedColumn(ds, "SubTotal", "InboundTotal + FulfillableTotal");

            ds = FilterSortData(ds, "", "SellerSKU ASC");

            return ds;
        }

        public DataSet GetSalesRecord(string parameter, string value)
        {
            string query = "SELECT [MWS].[dbo].[SalesRecord].[FNSKU] AS " + '"' + "FNSKU" + '"'
                + ",[MWS].[dbo].[SalesRecord].[AccountID] AS " + '"' + "AmazonAccountID" + '"'
                + ",[MWS].[dbo].[SalesRecord].[Quantity] AS " + '"' + "Quantity" + '"'
                + ",[MWS].[dbo].[SalesRecord].[CurrencyID] AS " + '"' + "CurrencyID" + '"'
                + ",[MWS].[dbo].[SalesRecord].[Date] AS " + '"' + "Date" + '"'
                + "FROM [MWS].[dbo].[SalesRecord] ";
            DataSet ds = GetAllData(query);
            
            return ds;
        }
    }
}