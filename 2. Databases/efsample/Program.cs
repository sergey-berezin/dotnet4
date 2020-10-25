using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace efsample
{
    class Author 
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    class LibraryContext : DbContext 
    {
        public DbSet<Author> Authors { get; set; }    

        protected override void OnConfiguring(DbContextOptionsBuilder o) => o.UseSqlite("Data Source=library.db");
    }

    class Program
    {
        static void Main(string[] args)
        {
            using(var db = new LibraryContext()) 
            {
                // db.Add(new Author { FirstName = "John", LastName = "Smith"});
                // db.SaveChanges();

                var query = db.Authors.Where(a => a.FirstName.StartsWith("F"));
                foreach(var a in query)
                    Console.WriteLine($"{a.AuthorId} {a.FirstName} {a.LastName}");
            }
        }
    }
}
