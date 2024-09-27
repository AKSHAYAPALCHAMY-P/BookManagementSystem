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
                DataBookGrid();
            }
        }

        private void DataBookGrid()
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT Title, Author, ISBN, Genre, PublicationYear FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);
                
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    BookGrid.DataSource = table;
                    BookGrid.DataBind();
                
            }
        }

        protected void BookGrid_RowEditing(object sender, GridViewEditEventArgs e)
         {
            int isbn = (int) BookGrid.DataKeys[e.NewEditIndex].Value;
            Session["EditBookISBN"] = isbn;
            Response.Redirect("Book.aspx");
        }

        


        protected void BookGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int isbn = (int) BookGrid.DataKeys[e.RowIndex].Value;
            DeleteBook(isbn);
            DataBookGrid();
        }

        protected void BookGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            BookGrid.EditIndex = -1;
            DataBookGrid();
        }
        protected void DeleteBook(int isbn)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM Books WHERE ISBN = @ISBN";
                SqlCommand cmd = new SqlCommand(query, conn);
                
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                
            }
        }

        protected void Back_Button(object sender, EventArgs e)
        {
            Response.Redirect("Book.aspx");
        }
    }
}
