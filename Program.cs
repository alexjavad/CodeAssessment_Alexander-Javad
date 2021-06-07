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
            //usually would have an EntityFramework DbSet to hold this dataset but I'll model that collection as a List<Transactions> and use Linq to operate on it for brevity.
            List<Transaction> transactions = new List<Transaction>(10);

            int arbitraryMaxAmount = 500;
            int numberOfRecords = 100;            

            for (int i = 0; i < numberOfRecords; i++)
            {
                double randNum = new Random().NextDouble();
                decimal randomAmount = (decimal)(randNum * arbitraryMaxAmount);
                transactions.Add(new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Amount = Math.Round(randomAmount, 2),
                    OrderDetails = "{ 'ProductId1': 2, 'ProductId3: 4, ...'",
                    CustomerId = Guid.NewGuid()
                });
            }

            Console.WriteLine(CalculateRewardsPoints(transactions));
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
            }

            return totalRewardsPoints;
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
