# ASP.NET Web Forms Training - Exercise 1
*Date: April 28, 2024*

## Bài tập 1: Tạo trang Default.aspx với chức năng ghép tên

### Yêu cầu:
Tạo một trang web sử dụng MasterPage để thực hiện chức năng ghép tên (First Name + Last Name = Full Name)

### Giao diện và kết quả:
![Giao diện trang web](photos/ver_280425.0.1_photo_1.png)
![Kết quả ghép tên](photos/ver_280425.0.1_photo_2.png)

### Các bước thực hiện:

1. **Tạo trang Default.aspx**
   - Sử dụng MasterPage (Site.Master)
   - Thêm các control cần thiết:
     - TextBox với ID=txtFirstName
     - TextBox với ID=txtLastName
     - Label với ID=blFullName
     - Button với ID=btnGenFullname

2. **Thiết kế giao diện**
   - Sử dụng Bootstrap để tạo layout đẹp
   - Đặt các control trong container và row
   - Sử dụng form-group để nhóm các control
   - Thêm CSS class form-control cho TextBox
   - Thêm CSS class btn btn-primary cho Button

3. **Xử lý sự kiện**
   - Double click vào Button để tạo sự kiện Click
   - Viết code xử lý trong Default.aspx.cs
   - Sử dụng string interpolation để ghép tên: `$"{txtFirstName.Text} {txtLastName.Text}"`

4. **Khai báo control trong Designer**
   - Mở file Default.aspx.designer.cs
   - Thêm khai báo cho các control:
     ```csharp
     protected global::System.Web.UI.WebControls.TextBox txtFirstName;
     protected global::System.Web.UI.WebControls.TextBox txtLastName;
     protected global::System.Web.UI.WebControls.Label blFullName;
     protected global::System.Web.UI.WebControls.Button btnGenFullname;
     ```

5. **Kiểm tra và chạy thử**
   - Build solution
   - Chạy ứng dụng
   - Nhập First Name và Last Name
   - Click nút Generate Full Name
   - Kiểm tra kết quả hiển thị

## Bài tập 2: Xây dựng hệ thống Authentication cho Admin

### Yêu cầu:
Tạo hệ thống đăng nhập cho admin với các chức năng:
- Form đăng nhập với username/password
- Kiểm tra quyền truy cập trang admin
- Lưu trạng thái đăng nhập trong Session
- Chuyển hướng về trang đăng nhập nếu chưa xác thực

### Các bước thực hiện:

1. **Tạo trang Login2.aspx**
   - Sử dụng MasterPage (Site.Master)
   - Thêm các control cần thiết:
     ```aspx
     <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
     <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
     <asp:CheckBox ID="chkRemember" runat="server" />
     <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" OnClick="btnLogin_Click" />
     <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" />
     ```
   - Thêm validation:
     ```aspx
     <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
         ControlToValidate="txtUsername"
         ErrorMessage="Vui lòng nhập tên đăng nhập"
         CssClass="text-danger"
         Display="Dynamic">
     </asp:RequiredFieldValidator>
     ```

2. **Xử lý đăng nhập trong Login2.aspx.cs**
   ```csharp
   protected void btnLogin_Click(object sender, EventArgs e)
   {
       string username = txtUsername.Text.Trim();
       string password = txtPassword.Text.Trim();

       // Hard-coded authentication
       if (username == "admin" && password == "@123123")
       {
           // Set session
           Session["IsAdmin"] = true;
           Session["Username"] = username;

           // Redirect to admin page
           Response.Redirect("~/Adminpage/AdminHome.aspx");
       }
       else
       {
           lblMessage.Text = "Tên đăng nhập hoặc mật khẩu không đúng!";
       }
   }
   ```

3. **Tạo AdminBasePage để kiểm tra quyền truy cập**
   ```csharp
   public class AdminBasePage : Page
   {
       protected override void OnInit(EventArgs e)
       {
           base.OnInit(e);
           CheckAdminAuthentication();
       }

       private void CheckAdminAuthentication()
       {
           if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
           {
               Response.Redirect("~/Login2.aspx");
           }
       }
   }
   ```

