<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navbar.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Navbar" %>

<nav class="bg-white border-b border-gray-200">
    <div class="container mx-auto px-4">
        <div class="flex justify-between h-16">
            <!-- Main Menu -->
            <div id="MainMenu" runat="server" class="hidden md:flex space-x-8">
                <a href="/" class="inline-flex items-center px-1 pt-1 text-gray-900 hover:text-blue-600">
                    Trang chủ
                </a>
                <a href="/events" class="inline-flex items-center px-1 pt-1 text-gray-900 hover:text-blue-600">
                    Sự kiện
                </a>
                <a href="/artists" class="inline-flex items-center px-1 pt-1 text-gray-900 hover:text-blue-600">
                    Nghệ sĩ
                </a>
                <a href="/venues" class="inline-flex items-center px-1 pt-1 text-gray-900 hover:text-blue-600">
                    Địa điểm
                </a>
                <a href="/organizers" class="inline-flex items-center px-1 pt-1 text-gray-900 hover:text-blue-600">
                    Nhà tổ chức
                </a>
            </div>

            <!-- Mobile Menu Button -->
            <div class="md:hidden flex items-center">
                <button id="MobileMenuButton"
                        runat="server"
                        type="button" 
                        class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-blue-500"
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
            <a href="/" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 hover:bg-gray-50">
                Trang chủ
            </a>
            <a href="/events" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 hover:bg-gray-50">
                Sự kiện
            </a>
            <a href="/artists" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 hover:bg-gray-50">
                Nghệ sĩ
            </a>
            <a href="/venues" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 hover:bg-gray-50">
                Địa điểm
            </a>
            <a href="/organizers" class="block pl-3 pr-4 py-2 text-base font-medium text-gray-900 hover:bg-gray-50">
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
