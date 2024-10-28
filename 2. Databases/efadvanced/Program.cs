using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace efsample
{
    public class Author 
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        virtual public ICollection<Book> Books { get; set; }

        public byte[] Photo { get; set; }
    }

    public class Book 
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        virtual public ICollection<Author> Authors { get; set; }
    }

    class LibraryContext : DbContext 
    {
        public DbSet<Author> Authors { get; set; }   
        public DbSet<Book> Books { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder o) 
            => o.UseLazyLoadingProxies(). UseSqlite("Data Source=library.db");
    }

    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new LibraryContext()) 
            {
                // var a = new Author { FirstName = "Frank", LastName = "Herbert" };
                // var b = new Book { Title = "Dune", Pages = 450 };
                // b.Authors = new List<Author>() { a };
                // a.Books = new List<Book>() { b };
                // db.Add(b);
                // db.SaveChanges();


                foreach(var a2 in db.Authors/*.Include(a => a.Books)*/)
                {
                    Console.WriteLine($"{a2.AuthorId} {a2.FirstName} {a2.LastName}");
                    // db.Entry(a2).Collection(a => a.Books).Load();
                    foreach(var b2 in a2.Books)
                         Console.WriteLine($"- {b2.BookId} {b2.Title} {b2.Pages}");
                }
            }
        }
    }
}
