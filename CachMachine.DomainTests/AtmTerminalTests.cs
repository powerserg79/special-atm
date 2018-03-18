using System;
using System.Collections.Generic;
using System.Linq;
using CashMachine.Domain;
using CashMachine.Domain.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CashMachine.DomainTests
{
    [TestClass]
    public class AtmTerminalTests
    {
        private List<MoneyStack> atmVault;

        [TestInitialize]
        public void Initialise()
        {
            atmVault = new List<MoneyStack>();

            // 100 x of each pence denominations
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 5, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 10, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 20, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 50, Quantity = 100 });

            // £ denominations
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 100, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 100, Quantity = 100 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 500, Quantity = 50 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 1000, Quantity = 50 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 2000, Quantity = 50 });
            atmVault.Add(new EnglishMoneyStack() { BaseValue = 5000, Quantity = 50 });
        }

        [TestMethod]
        public void PrimaryDispenserReturnsLeastPiecesOfMoney()
        {
            EnglishAtmVault vault = new EnglishAtmVault(atmVault);
            var cashDispenser = new PrimaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            decimal withdrawalAmount = 120m;

            var result = atmTerminal.Withdraw(withdrawalAmount);

            var fiftyPoundCount = result.First(x => x.BaseValue == 5000).Quantity;
            var twentyPoundCount = result.First(x => x.BaseValue == 2000).Quantity;

            Assert.AreEqual(2, fiftyPoundCount);
            Assert.AreEqual(1, twentyPoundCount);
        }

        [TestMethod]
        public void SecondaryDispenserReturnsMostly20PoundNotes()
        {
            EnglishAtmVault vault = new EnglishAtmVault(atmVault);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £120.00 withdrawn
            var withdrawalAmount = 120;
            IEnumerable<MoneyStack> result = atmTerminal.Withdraw(withdrawalAmount);

            int count = result.First(x => x.BaseValue == 2000).Quantity;

            Assert.AreEqual(6, count);
        }

        [TestMethod]
        public void SecondaryDispenserReturnsCorrectVaultBalanceBeforeWithdrawal()
        {
            // £3 in the vault
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });

            EnglishAtmVault vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            var result = atmTerminal.Balance;
            var expected = 3m;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SecondaryDispenserReturnsCorrectVaultBalanceAfterWithdrawal()
        {
            // £3 in the vault
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });

            EnglishAtmVault vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £1.20 taken from the vault
            var withdrawalAmount = 1.2m;
            var payload = atmTerminal.Withdraw(withdrawalAmount);
            var atmBalance = atmTerminal.Balance;
            var expected = 1.8m;

            Assert.AreEqual(expected, atmBalance);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Insufficient funds to perform the transaction.")]
        public void PrimaryDispenserDoesntOverdraw()
        {
            // £3 in the vault
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });

            var vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new PrimaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £1.20 taken from the vault
            var withdrawalAmount = 3.2m;
            atmTerminal.Withdraw(withdrawalAmount);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Insufficient funds to perform the transaction.")]
        public void SecondaryDispenserDoesntOverdraw()
        {
            // £3 in the vault
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2, Quantity = 100 });

            var vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £1.20 taken from the vault
            var withdrawalAmount = 3.2m;
            atmTerminal.Withdraw(withdrawalAmount);
        }
        
        [TestMethod]
        public void SecondaryDispenserCanWithdrawMoreThanOnce()
        {
            EnglishAtmVault vault = new EnglishAtmVault(atmVault);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            var withdrawalAmount1 = 100m;
            atmTerminal.Withdraw(withdrawalAmount1);
            var atmBalance1 = atmTerminal.Balance;
            var expected1 = 4438m;

            Assert.AreEqual(expected1, atmBalance1);

            var withdrawalAmount2 = 123m;
            atmTerminal.Withdraw(withdrawalAmount2);
            var atmBalance2 = atmTerminal.Balance;
            var expected2 = 4315m;

            Assert.AreEqual(expected2, atmBalance2);
        }  
             
        [TestMethod]
        public void SecondaryDispenserCanWithdrawWithEmptyMoneyStack()
        {
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2000, Quantity = 0 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1000, Quantity = 10 });

            EnglishAtmVault vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);
            
            var withdrawalAmount = 100m;
            atmTerminal.Withdraw(withdrawalAmount);
            var atmBalance = atmTerminal.Balance;
            var expected = 0m;

            Assert.AreEqual(expected, atmBalance);
        } 
        [TestMethod]
        public void PrimaryDispenserCanWithdrawMoreThanOnce()
        {
            EnglishAtmVault vault = new EnglishAtmVault(atmVault);
            var cashDispenser = new PrimaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            var withdrawalAmount1 = 100m;
            atmTerminal.Withdraw(withdrawalAmount1);
            var atmBalance1 = atmTerminal.Balance;
            var expected1 = 4438m;

            Assert.AreEqual(expected1, atmBalance1);

            var withdrawalAmount2 = 123m;
            atmTerminal.Withdraw(withdrawalAmount2);
            var atmBalance2 = atmTerminal.Balance;
            var expected2 = 4315m;

            Assert.AreEqual(expected2, atmBalance2);
        }  
             
        [TestMethod]
        public void PrimaryDispenserCanWithdrawWithEmptyMoneyStack()
        {
            var moneyStacks = new List<EnglishMoneyStack>();
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 2000, Quantity = 0 });
            moneyStacks.Add(new EnglishMoneyStack() { BaseValue = 1000, Quantity = 10 });

            EnglishAtmVault vault = new EnglishAtmVault(moneyStacks);
            var cashDispenser = new PrimaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);
            
            var withdrawalAmount = 100m;
            atmTerminal.Withdraw(withdrawalAmount);
            var atmBalance = atmTerminal.Balance;
            var expected = 0m;

            Assert.AreEqual(expected, atmBalance);
        }
    }
}
