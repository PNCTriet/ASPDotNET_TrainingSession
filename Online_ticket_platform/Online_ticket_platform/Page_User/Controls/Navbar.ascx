<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Navbar" %>

<nav class="bg-white dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700">
    <div class="container mx-auto px-4">
        <div class="flex justify-between h-16">
            <!-- Main Menu -->
            <div id="MainMenu" runat="server" class="hidden md:flex space-x-8">
                <a href="/" class="inline-flex items-center px-1 pt-1 text-gray-900 dark:text-white hover:text-blue-600 dark:hover:text-blue-400">
                    Trang chủ
                </a>
                <a href="/events" class="inline-flex items-center px-1 pt-1 text-gray-900 dark:text-white hover:text-blue-600 dark:hover:text-blue-400">
                    Sự kiện
                </a>
                <a href="/artists" class="inline-flex items-center px-1 pt-1 text-gray-900 dark:text-white hover:text-blue-600 dark:hover:text-blue-400">
                    Nghệ sĩ
                </a>
                <a href="/venues" class="inline-flex items-center px-1 pt-1 text-gray-900 dark:text-white hover:text-blue-600 dark:hover:text-blue-400">
                    Địa điểm
                </a>
                <a href="/organizers" class="inline-flex items-center px-1 pt-1 text-gray-900 dark:text-white hover:text-blue-600 dark:hover:text-blue-400">
                    Nhà tổ chức
                </a>
            </div>

            <!-- Mobile Menu Button -->
            <div class="md:hidden flex items-center">
                <button id="MobileMenuButton"
                        runat="server"
                        type="button" 
                        class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 dark:text-gray-500 hover:text-gray-500 dark:hover:text-gray-400 hover:bg-gray-100 dark:hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-blue-500"
                        aria-controls="mobile-menu" 
                        aria-expanded="false">
                    <span class="sr-only">Mở menu</span>
                    <i class="fas fa-bars"></i>
                </button>
            </div>
        </div>
    </div>

    <!-- Mobile Menu -->
    <div id="MobileMenu" runat="server" class="md:hidden hidden">
        <div class="pt-2 pb-3 space-y-1">
            <a href="/" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 dark:text-white hover:bg-gray-50 dark:hover:bg-gray-700">
                Trang chủ
            </a>
            <a href="/events" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 dark:text-white hover:bg-gray-50 dark:hover:bg-gray-700">
                Sự kiện
            </a>
            <a href="/artists" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 dark:text-white hover:bg-gray-50 dark:hover:bg-gray-700">
                Nghệ sĩ
            </a>
            <a href="/venues" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 dark:text-white hover:bg-gray-50 dark:hover:bg-gray-700">
                Địa điểm
            </a>
            <a href="/organizers" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 dark:text-white hover:bg-gray-50 dark:hover:bg-gray-700">
                Nhà tổ chức
            </a>
        </div>
    </div>
</nav>

<script>
    // Mobile menu toggle
    document.querySelector('button[aria-controls="mobile-menu"]').addEventListener('click', function() {
        const mobileMenu = document.getElementById('mobile-menu');
        const isExpanded = this.getAttribute('aria-expanded') === 'true';
        
        this.setAttribute('aria-expanded', !isExpanded);
        mobileMenu.classList.toggle('hidden');
    });
</script>
