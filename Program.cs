using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAssessment_Alexander_Javad
{
    class Program
    {
        static void Main(string[] args)
        {
            //usually would have an EntityFramework DbSet to hold this dataset but I'll model that collection as a List<Transactions>.
            List<Transaction> transactions = new List<Transaction>(100);

            int numberOfRecords = 100;            

            for (int i = 0; i < numberOfRecords; i++)
            {
                double randNum = new Random().NextDouble();
                decimal randomAmount = 0M;

                // without this, I was getting the same random number over and over again
                var task = Task.Run(() => GetRandomAmount(randNum));
                if (task.Wait(TimeSpan.FromMilliseconds(200)))
                    randomAmount = task.Result;

                // if you set a breakpoint on this line, the randomAmount variable will show an actually different amount on each iteration.
                // The speed of computation likely causes the random number generator to set itself as a constant for some reason.
                transactions.Add(new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Amount = Math.Round(randomAmount, 2),
                    OrderDetails = "{ 'ProductId1': 2, 'ProductId3: 4, ...'",
                    CustomerId = Guid.NewGuid()
                });
            }

            Console.WriteLine("Total Rewards Points: " + CalculateRewardsPoints(transactions));
            Console.ReadKey();
        }

        private static int CalculateRewardsPoints(List<Transaction> transactions)
        {
            int totalRewardsPoints = 0;
            foreach (Transaction t in transactions)
            {
                if (51 <= t.Amount && t.Amount < 100)
                {
                    int unitsOver50 = int.Parse(decimal.Truncate(t.Amount).ToString()) - 50;
                    t.RewardsPoints += unitsOver50;
                    totalRewardsPoints = unitsOver50;
                }
                else if (100 < t.Amount)
                {
                    int unitsOver50 = 50;
                    int unitsOver100 = int.Parse(decimal.Truncate(t.Amount).ToString()) - 100;
                    int points = unitsOver50 + 2 * unitsOver100;
                    t.RewardsPoints += points;
                    totalRewardsPoints += points;
                }

                Console.WriteLine(string.Format("Rewards Points for Transaction ({0}) with Amount ({1}) = {2}", t.Id, t.Amount, t.RewardsPoints));
            }

            return totalRewardsPoints;
        }

        private static decimal GetRandomAmount(double randNum)
        {
            double arbitraryMaxAmount = 500;
            return (decimal)(randNum * arbitraryMaxAmount);
        }
    }

    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string OrderDetails { get; set; }
        public Guid CustomerId { get; set; }

        public int RewardsPoints { get; set; }
    }
}
