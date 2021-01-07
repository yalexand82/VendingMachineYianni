using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineYianni
{
    public class Item
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Item(int id, decimal cost, string name, int quantity)
        {
            Id = id;
            Cost = cost;
            Name = name;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"Item Number { Id }: { Cost.ToString("C", new CultureInfo("en-US")) } { Name }";
        }
    }
}
