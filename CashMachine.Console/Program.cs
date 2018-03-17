using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashMachine.Domain;

namespace CashMachine.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string line = null;

            var atmMoneyStacks = new List<MoneyStack>();

            // 100 x of each pence denominations
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 1, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 2, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 5, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 10, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 20, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 50, Quantity = 100 });

            // £ denominations
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 100, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 100, Quantity = 100 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 500, Quantity = 50 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 1000, Quantity = 50 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 2000, Quantity = 50 });
            atmMoneyStacks.Add(new MoneyStack() { BaseValue = 5000, Quantity = 50 });

            var moneyVault = new VaultA(atmMoneyStacks);


            //var cashDispenser = new PrimaryCashDispenser(moneyVault);
            var cashDispenser = new SecondaryCashDispenser(moneyVault);

            var atmTerminal = new AtmTerminal(cashDispenser);

            System.Console.WriteLine($"ATM balance is {atmTerminal.Balance.ToString("C")}");
            System.Console.WriteLine("Enter a decimal value to withdraw");

            while ((line = System.Console.ReadLine()) != "exit")
            {
                decimal withdrawalAmount = decimal.Parse(line);

                var result = atmTerminal.Withdraw(withdrawalAmount);
                
                var moneyStackText = new StringBuilder();

                foreach (var MoneyStack in result)
                {
                    decimal poundFormat = (decimal)MoneyStack.BaseValue / 100;
                    bool isPound = MoneyStack.BaseValue % 100 == 0;

                    moneyStackText.Append(isPound ? $"£{poundFormat}" : $"{MoneyStack.BaseValue}p");
                    moneyStackText.Append($" x{MoneyStack.Quantity},");
                }

                System.Console.BackgroundColor = ConsoleColor.Blue;
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(moneyStackText.ToString().TrimEnd(','));
                System.Console.ResetColor();
                System.Console.WriteLine($"ATM balance is {atmTerminal.Balance.ToString("C")}");
                System.Console.WriteLine("Enter a decimal value to withdraw or type exit");
            }
            
        }
    }
}
