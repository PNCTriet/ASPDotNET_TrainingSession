<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="TrietPhamShopWeb.Adminpage.ManageUser" MasterPageFile="~/Adminpage/Adminsite.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Quản lý người dùng</h2>
        
        <!-- GridView để hiển thị danh sách người dùng -->
        <asp:GridView ID="gvUsers" runat="server" CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False" OnRowCommand="gvUsers_RowCommand">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" />
                <asp:BoundField DataField="FirstName" HeaderText="Họ" />
                <asp:BoundField DataField="LastName" HeaderText="Tên" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Phone" HeaderText="Số điện thoại" />
                <asp:TemplateField HeaderText="Thao tác">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary btn-sm"
                            CommandName="EditUser" CommandArgument='<%# Eval("UserID") %>'>
                            <i class="fas fa-edit"></i> Sửa
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger btn-sm"
                            CommandName="DeleteUser" CommandArgument='<%# Eval("UserID") %>'
                            OnClientClick='<%# "return confirm(\"Bạn có chắc chắn muốn xóa người dùng này?\");" %>'>
                            <i class="fas fa-trash"></i> Xóa
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
