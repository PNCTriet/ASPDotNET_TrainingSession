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

# Phân tích luồng kết nối và hiển thị dữ liệu trong ManageProducts.aspx

## 1. Cấu hình kết nối Database

### 1.1. Cấu hình Connection String
- File: `Web.config`
- Vị trí: Thẻ `<connectionStrings>`
- Cấu hình:
```xml
<connectionStrings>
  <add name="Northwind" 
       connectionString="Data Source=.;Initial Catalog=Northwind;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```
- Giải thích:
  - `Data Source=.`: Kết nối tới SQL Server local
  - `Initial Catalog=Northwind`: Sử dụng database Northwind
  - `Integrated Security=True`: Sử dụng Windows Authentication

### 1.2. Class Connection
- File: `Models/Connection.cs`
- Chức năng: Đọc connection string từ Web.config
```csharp
public static string GetConnectionString()
{
    return ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;
}
```

## 2. Luồng xử lý dữ liệu trong ManageProducts.aspx

### 2.1. Cấu trúc trang
- Master Page: `Adminpage/Adminsite.Master`
- Controls:
  - GridView (ID: gvProducts): Hiển thị danh sách sản phẩm
  - Các nút thao tác: Sửa, Xóa

### 2.2. Code-behind (ManageProducts.aspx.cs)

#### 2.2.1. Load dữ liệu
```csharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!IsPostBack)
    {
        LoadProducts();
    }
}
```

#### 2.2.2. Truy vấn dữ liệu
```csharp
private void LoadProducts()
{
    using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
    {
        string query = @"SELECT p.ProductID, p.ProductName, c.CategoryName, 
                        p.UnitPrice as Price, p.UnitsInStock as Stock
                        FROM Products p
                        LEFT JOIN Categories c ON p.CategoryID = c.CategoryID";
        
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            conn.Open();
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }
    }
}
```

### 2.3. Xử lý sự kiện

#### 2.3.1. Xử lý nút Sửa/Xóa
```csharp
protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
{
    if (e.CommandName == "EditProduct")
    {
        Response.Redirect($"EditProduct.aspx?id={e.CommandArgument}");
    }
    else if (e.CommandName == "DeleteProduct")
    {
        DeleteProduct(Convert.ToInt32(e.CommandArgument));
    }
}
```

#### 2.3.2. Xóa sản phẩm
```csharp
private void DeleteProduct(int productId)
{
    using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
    {
        string query = "DELETE FROM Products WHERE ProductID = @ProductID";
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@ProductID", productId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
    LoadProducts(); // Reload lại danh sách
}
```

## 3. Luồng dữ liệu

1. **Khởi tạo trang**:
   - Page_Load được gọi
   - Kiểm tra IsPostBack
   - Gọi LoadProducts() nếu lần đầu load

2. **Load dữ liệu**:
   - Tạo kết nối SQL
   - Thực thi query JOIN Products và Categories
   - Đổ dữ liệu vào DataTable
   - Bind dữ liệu vào GridView

3. **Hiển thị dữ liệu**:
   - GridView hiển thị các cột:
     - ProductID
     - ProductName
     - CategoryName
     - Price (định dạng tiền tệ)
     - Stock
     - Các nút thao tác

4. **Xử lý thao tác**:
   - Edit: Chuyển hướng tới EditProduct.aspx
   - Delete: Xóa sản phẩm và reload dữ liệu

## 4. Các điểm cần lưu ý

1. **Bảo mật**:
   - Sử dụng Windows Authentication
   - Parameterized queries để tránh SQL Injection
   - Kiểm tra quyền truy cập trang admin

2. **Hiệu năng**:
   - Sử dụng using để đảm bảo giải phóng tài nguyên
   - Chỉ load dữ liệu khi cần thiết (IsPostBack)
   - Sử dụng JOIN để lấy dữ liệu một lần

3. **UX/UI**:
   - Hiển thị thông báo xác nhận khi xóa
   - Định dạng tiền tệ cho cột giá
   - Responsive design với Bootstrap

## 5. Hướng dẫn setup chức năng xóa sản phẩm (theo mô hình 3 lớp, code gộp trong file .cs)

### Bước 1: Cập nhật tầng Data Access (DAL)
- Trong file code-behind (ManageProducts.aspx.cs), thêm hàm xóa sản phẩm:
  ```csharp
  private void DeleteProduct(int productId)
  {
      using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
      {
          string query = "DELETE FROM Products WHERE ProductID = @ProductID";
          using (SqlCommand cmd = new SqlCommand(query, conn))
          {
              cmd.Parameters.AddWithValue("@ProductID", productId);
              conn.Open();
              cmd.ExecuteNonQuery();
          }
      }
      LoadProducts(); // Reload lại danh sách
  }
  ```

