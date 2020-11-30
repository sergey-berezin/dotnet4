using System;
using Xunit;
using System.Linq;

namespace LibraryTests
{
    public class InMemoryLibraryTests
    {
        [Fact]
        public void Test1()
        {
            var db = new LibraryServer.Database.InMemoryLibrary();
            Assert.Equal(1, db.GetAllBooks().Count());
        }
    }
}
