using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BookManagementSystem.Web
{
    public partial class BookList : System.Web.UI.Page
    {
        string ConnectionString = "data source=MS-NB0101; database=BookManagementSystem; User ID=Akshaya; Password=Akshaya;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindBookGrid();
            }
        }

        private void BindBookGrid()
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Title, Author, ISBN, Genre, PublicationYear FROM Books";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    BookGrid.DataSource = table;
                    BookGrid.DataBind();
                }
            }
        }

        protected void BookGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int isbn = (int) BookGrid.DataKeys[e.NewEditIndex].Value;
            Session["EditBookISBN"] = isbn;
            Response.Redirect("Book.aspx");
        }

        protected void BookGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int isbn = (int)BookGrid.DataKeys[e.RowIndex].Value;
            string title = ((TextBox)BookGrid.Rows[e.RowIndex].FindControl("TitleTextBox")).Text;
            string author = ((TextBox)BookGrid.Rows[e.RowIndex].FindControl("AuthorTextBox")).Text;
            string genre = ((TextBox)BookGrid.Rows[e.RowIndex].FindControl("GenreTextBox")).Text;
            int publicationYear = int.Parse(((TextBox)BookGrid.Rows[e.RowIndex].FindControl("PublicationYearTextBox")).Text);

            UpdateBook(isbn, title, author, genre, publicationYear);

            BookGrid.EditIndex = -1;
            BindBookGrid();
        }

        private void UpdateBook(int isbn, string title, string author, string genre, int publicationYear)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, PublicationYear = @PublicationYear WHERE ISBN = @ISBN";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@Genre", genre);
                    cmd.Parameters.AddWithValue("@PublicationYear", publicationYear);
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        protected void BookGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int isbn = (int) BookGrid.DataKeys[e.RowIndex].Value;
            DeleteBook(isbn);
            BindBookGrid();
        }

        protected void BookGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BookGrid.EditIndex = -1;
            BindBookGrid();
        }
        protected void DeleteBook(int isbn)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM Books WHERE ISBN = @ISBN";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
