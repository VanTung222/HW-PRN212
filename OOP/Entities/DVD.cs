using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public class DVD : LibraryItem, IBorrowable
    {
        public string Director { get; set; }
        public int Runtime { get; set; }
        public string AgeRating { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsAvailable => BorrowDate == null || ReturnDate != null;

        public DVD(int id, string title, int publicationYear, string director)
            : base(id, title, publicationYear)
        {
            Director = director;
        }

        public override void DisplayInfo() => Console.WriteLine($"DVD: {Title}, Director: {Director}, Year: {PublicationYear}, Runtime: {Runtime} min, Age Rating: {AgeRating}, Available: {IsAvailable}");

        public override decimal CalculateLateReturnFee(int daysLate) => daysLate * 1.00m;

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
