using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    public class Book : Product
    {
        private int printLength;
        private string language, publisher;
        private string publicationDate;
        private string author;
        private string genre;

        public int PrintLength { get { return printLength; } set { printLength = value; } }

        public string Language { get { return language; } set { language = value; } }

        public string Publisher { get { return publisher; } set { publisher = value; } }

        public string PublicationDate { get { return publicationDate; } set { publicationDate = value; } }

        public string Author { get => author; set => author = value; }
        public string Genre { get => genre; set => genre = value; }

        public Book() : base()
        {
            this.printLength = 0;
            this.language = null;
            this.publisher = null;
            this.publicationDate = DateTime.Now.ToString("yyyy/MM/dd");
            this.author = null;
            this.genre = null;
        }
    }
}
