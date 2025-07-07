<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Event.aspx.cs" Inherits="Online_ticket_platform.Page_User.User_Event" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chi tiết sự kiện</title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <div class="card">
                <asp:Image ID="imgEvent" runat="server" CssClass="card-img-top" Style="max-height:300px;object-fit:cover;" />
                <div class="card-body">
                    <h2 class="card-title"><asp:Label ID="lblEventName" runat="server" /></h2>
                    <p class="card-text"><asp:Label ID="lblEventDescription" runat="server" /></p>
                    <p><strong>Thời gian:</strong> <asp:Label ID="lblEventTime" runat="server" /></p>
                    <p><strong>Địa điểm:</strong> <asp:Label ID="lblEventLocation" runat="server" /></p>
                    <p><strong>Giá vé:</strong> <asp:Label ID="lblEventPrice" runat="server" /></p>
                    <a href="User_Home.aspx" class="btn btn-secondary">Quay lại</a>
                </div>
            </div>
        </div>
    </form>
</body>
<!-- Thêm script dark mode trước khi đóng body -->
<script>
    function applyDarkMode() {
        if (localStorage.getItem('darkMode') === 'true') {
            document.body.classList.add('dark');
        } else {
            document.body.classList.remove('dark');
        }
    }
    applyDarkMode();
</script>
</html>
