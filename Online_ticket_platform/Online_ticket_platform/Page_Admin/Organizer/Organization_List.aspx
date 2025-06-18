<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="Organization_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organization_List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản lý tổ chức</title>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <!-- Begin Page Content -->
    <div class="container-fluid">
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">Quản lý tổ chức</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i> Thêm tổ chức mới
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm tổ chức...">
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-secondary" type="button" id="searchButton">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:GridView ID="gvOrganizations" runat="server" CssClass="table table-bordered table-striped" 
                            AutoGenerateColumns="false" OnRowCommand="gvOrganizations_RowCommand"
                            DataKeyNames="id">
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="ID" />
                                <asp:BoundField DataField="name" HeaderText="Tên tổ chức" />
                                <asp:BoundField DataField="contact_email" HeaderText="Email liên hệ" />
                                <asp:BoundField DataField="phone" HeaderText="Số điện thoại" />
                                <asp:BoundField DataField="address" HeaderText="Địa chỉ" />
                                <asp:BoundField DataField="created_at" HeaderText="Ngày tạo" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:TemplateField HeaderText="Thao tác">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm" 
                                            CommandName="EditOrganization" CommandArgument='<%# Eval("id") %>'
                                            data-id='<%# Eval("id") %>'
                                            data-name='<%# Eval("name") %>'
                                            data-email='<%# Eval("contact_email") %>'
                                            data-phone='<%# Eval("phone") %>'
                                            data-address='<%# Eval("address") %>'>
                                            <i class="fas fa-edit text-white"></i> Sửa
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm" 
                                            CommandName="DeleteOrganization" CommandArgument='<%# Eval("id") %>'
                                            data-id='<%# Eval("id") %>'
                                            data-name='<%# Eval("name") %>'
                                            OnClientClick="return false;">
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
    </div>

    <!-- Create Modal -->
    <div class="modal fade" id="createModal" tabindex="-1" aria-labelledby="createModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header create">
                    <h5 class="modal-title" id="createModalLabel">Thêm tổ chức mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="mb-3">
                                <label for="txtName" class="form-label required-field">Tên tổ chức</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEmail" class="form-label required-field">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtPhone" class="form-label required-field">Số điện thoại</label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtAddress" class="form-label required-field">Địa chỉ</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" required></asp:TextBox>
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
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa thông tin tổ chức</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditId" runat="server" />
                            <div class="mb-3">
                                <label for="txtEditName" class="form-label required-field">Tên tổ chức</label>
                                <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditEmail" class="form-label required-field">Email</label>
                                <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control" TextMode="Email" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditPhone" class="form-label required-field">Số điện thoại</label>
                                <asp:TextBox ID="txtEditPhone" runat="server" CssClass="form-control" required></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label for="txtEditAddress" class="form-label required-field">Địa chỉ</label>
                                <asp:TextBox ID="txtEditAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" required></asp:TextBox>
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
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnDeleteId" runat="server" />
                            <p>Bạn có chắc chắn muốn xóa tổ chức <strong id="deleteOrgName"></strong>?</p>
                            <div id="constraintWarning" class="constraint-list" style="display: none;">
                                <p class="text-danger"><strong>Lưu ý:</strong> Tổ chức này có dữ liệu liên quan:</p>
                                <ul id="constraintList"></ul>
                                <p class="text-warning">Bạn có muốn xóa tất cả dữ liệu liên quan không?</p>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <button type="button" class="btn btn-danger" id="btnForceDelete" style="display: none;">Xóa tất cả dữ liệu liên quan</button>
                    <asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script type="text/javascript">
        function showError(message, systemError) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: message,
                footer: systemError ? `Chi tiết lỗi: ${systemError}` : null
            });
        }

        function showSuccess(message) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: message,
                timer: 2000,
                showConfirmButton: false
            });
        }

        // Xử lý validation cho form edit
        function validateEditForm() {
            var editName = document.getElementById('<%= txtEditName.ClientID %>');
            var editEmail = document.getElementById('<%= txtEditEmail.ClientID %>');
            var editPhone = document.getElementById('<%= txtEditPhone.ClientID %>');
            var editAddress = document.getElementById('<%= txtEditAddress.ClientID %>');

            if (!editName.value || !editEmail.value || !editPhone.value || !editAddress.value) {
                if (!editName.value) editName.reportValidity();
                if (!editEmail.value) editEmail.reportValidity();
                if (!editPhone.value) editPhone.reportValidity();
                if (!editAddress.value) editAddress.reportValidity();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form create
        function validateCreateForm() {
            var name = document.getElementById('<%= txtName.ClientID %>');
            var email = document.getElementById('<%= txtEmail.ClientID %>');
            var phone = document.getElementById('<%= txtPhone.ClientID %>');
            var address = document.getElementById('<%= txtAddress.ClientID %>');

            if (!name.value || !email.value || !phone.value || !address.value) {
                if (!name.value) name.reportValidity();
                if (!email.value) email.reportValidity();
                if (!phone.value) phone.reportValidity();
                if (!address.value) address.reportValidity();
                return false;
            }
            return true;
        }

        // Xử lý sự kiện click nút Update
        document.getElementById('<%= btnUpdate.ClientID %>').onclick = function(e) {
            e.preventDefault();
            if (validateEditForm()) {
                // Nếu form hợp lệ, submit form
                __doPostBack('<%= btnUpdate.UniqueID %>', '');
            }
        };

        // Xử lý sự kiện click nút Create
        $(document).ready(function() {
            $('#<%= btnCreate.ClientID %>').click(function(e) {
                e.preventDefault();
                if (validateCreateForm()) {
                    // Nếu form hợp lệ, submit form
                    __doPostBack('<%= btnCreate.UniqueID %>', '');
                }
            });
        });

        // Xử lý sự kiện khi modal edit được hiển thị
        document.getElementById('editModal').addEventListener('show.bs.modal', function(event) {
            // Lấy nút đã click
            var button = event.relatedTarget;
            if (!button) return;

            // Lấy dữ liệu từ data attributes
            var id = button.getAttribute('data-id');
            var name = button.getAttribute('data-name');
            var email = button.getAttribute('data-email');
            var phone = button.getAttribute('data-phone');
            var address = button.getAttribute('data-address');

            // Điền dữ liệu vào form
            document.getElementById('<%= hdnEditId.ClientID %>').value = id;
            document.getElementById('<%= txtEditName.ClientID %>').value = name;
            document.getElementById('<%= txtEditEmail.ClientID %>').value = email;
            document.getElementById('<%= txtEditPhone.ClientID %>').value = phone;
            document.getElementById('<%= txtEditAddress.ClientID %>').value = address;
        });

        // Xử lý sự kiện khi modal create được hiển thị
        $('#createModal').on('show.bs.modal', function() {
            // Reset validation state
            $('#<%= txtName.ClientID %>').val('');
            $('#<%= txtEmail.ClientID %>').val('');
            $('#<%= txtPhone.ClientID %>').val('');
            $('#<%= txtAddress.ClientID %>').val('');
        });

        // Hàm hiển thị modal edit
        function showEditModal(id) {
            var button = document.querySelector(`[data-id="${id}"]`);
            if (!button) return;

            var editModal = new bootstrap.Modal(document.getElementById('editModal'));
            
            // Lấy dữ liệu từ data attributes
            document.getElementById('<%= hdnEditId.ClientID %>').value = id;
            document.getElementById('<%= txtEditName.ClientID %>').value = button.getAttribute('data-name');
            document.getElementById('<%= txtEditEmail.ClientID %>').value = button.getAttribute('data-email');
            document.getElementById('<%= txtEditPhone.ClientID %>').value = button.getAttribute('data-phone');
            document.getElementById('<%= txtEditAddress.ClientID %>').value = button.getAttribute('data-address');
            
            editModal.show();
        }

        // Hàm hiển thị modal delete
        function showDeleteModal(id) {
            var button = document.querySelector(`[data-id="${id}"]`);
            if (!button) return;

            var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
            
            // Lấy tên tổ chức từ data attribute
            document.getElementById('deleteOrgName').textContent = button.getAttribute('data-name');
            document.getElementById('<%= hdnDeleteId.ClientID %>').value = id;
            
            // Ẩn các phần tử liên quan đến ràng buộc
            document.getElementById('constraintWarning').style.display = 'none';
            document.getElementById('btnForceDelete').style.display = 'none';
            document.getElementById('constraintList').innerHTML = '';
            
            // Hiển thị modal
            deleteModal.show();
        }

        // Xử lý sự kiện click nút Delete
        document.getElementById('<%= btnDelete.ClientID %>').onclick = function(e) {
            e.preventDefault();
            var id = document.getElementById('<%= hdnDeleteId.ClientID %>').value;
            
            // Kiểm tra ràng buộc
            checkConstraints(id, function(constraints) {
                var constraintWarning = document.getElementById('constraintWarning');
                var constraintList = document.getElementById('constraintList');
                var btnForceDelete = document.getElementById('btnForceDelete');
                
                if (constraints && constraints.length > 0) {
                    // Hiển thị thông tin ràng buộc
                    constraintWarning.style.display = 'block';
                    constraintList.innerHTML = '';
                    constraints.forEach(function(constraint) {
                        var li = document.createElement('li');
                        li.textContent = constraint;
                        constraintList.appendChild(li);
                    });
                    
                    // Hiển thị nút force delete
                    btnForceDelete.style.display = 'inline-block';
                    
                    // Ẩn nút xóa thông thường
                    document.getElementById('<%= btnDelete.ClientID %>').style.display = 'none';
                } else {
                    // Nếu không có ràng buộc, thực hiện xóa ngay
                    __doPostBack('<%= btnDelete.UniqueID %>', '');
                }
            });
        };

        // Xử lý sự kiện click nút Force Delete
        document.getElementById('btnForceDelete').onclick = function(e) {
            e.preventDefault();
            // Thêm tham số forceDelete vào form
            var forceDeleteInput = document.createElement('input');
            forceDeleteInput.type = 'hidden';
            forceDeleteInput.name = 'forceDelete';
            forceDeleteInput.value = 'true';
            document.getElementById('form1').appendChild(forceDeleteInput);
            
            // Thực hiện xóa
            __doPostBack('<%= btnDelete.UniqueID %>', '');
        };

        // Hàm kiểm tra ràng buộc
        function checkConstraints(id, callback) {
            $.ajax({
                url: 'Organization_List.aspx/CheckConstraints',
                type: 'POST',
                data: JSON.stringify({ organizationId: id }),
                contentType: 'application/json',
                success: function(response) {
                    if (callback) {
                        callback(response.d);
                    }
                },
                error: function(xhr, status, error) {
                    showError('Không thể kiểm tra ràng buộc: ' + error);
                    if (callback) {
                        callback([]);
                    }
                }
            });
        }

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
            var editButtons = document.querySelectorAll('[id*="btnEdit"]');
            var deleteButtons = document.querySelectorAll('[id*="btnDelete"]');

            editButtons.forEach(function(button) {
                button.addEventListener('click', function(e) {
                    e.preventDefault();
                    var id = this.getAttribute('data-id');
                    showEditModal(id);
                });
            });

            deleteButtons.forEach(function(button) {
                button.addEventListener('click', function(e) {
                    e.preventDefault();
                    var id = this.getAttribute('data-id');
                    showDeleteModal(id);
                });
            });
        }

        $(document).ready(function() {
            // Search functionality
            $("#searchInput").on("keyup", function() {
                var value = $(this).val().toLowerCase();
                $("#<%= gvOrganizations.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            // Search button click
            $("#searchButton").on("click", function() {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvOrganizations.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            // Clear search when input is cleared
            $("#searchInput").on("input", function() {
                if ($(this).val() === "") {
                    $("#<%= gvOrganizations.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
