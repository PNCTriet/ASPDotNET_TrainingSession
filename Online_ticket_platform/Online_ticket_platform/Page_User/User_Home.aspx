<%@ Page Language="C#" MasterPageFile="~/Page_User/User.Master" AutoEventWireup="true" CodeBehind="User_Home.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Home" %>
<%@ Register Src="~/Page_User/Controls/Slidebar.ascx" TagPrefix="uc1" TagName="Slidebar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang Người Dùng</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid p-0">
        <uc1:Slidebar ID="Slidebar1" runat="server" />
    </div>
    <div class="container mt-4 bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6">
        <h2 class="text-gray-900 dark:text-gray-100">Chào mừng đến với trang người dùng!</h2>
        <p class="text-gray-600 dark:text-gray-400 mt-2">Khám phá các sự kiện hấp dẫn và mua vé trực tuyến một cách dễ dàng.</p>
    </div>
</asp:Content>
