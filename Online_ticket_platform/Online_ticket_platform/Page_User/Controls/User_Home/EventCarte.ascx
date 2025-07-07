<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCarte.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.User_Home.EventCarte" %>

<div class="card h-full bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden transition-transform duration-300 hover:scale-105 flex flex-col">
    <!-- Event Banner -->
    <div class="relative aspect-[16/9]">
        <!-- Blurred Background -->
        <div class="absolute inset-0 overflow-hidden">
            <img id="imgEventBannerBg" runat="server" 
                 src="/assets/images/event-placeholder.jpg" 
                 alt="Event Banner Background" 
                 class="absolute inset-0 w-full h-full object-cover scale-110 blur-[8px] opacity-50" />
        </div>
        <!-- Main Image Container -->
        <div class="absolute inset-0 flex items-center justify-center">
            <div class="relative w-[90%] h-[90%]">
                <!-- Main Image -->
                <img id="imgEventBanner" runat="server" 
                     src="/assets/images/event-placeholder.jpg" 
                     alt="Event Banner" 
                     class="absolute inset-0 w-full h-full object-contain rounded-2xl" />
            </div>
        </div>
        <!-- Ticket Count Badge -->
        <div class="absolute top-2 right-2">
            <span class="px-2 py-1 text-xs font-semibold text-white bg-blue-600 rounded-full">
                <i class="fas fa-ticket-alt mr-1"></i>
                <asp:Label ID="lblTicketCount" runat="server" Text="3 loại vé" />
            </span>
        </div>
    </div>

    <!-- Event Content -->
    <div class="p-4 flex-grow">
        <!-- Event Title -->
        <h3 class="text-lg font-bold text-gray-900 dark:text-white mb-2">
            <asp:Label ID="lblEventTitle" runat="server" Text="Tên sự kiện" />
        </h3>

        <!-- Event Details -->
        <div class="space-y-2 mb-4">
            <!-- Date -->
            <div class="flex items-center text-gray-600 dark:text-gray-400">
                <i class="fas fa-calendar-alt w-5"></i>
                <asp:Label ID="lblEventDate" runat="server" Text="01/01/2024 20:00" CssClass="ml-2" />
            </div>
            <!-- Location -->
            <div class="flex items-center text-gray-600 dark:text-gray-400">
                <i class="fas fa-map-marker-alt w-5"></i>
                <asp:Label ID="lblEventLocation" runat="server" Text="Địa điểm tổ chức" CssClass="ml-2" />
            </div>
        </div>

        <!-- Ticket Types -->
        <div class="space-y-2">
            <h4 class="text-sm font-semibold text-gray-700 dark:text-gray-300">Loại vé:</h4>
            <div class="flex flex-wrap gap-2">
                <asp:Repeater ID="TicketTypesRepeater" runat="server">
                    <ItemTemplate>
                        <span class="px-2 py-1 text-xs font-medium bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-200 rounded-full">
                            <%# Eval("Name") %> - <%# Eval("Price", "{0:N0}đ") %>
                        </span>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <!-- Action Button -->
    <div class="p-4 pt-0">
        <asp:HyperLink ID="lnkDetail" runat="server" 
                      CssClass="block w-full text-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors"
                      NavigateUrl='<%# Eval("EventId", "/Page_User/User_Event.aspx?eventId={0}") %>'
                      >
            Xem chi tiết
        </asp:HyperLink>
    </div>
</div>
