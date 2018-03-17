using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace api
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class api : System.Web.Services.WebService
    {
        SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString);
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public int book(Book newBook)
        {
            int result = 0;
            try
            {
                SqlCommand com = new SqlCommand();
                string authorId = "";
                if(newBook.author!=null)
                {
                    Author a = newBook.author;
                    string queryAuthor = "SELECT Id FROM Author WHERE first_name = '" + a.first_name + "' OR name = '" + a.name + "'";
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = queryAuthor;
                    con.Open();
                    authorId = com.ExecuteScalar().ToString();
                    con.Close();


                    string insertAuthor = "INSERT INTO [dbo].[Author]([first_name],[family_name],[date_of_birth],[date_of_death],[name],[lifespan],[url]) " +
                                            "VALUES('" + a.first_name + "','" + a.family_name + "','" + a.date_of_birth.ToShortDateString() + "','" +
                                            a.date_of_death.ToShortDateString() + "','" + a.name + "','" + a.lifespan + "','" + a.url + "')";
                    com = new SqlCommand();
                    com.Connection = con;
                    com.CommandText = insertAuthor;
                    com.CommandType = CommandType.Text;
                    con.Open();
                    com.ExecuteNonQuery();
                    con.Close();


                }
                if (authorId == "")
                    authorId = "null";
                string sqlQuery = "INSERT INTO [dbo].[Book]([title],[author],[summary],[ISBN],[url]) " +
                                     "VALUES('" + newBook.title + "'," + authorId + ",'" + newBook.summary + "','" + newBook.ISBN + "','" + newBook.url + "')";
                com = new SqlCommand();
                com.Connection = con;
                com.CommandText = sqlQuery;
                con.Open();
                authorId = com.ExecuteScalar().ToString();
                con.Close();


            }
            catch { result = 1; }//error

            return result;
        }

        [WebMethod]
        public List<Book> books(string ID = "")
        {
            List<Book> queriedBooks = new List<Book>();
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                con.Open();

                string sqlQuery = "SELECT b.[title] as 'book_title'" +
                                        ",b.[summary] as 'book_summary'" +
                                        ",b.[ISBN] as 'book_ISBN'" +
                                        ",b.[url] as 'book_url'" +
                                        ",b.[Id] as 'book_Id'" +
                                        ",a.[first_name] as 'author_first_name'" +
                                        ",a.[family_name] as 'author_family_name'" +
                                        ",a.[date_of_birth] as 'author_date_of_birth'" +
                                        ",a.[date_of_death] as 'author_date_of_death'" +
                                        ",a.[name] as 'author_name'" +
                                        ",a.[lifespan] as 'author_lifespan'" +
                                        ",a.[url] as 'author_url' " +
                                    "FROM [Library].[dbo].[Book] b join " +
                                    "Author a on b.author = a.Id";
                if (!String.IsNullOrEmpty(ID))
                    sqlQuery += " where b.Id = " + ID;
                SqlDataAdapter da = new SqlDataAdapter(sqlQuery, con);
                da.Fill(ds);
                dt = ds.Tables[0];

                con.Close();

                if(dt.Rows.Count>0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        Book b = new Book();
                        b.title = dr["book_title"].ToString();
                        b.summary = dr["book_summary"].ToString();
                        b.ISBN = dr["book_ISBN"].ToString();
                        b.url = dr["book_url"].ToString();
                        b.ID = dr["book_Id"].ToString();

                        Author a = new Author();
                        a.family_name = dr["author_family_name"].ToString();
                        a.first_name = dr["author_family_name"].ToString();
                        a.date_of_birth = Convert.ToDateTime(dr["author_date_of_birth"].ToString());
                        a.date_of_death = Convert.ToDateTime(dr["author_date_of_death"].ToString());
                        a.name = dr["author_name"].ToString();
                        a.lifespan = dr["author_lifespan"].ToString();
                        a.url = dr["author_url"].ToString();

                        b.author = a;

                        queriedBooks.Add(b);
                    }

                }
            }
            catch { }
            return queriedBooks;
        }
    }
}