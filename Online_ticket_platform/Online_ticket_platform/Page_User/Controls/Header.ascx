<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Header" %>

<header class="bg-white shadow-sm">
    <div class="container mx-auto px-4 py-3">
        <div class="flex items-center justify-between">
            <!-- Logo -->
            <div class="flex-shrink-0">
                <a href="/" class="flex items-center">
                    <img src="/assets/images/logo.png" alt="OCX Logo" class="h-8 w-auto" />
                    <span class="ml-2 text-xl font-bold text-gray-900">OCX</span>
                </a>
            </div>

            <!-- Search Bar -->
            <div class="hidden md:block flex-1 max-w-lg mx-8">
                <div class="relative">
                    <input type="text" 
                           id="SearchInput"
                           runat="server"
                           class="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                           placeholder="Tìm kiếm sự kiện..." />
                    <button id="SearchButton"
                            runat="server"
                            class="absolute right-3 top-2.5 text-gray-400 hover:text-gray-600">
                        <i class="fas fa-search"></i>
                    </button>
                </div>
            </div>

            <!-- User Actions -->
            <div class="flex items-center space-x-4">
                <a href="/events" class="text-gray-600 hover:text-gray-900">Sự kiện</a>
                <a href="/tickets" class="text-gray-600 hover:text-gray-900">Vé của tôi</a>
                <a href="/login" class="px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition-colors">
                    Đăng nhập
                </a>
            </div>
        </div>
    </div>
</header>
