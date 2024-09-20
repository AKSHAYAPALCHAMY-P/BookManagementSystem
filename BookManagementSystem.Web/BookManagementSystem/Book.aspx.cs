using System;
using System.Data.SqlClient;

namespace BookManagementSystem.Web.BookManagementSystem
{
    public partial class Book : System.Web.UI.Page
    {
        public  string ConnectionString = "data source=MS-NB0101; database=BookManagementSystem; User ID=Akshaya; Password=Akshaya;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CreateTableIfNotExists();
                if(Session["EditBookISBN"] != null)
                {
                    int isbn = (int) Session["EditBookISBN"];
                    LoadBookDetails(isbn);
                }
                Session.Remove("EditBookISBN");
            }
        }

        protected void CreateTableIfNotExists()
        {
            string createTableQuery = @"
                IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='Books' AND xtype='U')
                CREATE TABLE Books (Title VARCHAR(100), Author VARCHAR(100), ISBN INT PRIMARY KEY, Genre VARCHAR(100), PublicationYear INT)";

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            using(SqlCommand cmd = new SqlCommand(createTableQuery, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void AddOrUpdateBookButton_Click(object sender, EventArgs e)
        {
            if(int.TryParse(ISBNInput.Text, out int newIsbn) && int.TryParse(YearInput.Text, out int publicationYear))
            {
                using(SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                  
                    int? originalISBN = Session["EditBookISBN"] as int?;
                    bool isUpdating = originalISBN != null;

                   
                    if(isUpdating && originalISBN != newIsbn)
                    {
                        if(BookExists(conn, newIsbn))
                        {
                            Response.Write("<script>alert('A book with this new ISBN already exists. Please use a unique ISBN.');</script>");
                            return;
                        }
                    }

                    
                    UpdateBook(conn, newIsbn, publicationYear, isUpdating ? originalISBN.Value : (int?) null);
                }

                Response.Redirect("BookList.aspx");
            }
            else
            {
                Response.Write("<script>alert('Please enter valid numeric values for ISBN and Publication Year.');</script>");
            }
        }

        private void UpdateBook(SqlConnection conn, int newIsbn, int publicationYear, int? originalISBN)
        {
            
                int newIsbn;
                if(int.TryParse(ISBNHiddenField.Value, out newIsbn))
                {
                    using(SqlConnection Conn = new SqlConnection(ConnectionString))
                    {
                        conn.Open();
                        string query = "UPDATE Books SET Title = @Title, Author = @Author, Genre = @Genre, PublicationYear = @PublicationYear WHERE ISBN = @ISBN";

                        using(SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Title", TitleInput.Text);
                            cmd.Parameters.AddWithValue("@Author", AuthorInput.Text);
                            cmd.Parameters.AddWithValue("@Genre", GenreInput.Text);
                            cmd.Parameters.AddWithValue("@PublicationYear", Convert.ToInt32(YearInput.Text));
                            cmd.Parameters.AddWithValue("@ISBN", newIsbn);

                            try
                            {
                                cmd.ExecuteNonQuery();
                                Response.Redirect("BookList.aspx");
                            }
                            catch(SqlException ex)
                            {
                                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                            }
                        }
                    }
                
                else
                {
                    Response.Write("<script>alert('Invalid ISBN.');</script>");
                }
            }

        }

        private bool BookExists(SqlConnection conn, int isbn)
        {
            string query = "SELECT COUNT(*) FROM Books WHERE ISBN = @ISBN";
            using(SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                return (int) cmd.ExecuteScalar() > 0;
            }
        }


        private void InsertBook(SqlConnection conn, int isbn, int publicationYear)
        {
            string insertQuery = "INSERT INTO Books (Title, Author, ISBN, Genre, PublicationYear) VALUES (@Title, @Author, @ISBN, @Genre, @PublicationYear)";
            using(SqlCommand cmd = new SqlCommand(insertQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Title", TitleInput.Text);
                cmd.Parameters.AddWithValue("@Author", AuthorInput.Text);
                cmd.Parameters.AddWithValue("@ISBN", isbn);
                cmd.Parameters.AddWithValue("@Genre", GenreInput.Text);
                cmd.Parameters.AddWithValue("@PublicationYear", publicationYear);

                cmd.ExecuteNonQuery();
            }
        }

        private void LoadBookDetails(int isbn)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Books WHERE ISBN = @ISBN";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ISBN", isbn);
                    conn.Open();
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            TitleInput.Text = reader["Title"].ToString();
                            AuthorInput.Text = reader["Author"].ToString();
                            GenreInput.Text = reader["Genre"].ToString();
                            YearInput.Text = reader["PublicationYear"].ToString();
                            ISBNHiddenField.Value = reader["ISBN"].ToString();
                        }
                    }
                }
            }
        }
    }
}
