<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Slidebar.ascx.cs" Inherits="Online_ticket_platform.Page_User.Controls.Slidebar" %>

<div class="slider-container">
    <div class="slider">
        <asp:Repeater ID="rptSlides" runat="server">
            <ItemTemplate>
                <div class="slide">
                    <img src='<%# Eval("ImageUrl") %>' alt='<%# Eval("AltText") %>'>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
    <button type="button" class="slider-arrow prev-arrow" onclick="prevSlide()">
        <i class="fas fa-chevron-left"></i>
    </button>
    <button type="button" class="slider-arrow next-arrow" onclick="nextSlide()">
        <i class="fas fa-chevron-right"></i>
    </button>
    
    <div class="slider-dots">
        <asp:Repeater ID="rptDots" runat="server">
            <ItemTemplate>
                <span class="dot" onclick="goToSlide(<%# Container.ItemIndex %>)"></span>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

<style>
    .slider-container {
        position: relative;
        width: 100%;
        background: #000;
        overflow: hidden;
        border-radius: 20px;
        margin: 20px 0;
    }

    .slider {
        display: flex;
        transition: transform 0.5s ease;
        border-radius: 20px;
    }

    .slide {
        min-width: 100%;
        height: 500px;
        overflow: hidden;
        border-radius: 20px;
    }

    .slide img {
        width: 100%;
        height: 100%;
        object-fit: cover;
        transition: transform 0.3s ease;
        border-radius: 20px;
    }

    .slide img:hover {
        transform: scale(1.1);
    }

    .slider-arrow {
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
        background: rgba(0, 0, 0, 0.5);
        color: white !important;
        border: 1px solid white;
        padding: 15px 20px;
        cursor: pointer;
        font-size: 18px;
        transition: all 0.3s ease;
        border-radius: 50%;
        width: 50px;
        height: 50px;
        display: flex;
        align-items: center;
        justify-content: center;
        text-decoration: none;
    }

    .slider-arrow:hover {
        background: rgba(0, 0, 0, 0.8);
        transform: translateY(-50%) scale(1.1);
        text-decoration: none;
    }

    .prev-arrow {
        left: 20px;
    }

    .next-arrow {
        right: 20px;
    }

    .slider-dots {
        position: absolute;
        bottom: 20px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
        gap: 10px;
        background: rgba(0, 0, 0, 0.5);
        padding: 10px 20px;
        border-radius: 30px;
    }

    .dot {
        width: 12px;
        height: 12px;
        border: 1px solid white;
        border-radius: 50%;
        cursor: pointer;
        transition: all 0.3s ease;
        display: inline-block;
    }

    .dot:hover {
        transform: scale(1.2);
    }

    .dot.active {
        background: white;
        transform: scale(1.2);
    }
</style>

<script>
    let currentSlide = 0;
    const slider = document.querySelector('.slider');
    const slides = document.querySelectorAll('.slide');
    const dots = document.querySelectorAll('.dot');
    let slideInterval;

    function updateSlider() {
        slider.style.transform = `translateX(-${currentSlide * 100}%)`;
        dots.forEach((dot, index) => {
            dot.classList.toggle('active', index === currentSlide);
        });
    }

    function nextSlide() {
        currentSlide = (currentSlide + 1) % slides.length;
        updateSlider();
        resetAutoSlide();
    }

    function prevSlide() {
        currentSlide = (currentSlide - 1 + slides.length) % slides.length;
        updateSlider();
        resetAutoSlide();
    }

    function goToSlide(index) {
        currentSlide = index;
        updateSlider();
        resetAutoSlide();
    }

    function startAutoSlide() {
        slideInterval = setInterval(() => {
            currentSlide = (currentSlide + 1) % slides.length;
            updateSlider();
        }, 5000);
    }

    function resetAutoSlide() {
        clearInterval(slideInterval);
        startAutoSlide();
    }

    // Initialize
    document.addEventListener('DOMContentLoaded', function() {
        updateSlider();
        startAutoSlide();
    });
</script>