### Bước 2: Cập nhật tầng Business Logic (BLL)
- Nếu muốn tách riêng, có thể tạo hàm DeleteProduct ở lớp BLL và gọi từ code-behind. Tuy nhiên, ví dụ này gộp luôn vào file .cs.

### Bước 3: Cập nhật tầng Presentation (UI)
- Trong GridView, đã có sẵn nút xóa với CommandName="DeleteProduct".
- Trong sự kiện `gvProducts_RowCommand`, xử lý như sau:
  ```csharp
  protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
  {
      if (e.CommandName == "EditProduct")
      {
          Response.Redirect($"EditProduct.aspx?id={e.CommandArgument}");
      }
      else if (e.CommandName == "DeleteProduct")
      {
          DeleteProduct(Convert.ToInt32(e.CommandArgument));
      }
  }
  ```

### Bước 4: Cập nhật giao diện xác nhận xóa
- Đã có sẵn xác nhận xóa trong thuộc tính `OnClientClick` của nút xóa:
  ```aspx
  OnClientClick='<%# "return confirm(\"Bạn có chắc chắn muốn xóa sản phẩm " + Eval("ProductName") + "?\");" %>'
  ```

### Bước 5: Kiểm tra và hoàn thiện
- Build lại project.
- Truy cập trang quản lý sản phẩm, thử xóa một sản phẩm để kiểm tra.

### Lưu ý:
- Đảm bảo tài khoản đăng nhập có quyền xóa dữ liệu trong database.
- Nếu muốn tách riêng các lớp DAL/BLL, hãy tạo các class tương ứng và gọi qua các lớp này thay vì code trực tiếp trong file .cs.

# Hướng dẫn thiết lập chức năng Edit Product với Modal Popup

## 1. Cấu trúc Modal
```html
<!-- Edit Product Modal -->
<div class="modal fade" id="editProductModal" tabindex="-1" aria-labelledby="editProductModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="editProductModalLabel">Sửa thông tin sản phẩm</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <form id="editProductForm">
                    <input type="hidden" id="productId" name="productId" />
                    <div class="mb-3">
                        <label for="productName" class="form-label">Tên sản phẩm</label>
                        <input type="text" class="form-control" id="productName" name="productName" required>
                    </div>
                    <div class="mb-3">
                        <label for="categoryName" class="form-label">Danh mục</label>
                        <input type="text" class="form-control" id="categoryName" name="categoryName" readonly>
                    </div>
                    <div class="mb-3">
                        <label for="price" class="form-label">Giá</label>
                        <input type="number" class="form-control" id="price" name="price" required>
                    </div>
                    <div class="mb-3">
                        <label for="stock" class="form-label">Tồn kho</label>
                        <input type="number" class="form-control" id="stock" name="stock" required>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Đóng</button>
                <button type="button" class="btn btn-primary" id="btnSaveChanges">Lưu thay đổi</button>
            </div>
        </div>
    </div>
</div>
```

## 2. Thiết lập nút Edit trong GridView
```html
<asp:TemplateField HeaderText="Thao tác">
    <ItemTemplate>
        <button type="button" class="btn btn-primary btn-sm btn-edit" 
            data-id='<%# Eval("ProductID") %>'
            data-name='<%# Eval("ProductName") %>'
            data-category='<%# Eval("CategoryName") %>'
            data-price='<%# Eval("Price") %>'
            data-stock='<%# Eval("Stock") %>'>
            <i class="fas fa-edit"></i> Sửa
        </button>
    </ItemTemplate>
</asp:TemplateField>
```

## 3. Thêm thư viện cần thiết
```html
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- Bootstrap 5 -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>
```

