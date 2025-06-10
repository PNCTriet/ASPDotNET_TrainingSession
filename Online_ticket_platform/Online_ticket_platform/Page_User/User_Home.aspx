<%@ Page Language="C#" MasterPageFile="~/Page_User/User.Master" AutoEventWireup="true" CodeBehind="User_Home.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Home" %>
<%@ Register Src="~/Page_User/Controls/Slidebar.ascx" TagPrefix="uc1" TagName="Slidebar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang Người Dùng</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid p-0">
        <uc1:Slidebar ID="Slidebar1" runat="server" />
    </div>
    <div class="container mt-4">
        <h2>Chào mừng đến với trang người dùng!</h2>
    </div>
</asp:Content>
