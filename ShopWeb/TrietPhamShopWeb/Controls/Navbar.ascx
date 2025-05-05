<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="TrietPhamShopWeb.Controls.Navbar" %>

<nav class="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
    <div class="container">
        <a class="navbar-brand" href="../Default.aspx">
            <i class="fas fa-store me-2"></i>Triet Pham Shop
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav ms-auto">
                <li class="nav-item">
                    <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/Default.aspx" CssClass="nav-link">
                        <i class="fas fa-home me-1"></i> Trang chủ
                    </asp:HyperLink>
                </li>
                <li class="nav-item">
                    <asp:HyperLink ID="lnkAdmin" runat="server" NavigateUrl="~/Adminpage/AdminHome.aspx" CssClass="nav-link">
                        <i class="fas fa-user-shield me-1"></i> Admin
                    </asp:HyperLink>
                </li>
            </ul>
        </div>
    </div>
</nav>
