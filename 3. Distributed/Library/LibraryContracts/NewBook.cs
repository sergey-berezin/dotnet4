using System;

namespace LibraryContracts
{
    public class NewBook
    {
        public string Title { get; set; }

        public int Pages { get; set; }

        public string JpegCover { get; set; } // Base64
    }
}
