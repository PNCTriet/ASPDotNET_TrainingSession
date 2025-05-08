<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageProducts" MasterPageFile="~/Adminpage/Adminsite.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">

        <h1 class="h3 mb-2 text-gray-800">Quản lý sản phẩm</h1>
        <p class="mb-4">Danh sách sản phẩm của cửa hàng. Bạn có thể thêm, sửa, xóa sản phẩm từ đây.</p>

        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Danh sách sản phẩm</h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvProducts" runat="server" 
                        CssClass="table table-bordered" 
                        AutoGenerateColumns="False"
                        OnRowCommand="gvProducts_RowCommand"
                        OnPageIndexChanging="gvProducts_PageIndexChanging"
                        AllowPaging="True"
                        PageSize="10"
                        PagerStyle-CssClass="pagination"
                        PagerStyle-HorizontalAlign="Center"
                        HeaderStyle-CssClass="thead-light"
                        RowStyle-CssClass="align-middle">
                        <Columns>
                            <asp:BoundField DataField="ProductID" HeaderText="ID" />
                            <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Danh mục" />
                            <asp:BoundField DataField="Price" HeaderText="Giá" DataFormatString="{0:N0} đ" />
                            <asp:BoundField DataField="Stock" HeaderText="Tồn kho" />
                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>
                                    <button type="button" class="btn btn-primary btn-sm btn-edit" 
                                        data-id='<%# Eval("ProductID") %>'
                                        data-name='<%# Eval("ProductName") %>'
                                        data-category='<%# Eval("CategoryName") %>'
                                        data-price='<%# Eval("Price") %>'
                                        data-stock='<%# Eval("Stock") %>'>
                                        <i class="fas fa-edit"></i> Sửa
                                    </button>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                        CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>'
                                        OnClientClick='<%# "return confirm(\"Bạn có chắc chắn muốn xóa sản phẩm " + Eval("ProductName") + "?\");" %>'>
                                        <i class="fas fa-trash"></i> Xóa
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Product Modal -->
    <div class="modal fade" id="editProductModal" tabindex="-1" aria-labelledby="editProductModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editProductModalLabel">Sửa thông tin sản phẩm</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="editProductForm">
                        <input type="hidden" id="productId" name="productId" />
                        <div class="mb-3">
                            <label for="productName" class="form-label">Tên sản phẩm</label>
                            <input type="text" class="form-control" id="productName" name="productName" required>
                        </div>
                        <div class="mb-3">
                            <label for="categoryName" class="form-label">Danh mục</label>
                            <input type="text" class="form-control" id="categoryName" name="categoryName" readonly>
                        </div>
                        <div class="mb-3">
                            <label for="price" class="form-label">Giá</label>
                            <input type="number" class="form-control" id="price" name="price" required>
                        </div>
                        <div class="mb-3">
                            <label for="stock" class="form-label">Tồn kho</label>
                            <input type="number" class="form-control" id="stock" name="stock" required>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <button type="button" class="btn btn-primary" id="btnSaveChanges">Lưu thay đổi</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Page level plugins -->
    <link href="../vendor/datatables/dataTables.bootstrap4.min.css" rel="stylesheet">
    <script src="../vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="../vendor/datatables/dataTables.bootstrap4.min.js"></script>

    <!-- Page level custom scripts -->
    <script type="text/javascript">
        // Kiểm tra jQuery đã load chưa
        if (typeof jQuery != 'undefined') {
            console.log('jQuery version:', $.fn.jquery);
        } else {
            console.error('jQuery is not loaded!');
        }

        // Kiểm tra Bootstrap
        if (typeof bootstrap != 'undefined') {
            console.log('Bootstrap version:', bootstrap.Modal.VERSION);
        } else {
            console.error('Bootstrap is not loaded!');
        }

        // Thêm sự kiện click trực tiếp vào nút
        document.addEventListener('DOMContentLoaded', function() {
            console.log('DOM loaded');
            
            // Thử bắt sự kiện click bằng JavaScript thuần
            document.querySelectorAll('.btn-edit').forEach(function(button) {
                button.addEventListener('click', function(e) {
                    console.log('Button clicked (vanilla JS)');
                    e.preventDefault();
                });
            });

            // Thêm sự kiện click cho nút Save Changes
            var saveButton = document.getElementById('btnSaveChanges');
            if (saveButton) {
                console.log('Save button found');
                saveButton.addEventListener('click', function(e) {
                    console.log('Save button clicked (vanilla JS)');
                });
            } else {
                console.error('Save button not found!');
            }
        });

        $(document).ready(function () {
            console.log('jQuery ready');
            
            // Kiểm tra có bao nhiêu nút edit
            console.log('Number of edit buttons:', $('.btn-edit').length);

            // Thử bắt sự kiện click bằng jQuery
            $('.btn-edit').on('click', function(e) {
                console.log('Button clicked (jQuery)');
                e.preventDefault();
            });

            // Xử lý sự kiện click nút Edit
            $(document).on('click', '.btn-edit', function () {
                console.log('Edit button clicked');
                // Lấy dữ liệu từ data attributes
                var productId = $(this).data('id');
                var productName = $(this).data('name');
                var categoryName = $(this).data('category');
                var price = $(this).data('price');
                var stock = $(this).data('stock');

                console.log('Product data:', {
                    id: productId,
                    name: productName,
                    category: categoryName,
                    price: price,
                    stock: stock
                });

                // Điền dữ liệu vào form
                $('#productId').val(productId);
                $('#productName').val(productName);
                $('#categoryName').val(categoryName);
                $('#price').val(price);
                $('#stock').val(stock);

                // Hiển thị modal
                var editModal = new bootstrap.Modal(document.getElementById('editProductModal'));
                editModal.show();
            });

            var dataTable = $('#<%= gvProducts.ClientID %>').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Vietnamese.json"
                },
                "pageLength": 10,
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]],
                "ordering": true,
                "searching": true,
                "responsive": true,
                "processing": true,
                "serverSide": false,
                "deferRender": true,
                "dom": '<"top"lf>rt<"bottom"ip><"clear">',
                "pagingType": "full_numbers",
                "initComplete": function () {
                    $('.paginate_button').addClass('btn btn-sm btn-outline-primary');
                    $('.paginate_button.current').addClass('btn-primary');
                }
            });

            // Xử lý sự kiện click nút Save Changes
            $('#btnSaveChanges').on('click', function (e) {
                console.log('Save button clicked (jQuery)');
                e.preventDefault(); // Prevent form submission
                
                // Validate form
                var form = $('#editProductForm')[0];
                if (!form.checkValidity()) {
                    console.log('Form validation failed');
                    form.reportValidity();
                    return;
                }
                console.log('Form validation passed');

                if (confirm('Bạn có chắc chắn muốn lưu các thay đổi?')) {
                    // Tạo object chứa dữ liệu cần gửi
                    var productData = {
                        productId: parseInt($('#productId').val()),
                        productName: $('#productName').val(),
                        price: parseFloat($('#price').val()),
                        stock: parseInt($('#stock').val())
                    };

                    console.log('Preparing to send data:', productData);

                    // Gửi request Ajax
                    console.log('Sending Ajax request...');
                    $.ajax({
                        type: "POST",
                        url: "ManageProducts.aspx/UpdateProduct",
                        data: JSON.stringify(productData),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        beforeSend: function() {
                            console.log('Request is being sent...');
                        },
                        success: function (response) {
                            console.log('Received server response:', response);
                            if (response.d) {
                                console.log('Update successful');
                                // Đóng modal
                                var editModal = bootstrap.Modal.getInstance(document.getElementById('editProductModal'));
                                editModal.hide();

                                // Hiển thị thông báo thành công
                                alert('Cập nhật sản phẩm thành công!');

                                // Cập nhật dữ liệu trong DataTable
                                console.log('Refreshing DataTable...');
                                var table = $('#<%= gvProducts.ClientID %>').DataTable();
                                table.destroy(); // Hủy DataTable hiện tại
                                
                                // Gọi WebMethod để lấy dữ liệu mới
                                $.ajax({
                                    type: "POST",
                                    url: "ManageProducts.aspx/GetAllProducts",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (response) {
                                        console.log('Products loaded successfully');
                                        // Khởi tạo lại DataTable
                                        table = $('#<%= gvProducts.ClientID %>').DataTable({
                                            "language": {
                                                "url": "//cdn.datatables.net/plug-ins/1.10.24/i18n/Vietnamese.json"
                                            },
                                            "pageLength": 10,
                                            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "Tất cả"]],
                                            "ordering": true,
                                            "searching": true,
                                            "responsive": true,
                                            "processing": true,
                                            "serverSide": false,
                                            "deferRender": true,
                                            "dom": '<"top"lf>rt<"bottom"ip><"clear">',
                                            "pagingType": "full_numbers"
                                        });
                                        console.log('DataTable reinitialized');
                                    },
                                    error: function (xhr, status, error) {
                                        console.error('Error loading products:', error);
                                        alert('Có lỗi xảy ra khi tải dữ liệu!');
                                    }
                                });
                            } else {
                                console.log('Update failed');
                                alert('Có lỗi xảy ra khi cập nhật sản phẩm!');
                            }
                        },
                        error: function (xhr, status, error) {
                            console.error('Ajax error details:', {
                                status: status,
                                error: error,
                                responseText: xhr.responseText,
                                statusText: xhr.statusText
                            });
                            alert('Có lỗi xảy ra khi cập nhật sản phẩm!');
                        }
                    });
                } else {
                    console.log('User cancelled the update');
                }
            });
        });
    </script>

    <style type="text/css">
        .table-responsive {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }
        .table th, .table td {
            white-space: nowrap;
            vertical-align: middle !important;
        }
        .table th {
            background-color: #f8f9fc;
            border-bottom: 2px solid #e3e6f0;
        }
        .table td {
            border-bottom: 1px solid #e3e6f0;
        }
        .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.75rem;
            line-height: 1.5;
            border-radius: 0.2rem;
        }
        .dataTables_wrapper .dataTables_length, 
        .dataTables_wrapper .dataTables_filter {
            margin-bottom: 1rem;
        }
        .dataTables_wrapper .dataTables_info {
            padding-top: 1rem;
        }
        .dataTables_wrapper .dataTables_paginate {
            padding-top: 1rem;
        }
        /* Điều chỉnh kích thước cột */
        #<%= gvProducts.ClientID %> th:nth-child(1),
        #<%= gvProducts.ClientID %> td:nth-child(1) {
            width: 5% !important;
            max-width: 5% !important;
        }
        #<%= gvProducts.ClientID %> th:nth-child(2),
        #<%= gvProducts.ClientID %> td:nth-child(2) {
            width: 30% !important;
            max-width: 30% !important;
        }
        #<%= gvProducts.ClientID %> th:nth-child(3),
        #<%= gvProducts.ClientID %> td:nth-child(3) {
            width: 20% !important;
            max-width: 20% !important;
        }
        #<%= gvProducts.ClientID %> th:nth-child(4),
        #<%= gvProducts.ClientID %> td:nth-child(4) {
            width: 15% !important;
            max-width: 15% !important;
        }
        #<%= gvProducts.ClientID %> th:nth-child(5),
        #<%= gvProducts.ClientID %> td:nth-child(5) {
            width: 10% !important;
            max-width: 10% !important;
        }
        #<%= gvProducts.ClientID %> th:nth-child(6),
        #<%= gvProducts.ClientID %> td:nth-child(6) {
            width: 20% !important;
            max-width: 20% !important;
        }
    </style>
</asp:Content>
