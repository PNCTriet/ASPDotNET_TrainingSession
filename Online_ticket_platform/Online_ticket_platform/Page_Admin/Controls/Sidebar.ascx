<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sidebar.ascx.cs" Inherits="Online_ticket_platform.Page_Admin.Controls.sidebar" %>

<!-- Sidebar -->
<ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">
    <!-- Sidebar - Brand -->
    <a class="sidebar-brand d-flex align-items-center justify-content-center" href="Admin_Home.aspx">
        <div class="sidebar-brand-icon rotate-n-15">
            <i class="fas fa-laugh-wink"></i>
        </div>
        <div class="sidebar-brand-text mx-3">Ticket Platform</div>
    </a>

    <!-- Divider -->
    <hr class="sidebar-divider my-0">

    <!-- Nav Item - Dashboard -->
    <li class="nav-item" id="menu-home">
        <a class="nav-link" href="Admin_Home.aspx">
            <i class="fas fa-fw fa-tachometer-alt"></i>
            <span>Dashboard</span>
        </a>
    </li>

    <!-- Divider -->
    <hr class="sidebar-divider">

    <!-- Heading -->
    <div class="sidebar-heading">
        Quản lý hệ thống
    </div>

    <!-- Users -->
    <li class="nav-item" id="menu-users">
        <a class="nav-link" href="User_List.aspx">
            <i class="fas fa-fw fa-users"></i>
            <span>Người dùng</span>
        </a>
    </li>

    <!-- Organizations -->
    <li class="nav-item" id="menu-organizations">
        <a class="nav-link" href="Organization_List.aspx">
            <i class="fas fa-fw fa-building"></i>
            <span>Tổ chức</span>
        </a>
    </li>

    <!-- Events -->
    <li class="nav-item" id="menu-events">
        <a class="nav-link" href="Event_List.aspx">
            <i class="fas fa-fw fa-calendar"></i>
            <span>Sự kiện</span>
        </a>
    </li>

    <!-- Event Settings -->
    <!-- <li class="nav-item" id="menu-event-settings">
        <a class="nav-link" href="EventSetting_List.aspx">
            <i class="fas fa-fw fa-cogs"></i>
            <span>Cài đặt sự kiện</span>
        </a>
    </li> -->

    <!-- Tickets -->
    <li class="nav-item" id="menu-tickets">
        <a class="nav-link" href="Ticket_List.aspx">
            <i class="fas fa-fw fa-ticket-alt"></i>
            <span>Vé</span>
        </a>
    </li>

    <!-- Orders -->
    <li class="nav-item" id="menu-orders">
        <a class="nav-link" href="Order_List.aspx">
            <i class="fas fa-fw fa-shopping-cart"></i>
            <span>Đơn hàng</span>
        </a>
    </li>

    <!-- Order Items -->
    <li class="nav-item" id="menu-order-items">
        <a class="nav-link" href="OrderItem_List.aspx">
            <i class="fas fa-fw fa-list"></i>
            <span>Chi tiết đơn hàng</span>
        </a>
    </li>

    <!-- Payments -->
    <!-- <li class="nav-item" id="menu-payments">
        <a class="nav-link" href="Payment_List.aspx">
            <i class="fas fa-fw fa-credit-card"></i>
            <span>Thanh toán</span>
        </a>
    </li> -->

    <!-- Checkin Logs -->
    <!-- <li class="nav-item" id="menu-checkin-logs">
        <a class="nav-link" href="CheckinLog_List.aspx">
            <i class="fas fa-fw fa-clipboard-check"></i>
            <span>Nhật ký check-in</span>
        </a>
    </li> -->

    <!-- Promo Codes -->
    <!-- <li class="nav-item" id="menu-promo-codes">
        <a class="nav-link" href="PromoCode_List.aspx">
            <i class="fas fa-fw fa-tags"></i>
            <span>Mã giảm giá</span>
        </a>
    </li> -->

    <!-- Order Promos -->
    <!-- <li class="nav-item" id="menu-order-promos">
        <a class="nav-link" href="OrderPromo_List.aspx">
            <i class="fas fa-fw fa-percentage"></i>
            <span>Áp dụng mã giảm giá</span>
        </a>
    </li> -->

    <!-- Referral Codes -->
    <!-- <li class="nav-item" id="menu-referral-codes">
        <a class="nav-link" href="ReferralCode_List.aspx">
            <i class="fas fa-fw fa-user-plus"></i>
            <span>Mã giới thiệu</span>
        </a>
    </li> -->

    <!-- Email Logs -->
    <!-- <li class="nav-item" id="menu-email-logs">
        <a class="nav-link" href="EmailLog_List.aspx">
            <i class="fas fa-fw fa-envelope"></i>
            <span>Nhật ký email</span>
        </a>
    </li> -->

    <!-- Webhook Logs -->
    <!-- <li class="nav-item" id="menu-webhook-logs">
        <a class="nav-link" href="WebhookLog_List.aspx">
            <i class="fas fa-fw fa-exchange-alt"></i>
            <span>Nhật ký webhook</span>
        </a>
    </li> -->

    <!-- Webhook Subscriptions -->
    <!-- <li class="nav-item" id="menu-webhook-subscriptions">
        <a class="nav-link" href="WebhookSubscription_List.aspx">
            <i class="fas fa-fw fa-bell"></i>
            <span>Đăng ký webhook</span>
        </a>
    </li> -->

    <!-- Tracking Visits -->
    <!-- <li class="nav-item" id="menu-tracking-visits">
        <a class="nav-link" href="TrackingVisit_List.aspx">
            <i class="fas fa-fw fa-chart-line"></i>
            <span>Theo dõi truy cập</span>
        </a>
    </li> -->

    <!-- Images -->
    <li class="nav-item" id="menu-images">
        <a class="nav-link" href="Image_List.aspx">
            <i class="fas fa-fw fa-images"></i>
            <span>Hình ảnh</span>
        </a>
    </li>

    <!-- Image Links -->
    <li class="nav-item" id="menu-image-links">
        <a class="nav-link" href="ImageLink_List.aspx">
            <i class="fas fa-fw fa-link"></i>
            <span>Liên kết hình ảnh</span>
        </a>
    </li>

    <!-- User Organizations -->
    <!-- <li class="nav-item" id="menu-user-organizations">
        <a class="nav-link" href="UserOrganization_List.aspx">
            <i class="fas fa-fw fa-user-tie"></i>
            <span>Người dùng tổ chức</span>
        </a>
    </li> -->

    <!-- Divider -->
    <hr class="sidebar-divider d-none d-md-block">

    <!-- Sidebar Toggler (Sidebar) -->
    <div class="text-center d-none d-md-inline">
        <button class="rounded-circle border-0" id="sidebarToggle"></button>
    </div>
</ul>
<!-- End of Sidebar -->
