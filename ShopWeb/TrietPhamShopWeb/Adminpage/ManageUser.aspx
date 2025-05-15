<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageUser" MasterPageFile="~/Adminpage/Adminsite.Master" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Bootstrap 5 JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    <!-- Font Awesome -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="h3 mb-2 text-gray-800">Quản lý người dùng</h1>
        <p class="mb-4">Danh sách người dùng của hệ thống. Bạn có thể xem, sửa, xóa thông tin người dùng từ đây.</p>

        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Danh sách người dùng</h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvUsers" runat="server" 
                        CssClass="table table-bordered" 
                        AutoGenerateColumns="False"
                        OnRowCommand="gvUsers_RowCommand"
                        OnPageIndexChanging="gvUsers_PageIndexChanging"
                        AllowPaging="True"
                        PageSize="10"
                        DataKeyNames="UserID">
                        <Columns>
                            <asp:BoundField DataField="UserID" HeaderText="ID" />
                            <asp:BoundField DataField="Username" HeaderText="Tên đăng nhập" />
                            <asp:BoundField DataField="FirstName" HeaderText="Họ" />
                            <asp:BoundField DataField="LastName" HeaderText="Tên" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="RoleName" HeaderText="Vai trò" />
                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <div class="d-flex align-items-center">
                                        <span class="status-dot <%# GetStatusClass(Eval("IsActive")) %>"></span>
                                        <span class="ms-2"><%# GetStatusText(Eval("IsActive")) %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm btn-edit"
                                        CommandName="EditUser" CommandArgument='<%# Eval("UserID") %>'
                                        data-id='<%# Eval("UserID") %>'
                                        data-username='<%# Eval("Username") %>'
                                        data-email='<%# Eval("Email") %>'
                                        data-firstname='<%# Eval("FirstName") %>'
                                        data-lastname='<%# Eval("LastName") %>'
                                        data-roleid='<%# Eval("RoleID") %>'
                                        data-isactive='<%# Eval("IsActive") %>'>
                                        <i class="fas fa-edit"></i> Sửa
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                                        CommandName="DeleteUser" CommandArgument='<%# Eval("UserID") %>'
                                        OnClientClick='<%# "return confirm(\"Bạn có chắc chắn muốn xóa người dùng " + Eval("Username") + "?\");" %>'>
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

    <!-- Edit User Modal -->
    <div class="modal fade" id="editUserModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Sửa thông tin người dùng</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hdnUserId" runat="server" />
                    <div class="mb-3">
                        <label for="txtUsername" class="form-label">Tên đăng nhập</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtFirstName" class="form-label">Họ</label>
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtLastName" class="form-label">Tên</label>
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" required="required"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="ddlRole" class="form-label">Vai trò</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <div class="form-check">
                            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label" for="chkIsActive">Kích hoạt</label>
                        </div>
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
        $(document).ready(function () {
            // Xử lý sự kiện click nút Edit
            $(document).on('click', '.btn-edit', function (e) {
                e.preventDefault();
                var userId = $(this).data('id');
                var username = $(this).data('username');
                var email = $(this).data('email');
                var firstName = $(this).data('firstname');
                var lastName = $(this).data('lastname');
                var roleId = $(this).data('roleid');
                var isActive = $(this).data('isactive');

                // Điền dữ liệu vào form
                $('#<%= hdnUserId.ClientID %>').val(userId);
                $('#<%= txtUsername.ClientID %>').val(username);
                $('#<%= txtFirstName.ClientID %>').val(firstName);
                $('#<%= txtLastName.ClientID %>').val(lastName);
                $('#<%= txtEmail.ClientID %>').val(email);
                $('#<%= ddlRole.ClientID %>').val(roleId);
                $('#<%= chkIsActive.ClientID %>').prop('checked', isActive);

                // Hiển thị modal
                var editModal = new bootstrap.Modal(document.getElementById('editUserModal'));
                editModal.show();
            });

            // Khởi tạo DataTable
            var dataTable = $('#<%= gvUsers.ClientID %>').DataTable({
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
        #<%= gvUsers.ClientID %> th:nth-child(1),
        #<%= gvUsers.ClientID %> td:nth-child(1) {
            width: 5% !important;
            max-width: 5% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(2),
        #<%= gvUsers.ClientID %> td:nth-child(2) {
            width: 15% !important;
            max-width: 15% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(3),
        #<%= gvUsers.ClientID %> td:nth-child(3) {
            width: 10% !important;
            max-width: 10% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(4),
        #<%= gvUsers.ClientID %> td:nth-child(4) {
            width: 10% !important;
            max-width: 10% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(5),
        #<%= gvUsers.ClientID %> td:nth-child(5) {
            width: 20% !important;
            max-width: 20% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(6),
        #<%= gvUsers.ClientID %> td:nth-child(6) {
            width: 15% !important;
            max-width: 15% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(7),
        #<%= gvUsers.ClientID %> td:nth-child(7) {
            width: 10% !important;
            max-width: 10% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(8),
        #<%= gvUsers.ClientID %> td:nth-child(8) {
            width: 10% !important;
            max-width: 10% !important;
        }
        #<%= gvUsers.ClientID %> th:nth-child(9),
        #<%= gvUsers.ClientID %> td:nth-child(9) {
            width: 15% !important;
            max-width: 15% !important;
        }
        /* Status dot styles */
        .status-dot {
            width: 10px;
            height: 10px;
            border-radius: 50%;
            display: inline-block;
        }
        .status-active {
            background-color: #28a745;
            box-shadow: 0 0 0 2px rgba(40, 167, 69, 0.2);
        }
        .status-inactive {
            background-color: #dc3545;
            box-shadow: 0 0 0 2px rgba(255, 193, 7, 0.2);
        }
        .status-blocked {
            background-color: #ffc107;
            box-shadow: 0 0 0 2px rgba(220, 53, 69, 0.2);
        }
        /* Status text colors */
        .status-active + span {
            color: #28a745;
        }
        .status-inactive + span {
            color: #dc3545;
        }
        .status-blocked + span {
            color: #ffc107;
        }
    </style>
</asp:Content>
