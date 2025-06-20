<%@ Page Title="" Language="C#" MasterPageFile="~/Page_Admin/Organizer/Admin.Master" AutoEventWireup="true" CodeBehind="Image_List.aspx.cs" Inherits="Online_ticket_platform.Page_Admin.Organizer.Image_List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Quản lý Hình ảnh</title>
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
                        <h3 class="card-title">Quản lý Hình ảnh</h3>
                        <div class="card-tools">
                            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
                                <i class="fas fa-plus"></i> Thêm hình ảnh mới
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <!-- Search Box -->
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="searchInput" class="form-control" placeholder="Tìm kiếm hình ảnh...">
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
                                <asp:GridView ID="gvImages" runat="server" CssClass="table table-bordered table-striped" 
                                    AutoGenerateColumns="false" OnRowCommand="gvImages_RowCommand"
                                    DataKeyNames="Id" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvImages_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:TemplateField HeaderText="Hình ảnh">
                                            <ItemTemplate>
                                                <img src='<%# Eval("FilePath") %>' alt='<%# Eval("AltText") %>' 
                                                     class="image-preview img-thumbnail" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FileName" HeaderText="Tên file" />
                                        <asp:BoundField DataField="FileType" HeaderText="Loại file" />
                                        <asp:BoundField DataField="FileSize" HeaderText="Kích thước (KB)" />
                                        <asp:BoundField DataField="AltText" HeaderText="Alt Text" />
                                        <asp:TemplateField HeaderText="Ngày upload">
                                            <ItemTemplate>
                                                <%# ((DateTime)Eval("UploadedAt")).ToString("dd/MM/yyyy HH:mm") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thao tác">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm" 
                                                    CommandName="EditRow" CommandArgument='<%# Eval("Id") %>'
                                                    data-id='<%# Eval("Id") %>'
                                                    data-filepath='<%# Eval("FilePath") %>'
                                                    data-filename='<%# Eval("FileName") %>'
                                                    data-filetype='<%# Eval("FileType") %>'
                                                    data-filesize='<%# Eval("FileSize") %>'
                                                    data-alttext='<%# Eval("AltText") %>'
                                                    data-uploadedby='<%# Eval("UploadedBy") %>'>
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
                    <h5 class="modal-title" id="createModalLabel">Thêm hình ảnh mới</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtFilePath" class="form-label required-field">Đường dẫn file</label>
                                        <asp:TextBox ID="txtFilePath" runat="server" CssClass="form-control" required />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtFileName" class="form-label">Tên file</label>
                                        <asp:TextBox ID="txtFileName" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtFileType" class="form-label">Loại file</label>
                                        <asp:TextBox ID="txtFileType" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtFileSize" class="form-label">Kích thước (KB)</label>
                                        <asp:TextBox ID="txtFileSize" runat="server" CssClass="form-control" TextMode="Number" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtAltText" class="form-label">Alt Text</label>
                                        <asp:TextBox ID="txtAltText" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtUploadedBy" class="form-label">Người upload</label>
                                        <asp:TextBox ID="txtUploadedBy" runat="server" CssClass="form-control" TextMode="Number" />
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
                    <asp:Button ID="btnCreate" runat="server" Text="Thêm mới" CssClass="btn btn-primary" OnClick="btnCreate_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header edit">
                    <h5 class="modal-title" id="editModalLabel">Chỉnh sửa hình ảnh</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnEditId" runat="server" />
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditFilePath" class="form-label required-field">Đường dẫn file</label>
                                        <asp:TextBox ID="txtEditFilePath" runat="server" CssClass="form-control" required />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditFileName" class="form-label">Tên file</label>
                                        <asp:TextBox ID="txtEditFileName" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditFileType" class="form-label">Loại file</label>
                                        <asp:TextBox ID="txtEditFileType" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditFileSize" class="form-label">Kích thước (KB)</label>
                                        <asp:TextBox ID="txtEditFileSize" runat="server" CssClass="form-control" TextMode="Number" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditAltText" class="form-label">Alt Text</label>
                                        <asp:TextBox ID="txtEditAltText" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="mb-3">
                                        <label for="txtEditUploadedBy" class="form-label">Người upload</label>
                                        <asp:TextBox ID="txtEditUploadedBy" runat="server" CssClass="form-control" TextMode="Number" />
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
                    <p>Bạn có chắc chắn muốn xóa hình ảnh này?</p>
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
            var editFilePath = document.getElementById('<%= txtEditFilePath.ClientID %>');

            if (!editFilePath.value) {
                editFilePath.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form create
        function validateCreateForm() {
            var filePath = document.getElementById('<%= txtFilePath.ClientID %>');

            if (!filePath.value) {
                filePath.focus();
                return false;
            }
            return true;
        }

        // Xử lý validation cho form delete
        function validateDeleteForm() {
            var imageId = document.getElementById('<%= hdnDeleteId.ClientID %>');
            if (!imageId.value) {
                showError('Không tìm thấy ID hình ảnh!');
                return false;
            }
            return true;
        }

        function showDeleteModal(imageId) {
            // Hiển thị modal xóa
            document.getElementById('<%= hdnDeleteId.ClientID %>').value = imageId;
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
                $("#<%= gvImages.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchButton").on("click", function() {
                var value = $("#searchInput").val().toLowerCase();
                $("#<%= gvImages.ClientID %> tr").filter(function() {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });

            $("#searchInput").on("input", function() {
                if ($(this).val() === "") {
                    $("#<%= gvImages.ClientID %> tr").show();
                }
            });
        });
    </script>
</asp:Content>
