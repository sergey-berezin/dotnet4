using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace efsample
{
    class Author 
    {
        public int AuthorId { get; set; }
        [ConcurrencyCheck]
        public string FirstName { get; set; }
        [ConcurrencyCheck]
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
                foreach(var au in db.Authors)
                    Console.WriteLine($"{au.FirstName} {au.LastName}");
                // db.Add(new Author { FirstName = "John", LastName = "Smith"});
                // db.SaveChanges();

                var a = db.Authors.Where(a => a.FirstName.StartsWith("J")).FirstOrDefault();
                a.LastName = Console.ReadLine();
                try 
                {
                    db.SaveChanges();
                }
                catch(DbUpdateConcurrencyException exc) 
                {
                    FixConcurrentAccess(db, exc.Entries);
                }
            }
        }

        static private void FixConcurrentAccess(LibraryContext db, IReadOnlyList<EntityEntry> entries)
        {
            foreach(var e in entries) {
                var a3 = (Author)e.Entity;
                var newValues = e.GetDatabaseValues();
                Console.WriteLine($"New last name detected: {newValues["LastName"]} ");
                e.OriginalValues.SetValues(newValues);
            }    
            try {
                db.SaveChanges();
            }
            catch(DbUpdateConcurrencyException exc)
            {
                 FixConcurrentAccess(db, exc.Entries);
            }
        }
    }
}

// w = compute(x,y,z)