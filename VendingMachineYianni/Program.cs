using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineYianni
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] coinNames = new string[] { "pennies", "nickels", "dimes", "quarters" };
            int[] coinDenominations = new int[] { 1, 5, 10, 25 };
            int[] loadedAmounts = new int[] { 0, 0, 3, 1 };
            VendingMachine vendingMachine = new VendingMachine(coinNames, coinDenominations, loadedAmounts);
            
            string input;
            int itemId = 0;

            vendingMachine.Items.Add(new Item(1, 1.00m, "Coke", 10));
            vendingMachine.Items.Add(new Item(2, 1.00m, "Sprite", 10));
            vendingMachine.Items.Add(new Item(3, 0.70m, "Snickers", 10));
            vendingMachine.Items.Add(new Item(4, 0.35m, "Gum", 10));
            vendingMachine.Items.Add(new Item(5, 0.80m, "Skittles", 5));

            Console.WriteLine("Please select item number or q to quit:");
            vendingMachine.PrintItems();
            input = Console.ReadLine().ToLower();
            
            while (input != "q")
            {
                bool correctSelection = false;
                while (!correctSelection)
                {
                    if (!Int32.TryParse(input, out itemId) || !vendingMachine.ValidItemId(itemId))
                    {
                        Console.WriteLine("Please select a valid item number, please try again.");
                        correctSelection = false;
                    }
                    else if (vendingMachine.GetItem(itemId).Quantity == 0)
                    {
                        Console.WriteLine("Out of stock for that item, please select another item.");
                        correctSelection = false;
                    }
                    else
                    {
                        correctSelection = true;
                    }

                    if (!correctSelection)
                    {
                        vendingMachine.PrintItems();
                        input = Console.ReadLine().ToLower();
                    }
                }

                // Load coins into the vending machine for purchase
                int[] inputCoins = new int[vendingMachine.CoinNames.Length]; 
                for (int i = vendingMachine.CoinNames.Length - 1; i >= 0; i--)
                    inputCoins[i] = LoadCoins(vendingMachine.CoinNames[i]);

                Console.WriteLine(vendingMachine.PurchaseItem(itemId, inputCoins));

                Console.WriteLine("\nPlease select item number or q to quit:");
                vendingMachine.PrintItems();
                input = Console.ReadLine().ToLower();
            }
        }

        public static int LoadCoins(string coinName)
        {
            string input;
            uint loadAmount;
            Console.WriteLine($"Please enter number of {coinName} to load:");
            input = Console.ReadLine();

            while (!UInt32.TryParse(input, out loadAmount))
            {
                Console.WriteLine($"Number of {coinName} needs to be a non-negative integer, please try again: ");
                input = Console.ReadLine();
            }

            return (int)loadAmount;
        }
    }
}