## 4. Xử lý JavaScript/jQuery
```javascript
$(document).ready(function () {
    // Xử lý sự kiện click nút Edit
    $(document).on('click', '.btn-edit', function () {
        // Lấy dữ liệu từ data attributes
        var productId = $(this).data('id');
        var productName = $(this).data('name');
        var categoryName = $(this).data('category');
        var price = $(this).data('price');
        var stock = $(this).data('stock');

        // Điền dữ liệu vào form
        $('#productId').val(productId);
        $('#productName').val(productName);
        $('#categoryName').val(categoryName);
        $('#price').val(price);
        $('#stock').val(stock);

        // Hiển thị modal
        var editModal = new bootstrap.Modal(document.getElementById('editProductModal'));
        editModal.show();
    });

    // Xử lý sự kiện click nút Save Changes
    $('#btnSaveChanges').click(function () {
        if (confirm('Bạn có chắc chắn muốn lưu các thay đổi?')) {
            // Tạo object chứa dữ liệu cần gửi
            var productData = {
                productId: $('#productId').val(),
                productName: $('#productName').val(),
                price: $('#price').val(),
                stock: $('#stock').val()
            };

            // Gửi request Ajax
            $.ajax({
                type: "POST",
                url: "ManageProducts.aspx/UpdateProduct",
                data: JSON.stringify(productData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        // Đóng modal và reload trang
                        var editModal = bootstrap.Modal.getInstance(document.getElementById('editProductModal'));
                        editModal.hide();
                        location.reload();
                        alert('Cập nhật sản phẩm thành công!');
                    } else {
                        alert('Có lỗi xảy ra khi cập nhật sản phẩm!');
                    }
                },
                error: function (xhr, status, error) {
                    alert('Có lỗi xảy ra khi cập nhật sản phẩm!');
                }
            });
        }
    });
});
```

## 5. Xử lý Code-behind (C#)
```csharp
[WebMethod]
public static bool UpdateProduct(int productId, string productName, decimal price, int stock)
{
    try
    {
        using (SqlConnection conn = new SqlConnection(Connection.GetConnectionString()))
        {
            string query = @"UPDATE Products 
                           SET ProductName = @ProductName,
                               UnitPrice = @Price,
                               UnitsInStock = @Stock
                           WHERE ProductID = @ProductID";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@Stock", stock);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }
    }
    catch (Exception ex)
    {
        // Log error
        return false;
    }
}
```

## 6. Các bước thực hiện
1. **Thiết lập Modal**:
   - Tạo cấu trúc HTML cho modal với các trường input cần thiết
   - Sử dụng Bootstrap 5 classes để tạo giao diện
   - Thêm các nút điều khiển (Đóng, Lưu thay đổi)

2. **Thiết lập nút Edit**:
   - Thêm button với class `btn-edit`
   - Lưu trữ dữ liệu sản phẩm trong data attributes
   - Sử dụng Font Awesome cho icon

3. **Thêm thư viện**:
   - jQuery cho xử lý DOM và Ajax
   - Bootstrap 5 cho modal và styling
   - Đặt trong HeadContent để đảm bảo load trước khi sử dụng

4. **Xử lý JavaScript**:
   - Bắt sự kiện click nút Edit
   - Lấy dữ liệu từ data attributes
   - Điền dữ liệu vào form
   - Hiển thị modal
   - Xử lý lưu thay đổi qua Ajax

5. **Xử lý Server-side**:
   - Tạo WebMethod để xử lý update
   - Sử dụng parameterized query để tránh SQL injection
   - Xử lý lỗi và trả về kết quả

## 7. Lưu ý quan trọng
- Đảm bảo thư viện jQuery và Bootstrap được load đúng thứ tự
- Sử dụng parameterized queries để tránh SQL injection
- Xử lý lỗi và hiển thị thông báo phù hợp
- Kiểm tra dữ liệu trước khi gửi lên server
- Sử dụng Bootstrap 5 modal API để quản lý modal
- Reload trang sau khi cập nhật thành công để hiển thị dữ liệu mới

# Hướng dẫn tích hợp tính năng đánh giá độ mạnh mật khẩu

## 1. Giới thiệu
Tính năng đánh giá độ mạnh mật khẩu giúp người dùng tạo mật khẩu an toàn hơn bằng cách:
- Hiển thị thanh progress bar thể hiện độ mạnh
- Cung cấp phản hồi trực quan về độ mạnh mật khẩu
- Đưa ra gợi ý cải thiện mật khẩu

## 2. Các bước thực hiện

### 2.1. Thêm thư viện zxcvbn
```html
<!-- Thêm vào phần head của trang Register.aspx -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/zxcvbn/4.4.2/zxcvbn.js"></script>
```
- zxcvbn là thư viện đánh giá mật khẩu được phát triển bởi Dropbox
- Cung cấp đánh giá chính xác về độ mạnh mật khẩu
- Phát hiện các mật khẩu yếu và dễ đoán

