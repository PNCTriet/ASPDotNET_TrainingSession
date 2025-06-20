<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="OrderItem_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organizer.OrderItem_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách chi tiết đơn hàng</title>
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <style>
        .modal-header {
            color: white;
        }

            .modal-header.edit {
                background-color: #f6c23e;
            }

            .modal-header.delete {
                background-color: #e74a3b;
            }

            .modal-header.create {
                background-color: #4e73df;
            }

        .modal-footer {
            background-color: #f8f9fc;
        }

        .btn-primary {
            background-color: #4e73df;
            border-color: #4e73df;
        }

            .btn-primary:hover {
                background-color: #2e59d9;
                border-color: #2653d4;
            }

        .btn-warning {
            background-color: #f6c23e;
            border-color: #f6c23e;
            color: #fff;
        }

            .btn-warning:hover {
                background-color: #f4b619;
                border-color: #f4b30d;
                color: #fff;
            }

        .btn-danger {
            background-color: #e74a3b;
            border-color: #e74a3b;
        }

            .btn-danger:hover {
                background-color: #e02d1b;
                border-color: #d52a1a;
            }

        .card {
            border: none;
            box-shadow: 0 0.15rem 1.75rem 0 rgba(58, 59, 69, 0.15);
        }

        .card-header {
            background-color: #f8f9fc;
            border-bottom: 1px solid #e3e6f0;
        }

        .table th {
            background-color: #f8f9fc;
        }

        .form-control:focus {
            border-color: #bac8f3;
            box-shadow: 0 0 0 0.2rem rgba(78, 115, 223, 0.25);
        }

        .required-field::after {
            content: " *";
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">Quản lý chi tiết đơn hàng</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i>Thêm chi tiết mới
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm chi tiết...">
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-secondary" type="button" id="searchButton">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvOrderItems" runat="server" CssClass="table table-bordered table-striped"
                                    AutoGenerateColumns="false" OnRowCommand="gvOrderItems_RowCommand"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="OrderId" HeaderText="Mã đơn hàng" />
                                        <asp:BoundField DataField="TicketId" HeaderText="Mã vé" />
                                        <asp:BoundField DataField="Quantity" HeaderText="Số lượng" />
                                        <asp:BoundField DataField="PriceSnapshot" HeaderText="Giá tại thời điểm" DataFormatString="{0:N0}" />
                                        <asp:TemplateField HeaderText="Thao tác">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm"
                                                    CommandName="EditOrderItem" CommandArgument='<%# Eval("Id") %>'
                                                    data-id='<%# Eval("Id") %>'
                                                    data-orderid='<%# Eval("OrderId") %>'
                                                    data-ticketid='<%# Eval("TicketId") %>'
                                                    data-quantity='<%# Eval("Quantity") %>'
                                                    data-pricesnapshot='<%# Eval("PriceSnapshot") %>'>
                                                    <i class="fas fa-edit text-white"></i> Sửa
                                                </asp:LinkButton>
                                                <button type="button" class="btn btn-danger btn-sm"
                                                    onclick='showDeleteModal(<%# Eval("Id") %>)'>
                                                    <i class="fas fa-trash"></i>Xóa
                                                </button>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Create Modal -->
    <div class="modal fade" id="createModal" tabindex="-1" aria-labelledby="createModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header create">
                    <h5 class="modal-title" id="createModalLabel">Thêm chi tiết đơn hàng</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="txtOrderId" class="form-label required-field">Mã đơn hàng</label>
                                <asp:TextBox ID="txtOrderId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtTicketId" class="form-label required-field">Mã vé</label>
                                <asp:TextBox ID="txtTicketId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtQuantity" class="form-label required-field">Số lượng</label>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPriceSnapshot" class="form-label required-field">Giá tại thời điểm</label>
                                <asp:TextBox ID="txtPriceSnapshot" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnCreate" runat="server" Text="Thêm mới" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Edit Modal -->
    <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header edit">
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa chi tiết đơn hàng</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditId" runat="server" />
                            <div class="mb-3">
                                <label for="txtEditOrderId" class="form-label required-field">Mã đơn hàng</label>
                                <asp:TextBox ID="txtEditOrderId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditTicketId" class="form-label required-field">Mã vé</label>
                                <asp:TextBox ID="txtEditTicketId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditQuantity" class="form-label required-field">Số lượng</label>
                                <asp:TextBox ID="txtEditQuantity" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditPriceSnapshot" class="form-label required-field">Giá tại thời điểm</label>
                                <asp:TextBox ID="txtEditPriceSnapshot" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" CssClass="btn btn-warning" OnClick="btnUpdate_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Delete Modal -->
    <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header delete">
                    <h5 class="modal-title" id="deleteModalLabel">Xác nhận xóa</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnDeleteId" runat="server" />
                    <asp:HiddenField ID="hdnForceDelete" runat="server" Value="false" />
                    <p>Bạn có chắc chắn muốn xóa chi tiết đơn hàng này?</p>
                    <div id="constraintWarning" class="alert alert-warning" style="display: none;">
                        <p><strong>Lưu ý:</strong> Chi tiết này có dữ liệu liên quan:</p>
                        <ul id="constraintList"></ul>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="Xóa" OnClick="btnDelete_Click" />
                </div>
            </div>
        </div>
    </div>
    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        // Xử lý sự kiện khi trang được tải
        document.addEventListener('DOMContentLoaded', function () {
            initializeButtons();
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            initializeButtons();
        });
        function initializeButtons() {
            var btnCreate = document.getElementById('<%= btnCreate.ClientID %>');
            if (btnCreate) {
                btnCreate.onclick = function (e) {
                    e.preventDefault();
                    if (validateCreateForm()) {
                        __doPostBack('<%= btnCreate.UniqueID %>', '');
                    }
                };
            }
            var btnUpdate = document.getElementById('<%= btnUpdate.ClientID %>');
            if (btnUpdate) {
                btnUpdate.onclick = function (e) {
                    e.preventDefault();
                    if (validateEditForm()) {
                        __doPostBack('<%= btnUpdate.UniqueID %>', '');
                    }
                };
            }
            var btnDelete = document.getElementById('<%= btnDelete.ClientID %>');
            if (btnDelete) {
                btnDelete.onclick = function (e) {
                    e.preventDefault();
                    if (validateDeleteForm()) {
                        __doPostBack('<%= btnDelete.UniqueID %>', '');
                    }
                };
            }
        }
        function validateEditForm() {
            var editOrderId = document.getElementById('<%= txtEditOrderId.ClientID %>');
            var editTicketId = document.getElementById('<%= txtEditTicketId.ClientID %>');
            var editQuantity = document.getElementById('<%= txtEditQuantity.ClientID %>');
            var editPriceSnapshot = document.getElementById('<%= txtEditPriceSnapshot.ClientID %>');
            if (!editOrderId.value || !editTicketId.value || !editQuantity.value || !editPriceSnapshot.value) {
                if (!editOrderId.value) editOrderId.focus();
                else if (!editTicketId.value) editTicketId.focus();
                else if (!editQuantity.value) editQuantity.focus();
                else if (!editPriceSnapshot.value) editPriceSnapshot.focus();
                return false;
            }
            return true;
        }
        function validateCreateForm() {
            var orderId = document.getElementById('<%= txtOrderId.ClientID %>');
            var ticketId = document.getElementById('<%= txtTicketId.ClientID %>');
            var quantity = document.getElementById('<%= txtQuantity.ClientID %>');
            var priceSnapshot = document.getElementById('<%= txtPriceSnapshot.ClientID %>');
            if (!orderId.value || !ticketId.value || !quantity.value || !priceSnapshot.value) {
                if (!orderId.value) orderId.focus();
                else if (!ticketId.value) ticketId.focus();
                else if (!quantity.value) quantity.focus();
                else if (!priceSnapshot.value) priceSnapshot.focus();
                return false;
            }
            return true;
        }
        function validateDeleteForm() {
            var itemId = document.getElementById('<%= hdnDeleteId.ClientID %>');
            if (!itemId.value) {
                showError('Không tìm thấy ID chi tiết!');
                return false;
            }
            return true;
        }
        function showDeleteModal(itemId) {
            document.getElementById('<%= hdnForceDelete.ClientID %>').value = 'false';
            PageMethods.CheckConstraints(itemId, function (result) {
                var constraintWarning = document.getElementById('constraintWarning');
                var constraintList = document.getElementById('constraintList');
                var hdnForceDelete = document.getElementById('<%= hdnForceDelete.ClientID %>');
                if (result.success && result.data && result.data.length > 0) {
                    constraintWarning.style.display = 'block';
                    constraintList.innerHTML = '';
                    for (var i = 0; i < result.data.length; i++) {
                        var li = document.createElement('li');
                        li.textContent = result.data[i];
                        constraintList.appendChild(li);
                    }
                    hdnForceDelete.value = 'true';
                } else {
                    constraintWarning.style.display = 'none';
                    hdnForceDelete.value = 'false';
                }
                document.getElementById('<%= hdnDeleteId.ClientID %>').value = itemId;
                var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
                deleteModal.show();
            }, function (error) {
                showError('Lỗi khi kiểm tra ràng buộc: ' + error);
            });
        }
        function showSuccess(message) {
            Swal.fire({ icon: 'success', title: 'Thành công', text: message, timer: 2000, showConfirmButton: false });
        }
        function showError(message) {
            Swal.fire({ icon: 'error', title: 'Lỗi', text: message });
        }
        $(document).ready(function () {
            $("#searchInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#<%= gvOrderItems.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#searchButton").on("click", function () {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvOrderItems.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#searchInput").on("input", function () {
                if ($(this).val() === "") {
                    $("#<%= gvOrderItems.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
