<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login2.aspx.cs" Inherits="TrietPhamShopWeb.Login2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-xl-10 col-lg-12 col-md-9">
                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0">
                        <div class="row">
                            <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                            <div class="col-lg-6">
                                <div class="p-5">
                                    <div class="text-center">
                                        <h1 class="h4 text-gray-900 mb-4">Đăng nhập Admin</h1>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control form-control-user" 
                                            placeholder="Tên đăng nhập"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                            ControlToValidate="txtUsername"
                                            ErrorMessage="Vui lòng nhập tên đăng nhập"
                                            CssClass="text-danger"
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" 
                                            CssClass="form-control form-control-user" 
                                            placeholder="Mật khẩu"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                            ControlToValidate="txtPassword"
                                            ErrorMessage="Vui lòng nhập mật khẩu"
                                            CssClass="text-danger"
                                            Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <div class="custom-control custom-checkbox small">
                                            <asp:CheckBox ID="chkRemember" runat="server" CssClass="custom-control-input" />
                                            <label class="custom-control-label" for="chkRemember">Ghi nhớ đăng nhập</label>
                                        </div>
                                    </div>
                                    <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" 
                                        CssClass="btn btn-primary btn-user btn-block" OnClick="btnLogin_Click" />
                                    <hr>
                                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
