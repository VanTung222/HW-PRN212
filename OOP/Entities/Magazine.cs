using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public class Magazine : LibraryItem
    {
        public int IssueNumber { get; set; }
        public string Publisher { get; set; }

        public Magazine(int id, string title, int publicationYear, int issueNumber)
            : base(id, title, publicationYear)
        {
            IssueNumber = issueNumber;
        }

        public override void DisplayInfo() => Console.WriteLine($"Magazine: {Title}, Issue: {IssueNumber}, Year: {PublicationYear}, Publisher: {Publisher}");
    }
}
