using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryServer.Database;

namespace LibraryServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private ILibraryDB dB;

        public BooksController(ILibraryDB db)
        {
            this.dB = db;
        }

        public Book[] GetBooks() 
        {
            // return new Book[] 
            // {
            //     new Book() { ID = 1, Title = "A", Pages = 500 },
            //     new Book() { ID = 2, Title = "B", Pages = 400 }
            // };
            return dB.GetAllBooks().ToArray();
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id) 
        {
            var b = dB.TryGetBook(id);
            if(b != null) 
                return b;
            else 
                return StatusCode(404, "Book with given id is not found"); 
        }

        [HttpPut]
        public Book AddBook(NewBook book) 
        {
            Console.WriteLine($"{book.Title}, {book.Pages}");
            return dB.AddNewBook(book);
        }
    }
}
