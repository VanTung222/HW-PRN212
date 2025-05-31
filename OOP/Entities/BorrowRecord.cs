using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public record BorrowRecord(
        int ItemId,
        string Title,
        DateTime BorrowDate,
        DateTime? ReturnDate,
        string BorrowerName)
    {
        public string LibraryLocation { get; init; } = "Unknown";
        public override string ToString() => $"Borrow Record: {Title} (ID: {ItemId}), Borrowed by {BorrowerName} on {BorrowDate:yyyy-MM-dd}, " +
            $"Returned: {(ReturnDate?.ToString("yyyy-MM-dd") ?? "Not returned")}, Location: {LibraryLocation}";
    }
}