4. **Cập nhật Navbar để thêm link Admin**
   ```aspx
   <asp:HyperLink ID="lnkAdmin" runat="server" 
       NavigateUrl="~/Login2.aspx" 
       CssClass="nav-link">
       <i class="fas fa-user-shield me-1"></i> Admin
   </asp:HyperLink>
   ```

5. **Kế thừa AdminBasePage cho các trang admin**
   ```csharp
   public partial class AdminHome : AdminBasePage
   {
       protected void Page_Load(object sender, EventArgs e)
       {
           if (!IsPostBack)
           {
               // Xử lý logic khi trang được tải
           }
       }
   }
   ```

### Lưu ý quan trọng:
- Luôn kiểm tra Session["IsAdmin"] trước khi cho phép truy cập trang admin
- Sử dụng AdminBasePage để tránh lặp code kiểm tra quyền
- Xử lý logout bằng cách xóa Session
- Bảo mật mật khẩu trong môi trường production
- Sử dụng HTTPS để bảo vệ thông tin đăng nhập

### Cách sửa lỗi thường gặp:
1. Nếu gặp lỗi "The name 'xxx' does not exist in the current context":
   - Kiểm tra file .designer.cs
   - Đảm bảo đã khai báo control
   - Build lại solution

2. Nếu gặp lỗi "Could not load type 'TrietPhamShopWeb.Global'":
   - Kiểm tra namespace trong Global.asax và Global.asax.cs
   - Clean và build lại solution
   - Kiểm tra file Global.asax.cs có được include trong project 

3. Nếu gặp lỗi "The type or namespace name 'AdminBasePage' could not be found":
   - Kiểm tra namespace trong AdminBasePage.cs
   - Đảm bảo file AdminBasePage.cs được include trong project
   - Clean và build lại solution 

### Giải thích chi tiết về AdminBasePage và luồng xử lý Authentication

#### 1. AdminBasePage là gì?
AdminBasePage là một lớp cơ sở (base class) được tạo ra để:
- Kiểm tra quyền truy cập cho tất cả các trang admin
- Tránh việc lặp lại code kiểm tra authentication
- Đảm bảo tính nhất quán trong việc kiểm tra quyền truy cập

#### 2. Cấu trúc của AdminBasePage
```csharp
public class AdminBasePage : Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        CheckAdminAuthentication();
    }

    private void CheckAdminAuthentication()
    {
        if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
        {
            Response.Redirect("~/Login2.aspx");
        }
    }
}
```

#### 3. Luồng xử lý Authentication
1. **Khi người dùng truy cập trang admin:**
   ```
   User -> AdminPage -> AdminBasePage.OnInit() -> CheckAdminAuthentication()
   ```

2. **Kiểm tra Session:**
   ```csharp
   if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
   ```
   - Nếu Session["IsAdmin"] chưa tồn tại hoặc là false
   - Chuyển hướng về trang Login2.aspx
   - Nếu không, cho phép truy cập trang admin

3. **Khi đăng nhập thành công:**
   ```csharp
   Session["IsAdmin"] = true;
   Session["Username"] = username;
   ```
   - Lưu trạng thái đăng nhập vào Session
   - Chuyển hướng đến trang admin

#### 4. Cách sử dụng AdminBasePage
1. **Tạo trang admin mới:**
   ```csharp
   public partial class AdminHome : AdminBasePage
   {
       protected void Page_Load(object sender, EventArgs e)
       {
           // Code xử lý trang admin
       }
   }
   ```

2. **Thứ tự thực thi:**
   ```
   Page_Init -> AdminBasePage.OnInit() -> CheckAdminAuthentication() -> Page_Load
   ```

