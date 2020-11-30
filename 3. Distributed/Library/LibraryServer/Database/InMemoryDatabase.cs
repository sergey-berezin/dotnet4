using System.Collections.Generic;
using LibraryContracts;

namespace LibraryServer.Database 
{
    public interface ILibraryDB 
    {
        IEnumerable<Book> GetAllBooks();
        Book TryGetBook(int id);

        Book AddNewBook(NewBook nb);
    }

    public class InMemoryLibrary : ILibraryDB
    {
        public Book AddNewBook(NewBook nb)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return new Book[1] {
                new Book() {
                    Title = "A",
                    Pages = 200
                }
            };

        }

        public Book TryGetBook(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}