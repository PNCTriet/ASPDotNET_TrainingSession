<%@ Page Language="C#" MasterPageFile="~/Page_User/User.Master" AutoEventWireup="true" CodeBehind="User_Home.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Home" %>
<%@ Register Src="~/Page_User/Controls/User_Home/Slidebar.ascx" TagPrefix="uc1" TagName="Slidebar" %>
<%@ Register Src="~/Page_User/Controls/User_Home/EventCarte.ascx" TagPrefix="uc1" TagName="EventCarte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Trang Người Dùng</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid p-0">
        <uc1:Slidebar ID="Slidebar1" runat="server" />
    </div>
    <div class="container mt-4">
        <!-- Welcome Section -->
        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6 mb-6">
            <h2 class="text-gray-900 dark:text-gray-100">Chào mừng đến với trang người dùng!</h2>
            <p class="text-gray-600 dark:text-gray-400 mt-2">Khám phá các sự kiện hấp dẫn và mua vé trực tuyến một cách dễ dàng.</p>
        </div>

        <!-- Events Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <!-- Event Card 1 -->
            <uc1:EventCarte ID="EventCarte1" runat="server" />
            
            <!-- Event Card 2 -->
            <uc1:EventCarte ID="EventCarte2" runat="server" />
            
            <!-- Event Card 3 -->
            <uc1:EventCarte ID="EventCarte3" runat="server" />

            <!-- Event Card 4 -->
            <uc1:EventCarte ID="EventCarte4" runat="server" />

            <!-- Event Card 5 -->
            <uc1:EventCarte ID="EventCarte5" runat="server" />

            <!-- Event Card 6 -->
            <uc1:EventCarte ID="EventCarte6" runat="server" />
        </div>
    </div>
</asp:Content>
