using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Entities
{
    public class Library
    {
        private LibraryItem[] items = new LibraryItem[100]; // Fixed-size array for simplicity
        private int itemCount = 0; // Tracks the number of items in the array

        public void AddItem(LibraryItem item)
        {
            if (itemCount >= items.Length)
                throw new InvalidOperationException("Library is full");
            items[itemCount++] = item;
            Console.WriteLine($"Added: {item.Title}");
        }

        public LibraryItem SearchByTitle(string title)
        {
            for (int i = 0; i < itemCount; i++)
            {
                if (items[i].Title.ContainsIgnoreCase(title))
                    return items[i];
            }
            return null;
        }

        public void DisplayAllItems()
        {
            Console.WriteLine("\n===== All Library Items =====");
            if (itemCount == 0)
            {
                Console.WriteLine("No items in the library.");
                return;
            }
            for (int i = 0; i < itemCount; i++)
            {
                items[i].DisplayInfo();
            }
        }

        public bool UpdateItemTitle(int id, ref string title)
        {
            for (int i = 0; i < itemCount; i++)
            {
                if (items[i].Id == id)
                {
                    title = items[i].Title;
                    items[i].Title = "New Title";
                    return true;
                }
            }
            return false;
        }

        public ref LibraryItem GetItemReference(int id)
        {
            for (int i = 0; i < itemCount; i++)
            {
                if (items[i].Id == id)
                    return ref items[i]; 
            }
            throw new InvalidOperationException("Item not found");
        }
    }
}
