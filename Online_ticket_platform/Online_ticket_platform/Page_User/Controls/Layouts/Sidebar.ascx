<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sidebar.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Sidebar" %>

<div class="bg-white dark:bg-gray-800 h-full w-62 border-r border-gray-200 dark:border-gray-700 rounded-2xl shadow-md overflow-hidden">
    <!-- User Profile Section -->
    <div class="p-4 border-b border-gray-200 dark:border-gray-700">
        <div class="flex items-center space-x-3">
            <img id="UserAvatar"
                runat="server"
                src="~/Public/assets/images/default-avatar.jpg"
                alt="User Avatar"
                class="h-10 w-10 rounded-full" />
            <div>
                <p class="text-sm font-medium text-gray-900 dark:text-white">
                    <asp:Label ID="UserName" runat="server" Text="Nguyễn Văn A" />
                </p>
                <p class="text-xs text-gray-500 dark:text-gray-400">
                    <asp:Label ID="UserEmail" runat="server" Text="user@example.com" />
                </p>
            </div>
        </div>
    </div>

    <!-- Navigation Menu -->
    <nav class="mt-4">
        <div id="NavigationMenu" runat="server" class="px-4 space-y-1">
            <a href="/dashboard" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                <i class="fas fa-home w-6 text-gray-400 dark:text-gray-500"></i>
                Tổng quan
            </a>
            <a href="/tickets" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                <i class="fas fa-ticket-alt w-6 text-gray-400 dark:text-gray-500"></i>
                Vé của tôi
            </a>
            <a href="/favorites" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                <i class="fas fa-heart w-6 text-gray-400 dark:text-gray-500"></i>
                Yêu thích
            </a>
            <a href="/notifications" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                <i class="fas fa-bell w-6 text-gray-400 dark:text-gray-500"></i>
                Thông báo
            </a>
        </div>

        <!-- Account Settings Section -->
        <div id="AccountSettings" runat="server" class="mt-8">
            <h3 class="px-4 text-xs font-semibold text-gray-500 dark:text-gray-400 uppercase tracking-wider">Tài khoản
            </h3>
            <div class="mt-2 px-4 space-y-1">
                <a href="/profile" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                    <i class="fas fa-user w-6 text-gray-400 dark:text-gray-500"></i>
                    Thông tin cá nhân
                </a>
                <a href="/settings" class="flex items-center px-2 py-2 text-sm font-medium text-gray-900 dark:text-gray-100 rounded-md hover:bg-gray-50 dark:hover:bg-gray-700">
                    <i class="fas fa-cog w-6 text-gray-400 dark:text-gray-500"></i>
                    Cài đặt
                </a>
                <a href="/logout" class="flex items-center px-2 py-2 text-sm font-medium text-red-600 dark:text-red-400 rounded-md hover:bg-red-50 dark:hover:bg-red-900/20">
                    <i class="fas fa-sign-out-alt w-6 text-red-400 dark:text-red-500"></i>
                    Đăng xuất
                </a>
            </div>
        </div>
    </nav>
</div>