#### 5. Ví dụ về luồng xử lý
1. **Truy cập trang admin khi chưa đăng nhập:**
   ```
   User -> AdminHome.aspx 
   -> AdminBasePage.OnInit() 
   -> CheckAdminAuthentication() 
   -> Session["IsAdmin"] = null 
   -> Redirect to Login2.aspx
   ```

2. **Đăng nhập thành công:**
   ```
   User -> Login2.aspx 
   -> Nhập thông tin 
   -> btnLogin_Click() 
   -> Set Session["IsAdmin"] = true 
   -> Redirect to AdminHome.aspx
   ```

3. **Truy cập trang admin sau khi đăng nhập:**
   ```
   User -> AdminHome.aspx 
   -> AdminBasePage.OnInit() 
   -> CheckAdminAuthentication() 
   -> Session["IsAdmin"] = true 
   -> Load AdminHome.aspx
   ```

#### 6. Lợi ích của việc sử dụng AdminBasePage
1. **Tính tái sử dụng:**
   - Không cần viết lại code kiểm tra authentication
   - Dễ dàng thêm trang admin mới

2. **Bảo mật:**
   - Kiểm tra quyền truy cập ở mức Page_Init
   - Ngăn chặn truy cập trái phép

3. **Bảo trì:**
   - Dễ dàng thay đổi logic kiểm tra quyền
   - Tập trung code authentication tại một nơi

#### 7. Quản lý Session Timeout
1. **Thời gian mặc định của Session:**
   - Session mặc định có thời gian timeout là 20 phút
   - Sau thời gian này, Session sẽ tự động hết hạn
   - Người dùng sẽ phải đăng nhập lại

2. **Cấu hình Session Timeout:**
   ```xml
   <!-- Trong file Web.config -->
   <configuration>
     <system.web>
       <!-- Cấu hình timeout cho toàn bộ ứng dụng -->
       <sessionState timeout="30" />
     </system.web>
   </configuration>
   ```
   - Thời gian được tính bằng phút
   - Có thể điều chỉnh giá trị timeout tùy nhu cầu
   - Nên đặt giá trị phù hợp với yêu cầu bảo mật

3. **Cấu hình Session cho từng trang:**
   ```csharp
   // Trong code-behind của trang
   protected void Page_Load(object sender, EventArgs e)
   {
       // Đặt timeout cho Session hiện tại
       Session.Timeout = 60; // 60 phút
   }
   ```

4. **Kiểm tra Session hết hạn:**
   ```csharp
   protected void CheckSessionExpired()
   {
       if (Session["IsAdmin"] == null)
       {
           // Session đã hết hạn
           Response.Redirect("~/Login2.aspx");
       }
   }
   ```

5. **Xử lý Session hết hạn:**
   - Tự động chuyển hướng về trang đăng nhập
   - Hiển thị thông báo cho người dùng
   - Lưu lại URL trang đang xem để quay lại sau khi đăng nhập

6. **Best Practices:**
   - Nên đặt timeout ngắn cho các ứng dụng có tính bảo mật cao
   - Thông báo cho người dùng khi Session sắp hết hạn
   - Có cơ chế "Remember Me" để kéo dài thời gian đăng nhập
   - Lưu log khi Session hết hạn để theo dõi

7. **Ví dụ về cấu hình Session trong Web.config:**
   ```xml
   <configuration>
     <system.web>
       <sessionState 
         mode="InProc"
         timeout="30"
         cookieName=".ASPXSESSION"
         cookieless="UseCookies"
         />
     </system.web>
   </configuration>
   ```
   - mode="InProc": Lưu Session trong bộ nhớ của web server
   - timeout="30": Session timeout sau 30 phút
   - cookieName: Tên cookie lưu Session ID
   - cookieless: Sử dụng cookie để lưu Session ID

8. **Các mode lưu trữ Session:**
   - InProc: Lưu trong bộ nhớ web server (mặc định)
   - StateServer: Lưu trong Windows Service
   - SQLServer: Lưu trong database
   - Custom: Lưu tùy chỉnh