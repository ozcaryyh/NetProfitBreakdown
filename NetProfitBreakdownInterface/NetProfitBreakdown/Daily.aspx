<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" 
    CodeBehind="Daily.aspx.cs" Inherits="AmazonManagementSystem.NetProfitBreakdown.Daily" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BannerContent" runat="server" ContentPlaceHolderID="InfoContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:Timer ID="tmrUpdate" runat="server" Interval="1000" 
        ontick="tmrUpdate_Tick">
    </asp:Timer>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tmrUpdate" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <table cellpadding="2" width="100%" border="0" runat="server" id="tblInfo" class="info">
                <tr id="tr1" runat="server">
                    <td align="left">
                        Currency:
                    </td>
                    <td align="center">
                        <asp:DetailsView ID="DetailsView1" runat="server" 
                            AutoGenerateRows="True" Height="20px" Width="100px">
                        </asp:DetailsView>
                    </td>
                    <td align="left">
                        Rate:
                    </td>
                    <td align="center">
                        <asp:Label ID="lblRate" runat="server" Text=''></asp:Label>
                    </td>
                    <td align="left">
                        Server Current Time:
                    </td>
                    <td align="center">
                        <asp:Label ID="lblTime" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetNetProfitData"
        TypeName="AmazonManagementSystem.NetProfitBreakdown.Daily">
    </asp:ObjectDataSource>
    <asp:HiddenField runat="server" ID="_repostcheckcode" />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource2"
        OnPreRender="ListView1_PreRender" OnItemCommand="ListView1_ItemCommand1"
        OnSorting="ListView1_Sorting">
        <LayoutTemplate>
            <table cellpadding="2" width="630px" border="0" runat="server" id="tblProducts">
                <thead>
                    <tr id="Tr1" runat="server" style="background-color: #81DAF5">
                        <th id="Th1" runat="server">
                            SellerSKU
                        </th>
                        <th id="Th2" runat="server">
                            Net Profit
                        </th>
                        <th id="Th3" runat="server">
                            Cost (RMB)
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr runat="server" id="itemPlaceholder" />
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                            <th id="Th9" runat="server" align="left">
                                <asp:Label ID="TotalLabel" runat="Server" Text="Total:" />
                            </th>
                            <th id="Th10" runat="server" align="right">
                                <asp:Label ID="TotalNetProfit" runat="Server" Text="" />
                            </th>
                            <th />
                            <th id="Th11" runat="server" align="right">
                                <asp:Label ID="TotalCost" runat="Server" Text="" />
                            </th>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr id="Tr2" runat="server">
                <td align="left">
                    <asp:Label ID="SellerSKULabel" runat="Server" Text='<%#Eval("SellerSKU") %>'
                        Width="300px" />
                </td>
                <td align="right">
                    <asp:Label ID="NetProfitLabel" runat="Server" Text='<%#(String.IsNullOrEmpty(Eval("Cost").ToString())?"$0.00":DataBinder.Eval(Container.DataItem,"Cost","{0:c}"))%>' />
                </td>
                <td align="right">
                    <asp:Label ID="CostLabel" runat="Server" Text='<%#(String.IsNullOrEmpty(Eval("Cost").ToString())?"$0.00":DataBinder.Eval(Container.DataItem,"Cost","{0:c}"))%>' />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color: #EFEFEF">
                <td align="left">
                    <asp:Label ID="SellerSKULabel" runat="Server" Text='<%#Eval("SellerSKU") %>'
                        Width="300px" />
                </td>
                <td align="right">
                    <asp:Label ID="NetProfitLabel" runat="Server" Text='<%#(String.IsNullOrEmpty(Eval("Cost").ToString())?"$0.00":DataBinder.Eval(Container.DataItem,"Cost","{0:c}"))%>' />
                </td>
                <td align="right">
                    <asp:Label ID="CostLabel" runat="Server" Text='<%#(String.IsNullOrEmpty(Eval("Cost").ToString())?"$0.00":DataBinder.Eval(Container.DataItem,"Cost","{0:c}"))%>' />
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1" OnPreRender="DataPager1_PreRender">
        <Fields>
            <asp:NumericPagerField ButtonCount="10" />
            <asp:TemplatePagerField>
                <PagerTemplate>
                    <asp:Label ID="TotalLabel" runat="server" Text="Maxium records:" />
                    <asp:TextBox ID="MaxiumRecordTextBox" runat="server" OnTextChanged="MaxiumRecordTextBox_OnTextChanged" />
                </PagerTemplate>
            </asp:TemplatePagerField>
        </Fields>
    </asp:DataPager>
</asp:Content>
