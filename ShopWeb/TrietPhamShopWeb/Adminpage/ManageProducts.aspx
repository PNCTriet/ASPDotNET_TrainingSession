<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageProducts" MasterPageFile="~/Adminpage/Adminsite.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Begin Page Content -->
    <div class="container-fluid">

        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Quản lý sản phẩm</h1>
        <p class="mb-4">Danh sách sản phẩm của cửa hàng. Bạn có thể thêm, sửa, xóa sản phẩm từ đây.</p>

        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Danh sách sản phẩm</h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvProducts" runat="server" 
                        CssClass="table table-bordered" 
                        Width="100%" 
                        CellSpacing="0"
                        AutoGenerateColumns="false"
                        OnRowCommand="gvProducts_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ProductID" HeaderText="ID" />
                            <asp:BoundField DataField="ProductName" HeaderText="Tên sản phẩm" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Danh mục" />
                            <asp:BoundField DataField="Price" HeaderText="Giá" DataFormatString="{0:N0} đ" />
                            <asp:BoundField DataField="Stock" HeaderText="Tồn kho" />
                            <asp:TemplateField HeaderText="Thao tác">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" runat="server" 
                                        CssClass="btn btn-primary btn-sm"
                                        CommandName="EditProduct"
                                        CommandArgument='<%# Eval("ProductID") %>'>
                                        <i class="fas fa-edit"></i> Sửa
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" 
                                        CssClass="btn btn-danger btn-sm"
                                        CommandName="DeleteProduct"
                                        CommandArgument='<%# Eval("ProductID") %>'
                                        OnClientClick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');">
                                        <i class="fas fa-trash"></i> Xóa
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>
    <!-- /.container-fluid -->
</asp:Content>
