<%@ Page Title="Admin Home" Language="C#" MasterPageFile="~/Adminpage/Adminsite.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.AdminHome" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .form-group {
            margin-bottom: 20px;
            display: flex;
            flex-direction: row;
            align-items: flex-start;
            width: 100%;
            justify-content: flex-start;
            gap: 20px;
        }
        .form-control {
            padding: 5px;
            width: 300px !important;
        }
        .form-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f8f9fa;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }
        .form-container-2 {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f8f9fa;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            display: flex;
            flex-direction: column;
            align-items: flex-start;
        }
        .page-title {
            text-align: left;
            margin-bottom: 30px;
            color: #333;
            width: 100%;
            padding-left: 170px;
        }
        .btn-submit {
            width: 300px !important;
            margin-top: 20px;
            margin-left: 170px;
        }
        .text-danger {
            text-align: left;
            width: 100%;
            margin-top: 5px;
        }
        .form-group label {
            min-width: 150px;
            text-align: left;
            font-weight: 500;
            padding-top: 5px;
        }
        /* Đảm bảo dropdown có cùng chiều rộng với input */
        select.form-control {
            width: 300px !important;
        }
        /* Điều chỉnh validator để hiển thị dưới input */
        .form-group .validator-container {
            width: 100%;
            display: flex;
            justify-content: flex-start;
            margin-top: 5px;
        }
        .input-container {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            width: 300px;
        }
        .form-row {
            display: flex;
            justify-content: flex-start;
            width: 100%;
            margin-bottom: 20px;
        }
    </style>

    <div class="container">
        <div class="form-container">
            <h2 class="page-title">Add New/ Edit Product</h2>
            <div class="form-container-2">
            <div class="form-group">
                <asp:Label ID="lblProductEdit" runat="server" Text="Product Edit:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtProductEdit" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvProductEdit" runat="server" 
                            ControlToValidate="txtProductEdit"
                            ErrorMessage="Product Edit is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblProductName" runat="server" Text="Product Name:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvProductName" runat="server" 
                            ControlToValidate="txtProductName"
                            ErrorMessage="Product Name is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblSupplier" runat="server" Text="Supplier:"></asp:Label>
                <div class="input-container">
                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control">
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                    </asp:DropDownList>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvSupplier" runat="server" 
                            ControlToValidate="ddlSupplier"
                            ErrorMessage="Supplier is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblCategory" runat="server" Text="Category:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control"></asp:TextBox>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" 
                            ControlToValidate="txtCategory"
                            ErrorMessage="Category is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblQuantityPerUnit" runat="server" Text="Quantity per Unit:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtQuantityPerUnit" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvQuantityPerUnit" runat="server" 
                            ControlToValidate="txtQuantityPerUnit"
                            ErrorMessage="Quantity per Unit is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblUnitPrice" runat="server" Text="Unit Price:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01" min="0"></asp:TextBox>
                    <div class="validator-container">
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
                </div>
            </div>
            <div class="form-group">
                <asp:Label ID="lblUnitStock" runat="server" Text="Unit Stock:"></asp:Label>
                <div class="input-container">
                    <asp:TextBox ID="txtUnitStock" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <div class="validator-container">
                        <asp:RequiredFieldValidator ID="rfvUnitStock" runat="server" 
                            ControlToValidate="txtUnitStock"
                            ErrorMessage="Unit Stock is required"
                            CssClass="text-danger"
                            Display="Dynamic">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            </div>
            <div class="form-group">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary btn-submit" OnClientClick="return showConfirmModal();" />
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
        function showConfirmModal() {
            var myModal = new bootstrap.Modal(document.getElementById('confirmModal'));
            myModal.show();
            return false;
        }
    </script>
</asp:Content>
