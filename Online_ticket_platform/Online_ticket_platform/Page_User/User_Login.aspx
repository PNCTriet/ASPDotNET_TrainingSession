<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Login.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Login - Online Ticket Platform</title>
    
    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/css/sb-admin-2.min.css" rel="stylesheet" />
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />
    
    <style>
        .bg-login-image {
            background: url('../Public/assets/images/logo.jpg') center;
            background-size: cover;
        }
        .error-message {
            color: #e74a3b;
            font-size: 0.875rem;
            margin-top: 0.25rem;
        }
        .success-message {
            color: #1cc88a;
            font-size: 0.875rem;
            margin-top: 0.25rem;
        }
    </style>
</head>
<body class="bg-gradient-primary">
    <form id="form1" runat="server">
        <div class="container">
            <!-- Outer Row -->
            <div class="row justify-content-center">
                <div class="col-xl-10 col-lg-12 col-md-9">
                    <div class="card o-hidden border-0 shadow-lg my-5">
                        <div class="card-body p-0">
                            <!-- Nested Row within Card Body -->
                            <div class="row">
                                <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>
                                <div class="col-lg-6">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h1 class="h4 text-gray-900 mb-4">Welcome Back!</h1>
                                        </div>
                                        
                                        <!-- Error/Success Messages -->
                                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                                            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                        </asp:Panel>
                                        
                                        <div class="form-group">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Enter Email Address..." TextMode="Email" />
                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                                                ControlToValidate="txtEmail" 
                                                ErrorMessage="Email is required" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                                ControlToValidate="txtEmail" 
                                                ErrorMessage="Please enter a valid email address" 
                                                ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                        </div>
                                        
                                        <div class="form-group">
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Password" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                                ControlToValidate="txtPassword" 
                                                ErrorMessage="Password is required" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                        </div>
                                        
                                        <div class="form-group">
                                            <div class="custom-control custom-checkbox small">
                                                <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="custom-control-input" />
                                                <label class="custom-control-label" for="<%= chkRememberMe.ClientID %>">Remember Me</label>
                                            </div>
                                        </div>
                                        
                                        <asp:Button ID="btnLogin" runat="server" Text="Login" 
                                            CssClass="btn btn-primary btn-user btn-block" 
                                            OnClick="btnLogin_Click" />
                                        
                                        <hr />
                                        
                                        <div class="text-center">
                                            <a class="small" href="User_ForgotPassword.aspx">Forgot Password?</a>
                                        </div>
                                        <div class="text-center">
                                            <a class="small" href="User_Register.aspx">Create an Account!</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap core JavaScript-->
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../Scripts/bootstrap.bundle.min.js"></script>
    <!-- Core plugin JavaScript-->
    <script src="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/vendor/jquery-easing/jquery.easing.min.js"></script>
    <!-- Custom scripts for all pages-->
    <script src="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/js/sb-admin-2.min.js"></script>
</body>
</html>
