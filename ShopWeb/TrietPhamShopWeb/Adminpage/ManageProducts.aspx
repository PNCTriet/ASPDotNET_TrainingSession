<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageProducts" MasterPageFile="~/Adminpage/Adminsite.Master" %>

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
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="EditProduct" CommandArgument='<%# Eval("ProductID") %>'>
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

    <!-- Page level plugins -->
    <link href="../vendor/datatables/dataTables.bootstrap4.min.css" rel="stylesheet">
    <script src="../vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="../vendor/datatables/dataTables.bootstrap4.min.js"></script>

    <!-- Page level custom scripts -->
    <script type="text/javascript">
        $(document).ready(function () {
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
                    // Thêm class Bootstrap cho các nút phân trang
                    $('.paginate_button').addClass('btn btn-sm btn-outline-primary');
                    $('.paginate_button.current').addClass('btn-primary');
                }
            });

            // Hàm để refresh DataTable
            window.refreshDataTable = function() {
                dataTable.ajax.reload();
            };
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
