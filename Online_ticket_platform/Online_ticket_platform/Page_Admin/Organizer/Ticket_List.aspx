<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="Ticket_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organizer.Ticket_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Danh sách vé</title>
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
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
        .constraint-list {
            margin-top: 15px;
            padding: 10px;
            background-color: #f8f9fc;
            border-radius: 5px;
        }
        .constraint-list ul {
            margin-bottom: 0;
            padding-left: 20px;
        }
        .constraint-list li {
            margin-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    <!-- Begin Page Content -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">Quản lý vé</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i> Thêm vé mới
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm vé...">
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
                                <asp:GridView ID="gvTickets" runat="server" CssClass="table table-bordered table-striped" 
                                    AutoGenerateColumns="false" OnRowCommand="gvTickets_RowCommand"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="EventId" HeaderText="Sự kiện" />
                                        <asp:BoundField DataField="Name" HeaderText="Tên vé" />
                                        <asp:BoundField DataField="Price" HeaderText="Giá" DataFormatString="{0:N0} VNĐ" />
                                        <asp:BoundField DataField="Type" HeaderText="Loại vé" />
                                        <asp:BoundField DataField="Total" HeaderText="Tổng số" />
                                        <asp:BoundField DataField="Sold" HeaderText="Đã bán" />
                                        <asp:TemplateField HeaderText="Ngày bán">
                                            <ItemTemplate>
                                                <%# ((DateTime)Eval("StartSaleDate")).ToString("dd/MM/yyyy") %> - 
                                                <%# ((DateTime)Eval("EndSaleDate")).ToString("dd/MM/yyyy") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trạng thái">
                                            <ItemTemplate>
                                                <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "Đang bán" : "Ngừng bán" %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thao tác">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm" 
                                                    CommandName="EditTicket" CommandArgument='<%# Eval("Id") %>'
                                                    data-id='<%# Eval("Id") %>'
                                                    data-name='<%# Eval("Name") %>'
                                                    data-price='<%# Eval("Price") %>'
                                                    data-type='<%# Eval("Type") %>'
                                                    data-total='<%# Eval("Total") %>'
                                                    data-startdate='<%# ((DateTime)Eval("StartSaleDate")).ToString("yyyy-MM-dd") %>'
                                                    data-enddate='<%# ((DateTime)Eval("EndSaleDate")).ToString("yyyy-MM-dd") %>'
                                                    data-isactive='<%# Eval("IsActive") %>'>
                                                    <i class="fas fa-edit text-white"></i> Sửa
                                                </asp:LinkButton>
                                                <button type="button" class="btn btn-danger btn-sm" 
                                                    onclick='showDeleteModal(<%# Eval("Id") %>)'>
                                                    <i class="fas fa-trash"></i> Xóa
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
                    <h5 class="modal-title" id="createModalLabel">Thêm vé mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="txtEventId" class="form-label required-field">Sự kiện</label>
                                <asp:DropDownList ID="ddlEventId" runat="server" CssClass="form-control" required></asp:DropDownList>
                            </div>
                            <div class="mb-3">
                                <label for="txtName" class="form-label required-field">Tên vé</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPrice" class="form-label required-field">Giá vé</label>
                                <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtType" class="form-label required-field">Loại vé</label>
                                <asp:TextBox ID="txtType" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtTotal" class="form-label required-field">Tổng số vé</label>
                                <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtStartSaleDate" class="form-label required-field">Ngày bắt đầu bán</label>
                                <asp:TextBox ID="txtStartSaleDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEndSaleDate" class="form-label required-field">Ngày kết thúc bán</label>
                                <asp:TextBox ID="txtEndSaleDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label class="form-check-label" for="chkIsActive">Đang bán</label>
                                </div>
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
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa thông tin vé</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditId" runat="server" />
                            <div class="mb-3">
                                <label for="txtEditEventId" class="form-label required-field">Sự kiện</label>
                                <asp:DropDownList ID="ddlEditEventId" runat="server" CssClass="form-control" required></asp:DropDownList>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditName" class="form-label required-field">Tên vé</label>
                                <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditPrice" class="form-label required-field">Giá vé</label>
                                <asp:TextBox ID="txtEditPrice" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditType" class="form-label required-field">Loại vé</label>
                                <asp:TextBox ID="txtEditType" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditTotal" class="form-label required-field">Tổng số vé</label>
                                <asp:TextBox ID="txtEditTotal" runat="server" CssClass="form-control" TextMode="Number" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditStartSaleDate" class="form-label required-field">Ngày bắt đầu bán</label>
                                <asp:TextBox ID="txtEditStartSaleDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditEndSaleDate" class="form-label required-field">Ngày kết thúc bán</label>
                                <asp:TextBox ID="txtEditEndSaleDate" runat="server" CssClass="form-control" TextMode="Date" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkEditIsActive" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="chkEditIsActive">Đang bán</label>
                                </div>
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
                    <p>Bạn có chắc chắn muốn xóa vé này?</p>
                    <div id="constraintWarning" class="alert alert-warning" style="display: none;">
                        <p><strong>Lưu ý:</strong> Vé này có dữ liệu liên quan:</p>
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
        document.addEventListener('DOMContentLoaded', function() {
            initializeButtons();
        });

        // Xử lý sự kiện khi GridView được tải lại sau postback
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            initializeButtons();
        });

        // Hàm khởi tạo sự kiện cho các nút
        function initializeButtons() {
            // Xử lý nút Create
            var btnCreate = document.getElementById('<%= btnCreate.ClientID %>');
            if (btnCreate) {
                btnCreate.onclick = function(e) {
                    e.preventDefault();
                    if (validateCreateForm()) {
                        __doPostBack('<%= btnCreate.UniqueID %>', '');
                    }
                };
            }

            // Xử lý nút Update
            var btnUpdate = document.getElementById('<%= btnUpdate.ClientID %>');
            if (btnUpdate) {
                btnUpdate.onclick = function(e) {
                    e.preventDefault();
                    if (validateEditForm()) {
                        __doPostBack('<%= btnUpdate.UniqueID %>', '');
                    }
                };
            }

            // Xử lý nút Delete
            var btnDelete = document.getElementById('<%= btnDelete.ClientID %>');
            if (btnDelete) {
                btnDelete.onclick = function(e) {
                    e.preventDefault();
                    if (validateDeleteForm()) {
                        __doPostBack('<%= btnDelete.UniqueID %>', '');
                    }
                };
            }
        }

        // Xử lý validation cho form edit
        function validateEditForm() {
            var editName = document.getElementById('<%= txtEditName.ClientID %>');
            var editPrice = document.getElementById('<%= txtEditPrice.ClientID %>');
            var editType = document.getElementById('<%= txtEditType.ClientID %>');
            var editTotal = document.getElementById('<%= txtEditTotal.ClientID %>');
            var editStartDate = document.getElementById('<%= txtEditStartSaleDate.ClientID %>');
            var editEndDate = document.getElementById('<%= txtEditEndSaleDate.ClientID %>');

            if (!editName.value || !editPrice.value || !editType.value || !editTotal.value || !editStartDate.value || !editEndDate.value) {
                if (!editName.value) editName.focus();
                else if (!editPrice.value) editPrice.focus();
                else if (!editType.value) editType.focus();
                else if (!editTotal.value) editTotal.focus();
                else if (!editStartDate.value) editStartDate.focus();
                else if (!editEndDate.value) editEndDate.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form create
        function validateCreateForm() {
            var name = document.getElementById('<%= txtName.ClientID %>');
            var price = document.getElementById('<%= txtPrice.ClientID %>');
            var type = document.getElementById('<%= txtType.ClientID %>');
            var total = document.getElementById('<%= txtTotal.ClientID %>');
            var startDate = document.getElementById('<%= txtStartSaleDate.ClientID %>');
            var endDate = document.getElementById('<%= txtEndSaleDate.ClientID %>');

            if (!name.value || !price.value || !type.value || !total.value || !startDate.value || !endDate.value) {
                if (!name.value) name.focus();
                else if (!price.value) price.focus();
                else if (!type.value) type.focus();
                else if (!total.value) total.focus();
                else if (!startDate.value) startDate.focus();
                else if (!endDate.value) endDate.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form delete
        function validateDeleteForm() {
            var ticketId = document.getElementById('<%= hdnDeleteId.ClientID %>');
            if (!ticketId.value) {
                showError('Không tìm thấy ID vé!');
                return false;
            }
            return true;
        }

        function showDeleteModal(ticketId) {
            // Reset force delete flag
            document.getElementById('<%= hdnForceDelete.ClientID %>').value = 'false';
            
            // Kiểm tra ràng buộc
            PageMethods.CheckConstraints(ticketId, function(result) {
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
                
                // Hiển thị modal xóa
                document.getElementById('<%= hdnDeleteId.ClientID %>').value = ticketId;
                var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
                deleteModal.show();
            }, function(error) {
                showError('Lỗi khi kiểm tra ràng buộc: ' + error);
            });
        }

        // Hàm hiển thị thông báo thành công
        function showSuccess(message) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công',
                text: message,
                timer: 2000,
                showConfirmButton: false
            });
        }

        // Hàm hiển thị thông báo lỗi
        function showError(message) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: message
            });
        }

        // Search functionality
        $(document).ready(function() {
            $("#searchInput").on("keyup", function() {
                var value = $(this).val().toLowerCase();
                $("#<%= gvTickets.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchButton").on("click", function() {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvTickets.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchInput").on("input", function() {
                if ($(this).val() === "") {
                    $("#<%= gvTickets.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
