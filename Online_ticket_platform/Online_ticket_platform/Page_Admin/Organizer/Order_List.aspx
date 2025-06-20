<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="Order_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organizer.Order_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách đơn hàng</title>
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
                        <h3 class="card-title">Quản lý đơn hàng</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i>Thêm đơn hàng mới
                           
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm đơn hàng...">
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
                                <asp:GridView ID="gvOrders" runat="server" CssClass="table table-bordered table-striped"
                                    AutoGenerateColumns="false" OnRowCommand="gvOrders_RowCommand"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="UserId" HeaderText="Mã người dùng" />
                                        <asp:BoundField DataField="Status" HeaderText="Trạng thái" />
                                        <asp:BoundField DataField="PaymentMethod" HeaderText="Phương thức thanh toán" />
                                        <asp:BoundField DataField="Amount" HeaderText="Tổng tiền" DataFormatString="{0:N0}" />
                                        <asp:TemplateField HeaderText="Ngày tạo">
                                            <ItemTemplate>
                                                <%# ((DateTime)Eval("CreatedAt")).ToString("yyyy-MM-dd HH:mm") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thao tác">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm"
                                                    CommandName="EditOrder" CommandArgument='<%# Eval("Id") %>'
                                                    data-id='<%# Eval("Id") %>'
                                                    data-userid='<%# Eval("UserId") %>'
                                                    data-status='<%# Eval("Status") %>'
                                                    data-paymentmethod='<%# Eval("PaymentMethod") %>'
                                                    data-amount='<%# Eval("Amount") %>'
                                                    data-createdat='<%# ((DateTime)Eval("CreatedAt")).ToString("yyyy-MM-dd HH:mm") %>'>
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
                    <h5 class="modal-title" id="createModalLabel">Thêm đơn hàng mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="txtUserId" class="form-label required-field">Mã người dùng</label>
                                <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtStatus" class="form-label required-field">Trạng thái</label>
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPaymentMethod" class="form-label required-field">Phương thức thanh toán</label>
                                <asp:TextBox ID="txtPaymentMethod" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtAmount" class="form-label required-field">Tổng tiền</label>
                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" required></asp:TextBox>
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
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa đơn hàng</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditId" runat="server" />
                            <div class="mb-3">
                                <label for="txtEditUserId" class="form-label required-field">Mã người dùng</label>
                                <asp:TextBox ID="txtEditUserId" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditStatus" class="form-label required-field">Trạng thái</label>
                                <asp:TextBox ID="txtEditStatus" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditPaymentMethod" class="form-label required-field">Phương thức thanh toán</label>
                                <asp:TextBox ID="txtEditPaymentMethod" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditAmount" class="form-label required-field">Tổng tiền</label>
                                <asp:TextBox ID="txtEditAmount" runat="server" CssClass="form-control" required></asp:TextBox>
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
                    <p>Bạn có chắc chắn muốn xóa đơn hàng này?</p>
                    <div id="constraintWarning" class="alert alert-warning" style="display: none;">
                        <p><strong>Lưu ý:</strong> Đơn hàng này có dữ liệu liên quan:</p>
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
            var editUserId = document.getElementById('<%= txtEditUserId.ClientID %>');
            var editStatus = document.getElementById('<%= txtEditStatus.ClientID %>');
            var editPaymentMethod = document.getElementById('<%= txtEditPaymentMethod.ClientID %>');
            var editAmount = document.getElementById('<%= txtEditAmount.ClientID %>');
            if (!editUserId.value || !editStatus.value || !editPaymentMethod.value || !editAmount.value) {
                if (!editUserId.value) editUserId.focus();
                else if (!editStatus.value) editStatus.focus();
                else if (!editPaymentMethod.value) editPaymentMethod.focus();
                else if (!editAmount.value) editAmount.focus();
                return false;
            }
            return true;
        }
        function validateCreateForm() {
            var userId = document.getElementById('<%= txtUserId.ClientID %>');
            var status = document.getElementById('<%= txtStatus.ClientID %>');
            var paymentMethod = document.getElementById('<%= txtPaymentMethod.ClientID %>');
            var amount = document.getElementById('<%= txtAmount.ClientID %>');
            if (!userId.value || !status.value || !paymentMethod.value || !amount.value) {
                if (!userId.value) userId.focus();
                else if (!status.value) status.focus();
                else if (!paymentMethod.value) paymentMethod.focus();
                else if (!amount.value) amount.focus();
                return false;
            }
            return true;
        }
        function validateDeleteForm() {
            var orderId = document.getElementById('<%= hdnDeleteId.ClientID %>');
            if (!orderId.value) {
                showError('Không tìm thấy ID đơn hàng!');
                return false;
            }
            return true;
        }
        function showDeleteModal(orderId) {
            document.getElementById('<%= hdnForceDelete.ClientID %>').value = 'false';
            PageMethods.CheckConstraints(orderId, function (result) {
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
                document.getElementById('<%= hdnDeleteId.ClientID %>').value = orderId;
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
                $("#<%= gvOrders.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#searchButton").on("click", function () {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvOrders.ClientID %> tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
            $("#searchInput").on("input", function () {
                if ($(this).val() === "") {
                    $("#<%= gvOrders.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
