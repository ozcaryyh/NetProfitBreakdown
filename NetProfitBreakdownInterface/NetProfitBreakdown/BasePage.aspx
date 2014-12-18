<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BasePage.aspx.cs" Inherits="AmazonManagementSystem.NetProfitBreakdown.BasePage" %>
<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="BannerContent" ContentPlaceHolderID="InfoContent" runat="server">
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
            <asp:Label ID="lblTime" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
