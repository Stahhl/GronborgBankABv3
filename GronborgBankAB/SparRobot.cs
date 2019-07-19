using System.Collections.Generic;

namespace GronborgBankAB
{
    class SparRobot
    {
        public static List<string> CalculateFutureBalance(decimal balance, decimal interest, decimal monthlySavings, int numberOfYears)
        {
            decimal newBalance = balance;
            List<string> yearByYear = new List<string>();

            for (int i = 0; i < numberOfYears; i++)
            {
                newBalance = newBalance + (interest * newBalance) + (monthlySavings * 12);
                if (i == 0 || (i - 1) % 5 == 0 || i == numberOfYears - 1)
                {
                    yearByYear.Add($"Year: {(i + 1).ToString().PadRight(5)}\tBalance: {newBalance:0.00} SEK");
                }
            }
            return yearByYear;
        }
    }
}
