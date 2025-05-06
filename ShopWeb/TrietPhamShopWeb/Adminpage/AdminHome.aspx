<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Adminpage/Adminsite.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.AdminHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f8f9fa;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .page-title {
            text-align: center;
            margin-bottom: 30px;
            color: #333;
            width: 100%;
        }
        .form-table {
            width: 100%;
            border-collapse: collapse;
            margin: 0 auto;
        }
        .form-table td {
            padding: 15px 30px;
            vertical-align: middle;
        }
        .form-table td:first-child {
            width: 180px;
            text-align: left;
            font-weight: 500;
            padding-left: 50px;
        }
        .form-table td:last-child {
            text-align: center;
            padding-right: 50px;
        }
        .form-control {
            width: 300px !important;
            padding: 5px;
            margin: 0 auto;
        }
        .text-danger {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 5px;
            display: block;
            text-align: center;
        }
        .btn-submit {
            width: 300px !important;
            margin: 20px auto 0;
            display: block;
        }
        .input-container {
            width: 300px;
            margin: 0 auto;
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        .button-container {
            text-align: center;
            margin-top: 30px;
            display: flex;
            justify-content: center;
            gap: 20px;
        }
        .button-container .btn {
            min-width: 150px;
            padding: 10px 30px;
            font-size: 16px;
            font-weight: 500;
        }
        .btn-primary {
            background-color: #0d6efd;
            border-color: #0d6efd;
        }
        .btn-secondary {
            background-color: #6c757d;
            border-color: #6c757d;
        }
        .btn:hover {
            opacity: 0.9;
        }
    </style>

    <div class="container">
        <div class="form-container">
            <h2 class="page-title">Add New/ Edit Product</h2>
            <table class="form-table">
                <tr>
                    <td>
                        <asp:Label ID="lblProductEdit" runat="server" Text="Product Edit:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:TextBox ID="txtProductEdit" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductEdit" runat="server" 
                                ControlToValidate="txtProductEdit"
                                ErrorMessage="Product Edit is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProductName" runat="server" Text="Product Name:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                                ControlToValidate="txtProductName"
                                ErrorMessage="Product Name is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSupplier" runat="server" Text="Supplier:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                                <asp:ListItem Text="1" Value="1" />
                                <asp:ListItem Text="2" Value="2" />
                                <asp:ListItem Text="3" Value="3" />
                                <asp:ListItem Text="4" Value="4" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvSupplier" runat="server" 
                                ControlToValidate="ddlSupplier"
                                ErrorMessage="Supplier is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" Text="Category:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                                ControlToValidate="txtCategory"
                                ErrorMessage="Category is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblQuantityPerUnit" runat="server" Text="Quantity per Unit:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:TextBox ID="txtQuantityPerUnit" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvQuantityPerUnit" runat="server" 
                                ControlToValidate="txtQuantityPerUnit"
                                ErrorMessage="Quantity per Unit is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUnitPrice" runat="server" Text="Unit Price:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                            <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" min="0"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" 
                                ControlToValidate="txtUnitPrice"
                                ErrorMessage="Unit Price is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvUnitPrice" runat="server"
                                ControlToValidate="txtUnitPrice"
                                Type="Double"
                                MinimumValue="0"
                                MaximumValue="999999.99"
                                ErrorMessage="Unit Price must be greater than or equal to 0"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RangeValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblUnitStock" runat="server" Text="Unit Stock:"></asp:Label>
                    </td>
                    <td>
                        <div class="input-container">
                             <asp:TextBox ID="txtUnitStock" runat="server" CssClass="form-control" TextMode="Number" Step="1" min="0"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUnitStock" runat="server" 
                                ControlToValidate="txtUnitStock"
                                ErrorMessage="Unit Stock is required"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="rvUnitStock" runat="server"
                                ControlToValidate="txtUnitStock"
                                Type="Integer"
                                MinimumValue="0"
                                MaximumValue="999999"
                                ErrorMessage="Unit Stock must be greater than or equal to 0"
                                CssClass="text-danger"
                                Display="Dynamic">
                            </asp:RangeValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="button-container">
                <asp:Button ID="btnSubmit" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="return validateAndShowModal(event);" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClientClick="return resetForm(event);" />
            </div>
        </div>
    </div>

    <!-- Modal Confirm -->
    <div class="modal fade" id="confirmModal" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="confirmModalLabel">Xác nhận</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    Bạn có chắc chắn muốn lưu thông tin này?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <asp:Button ID="btnConfirmSubmit" runat="server" Text="Lưu" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateAndShowModal(event) {
            // Ngăn form submit mặc định
            if (event) {
                event.preventDefault();
            }

            // Kiểm tra validation sử dụng Page_ClientValidate
            var isValid = Page_ClientValidate();
            console.log('Kết quả validation:', isValid);

            if (!isValid) {
                return false;
            }

            // Nếu validation pass thì hiện modal
            console.log('Hiển thị modal');
            var myModal = new bootstrap.Modal(document.getElementById('confirmModal'));
            myModal.show();
            return false;
        }

        function resetForm(event) {
            // Ngăn form submit mặc định
            if (event) {
                event.preventDefault();
            }

            // Reset tất cả các input
            var inputs = document.querySelectorAll('input[type="text"], input[type="number"]');
            for (var i = 0; i < inputs.length; i++) {
                inputs[i].value = '';
            }

            // Reset dropdown về giá trị đầu tiên
            var dropdowns = document.querySelectorAll('select');
            for (var i = 0; i < dropdowns.length; i++) {
                dropdowns[i].selectedIndex = 0;
            }

            // Reset validation messages
            var validators = document.querySelectorAll('.text-danger');
            for (var i = 0; i < validators.length; i++) {
                validators[i].style.display = 'none';
            }

            return false;
        }
    </script>
</asp:Content>
