<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageProducts" MasterPageFile="~/Adminpage/Adminsite.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <script type="text/javascript">
        console.log('Head content loaded');
    </script>
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
                        DataKeyNames="ProductID,ProductName,Price,Stock">
                        <Columns>
                            <asp:BoundField DataField="ProductID" HeaderText="ID" />
                            <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm" HtmlEncode="false" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Danh mục" />
                            <asp:BoundField DataField="Price" HeaderText="Giá" DataFormatString="{0:N0} đ" />
                            <asp:BoundField DataField="Stock" HeaderText="Tồn kho" />
                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm btn-edit"
                                        CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>'
                                        data-id='<%# Eval("ProductID") %>'
                                        data-name='<%# Eval("ProductName") %>'
                                        data-price='<%# Eval("Price") %>'
                                        data-stock='<%# Eval("Stock") %>'>
                                        <i class="fas fa-edit"></i> Sửa
                                    </asp:LinkButton>
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
    <div class="modal fade" id="editProductModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Sửa thông tin sản phẩm</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnProductId" runat="server" />
                    <div class="mb-3">
                        <label for="txtProductName" class="form-label">Tên sản phẩm</label>
                        <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtPrice" class="form-label">Giá</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtStock" class="form-label">Tồn kho</label>
                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" required="required"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnSave" runat="server" Text="Lưu thay đổi" CssClass="btn btn-primary" OnClick="btnSave_Click" />
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
        console.log('Script section start');
        
        // Check if Bootstrap is loaded
        document.addEventListener('DOMContentLoaded', function() {
            console.log('DOM loaded');
            console.log('Bootstrap version:', typeof bootstrap !== 'undefined' ? bootstrap.version : 'Not loaded');
            console.log('jQuery version:', typeof $ !== 'undefined' ? $.fn.jquery : 'Not loaded');
            
            // Test modal functionality
            var testModal = new bootstrap.Modal(document.getElementById('editProductModal'));
            console.log('Modal instance created:', testModal);
        });

        $(document).ready(function () {
            console.log('jQuery ready - binding events');
            
            // Xử lý sự kiện click nút Edit
            $(document).on('click', '.btn-edit', function (e) {
                e.preventDefault();
                var productId = $(this).data('id');
                var productName = $(this).data('name');
                var price = $(this).data('price');
                var stock = $(this).data('stock');

                // Điền dữ liệu vào form
                $('#<%= hdnProductId.ClientID %>').val(productId);
                $('#<%= txtProductName.ClientID %>').val(productName);
                $('#<%= txtPrice.ClientID %>').val(price);
                $('#<%= txtStock.ClientID %>').val(stock);

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
                "dom": '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rt<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>',
                "pagingType": "full_numbers",
                "initComplete": function () {
                    // Style cho dropdown số lượng hiển thị
                    $('.dataTables_length select').addClass('form-select form-select-sm');
                    // Style cho ô tìm kiếm
                    $('.dataTables_filter input').addClass('form-control form-control-sm');
                    // Style cho các nút phân trang
                    $('.paginate_button').addClass('btn btn-sm');
                    $('.paginate_button.current').addClass('btn-primary');
                    $('.paginate_button:not(.current)').addClass('btn-outline-primary');
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
        /* Style cho DataTables */
        .dataTables_wrapper {
            padding: 15px 0;
        }
        .dataTables_length {
            margin-bottom: 15px;
        }
        .dataTables_length select {
            width: 80px !important;
            display: inline-block !important;
            margin: 0 5px;
        }
        .dataTables_filter {
            margin-bottom: 15px;
        }
        .dataTables_filter input {
            width: 200px !important;
            display: inline-block !important;
            margin-left: 5px;
        }
        .dataTables_info {
            padding-top: 15px;
            text-align: left;
        }
        .dataTables_paginate {
            padding-top: 15px;
            text-align: center !important;
            float: none !important;
        }
        .dataTables_paginate .paginate_button {
            margin: 0 2px;
            padding: 5px 10px;
            border-radius: 3px;
            cursor: pointer;
        }
        .dataTables_paginate .paginate_button.current {
            background: #4e73df !important;
            color: white !important;
            border: 1px solid #4e73df !important;
        }
        .dataTables_paginate .paginate_button:hover:not(.current) {
            background: #eaecf4 !important;
            color: #4e73df !important;
            border: 1px solid #eaecf4 !important;
        }
        .dataTables_paginate .paginate_button.disabled {
            color: #6c757d !important;
            cursor: not-allowed;
        }
        /* Style cho dropdown số lượng hiển thị */
        .dataTables_length select.form-select {
            padding: 0.25rem 2rem 0.25rem 0.5rem;
            font-size: 0.875rem;
            border-radius: 0.2rem;
            border: 1px solid #d1d3e2;
        }
        /* Style cho ô tìm kiếm */
        .dataTables_filter input.form-control {
            padding: 0.25rem 0.5rem;
            font-size: 0.875rem;
            border-radius: 0.2rem;
            border: 1px solid #d1d3e2;
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
