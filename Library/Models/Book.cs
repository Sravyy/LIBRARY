using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Book
  {
    private string _titlename;
    private int _id;
    private int _copies;

    public Book(string titlename, int copies, int id = 0)
    {
      _titlename = titlename;
      _id = id;
      _copies = copies;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = this.GetId() == newBook.GetId();
        bool titlenameEquality = this.GetTitlename() == newBook.GetTitlename();
        bool copiesEquality = this.Getcopies() == newBook.Getcopies();
        return (idEquality && titlenameEquality && copiesEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetTitlename().GetHashCode();
    }

    public string GetTitlename()
    {
      return _titlename;
    }
    public int GetId()
    {
      return _id;
    }

    public int Getcopies()
    {
      return _copies;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (titlename, copies) VALUES (@titlename, @copies);";

      MySqlParameter titlename = new MySqlParameter();
      titlename.ParameterName = "@titlename";
      titlename.Value = this._titlename;
      cmd.Parameters.Add(titlename);

      MySqlParameter copies = new MySqlParameter();
      copies.ParameterName = "@copies";
      copies.Value = this._copies;
      cmd.Parameters.Add(copies);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books ORDER BY copies;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int bookId = rdr.GetInt32(0);
        string bookTitlename = rdr.GetString(1);
        int bookcopies = rdr.GetInt32(2);
        Book newBook = new Book(bookTitlename, bookcopies, bookId);
        allBooks.Add(newBook);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBooks;
    }
    public static Book Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int bookId = 0;
      string bookName = "";
      int bookcopies = 0;

      while(rdr.Read())
      {
        bookId = rdr.GetInt32(0);
        bookName = rdr.GetString(1);
        bookcopies = rdr.GetInt32(2);
      }
      Book newBook = new Book(bookName, bookcopies, bookId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newBook;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteBooks()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("DELETE FROM books WHERE id = @BookId; DELETE FROM Authors_books WHERE book_id = @BookId;", conn);
        MySqlParameter bookIdParameter = new MySqlParameter();
        bookIdParameter.ParameterName = "@BookId";
        bookIdParameter.Value = this.GetId();

        cmd.Parameters.Add(bookIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }

      public void BookCheckout()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("UPDATE books SET copies = copies - 1 WHERE id = @bookId;", conn);
        MySqlParameter booksIdParameter = new MySqlParameter();
        booksIdParameter.ParameterName = "@bookId";
        booksIdParameter.Value = this.GetId();

        cmd.Parameters.Add(booksIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }

      public void DeleteThisBook()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        MySqlCommand cmd = new MySqlCommand("DELETE FROM books WHERE id = @bookId; DELETE FROM authors_books WHERE book_id = @bookId;", conn);
        MySqlParameter booksIdParameter = new MySqlParameter();
        booksIdParameter.ParameterName = "@bookId";
        booksIdParameter.Value = this.GetId();

        cmd.Parameters.Add(booksIdParameter);
        cmd.ExecuteNonQuery();

        if (conn != null)
        {
          conn.Close();
        }
      }

      public static List<Book> GetAlphaList()
      {
        List<Book> allBooks = new List<Book> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM books ORDER BY copies;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int bookId = rdr.GetInt32(0);
          string bookTitlename = rdr.GetString(1);
          int bookcopies = rdr.GetInt32(2);
          Book newBook = new Book(bookTitlename, bookcopies, bookId);
          allBooks.Add(newBook);
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allBooks;
      }

      public void AddAuthor(Author newAuthor)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO Authors_books (Author_id, book_id) VALUES (@AuthorId, @BookId);";

        MySqlParameter Author_id = new MySqlParameter();
        Author_id.ParameterName = "@AuthorId";
        Author_id.Value = newAuthor.GetId();
        cmd.Parameters.Add(Author_id);

        MySqlParameter book_id = new MySqlParameter();
        book_id.ParameterName = "@BookId";
        book_id.Value = _id;
        cmd.Parameters.Add(book_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn !=null)
        {
          conn.Dispose();
        }
      }

      public List<Author> GetAuthors()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT Author_id FROM Authors_Books WHERE book_id = @bookId;";

        MySqlParameter bookIdParameter = new MySqlParameter();
        bookIdParameter.ParameterName = "@bookId";
        bookIdParameter.Value = _id;
        cmd.Parameters.Add(bookIdParameter);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<int> AuthorIds =new List<int> {};
        while(rdr.Read())
        {
          int AuthorId = rdr.GetInt32(0);
          AuthorIds.Add(AuthorId);
        }
        rdr.Dispose();

        List<Author> Authors = new List<Author> {};
        foreach (int AuthorId in AuthorIds)
        {
          var AuthorQuery = conn.CreateCommand() as MySqlCommand;
          AuthorQuery.CommandText = @"SELECT * FROM Authors WHERE id = @AuthorId;";

          MySqlParameter AuthorIdParameter = new MySqlParameter();
          AuthorIdParameter.ParameterName = "@AuthorId";
          AuthorIdParameter.Value = AuthorId;
          AuthorQuery.Parameters.Add(AuthorIdParameter);

          var AuthorQueryRdr = AuthorQuery.ExecuteReader() as MySqlDataReader;
          while(AuthorQueryRdr.Read())
          {
            int thisAuthorId = AuthorQueryRdr.GetInt32(0);
            string copies = AuthorQueryRdr.GetString(1);
            Author foundAuthor = new Author(copies, thisAuthorId);
            Authors.Add(foundAuthor);
          }
          AuthorQueryRdr.Dispose();
        }
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return Authors;
      }

      public void AddPatron(Patron newPatron)
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId);";

              MySqlParameter patron_id = new MySqlParameter();
              patron_id.ParameterName = "@PatronId";
              patron_id.Value = _id;
              cmd.Parameters.Add(patron_id);

              MySqlParameter book_id = new MySqlParameter();
              book_id.ParameterName = "@BookId";
              book_id.Value = newPatron.GetId();
              cmd.Parameters.Add(book_id);

              cmd.ExecuteNonQuery();
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
          }

          public List<Patron> GetPatrons()
          {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT patrons.* FROM books JOIN patrons_books ON (books.id = patrons_books.book_id) JOIN patrons ON (patrons_books.patron_id = patrons.id) WHERE books.id = @BookId;";


            MySqlParameter BookId = new MySqlParameter();
            BookId.ParameterName = "@BookId";
            BookId.Value = _id;
            cmd.Parameters.Add(BookId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Patron> patrons = new List<Patron>{};

            while(rdr.Read())
            {
              int patronId = rdr.GetInt32(0);
              string patronname = rdr.GetString(1);
              string PatronCheckoutDate = rdr.GetString(2);
              Patron newPatron = new Patron(patronname, PatronCheckoutDate ,patronId);
              patrons.Add(newPatron);
            }
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
            return patrons;
          }

    }
  }
