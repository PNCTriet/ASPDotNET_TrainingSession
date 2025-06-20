<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="ImageLink_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organizer.ImageLink_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản lý Liên kết Hình ảnh</title>
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
        .image-preview {
            max-width: 100px;
            max-height: 100px;
            object-fit: cover;
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
                        <h3 class="card-title">Quản lý Liên kết Hình ảnh</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i> Thêm liên kết mới
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm liên kết hình ảnh...">
                                    <div class="input-group-append">
                                        <button class="btn btn-outline-secondary" type="button" id="searchButton">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvImageLinks" runat="server" CssClass="table table-bordered table-striped" 
                                    AutoGenerateColumns="false" OnRowCommand="gvImageLinks_RowCommand"
                                    DataKeyNames="Id" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvImageLinks_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:TemplateField HeaderText="Hình ảnh">
                                            <ItemTemplate>
                                                <img src='<%# Eval("Image.FilePath") %>' alt='<%# Eval("Image.AltText") %>' 
                                                     class="image-preview img-thumbnail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EntityType" HeaderText="Loại Entity" />
                                        <asp:BoundField DataField="EntityId" HeaderText="ID Entity" />
                                        <asp:BoundField DataField="UsageType" HeaderText="Loại Sử dụng" />
                                        <asp:TemplateField HeaderText="Ngày Liên kết">
                                            <ItemTemplate>
                                                <%# ((DateTime)Eval("LinkedAt")).ToString("dd/MM/yyyy HH:mm") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thao tác">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm" 
                                                    CommandName="EditRow" CommandArgument='<%# Eval("Id") %>'
                                                    data-id='<%# Eval("Id") %>'
                                                    data-imageid='<%# Eval("ImageId") %>'
                                                    data-entitytype='<%# Eval("EntityType") %>'
                                                    data-entityid='<%# Eval("EntityId") %>'
                                                    data-usagetype='<%# Eval("UsageType") %>'
                                                    data-organizationid='<%# Eval("OrganizationId") %>'
                                                    data-eventid='<%# Eval("EventId") %>'>
                                                    <i class="fas fa-edit text-white"></i> Sửa
                                                </asp:LinkButton>
                                                <button type="button" class="btn btn-danger btn-sm" 
                                                    onclick='showDeleteModal(<%# Eval("Id") %>)'>
                                                    <i class="fas fa-trash"></i> Xóa
                                                </button>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pagination" />
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
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header create">
                    <h5 class="modal-title" id="createModalLabel">Thêm liên kết hình ảnh mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlImage" class="form-label required-field">Hình ảnh</label>
                                        <asp:DropDownList ID="ddlImage" runat="server" CssClass="form-control" required>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEntityType" class="form-label required-field">Loại Entity</label>
                                        <asp:DropDownList ID="ddlEntityType" runat="server" CssClass="form-control" required>
                                            <asp:ListItem Text="-- Chọn loại --" Value="" />
                                            <asp:ListItem Text="Event" Value="event" />
                                            <asp:ListItem Text="Organization" Value="organization" />
                                            <asp:ListItem Text="User" Value="user" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEntityId" class="form-label required-field">ID Entity</label>
                                        <asp:TextBox ID="txtEntityId" runat="server" CssClass="form-control" TextMode="Number" required />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlUsageType" class="form-label required-field">Loại Sử dụng</label>
                                        <asp:DropDownList ID="ddlUsageType" runat="server" CssClass="form-control" required>
                                            <asp:ListItem Text="-- Chọn loại --" Value="" />
                                            <asp:ListItem Text="Banner" Value="banner" />
                                            <asp:ListItem Text="Thumbnail" Value="thumbnail" />
                                            <asp:ListItem Text="Gallery" Value="gallery" />
                                            <asp:ListItem Text="Logo" Value="logo" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlOrganization" class="form-label">Tổ chức (tùy chọn)</label>
                                        <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="-- Chọn tổ chức --" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEvent" class="form-label">Sự kiện (tùy chọn)</label>
                                        <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="-- Chọn sự kiện --" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <!-- Debug Info -->
                            <div id="createDebugInfo" class="alert alert-danger" style="display: none;">
                                <strong>Debug Info:</strong>
                                <pre id="createDebugDetails"></pre>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnAddImageLink" runat="server" Text="Thêm mới" CssClass="btn btn-primary" OnClick="btnAddImageLink_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header edit">
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa liên kết hình ảnh</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditImageLinkId" runat="server" />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEditImage" class="form-label required-field">Hình ảnh</label>
                                        <asp:DropDownList ID="ddlEditImage" runat="server" CssClass="form-control" required>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEditEntityType" class="form-label required-field">Loại Entity</label>
                                        <asp:DropDownList ID="ddlEditEntityType" runat="server" CssClass="form-control" required>
                                            <asp:ListItem Text="-- Chọn loại --" Value="" />
                                            <asp:ListItem Text="Event" Value="event" />
                                            <asp:ListItem Text="Organization" Value="organization" />
                                            <asp:ListItem Text="User" Value="user" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditEntityId" class="form-label required-field">ID Entity</label>
                                        <asp:TextBox ID="txtEditEntityId" runat="server" CssClass="form-control" TextMode="Number" required />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEditUsageType" class="form-label required-field">Loại Sử dụng</label>
                                        <asp:DropDownList ID="ddlEditUsageType" runat="server" CssClass="form-control" required>
                                            <asp:ListItem Text="-- Chọn loại --" Value="" />
                                            <asp:ListItem Text="Banner" Value="banner" />
                                            <asp:ListItem Text="Thumbnail" Value="thumbnail" />
                                            <asp:ListItem Text="Gallery" Value="gallery" />
                                            <asp:ListItem Text="Logo" Value="logo" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEditOrganization" class="form-label">Tổ chức (tùy chọn)</label>
                                        <asp:DropDownList ID="ddlEditOrganization" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="-- Chọn tổ chức --" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="ddlEditEvent" class="form-label">Sự kiện (tùy chọn)</label>
                                        <asp:DropDownList ID="ddlEditEvent" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="-- Chọn sự kiện --" Value="" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <!-- Debug Info -->
                            <div id="editDebugInfo" class="alert alert-danger" style="display: none;">
                                <strong>Debug Info:</strong>
                                <pre id="editDebugDetails"></pre>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnUpdateImageLink" runat="server" Text="Cập nhật" CssClass="btn btn-warning" OnClick="btnUpdateImageLink_Click" />
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
                    <p>Bạn có chắc chắn muốn xóa liên kết hình ảnh này?</p>
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
            var btnCreate = document.getElementById('<%= btnAddImageLink.ClientID %>');
            if (btnCreate) {
                btnCreate.onclick = function(e) {
                    e.preventDefault();
                    if (validateCreateForm()) {
                        __doPostBack('<%= btnAddImageLink.UniqueID %>', '');
                    }
                };
            }

            // Xử lý nút Update
            var btnUpdate = document.getElementById('<%= btnUpdateImageLink.ClientID %>');
            if (btnUpdate) {
                btnUpdate.onclick = function(e) {
                    e.preventDefault();
                    if (validateEditForm()) {
                        __doPostBack('<%= btnUpdateImageLink.UniqueID %>', '');
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
            var editImage = document.getElementById('<%= ddlEditImage.ClientID %>');
            var editEntityType = document.getElementById('<%= ddlEditEntityType.ClientID %>');
            var editEntityId = document.getElementById('<%= txtEditEntityId.ClientID %>');
            var editUsageType = document.getElementById('<%= ddlEditUsageType.ClientID %>');

            if (!editImage.value || !editEntityType.value || !editEntityId.value || !editUsageType.value) {
                if (!editImage.value) editImage.focus();
                else if (!editEntityType.value) editEntityType.focus();
                else if (!editEntityId.value) editEntityId.focus();
                else if (!editUsageType.value) editUsageType.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form create
        function validateCreateForm() {
            var image = document.getElementById('<%= ddlImage.ClientID %>');
            var entityType = document.getElementById('<%= ddlEntityType.ClientID %>');
            var entityId = document.getElementById('<%= txtEntityId.ClientID %>');
            var usageType = document.getElementById('<%= ddlUsageType.ClientID %>');

            if (!image.value || !entityType.value || !entityId.value || !usageType.value) {
                if (!image.value) image.focus();
                else if (!entityType.value) entityType.focus();
                else if (!entityId.value) entityId.focus();
                else if (!usageType.value) usageType.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form delete
        function validateDeleteForm() {
            var imageLinkId = document.getElementById('<%= hdnDeleteId.ClientID %>');
            if (!imageLinkId.value) {
                showError('Không tìm thấy ID liên kết hình ảnh!');
                return false;
            }
            return true;
        }

        function showDeleteModal(imageLinkId) {
            // Hiển thị modal xóa
            document.getElementById('<%= hdnDeleteId.ClientID %>').value = imageLinkId;
            var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
            deleteModal.show();
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
                $("#<%= gvImageLinks.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchButton").on("click", function() {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvImageLinks.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchInput").on("input", function() {
                if ($(this).val() === "") {
                    $("#<%= gvImageLinks.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
