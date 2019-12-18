using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines { get { return lineCollection; } }

        public void AddItem (Phone phone, int quantity)
        {
            CartLine line = lineCollection
                .Where(b => b.Phone.PhoneId == phone.PhoneId)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine { Phone = phone, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Phone phone)
        {
            lineCollection.RemoveAll(l => l.Phone.PhoneId == phone.PhoneId);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Phone.Price * e.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }

    public class CartLine
    {
        public Phone Phone { get; set; }
        public int Quantity { get; set; }
    }
}
