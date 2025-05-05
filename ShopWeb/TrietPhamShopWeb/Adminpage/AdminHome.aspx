<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Adminpage/Adminsite.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.AdminHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Admin Home</h2>
        <div class="form-group">
            <asp:Label ID="lblProductEdit" runat="server" Text="Product Edit:"></asp:Label>
            <asp:TextBox ID="txtProductEdit" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="lblProductName" runat="server" Text="Product Name:"></asp:Label>
            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn btn-primary" />
        </div>
    </div>
</asp:Content> 