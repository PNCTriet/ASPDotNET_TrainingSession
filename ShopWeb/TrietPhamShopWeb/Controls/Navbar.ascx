<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="TrietPhamShopWeb.Controls.Navbar" %>

<nav class="navbar">
    <div class="container">
        <ul class="nav-menu">
            <li class="nav-item">
                <a href="/" class="nav-link">Home</a>
            </li>
            <li class="nav-item">
                <a href="/Products" class="nav-link">Products</a>
            </li>
            <li class="nav-item">
                <a href="/Categories" class="nav-link">Categories</a>
            </li>
            <li class="nav-item">
                <a href="/About" class="nav-link">About</a>
            </li>
            <li class="nav-item">
                <a href="/Contact" class="nav-link">Contact</a>
            </li>
        </ul>
    </div>
</nav>

<style>
    .navbar {
        background-color: #343a40;
        padding: 10px 0;
    }
    .nav-menu {
        list-style: none;
        margin: 0;
        padding: 0;
        display: flex;
        gap: 20px;
    }
    .nav-item {
        position: relative;
    }
    .nav-link {
        color: #fff;
        text-decoration: none;
        padding: 8px 15px;
        display: block;
        transition: color 0.3s;
    }
    .nav-link:hover {
        color: #17a2b8;
    }
    .nav-item.active .nav-link {
        color: #17a2b8;
    }
</style> 