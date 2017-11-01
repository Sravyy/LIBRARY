using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/")]
      public ActionResult Index()
      {
        return View();
      }

      [HttpGet("/Authors")]
      public ActionResult Results2()
      {
        return View("Results",Author.GetAll());
      }

      [HttpPost("/Authors")]
      public ActionResult Results()
      {
        Author newAuthor = new Author (Request.Form["inputAuthor"]);
        newAuthor.Save();
        return View (Author.GetAll());
      }

      [HttpGet("/Authors/{id}")]
      public ActionResult ResultBook(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author selectedAuthor = Author.Find(id);
        List<Book> authorBooks = selectedAuthor.GetBooks();
        model.Add("author", selectedAuthor);
        model.Add("books", authorBooks);
        return View(model);
      }

      [HttpPost("/Authors/{id}")]
      public ActionResult ResultBook2(int id)
      {
        string bookcopies = Request.Form["inputBook"];
        Book newBook = new Book(bookcopies,(Int32.Parse(Request.Form["inputDate"])));
        newBook.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author selectedAuthor = Author.Find(Int32.Parse(Request.Form["author-id"]));
        selectedAuthor.AddBook(newBook);
        List<Book> authorBooks = selectedAuthor.GetBooks();
        model.Add("books", authorBooks);
        model.Add("author", selectedAuthor);
        return View("ResultBook", model);
      }


      [HttpGet("Authors/{id}/Checkout")]
      public ActionResult Checkout(int id)
      {
        Book foundBook = Book.Find(id);
        return View("checkout");
      }

      [HttpPost("Authors/{id}/Checkout")]
      public ActionResult CheckoutPost(int id)
      {
        Book foundBook = Book.Find(id);
        foundBook.BookCheckout();
        Patron foundPatron = Patron.Find(id);
        foundPatron.GetCheckOutDate();
        return RedirectToAction("BookAuthors");
      }

      [HttpGet("/Authors/{id}/books/new")]
      public ActionResult BookForm(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Author selectedAuthor = Author.Find(id);
        List<Book> allBooks = selectedAuthor.GetBooks();
        model.Add("author", selectedAuthor);
        model.Add("books", allBooks);
        return View("ResultBook", model);
      }

      [HttpPost("/BookList")]
      public ActionResult DeletePage2()
      {
        Book.DeleteAll();
        return View();
      }

      [HttpPost("/Authors/{id}/Delete")]
      public ActionResult DeleteAuthor(int id)
      {
        Author foundAuthor = Author.Find(id);
        foreach (Book classBook in foundAuthor.GetBooks())
          {
            classBook.DeleteBooks();
          }
        foundAuthor.DeleteAuthor();
        return View("DeletePage3");
      }

      [HttpPost("/Authors/Delete")]
      public ActionResult DeletePage()
      {
        Author.DeleteAll();
        return View();
      }

      [HttpGet("/BookList")]
      public ActionResult AlphaList()
      {
        return View(Book.GetAlphaList());
      }

      [HttpGet("/Books/{id}")]
      public ActionResult BookAuthors(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Book selectedBook = Book.Find(id);
        List<Author> allAuthors = selectedBook.GetAuthors();
        model.Add("book", selectedBook);
        model.Add("author", allAuthors);
        return View(model);
      }

      [HttpGet("/book/{id}/authors/new")]
      public ActionResult authorsForm(int id)
      {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Book selectedBook = Book.Find(id);
        List<Author> allAuthors = selectedBook.GetAuthors();
        model.Add("book", selectedBook);
        model.Add("author", allAuthors);
        return View(model);
      }

      [HttpPost("/Books/{id}")]
      public ActionResult BookAuthors2(int id)
      {

        Author newAuthor = new Author(Request.Form["inputAuthor"]);
        newAuthor.Save();

        Dictionary<string, object> model = new Dictionary<string, object>();
        Book selectedBook = Book.Find(Int32.Parse(Request.Form["book-id"]));
        selectedBook.AddAuthor(newAuthor);
        List<Author> allAuthors = selectedBook.GetAuthors();
        model.Add("book", selectedBook);
        model.Add("author", allAuthors);
        return View("BookAuthors", model);
      }
    }
  }
