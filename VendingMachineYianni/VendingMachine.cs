using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachineYianni
{
    class VendingMachine
    {
        public List<Item> Items { get; set; }
        public string[] CoinNames { get; set; }
        public int[] CoinDenominations { get; set; }
        public int[] LoadedAmounts { get; set; }

        public VendingMachine(string[] coinNames, int[] coinDenominations, int[] loadedAmounts)
        {
            Items = new List<Item>();
            CoinNames = coinNames;
            CoinDenominations = coinDenominations;
            LoadedAmounts = loadedAmounts;
        }

        public Item GetItem(int id)
        {
            return Items.SingleOrDefault(i => i.Id == id);
        }

        public bool ValidItemId(int id)
        {
            return Items.SingleOrDefault(i => i.Id == id) != null;
        }

        public void PrintItems()
        {
            Items.ForEach(Console.WriteLine);
        }

        public string PurchaseItem(int itemId, int[] inputCoins)
        {
            Item item = GetItem(itemId);
            int i, j, n = CoinDenominations.Length;

            decimal inputAmount = 0.00m;
            for (i = 0; i < n; i++)
                inputAmount += inputCoins[i] * (decimal)CoinDenominations[i] / 100.00m;
            
            if (item.Cost > inputAmount)
            {
                return "Not enough change deposited, please try again.";
            }
            else if (item.Cost == inputAmount)
            {
                item.Quantity--;
                for (i = 0; i < CoinDenominations.Length; i++)
                    LoadedAmounts[i] += inputCoins[i];
                return "Exact change deposited, no change returned.";
            }
            else
            {
                int[] solutionNumCoins = new int[] { 0, 0, 0, 0 }; // total number of coins across all denominations for each solution scenario
                int[,] changeAmounts = new int[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } }; // number of coins returned for each denomination for each solution scenario
                int diff;

                int[][] cA = new int[CoinDenominations.Length][]; // superfluous array so we can test Sum() function on array
                for (i = 0; i < n; i++)
                    cA[i] = new int[n];

                for (i = 0; i < CoinDenominations.Length; i++)
                    LoadedAmounts[i] += inputCoins[i];

                // try different solution scenarios for each coin combination
                for (i = n-1; i >= 0; i--)
                {
                    diff = (int)((inputAmount - item.Cost) * 100); // recalculate diff for each solution scenario
                    for (j = i; j >= 0; j--)
                    {
                        cA[i][j] = 0;
                        if (diff / CoinDenominations[j] <= LoadedAmounts[j])
                            changeAmounts[i,j] = diff / CoinDenominations[j];
                        else
                            changeAmounts[i,j] = LoadedAmounts[j];
                        LoadedAmounts[j] -= changeAmounts[i,j];
                        diff -= changeAmounts[i,j] * CoinDenominations[j];
                        cA[i][j] = changeAmounts[i, j];
                        Console.WriteLine("Num " + CoinNames[j] + " to return: " + changeAmounts[i,j]);
                    }
                    if (diff == 0)
                    {
                        // if we produce exact change, store how many coins are needed to produce change
                        int sum = cA[i].Sum();
                        for (j = i; j >= 0; j--)
                            solutionNumCoins[i] += changeAmounts[i, j];
                        //solutionNumCoins[i] = sum;
                    }
                    else
                    {
                        // if we can't produce exact change, store -1 to signify unable to produce exact change
                        solutionNumCoins[i] = -1;
                    }

                    // reload the coins withdrawn in order to try the next solution scenario
                    for (j = i; j >= 0; j--)
                        LoadedAmounts[j] += changeAmounts[i, j];
                    Console.WriteLine("----------------------------------------");
                }

                // determine which solution returned a valid and least number of coins
                int correctSolution = -1;
                int minCoins = 100;
                for (i = 0; i < n; i++)
                {
                    if (solutionNumCoins[i] > 0 && solutionNumCoins[i] < minCoins)
                    {
                        minCoins = solutionNumCoins[i];
                        correctSolution = i;
                    }
                }

                if (correctSolution >= 0)
                {
                    // we found a correct solution, decrement changeAmount coins from LoadedAmounts
                    string output = "Exact change returned: ";
                    for (j = 0; j < n; j++)
                    {
                        LoadedAmounts[j] -= changeAmounts[correctSolution, j];
                        output += $"{ changeAmounts[correctSolution, j] } { CoinNames[j] }, ";
                    }
                    item.Quantity--;
                    output = $"{ output.Substring(0, output.Length - 2) }.";
                    return output;
                }
                else
                {
                    return "Unable to return exact change, please try again.";
                }
            }
        }
    }
}
