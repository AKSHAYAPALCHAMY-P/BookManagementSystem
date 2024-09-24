<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Book.aspx.cs" Inherits="BookManagementSystem.Web.Book" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Management System</title>
    <link href="../StyleSheet/Style.css" rel="stylesheet" />
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>Kalakkal Kadhaikal</h1>
        </div>
        <div class="nav">
            <a href="../Default.aspx">Home</a>
            <a href="BookList.aspx">BookList</a>
        </div>

        <form id="form1" runat="server">
            <div>
                <fieldset>
                    <legend>ADD NEW</legend>

                    <asp:Label ID="TitleLabel" runat="server" Text="Title:"></asp:Label>
                    <asp:TextBox ID="TitleInput" runat="server"></asp:TextBox><br />
                    <br />

                    <asp:Label ID="AuthorLabel" runat="server" Text="Author:"></asp:Label>
                    <asp:TextBox ID="AuthorInput" runat="server"></asp:TextBox><br />
                    <br />

                    <asp:Label ID="ISBNLabel" runat="server" Text="ISBN:"></asp:Label>
                    <asp:TextBox ID="ISBNInput" runat="server"></asp:TextBox><br />
                    <br />

                    <asp:Label ID="GenreLabel" runat="server" Text="Genre:"></asp:Label>
                    <asp:TextBox ID="GenreInput" runat="server"></asp:TextBox><br />
                    <br />

                    <asp:Label ID="YearLabel" runat="server" Text="Publication Year:"></asp:Label>
                    <asp:TextBox ID="YearInput" runat="server"></asp:TextBox><br />
                    <br />

                    <asp:Button ID="SubmitButton" runat="server" OnClick="AddOrUpdateBookButton_Click" Text="Add Book" />

                    <asp:HiddenField ID="ISBNHiddenField" runat="server" />
                </fieldset>
            </div>
        </form>
    </div>
</body>
</html>
