<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="TrietPhamShopWeb.Controls.Header" %>

<header class="header">
    <div class="container">
        <div class="header-content">
            <div class="logo">
                <a href="/">
                    <img src="/Images/logo.png" alt="Logo" />
                </a>
            </div>
            <div class="header-right">
                <div class="search-box">
                    <input type="text" placeholder="Search..." />
                    <button type="button"><i class="fas fa-search"></i></button>
                </div>
                <div class="user-actions">
                    <a href="/Account/Login" class="btn btn-outline-primary">Login</a>
                    <a href="/Account/Register" class="btn btn-primary">Register</a>
                </div>
            </div>
        </div>
    </div>
</header>

<style>
    .header {
        background-color: #fff;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        padding: 15px 0;
    }
    .header-content {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .logo img {
        height: 40px;
    }
    .header-right {
        display: flex;
        align-items: center;
        gap: 20px;
    }
    .search-box {
        display: flex;
        align-items: center;
    }
    .search-box input {
        padding: 8px 15px;
        border: 1px solid #ddd;
        border-radius: 4px 0 0 4px;
        width: 200px;
    }
    .search-box button {
        padding: 8px 15px;
        border: 1px solid #ddd;
        border-left: none;
        background: #f8f9fa;
        border-radius: 0 4px 4px 0;
        cursor: pointer;
    }
    .user-actions {
        display: flex;
        gap: 10px;
    }
</style> 