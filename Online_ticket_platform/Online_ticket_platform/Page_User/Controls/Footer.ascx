<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Footer" %>

<footer class="bg-white dark:bg-gray-950 text-gray-900 dark:text-white">
    <div class="container mx-auto px-4 py-12">
        <div class="grid grid-cols-1 md:grid-cols-4 gap-8">
            <!-- About Section -->
            <div class="space-y-4">
                <h3 class="text-xl font-bold">Về OCX</h3>
                <p class="text-gray-600 dark:text-gray-400">
                    Nền tảng bán vé trực tuyến hàng đầu cho các sự kiện âm nhạc tại Việt Nam.
                </p>
                <div id="SocialLinks" runat="server" class="flex space-x-4">
                    <a href="#" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">
                        <i class="fab fa-facebook-f"></i>
                    </a>
                    <a href="#" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">
                        <i class="fab fa-instagram"></i>
                    </a>
                    <a href="#" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">
                        <i class="fab fa-twitter"></i>
                    </a>
                </div>
            </div>

            <!-- Quick Links -->
            <div id="QuickLinks" runat="server" class="space-y-4">
                <h3 class="text-xl font-bold">Liên Kết Nhanh</h3>
                <ul class="space-y-2">
                    <li><a href="/events" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Sự kiện</a></li>
                    <li><a href="/about" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Về chúng tôi</a></li>
                    <li><a href="/contact" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Liên hệ</a></li>
                    <li><a href="/faq" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">FAQ</a></li>
                </ul>
            </div>

            <!-- Support -->
            <div id="SupportLinks" runat="server" class="space-y-4">
                <h3 class="text-xl font-bold">Hỗ Trợ</h3>
                <ul class="space-y-2">
                    <li><a href="/help" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Trung tâm trợ giúp</a></li>
                    <li><a href="/terms" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Điều khoản sử dụng</a></li>
                    <li><a href="/privacy" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Chính sách bảo mật</a></li>
                    <li><a href="/refund" class="text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white">Chính sách hoàn tiền</a></li>
                </ul>
            </div>

            <!-- Contact -->
            <div class="space-y-4">
                <h3 class="text-xl font-bold">Liên Hệ</h3>
                <ul class="space-y-2 text-gray-600 dark:text-gray-400">
                    <li class="flex items-center">
                        <i class="fas fa-map-marker-alt mr-2"></i>
                        123 Đường ABC, Quận XYZ, TP.HCM
                    </li>
                    <li class="flex items-center">
                        <i class="fas fa-phone mr-2"></i>
                        (028) 1234 5678
                    </li>
                    <li class="flex items-center">
                        <i class="fas fa-envelope mr-2"></i>
                        support@ocx.vn
                    </li>
                </ul>
            </div>
        </div>

        <!-- Bottom Bar -->
        <div class="border-t border-gray-200 dark:border-gray-700 mt-12 pt-8 text-center text-gray-600 dark:text-gray-400">
            <p>&copy; <asp:Label ID="CopyrightYear" runat="server" /> OCX Online Ticketing Platform. All rights reserved.</p>
        </div>
    </div>
</footer>
