<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookList.aspx.cs" Inherits="BookManagementSystem.Web.BookList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book List</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Book List</h2>
             <asp:GridView ID="BookGrid" runat="server" AutoGenerateColumns="False"
                DataKeyNames="ISBN"
                OnRowEditing="BookGrid_RowEditing"
                OnRowUpdating="BookGrid_RowUpdating"
                OnRowCancelingEdit="BookGrid_RowCancelingEdit"
                OnRowDeleting="BookGrid_RowDeleting"
                CssClass="grid-view">
                
                <Columns>
                    <asp:TemplateField HeaderText="Title">
                        <ItemTemplate>
                            <asp:Label ID="TitleLabel" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TitleTextBox" runat="server" Text='<%# Bind("Title") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Author">
                        <ItemTemplate>
                            <asp:Label ID="AuthorLabel" runat="server" Text='<%# Bind("Author") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="AuthorTextBox" runat="server" Text='<%# Bind("Author") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ISBN">
                        <ItemTemplate>
                            <asp:Label ID="ISBNLabel" runat="server" Text='<%# Bind("ISBN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Genre">
                        <ItemTemplate>
                            <asp:Label ID="GenreLabel" runat="server" Text='<%# Bind("Genre") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="GenreTextBox" runat="server" Text='<%# Bind("Genre") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Publication Year">
                        <ItemTemplate>
                            <asp:Label ID="PublicationYearLabel" runat="server" Text='<%# Bind("PublicationYear") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="PublicationYearTextBox" runat="server" Text='<%# Bind("PublicationYear") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                            <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ISBN") %>' Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this book?');" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
