﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Header" %>

<header class="bg-white dark:bg-gray-800 shadow-sm">
    <div class="container mx-auto px-4 py-3">
        <div class="flex items-center justify-between">
            <!-- Logo -->
            <div class="flex-shrink-0">
                <a href="/" class="flex items-center">
                    <img src="../Public/assets/images/logo.jpg" alt="OCX Logo" class="h-8 w-auto" />
                    <span class="ml-2 text-xl font-bold text-gray-900 dark:text-white">OCX</span>
                </a>
            </div>

            <!-- Search Bar -->
            <div class="hidden md:block flex-1 max-w-lg mx-8">
                <div class="relative">
                    <input type="text"
                        id="SearchInput"
                        runat="server"
                        class="w-full px-4 py-2 rounded-lg border border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        placeholder="Tìm kiếm sự kiện..." />
                    <button id="SearchButton"
                        runat="server"
                        class="absolute right-3 top-2.5 text-gray-400 dark:text-gray-500 hover:text-gray-600 dark:hover:text-gray-300">
                        <i class="fas fa-search"></i>
                    </button>
                </div>
            </div>

            <!-- User Actions -->
            <div class="flex items-center space-x-4">
                <a href="/events" class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white">Sự kiện</a>
                <a href="/tickets" class="text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white">Vé của tôi</a>
                <asp:Panel ID="pnlUserInfo" runat="server" Visible="false">
                    <span class="text-gray-700 dark:text-gray-200 font-semibold mr-2">
                        Xin chào, <asp:Literal ID="litUserName" runat="server" />
                    </span>
                    <a href="../../Page_User/User_Logout.aspx" class="px-4 py-2 rounded-lg bg-red-600 text-white hover:bg-red-700 transition-colors">Đăng xuất</a>
                </asp:Panel>
                <asp:Panel ID="pnlLogin" runat="server" Visible="true">
                    <a href="../../Page_User/User_Login.aspx" class="px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition-colors">Đăng nhập</a>
                </asp:Panel>
            </div>
        </div>
    </div>
</header>
