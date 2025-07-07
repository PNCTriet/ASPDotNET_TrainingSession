<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Register.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Registration - Online Ticket Platform</title>
    
    <!-- Bootstrap CSS -->
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <!-- Custom styles -->
    <link href="../Public/Template/startbootstrap-sb-admin-2-master/startbootstrap-sb-admin-2-master/css/sb-admin-2.min.css" rel="stylesheet" />
    <!-- Google Fonts -->
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />
    
    <style>
        .bg-register-image {
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
                                <div class="col-lg-6 d-none d-lg-block bg-register-image"></div>
                                <div class="col-lg-6">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h1 class="h4 text-gray-900 mb-4">Create an Account!</h1>
                                        </div>
                                        <!-- Error/Success Messages -->
                                        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" role="alert">
                                            <asp:Literal ID="litMessage" runat="server"></asp:Literal>
                                        </asp:Panel>
                                        
                                        <div class="form-group">
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Full Name" />
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                                                ControlToValidate="txtName" 
                                                ErrorMessage="Name is required" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                        </div>
                                        
                                        <div class="form-group">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Email Address" TextMode="Email" />
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
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Phone Number" />
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
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control form-control-user" 
                                                placeholder="Confirm Password" TextMode="Password" />
                                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                                                ControlToValidate="txtConfirmPassword" 
                                                ErrorMessage="Confirm password is required" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                            <asp:CompareValidator ID="cvPassword" runat="server" 
                                                ControlToValidate="txtConfirmPassword" 
                                                ControlToCompare="txtPassword" 
                                                ErrorMessage="Passwords do not match" 
                                                CssClass="error-message" 
                                                Display="Dynamic" />
                                        </div>
                                        
                                        <asp:Button ID="btnRegister" runat="server" Text="Register Account" 
                                            CssClass="btn btn-primary btn-user btn-block" 
                                            OnClick="btnRegister_Click" />
                                        
                                        <hr />
                                        
                                        <div class="text-center">
                                            <a class="small" href="User_ForgotPassword.aspx">Forgot Password?</a>
                                        </div>
                                        <div class="text-center">
                                            <a class="small" href="User_Login.aspx">Already have an account? Login!</a>
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