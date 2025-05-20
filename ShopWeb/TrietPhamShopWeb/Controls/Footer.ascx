<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="TrietPhamShopWeb.Controls.Footer" %>

<footer class="footer mt-5 py-4 bg-light">
    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <h5>Về chúng tôi</h5>
                <p>Triet Pham Shop - Nơi cung cấp các sản phẩm chất lượng cao với giá cả phải chăng.</p>
            </div>
            <div class="col-md-4">
                <h5>Liên kết nhanh</h5>
                <ul class="list-unstyled">
                    <li><asp:HyperLink ID="lnkFooterHome" runat="server" NavigateUrl="~/Default.aspx">Trang chủ</asp:HyperLink></li>
                    <li><asp:HyperLink ID="lnkFooterProducts" runat="server" NavigateUrl="~/Products.aspx">Sản phẩm</asp:HyperLink></li>
                    <li><asp:HyperLink ID="lnkFooterAbout" runat="server" NavigateUrl="~/About.aspx">Giới thiệu</asp:HyperLink></li>
                    <li><asp:HyperLink ID="lnkFooterContact" runat="server" NavigateUrl="~/Contact.aspx">Liên hệ</asp:HyperLink></li>
                </ul>
            </div>
            <div class="col-md-4">
                <h5>Liên hệ</h5>
                <ul class="list-unstyled">
                    <li><i class="fas fa-phone"></i> Hotline: 0123 456 789</li>
                    <li><i class="fas fa-envelope"></i> Email: info@trietphamshop.com</li>
                    <li><i class="fas fa-map-marker-alt"></i> Địa chỉ: 123 Đường ABC, Quận XYZ, TP.HCM</li>
                </ul>
            </div>
        </div>
        <hr class="my-4" />
        <div class="row">
            <div class="col-md-12 text-center">
                <p class="mb-0" >&copy; <%= DateTime.Now.Year %> Triet Pham Shop. All rights reserved.</p>
            </div>
        </div>
    </div>
</footer>
