<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="TrietPhamShopWeb.Controls.Footer" %>

<footer class="footer">
    <div class="container">
        <div class="footer-content">
            <div class="footer-section">
                <h4>About Us</h4>
                <p>Your trusted online shopping destination for quality products and excellent service.</p>
            </div>
            <div class="footer-section">
                <h4>Quick Links</h4>
                <ul>
                    <li><a href="/About">About Us</a></li>
                    <li><a href="/Contact">Contact Us</a></li>
                    <li><a href="/Privacy">Privacy Policy</a></li>
                    <li><a href="/Terms">Terms & Conditions</a></li>
                </ul>
            </div>
            <div class="footer-section">
                <h4>Contact Info</h4>
                <ul class="contact-info">
                    <li><i class="fas fa-map-marker-alt"></i> 123 Street, City, Country</li>
                    <li><i class="fas fa-phone"></i> +123 456 7890</li>
                    <li><i class="fas fa-envelope"></i> info@example.com</li>
                </ul>
            </div>
            <div class="footer-section">
                <h4>Follow Us</h4>
                <div class="social-links">
                    <a href="#"><i class="fab fa-facebook"></i></a>
                    <a href="#"><i class="fab fa-twitter"></i></a>
                    <a href="#"><i class="fab fa-instagram"></i></a>
                    <a href="#"><i class="fab fa-linkedin"></i></a>
                </div>
            </div>
        </div>
        <div class="footer-bottom">
            <p>&copy; 2024 Your Company. All rights reserved.</p>
        </div>
    </div>
</footer>

<style>
    .footer {
        background-color: #343a40;
        color: #fff;
        padding: 50px 0 20px;
        margin-top: 50px;
    }
    .footer-content {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 30px;
        margin-bottom: 30px;
    }
    .footer-section h4 {
        color: #17a2b8;
        margin-bottom: 20px;
    }
    .footer-section ul {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    .footer-section ul li {
        margin-bottom: 10px;
    }
    .footer-section ul li a {
        color: #fff;
        text-decoration: none;
        transition: color 0.3s;
    }
    .footer-section ul li a:hover {
        color: #17a2b8;
    }
    .contact-info li {
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .social-links {
        display: flex;
        gap: 15px;
    }
    .social-links a {
        color: #fff;
        font-size: 20px;
        transition: color 0.3s;
    }
    .social-links a:hover {
        color: #17a2b8;
    }
    .footer-bottom {
        text-align: center;
        padding-top: 20px;
        border-top: 1px solid rgba(255,255,255,0.1);
    }
</style> 