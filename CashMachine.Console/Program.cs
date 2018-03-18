using CashMachine.Domain;
using CashMachine.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CashMachine.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string line = null;

            var atmMoneyStacks = new List<EnglishMoneyStack>();

            // £
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 100, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 100, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 500, Quantity = 50 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1000, Quantity = 50 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2000, Quantity = 50 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 5000, Quantity = 50 });
            // p
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 5, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 10, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 20, Quantity = 100 });
            atmMoneyStacks.Add(new EnglishMoneyStack() { BaseValue = 50, Quantity = 100 });

            var moneyVault = new EnglishAtmVault(atmMoneyStacks);
            CashDispenser cashDispenser;

            string algorithm = ConfigurationManager.AppSettings["DispensingAlgorithm"];

            // choose which algorithm to use based on a setting in the app.config
            if (algorithm == "1")
            {
                cashDispenser = new PrimaryCashDispenser(moneyVault);
            }
            else
            {
                cashDispenser = new SecondaryCashDispenser(moneyVault);
            }

            var atmTerminal = new AtmTerminal(cashDispenser);

            System.Console.WriteLine($"ATM balance is {atmTerminal.Balance.ToString("C")}");
            System.Console.WriteLine("Enter an amount to withdraw");

            while ((line = System.Console.ReadLine()) != "exit")
            {

                decimal valueToWithdraw;
                if (string.IsNullOrEmpty(line) || !decimal.TryParse(line, out valueToWithdraw))
                {
                    System.Console.WriteLine("Enter another amount to withdraw or type 'exit' to close the program");
                    continue;
                }

                decimal withdrawalAmount = decimal.Parse(line);
                var moneyStackText = new StringBuilder();

                try
                {
                    var result = atmTerminal.Withdraw(withdrawalAmount);

                    foreach (var MoneyStack in result)
                    {
                        decimal poundFormat = (decimal)MoneyStack.BaseValue / 100;
                        bool isPound = MoneyStack.BaseValue % 100 == 0;

                        moneyStackText.Append(isPound ? $"{MoneyStack.CurrencySymbol}{poundFormat}" : $"{MoneyStack.BaseValue}{MoneyStack.FractionalUnit}");
                        moneyStackText.Append($"x{MoneyStack.Quantity},");
                    }
                }
                catch (Exception e)
                {
                    moneyStackText = new StringBuilder();
                    moneyStackText.Append(e.Message);
                }

                System.Console.BackgroundColor = ConsoleColor.Blue;
                System.Console.ForegroundColor = ConsoleColor.White;
                System.Console.WriteLine(moneyStackText.ToString().TrimEnd(','));
                System.Console.ResetColor();
                System.Console.WriteLine($"ATM balance is {atmTerminal.Balance.ToString("C")}");
                System.Console.WriteLine("Enter another amount to withdraw or type 'exit' to close the program");
            }
        }
    }
}