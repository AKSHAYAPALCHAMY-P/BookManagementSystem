using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BookManagementSystem.Web
{
    public partial class BookList : System.Web.UI.Page
    {
        string Connectionstring = "data source=MS-NB0101; database=BookManagementSystem; User ID=Akshaya; Password=Akshaya;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using(SqlConnection conn = new SqlConnection(Connectionstring))
            {
                string SelectQuery = "SELECT * FROM Books";
                using(SqlCommand cmd = new SqlCommand(SelectQuery, conn))
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    BookGrid.DataSource = Table;
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
            GridViewRow row = BookGrid.Rows[e.RowIndex];
            string title = ((TextBox) row.FindControl("TitleTextBox")).Text;
            string author = ((TextBox) row.FindControl("AuthorTextBox")).Text;
            string genre = ((TextBox) row.FindControl("GenreTextBox")).Text;
            int publicationYear = Convert.ToInt32(((TextBox) row.FindControl("PublicationYearTextBox")).Text);
            int isbn = (int) BookGrid.DataKeys[e.RowIndex].Value;

            using(SqlConnection conn = new SqlConnection(Connectionstring))
            {
                string UpdateQuery = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, PublicationYear = @PublicationYear WHERE ISBN = @ISBN";
                using(SqlCommand cmd = new SqlCommand(UpdateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Author", author);
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    cmd.Parameters.AddWithValue("@Genre", genre);
                    cmd.Parameters.AddWithValue("@PublicationYear", publicationYear);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            BookGrid.EditIndex = -1;
            BindGrid();
        }

        protected void BookGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BookGrid.EditIndex = -1;
            BindGrid();
        }

        protected void BookGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int isbn = (int) BookGrid.DataKeys[e.RowIndex].Value;
            DeleteBook(isbn);
            BindGrid();
        }

        protected void DeleteBook(int isbn)
        {
            using(SqlConnection conn = new SqlConnection(Connectionstring))
            {
                string DeleteQuery = "DELETE FROM Books WHERE ISBN = @ISBN";
                using(SqlCommand cmd = new SqlCommand(DeleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}

