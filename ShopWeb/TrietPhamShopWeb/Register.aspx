<%@ Page Title="Đăng ký tài khoản" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="TrietPhamShopWeb.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/zxcvbn/4.4.2/zxcvbn.js"></script>

    <div class="container d-flex justify-content-center align-items-center" style="min-height: 100vh;">
        <div class="card shadow-lg p-4" style="width: 100%; max-width: 500px; align-items: center;">
            <div class="text-center mb-4">
                <h2 class="h4 text-gray-900">Đăng ký tài khoản</h2>
            </div>

            <!-- Message -->
            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger" Visible="false"></asp:Label>

            <!-- First Name -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Họ"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"
                    ControlToValidate="txtFirstName"
                    ErrorMessage="Vui lòng nhập họ"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <!-- Last Name -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Tên"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server"
                    ControlToValidate="txtLastName"
                    ErrorMessage="Vui lòng nhập tên"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <!-- Email -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Vui lòng nhập email"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                    ErrorMessage="Email không hợp lệ"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RegularExpressionValidator>
            </div>

            <!-- Password -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Mật khẩu" TextMode="Password"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
                    ErrorMessage="Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ cái viết hoa, chữ cái viết thường, số và ký tự đặc biệt"
                    CssClass="text-danger" Display="Dynamic">
                </asp:RegularExpressionValidator>
                <div class="progress mt-2" style="height: 5px;">
                    <div id="password-strength-bar" class="progress-bar" role="progressbar" style="width: 0%;"></div>
                </div>
                <small id="password-strength-text" class="form-text text-muted"></small>

            </div>



            <!-- Confirm Password -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" placeholder="Xác nhận mật khẩu" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ErrorMessage="Vui lòng xác nhận mật khẩu"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtPassword"
                    ErrorMessage="Mật khẩu xác nhận không khớp"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:CompareValidator>
            </div>

            <!-- Phone -->
            <div class="form-group mb-3">
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Số điện thoại"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server"
                    ControlToValidate="txtPhone"
                    ErrorMessage="Vui lòng nhập số điện thoại"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revPhone" runat="server"
                    ControlToValidate="txtPhone"
                    ValidationExpression="^[0-9]{10,11}$"
                    ErrorMessage="Số điện thoại không hợp lệ"
                    CssClass="text-danger"
                    Display="Dynamic">
                </asp:RegularExpressionValidator>
            </div>

            <!-- Submit -->
            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn btn-primary" OnClick="btnRegister_Click" />

            <!-- Links -->
            <div class="mt-4 text-center">
                <a class="small d-block mb-1" href="ForgotPassword.aspx">Quên mật khẩu?</a>
                <a class="small" href="login2.aspx">Đã có tài khoản? Đăng nhập!</a>
            </div>
        </div>
    </div>

    <!-- Links Javascript -->
    <script src="Scripts/Register.js" type="text/javascript"></script>
</asp:Content>
