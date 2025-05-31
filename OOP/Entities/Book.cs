using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public class Book : LibraryItem, IBorrowable
    {
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable => BorrowDate == null || ReturnDate != null;

        public Book(int id, string title, int publicationYear, string author)
            : base(id, title, publicationYear)
        {
            Author = author;
        }

        public override void DisplayInfo() => Console.WriteLine($"Book: {Title} by {Author}, Year: {PublicationYear}, Pages: {Pages}, Genre: {Genre}, Available: {IsAvailable}");


        public override decimal CalculateLateReturnFee(int daysLate) => daysLate * 0.75m;


        public void Borrow()
        {
            if (IsAvailable)
            {
                BorrowDate = DateTime.Now;
                ReturnDate = null;
                Console.WriteLine($"{Title} has been borrowed.");
            }
            else
                Console.WriteLine($"{Title} is currently not available.");
        }

        public void Return()
        {
            if (!IsAvailable)
            {
                ReturnDate = DateTime.Now;
                Console.WriteLine($"{Title} has been returned.");
            }
            else
                Console.WriteLine($"{Title} is already available.");
        }
    }
}