### 2.2. Thêm UI elements
```html
<!-- Thêm vào form đăng ký -->
<div class="form-group mb-3">
    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
        placeholder="Mật khẩu" TextMode="Password"></asp:TextBox>
    <div class="progress mt-2" style="height: 5px;">
        <div id="password-strength-bar" class="progress-bar" role="progressbar" 
            style="width: 0%;"></div>
    </div>
    <small id="password-strength-text" class="form-text text-muted"></small>
</div>
```
- Progress bar để hiển thị độ mạnh trực quan
- Text hiển thị mô tả độ mạnh và gợi ý

### 2.3. Tạo file JavaScript
```javascript
// Register.js
document.addEventListener('DOMContentLoaded', function() {
    const passwordInput = document.querySelector('input[type="password"]');
    const strengthBar = document.getElementById('password-strength-bar');
    const strengthText = document.getElementById('password-strength-text');

    passwordInput.addEventListener('input', function() {
        const password = this.value;
        const result = zxcvbn(password);
        
        // Update progress bar
        const strength = result.score;
        const width = (strength + 1) * 25; // Convert 0-4 to 25-100%
        strengthBar.style.width = width + '%';

        // Update colors and text based on strength
        switch(strength) {
            case 0:
                strengthBar.className = 'progress-bar bg-danger';
                strengthText.textContent = 'Rất yếu';
                break;
            case 1:
                strengthBar.className = 'progress-bar bg-warning';
                strengthText.textContent = 'Yếu';
                break;
            case 2:
                strengthBar.className = 'progress-bar bg-info';
                strengthText.textContent = 'Trung bình';
                break;
            case 3:
                strengthBar.className = 'progress-bar bg-primary';
                strengthText.textContent = 'Mạnh';
                break;
            case 4:
                strengthBar.className = 'progress-bar bg-success';
                strengthText.textContent = 'Rất mạnh';
                break;
        }

        // Show feedback if available
        if (result.feedback.warning) {
            strengthText.textContent += ' - ' + result.feedback.warning;
        }
    });
});
```

### 2.4. Thêm file JavaScript vào trang
```html
<!-- Thêm vào cuối file Register.aspx -->
<script src="Scripts/Register.js" type="text/javascript"></script>
```

## 3. Cách hoạt động

### 3.1. Thư viện zxcvbn
- Đánh giá mật khẩu dựa trên nhiều yếu tố:
  - Độ dài mật khẩu
  - Sự đa dạng của ký tự
  - Không chứa thông tin cá nhân
  - Không phải mật khẩu phổ biến
  - Không có quy luật dễ đoán

### 3.2. Luồng xử lý
1. Người dùng nhập mật khẩu
2. Event listener bắt sự kiện input
3. Gọi zxcvbn để đánh giá mật khẩu
4. Cập nhật UI dựa trên kết quả:
   - Thay đổi độ rộng progress bar
   - Thay đổi màu sắc
   - Hiển thị text mô tả
   - Hiển thị gợi ý cải thiện

### 3.3. Kết quả đánh giá
- Score 0: Rất yếu (đỏ)
- Score 1: Yếu (vàng)
- Score 2: Trung bình (xanh dương nhạt)
- Score 3: Mạnh (xanh dương đậm)
- Score 4: Rất mạnh (xanh lá)

## 4. Lợi ích
1. **Bảo mật**:
   - Giúp người dùng tạo mật khẩu mạnh
   - Phát hiện các mật khẩu yếu
   - Cung cấp gợi ý cải thiện

2. **UX/UI**:
   - Phản hồi trực quan
   - Tương tác real-time
   - Giao diện thân thiện

3. **Dễ tích hợp**:
   - Chỉ cần thêm 1 file JavaScript
   - Không cần backend
   - Hoạt động client-side

## 5. Lưu ý quan trọng
1. **Bảo mật**:
   - Đánh giá chỉ là tham khảo
   - Vẫn cần validation server-side
   - Không nên chỉ dựa vào client-side

2. **Hiệu năng**:
   - zxcvbn là thư viện nhẹ
   - Không ảnh hưởng đến performance
   - Có thể lazy load nếu cần

3. **Browser Support**:
   - Hoạt động trên mọi trình duyệt hiện đại
   - Cần jQuery và Bootstrap
   - Fallback cho trình duyệt cũ

## 6. Mở rộng
1. **Thêm yêu cầu mật khẩu**:
   - Độ dài tối thiểu
   - Yêu cầu ký tự đặc biệt
   - Yêu cầu chữ hoa/thường

2. **Tùy chỉnh UI**:
   - Thay đổi màu sắc
   - Thêm animation
   - Tùy chỉnh text

3. **Tích hợp với form validation**:
   - Disable nút submit nếu mật khẩu yếu
   - Hiển thị lỗi validation
   - Tự động focus vào ô mật khẩu
