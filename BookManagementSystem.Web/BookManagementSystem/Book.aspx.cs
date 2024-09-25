using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BookManagementSystem.Web
{
    public partial class Book : System.Web.UI.Page
    {
        public string ConnectionString = "data source=MS-NB0101; database=BookManagementSystem; User ID=Akshaya; Password=Akshaya;";

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
            }
        }

        protected void CreateTableIfNotExists()
        {
            string createTableQuery = @"
                IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='Books' AND xtype='U')
                CREATE TABLE Books (
                    Title VARCHAR(100), 
                    Author VARCHAR(100), 
                    ISBN INT PRIMARY KEY, 
                    Genre VARCHAR(100), 
                    PublicationYear INT
                )";

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

                  
                    if(isUpdating && originalISBN != newIsbn && BookExists(conn, newIsbn))
                    {
                        Response.Write("<script>alert('A book with this new ISBN already exists. Please use a unique ISBN.');</script>");
                        return;
                    }

                    if(isUpdating)
                    {
                        UpdateBook(conn, newIsbn, publicationYear, originalISBN.Value);
                    }
                    else
                    {
                        InsertBook(conn, newIsbn, publicationYear);
                    }
                }

                Response.Redirect("BookList.aspx");
            }
            else
            {
                Response.Write("<script>alert('Please enter valid numeric values for ISBN and Publication Year.');</script>");
            }
        }

        private void UpdateBook(SqlConnection conn, int newIsbn, int publicationYear, int originalISBN)
        {
            string updateQuery =  @"
                                    UPDATE Books
                                    SET Title = @Title, Author = @Author, Genre = @Genre, PublicationYear = @PublicationYear, ISBN = @NewISBN
                                    WHERE ISBN = @OriginalISBN";

            using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Title", TitleInput.Text);
                cmd.Parameters.AddWithValue("@Author", AuthorInput.Text);
                cmd.Parameters.AddWithValue("@Genre", GenreInput.Text);
                cmd.Parameters.AddWithValue("@PublicationYear", publicationYear);
                cmd.Parameters.AddWithValue("@NewISBN", newIsbn);
                cmd.Parameters.AddWithValue("@OriginalISBN", originalISBN);

                cmd.ExecuteNonQuery();
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
            string insertQuery = @"
                INSERT INTO Books (Title, Author, ISBN, Genre, PublicationYear) 
                VALUES (@Title, @Author, @ISBN, @Genre, @PublicationYear)";

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

                            if (reader["ISBN"] != DBNull.Value) //Non Existence value
                            {
                                ISBNHiddenField.Value = reader["ISBN"].ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}
