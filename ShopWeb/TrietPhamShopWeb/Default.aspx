<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TrietPhamShopWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- User Info Panel -->
    <asp:Panel ID="pnlUserInfo" runat="server" CssClass="user-info-panel mb-3">
        <div class="container">
            <div class="alert alert-info">
                Xin chào,
                <asp:Label ID="lblUsername" runat="server"></asp:Label>
            
            </div>
        </div>
    </asp:Panel>

    <!-- Product Grid Section -->
    <section class="product-grid-section py-5">
        <div class="container">
            <div class="section-header d-flex justify-content-between align-items-center mb-4">
                <h2 class="section-title">Sản Phẩm Mới</h2>
                <div class="view-options">
                    <select class="form-select sort-select">
                        <option>Sắp xếp theo</option>
                        <option>Giá: Thấp đến Cao</option>
                        <option>Giá: Cao đến Thấp</option>
                        <option>Mới nhất</option>
                    </select>
                </div>
            </div>

            <div class="row g-4">
                <asp:Repeater ID="ProductGridRepeater" runat="server">
                    <ItemTemplate>
                        <div class="col-6 col-md-4 col-lg-3">
                            <div class="product-card">
                                <div class="product-image">
                                    <a href='<%# "ProductDetail.aspx?id=" + Eval("ProductID") %>'>
                                        <img src='<%# GetProductImage(Eval("Images")) %>'
                                            alt='<%# Eval("ProductName") %>'
                                            class="main-image" />
                                        <!-- <img src='<%# GetProductHoverImage(Eval("Images")) %>'
                                            alt='<%# Eval("ProductName") %>'
                                            class="hover-image" /> -->
                                    </a>
                                </div>
                                <div class="product-info">
                                    <h3 class="product-title">
                                        <a href='<%# "ProductDetail.aspx?id=" + Eval("ProductID") %>'>
                                            <%# Eval("ProductName") %>
                                        </a>
                                    </h3>
                                    <div class="product-price">
                                        <span class="current-price">
                                            <%# string.Format("{0:N0}đ", Eval("UnitPrice")) %>
                                        </span>
                                        <%# ShowDiscount(Eval("UnitPrice")) %>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <div class="text-center mt-5">
                <button class="btn btn-outline-dark btn-load-more">Xem thêm sản phẩm</button>
            </div>
        </div>
    </section>

    <div class="container-fluid px-0">
        <!-- Featured Product Section -->
        <section class="featured-product py-5">
            <div class="container">
                <div class="row">
                    <div class="col-md-8">
                        <div class="featured-product-card">
                            <img src="https://placehold.co/800x600" alt="Featured Product" class="img-fluid rounded-4">
                            <div class="featured-product-info">
                                <h2 class="display-4 fw-bold">Sản Phẩm Tiêu Biểu</h2>
                                <p class="lead">Khám phá sản phẩm nổi bật của chúng tôi</p>
                                <a href="#" class="btn btn-dark btn-lg">Xem Chi Tiết</a>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="hot-products">
                            <h3 class="mb-4">Top 10 Sản Phẩm Hot</h3>
                            <div class="hot-products-list">
                                <% for (int i = 1; i <= 10; i++)
                                    { %>
                                <div class="hot-product-item">
                                    <img src="https://placehold.co/100x100" alt="Hot Product <%= i %>" class="rounded-3">
                                    <div class="hot-product-info">
                                        <h5>Sản Phẩm Hot <%= i %></h5>
                                        <p class="text-muted">Giá: <%= i * 1000000 %>đ</p>
                                    </div>
                                </div>
                                <% } %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <!-- Product Categories Section -->
        <section class="product-categories py-5">
            <div class="container">
                <h2 class="text-center mb-5">Danh Mục Sản Phẩm</h2>
                <div class="row">
                    <% for (int i = 1; i <= 3; i++)
                        { %>
                    <div class="col-12 mb-5">
                        <h3 class="category-title mb-4">Danh Mục <%= i %></h3>
                        <div class="product-slider-container">
                            <button type="button" class="slider-nav-btn prev-btn" onclick="return scrollSlider(this, 'left')">
                                <i class="fas fa-chevron-left"></i>
                            </button>
                            <div class="product-slider">
                                <div class="row flex-nowrap">
                                    <% for (int j = 1; j <= 6; j++)
                                        { %>
                                    <div class="col-md-3 col-sm-6">
                                        <div class="product-card">
                                            <div class="product-image">
                                                <img src="https://placehold.co/300x300" alt="Product <%= j %>" class="img-fluid">
                                            </div>
                                            <div class="product-info">
                                                <h5>Sản Phẩm <%= j %></h5>
                                                <p class="price"><%= j * 500000 %>đ</p>
                                                <button class="btn btn-outline-dark">Thêm Vào Giỏ</button>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>
                            </div>
                            <button type="button" class="slider-nav-btn next-btn" onclick="return scrollSlider(this, 'right')">
                                <i class="fas fa-chevron-right"></i>
                            </button>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
        </section>
    </div>

    <style>
        /* Featured Product Styles */
        .featured-product-card {
            position: relative;
            overflow: hidden;
            border-radius: 1rem;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            transition: transform 0.3s ease;
            height: 600px;
        }

        .featured-product-card:hover {
            transform: translateY(-5px);
        }

        .featured-product-card img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .featured-product-info {
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            padding: 2rem;
            background: linear-gradient(to top, rgba(0,0,0,0.8), transparent);
            color: white;
        }

        /* Hot Products Styles */
        .hot-products {
            background: #f8f9fa;
            padding: 1.5rem;
            border-radius: 1rem;
            height: 600px;
            display: flex;
            flex-direction: column;
        }

        .hot-products-list {
            flex: 1;
            overflow-y: auto;
            padding-right: 10px;
        }

        .hot-product-item {
            display: flex;
            align-items: center;
            padding: 1rem;
            border-bottom: 1px solid #dee2e6;
            transition: background-color 0.3s ease;
        }

            .hot-product-item:hover {
                background-color: #e9ecef;
            }

            .hot-product-item img {
                margin-right: 1rem;
                width: 80px;
                height: 80px;
                object-fit: cover;
            }

        .hot-product-info h5 {
            margin: 0;
            font-size: 1rem;
        }

        /* Product Categories Styles */
        .category-title {
            font-weight: 600;
            color: #1d1d1f;
        }

        .product-slider-container {
            position: relative;
            padding: 0 40px;
            touch-action: pan-y pinch-zoom;
        }

        .product-slider {
            position: relative;
            overflow: hidden;
            width: 100%;
            touch-action: pan-y pinch-zoom;
        }

            .product-slider .row {
                margin: 0;
                display: flex;
                transition: transform 0.3s ease;
                will-change: transform;
                touch-action: pan-y pinch-zoom;
            }

            .product-slider .col-md-3 {
                flex: 0 0 auto;
                width: 25%;
                padding: 0.5rem;
            }

        .slider-nav-btn {
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: white;
            border: 1px solid #dee2e6;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            z-index: 2;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: all 0.3s ease;
        }

            .slider-nav-btn:hover {
                background: #f8f9fa;
                box-shadow: 0 4px 8px rgba(0,0,0,0.15);
            }

        .prev-btn {
            left: 0;
        }

        .next-btn {
            right: 0;
        }

        .product-card {
            background: white;
            border-radius: 1rem;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0,0,0,0.05);
            transition: transform 0.3s ease;
            margin: 0.5rem;
        }

            .product-card:hover {
                transform: scale(1.05);
            }

        .product-image {
            overflow: hidden;
            position: relative;
        }

            .product-image img {
                width: 100%;
                height: auto;
                transition: transform 0.3s ease;
            }

        .product-card:hover .product-image img {
            transform: scale(1.1);
        }

        .product-info {
            padding: 1.5rem;
            text-align: center;
        }

            .product-info h5 {
                margin: 0 0 0.5rem;
                font-size: 1.1rem;
            }

        .price {
            color: #1d1d1f;
            font-weight: 600;
            margin-bottom: 1rem;
        }

        /* Custom Scrollbar for Hot Products */
        .hot-products-list::-webkit-scrollbar {
            width: 6px;
        }

        .hot-products-list::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 3px;
        }

        .hot-products-list::-webkit-scrollbar-thumb {
            background: #888;
            border-radius: 3px;
        }

            .hot-products-list::-webkit-scrollbar-thumb:hover {
                background: #555;
            }
    </style>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Khởi tạo tất cả các slider
            const sliders = document.querySelectorAll('.product-slider');
            sliders.forEach(initSlider);

            // Ngăn chặn scroll ngang ảnh hưởng đến scroll dọc
            document.querySelectorAll('.product-slider-container').forEach(container => {
                container.addEventListener('wheel', function (e) {
                    if (e.deltaY !== 0) {
                        e.preventDefault();
                    }
                }, { passive: false });
            });
        });

        function initSlider(slider) {
            if (slider.dataset.initiated) return; // Tránh init lại
            slider.dataset.initiated = "true";

            const row = slider.querySelector('.row');
            const items = Array.from(row.querySelectorAll('.col-md-3'));
            const itemWidth = items[0].offsetWidth;
            const itemsPerView = Math.floor(slider.offsetWidth / itemWidth);
            const totalItems = items.length;

            // Clone đầu và cuối
            for (let i = 0; i < itemsPerView; i++) {
                const cloneFirst = items[i].cloneNode(true);
                cloneFirst.classList.add("clone");
                row.appendChild(cloneFirst);

                const cloneLast = items[totalItems - 1 - i].cloneNode(true);
                cloneLast.classList.add("clone");
                row.insertBefore(cloneLast, row.firstChild);
            }

            const newTotalItems = row.querySelectorAll('.col-md-3').length;
            let currentTranslate = -itemsPerView * itemWidth;
            row.style.transform = `translateX(${currentTranslate}px)`;

            // Touch events
            let startX = 0;
            let scrollStartX = 0;

            const getTranslateX = () => {
                const matrix = new WebKitCSSMatrix(getComputedStyle(row).transform);
                return matrix.m41;
            };

            const resetPositionIfNeeded = () => {
                const maxScroll = -(totalItems + itemsPerView) * itemWidth;
                const minScroll = -itemsPerView * itemWidth;

                if (currentTranslate <= maxScroll) {
                    currentTranslate = -itemsPerView * itemWidth;
                    row.style.transition = 'none';
                    row.style.transform = `translateX(${currentTranslate}px)`;
                } else if (currentTranslate >= 0) {
                    currentTranslate = -totalItems * itemWidth;
                    row.style.transition = 'none';
                    row.style.transform = `translateX(${currentTranslate}px)`;
                }
            };

            row.addEventListener('touchstart', (e) => {
                startX = e.touches[0].pageX;
                scrollStartX = getTranslateX();
                row.style.transition = 'none';
            }, { passive: true });

            row.addEventListener('touchmove', (e) => {
                if (startX === null) return;
                const x = e.touches[0].pageX;
                const walk = (x - startX);
                currentTranslate = scrollStartX + walk;
                row.style.transform = `translateX(${currentTranslate}px)`;
                e.preventDefault();
            }, { passive: false });

            row.addEventListener('touchend', () => {
                resetPositionIfNeeded();
                row.style.transition = 'transform 0.3s ease'; // smooth lại
                startX = null;
            });

            // Optional: handle window resize
            window.addEventListener('resize', () => {
                // Đơn giản reload để tính lại offsetWidth và tránh bug
                location.reload();
            });
        }


        // function initSlider(slider) {
        //     const row = slider.querySelector('.row');
        //     const items = row.querySelectorAll('.col-md-3');
        //     const itemWidth = items[0].offsetWidth;
        //     const itemsPerView = Math.floor(slider.offsetWidth / itemWidth);
        //     const totalItems = items.length;

        //     // Clone các items đầu tiên và thêm vào cuối
        //     for (let i = 0; i < itemsPerView; i++) {
        //         const clone = items[i].cloneNode(true);
        //         row.appendChild(clone);
        //     }

        //     // Clone các items cuối cùng và thêm vào đầu
        //     for (let i = totalItems - 1; i >= totalItems - itemsPerView; i--) {
        //         const clone = items[i].cloneNode(true);
        //         row.insertBefore(clone, row.firstChild);
        //     }

        //     // Đặt vị trí ban đầu
        //     row.style.transform = `translateX(-${itemsPerView * itemWidth}px)`;

        //     // Thêm touch events cho mobile
        //     let startX, scrollLeft;
        //     row.addEventListener('touchstart', (e) => {
        //         startX = e.touches[0].pageX - row.offsetLeft;
        //         scrollLeft = new WebKitCSSMatrix(getComputedStyle(row).transform).m41;
        //     }, { passive: true });

        //     row.addEventListener('touchmove', (e) => {
        //         if (!startX) return;
        //         e.preventDefault();
        //         const x = e.touches[0].pageX - row.offsetLeft;
        //         const walk = (x - startX) * 2;
        //         row.style.transform = `translateX(${scrollLeft + walk}px)`;
        //     }, { passive: false });
        // }

        function scrollSlider(button, direction) {
            const container = button.closest('.product-slider-container');
            const slider = container.querySelector('.product-slider');
            const row = slider.querySelector('.row');
            const items = row.querySelectorAll('.col-md-3');
            const itemWidth = items[0].offsetWidth;
            const itemsPerView = Math.floor(slider.offsetWidth / itemWidth);
            const totalItems = items.length;

            // Lấy vị trí hiện tại
            const currentTransform = new WebKitCSSMatrix(getComputedStyle(row).transform);
            const currentPosition = currentTransform.m41;

            // Tính toán vị trí mới
            let newPosition;
            if (direction === 'left') {
                newPosition = currentPosition + itemWidth;
            } else {
                newPosition = currentPosition - itemWidth;
            }

            // Kiểm tra và xử lý infinite scroll
            const maxScroll = -(totalItems - itemsPerView) * itemWidth;
            const minScroll = 0;

            if (newPosition > minScroll) {
                // Nếu scroll quá đầu, nhảy về cuối
                newPosition = maxScroll;
            } else if (newPosition < maxScroll) {
                // Nếu scroll quá cuối, nhảy về đầu
                newPosition = minScroll;
            }

            // Thêm transition và scroll
            row.style.transition = 'transform 0.3s ease';
            row.style.transform = `translateX(${newPosition}px)`;

            // Xử lý transition end
            row.addEventListener('transitionend', function handler() {
                row.removeEventListener('transitionend', handler);
                row.style.transition = 'none';

                // Reset vị trí nếu cần
                if (newPosition === maxScroll) {
                    row.style.transform = `translateX(${minScroll}px)`;
                } else if (newPosition === minScroll) {
                    row.style.transform = `translateX(${maxScroll}px)`;
                }
            });
        }
    </script>
</asp:Content>
