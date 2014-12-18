using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmazonManagementSystem.DBFunctionLibrary
{
    public class SQLGenerator
    {
        private String parameter;
        public String Parameter
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    parameter = value;
                }
            }
        }

        private String inventorySQL = "SELECT [MWS].[dbo].[ProductAvailability].[FNSKU] AS " + '"' + "FNSKU" + '"'
                + ",[inFlow].[dbo].[BASE_Product].[Name] AS " + '"' + "SellerSKU" + '"'
                + ",[inFlow].[dbo].[BASE_Product].[Description] AS " + '"' + "" + '"'
                + ",ISNULL([inFlow].[dbo].[Base_InventoryCost].[AverageCost],0) AS " + '"' + "Cost" + '"'
                + ",[MWS].[dbo].[ProductAvailability].[Inbound] AS " + '"' + "Inbound" + '"'
                + ",[MWS].[dbo].[ProductAvailability].[Fulfillable] AS " + '"' + "Fulfillable" + '"'
                + "FROM [inFlow].[dbo].[BASE_Product] "
                + "INNER JOIN [inFlow].[dbo].[BASE_InventoryCost] ON [inFlow].[dbo].[BASE_Product].[ProdId]=[inFlow].[dbo].[BASE_InventoryCost].[ProdId] "
                + "INNER JOIN [MWS].[dbo].[ProductAvailability] ON [MWS].[dbo].[ProductAvailability].SellerSKU = [inFlow].[dbo].[BASE_Product].[Name] "
                + "WHERE [inFlow].[dbo].[BASE_InventoryCost].[CurrencyId] = '8'";
        public String InventorySQL
        {
            get
            {
                if (!String.IsNullOrEmpty(parameter))
                {
                    return inventorySQL + "AND (" + parameter + " LIKE '%'+@parameter+'%')";
                }
                else
                {
                    return inventorySQL;
                }
            }
        }
    }
}