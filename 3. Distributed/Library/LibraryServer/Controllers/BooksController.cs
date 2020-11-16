using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LibraryServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public Book[] GetBooks() 
        {
            return new Book[] 
            {
                new Book() { ID = 1, Title = "A", Pages = 500 },
                new Book() { ID = 2, Title = "B", Pages = 400 }
            };
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id) 
        {
            if(id == 1) return new Book() { ID = 1, Title = "A", Pages = 500 };
            else return StatusCode(404, "Book with given id is not found"); 
        }

        [HttpPut]
        public Book AddBook(NewBook book) 
        {
            Console.WriteLine($"{book.Title}, {book.Pages}");
            return new Book() { ID = 3, Title = book.Title, Pages = book.Pages };
        }
    }
}
