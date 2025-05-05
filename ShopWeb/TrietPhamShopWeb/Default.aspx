<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrietPhamShopWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <asp:Label ID="lblFirstName" runat="server" Text="First Name:"></asp:Label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblLastName" runat="server" Text="Last Name:"></asp:Label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnGenFullname" runat="server" Text="Generate Full Name" 
                        OnClick="btnGenFullname_Click" CssClass="btn btn-primary" />
                </div>
                <div class="form-group">
                    <asp:Label ID="lblFullName" runat="server" Text="Full Name:"></asp:Label>
                    <asp:Label ID="blFullName" runat="server" CssClass="form-control"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
