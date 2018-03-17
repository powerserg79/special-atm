using System;
using System.Collections.Generic;
using System.Linq;
using CashMachine.Domain;
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
            atmVault.Add(new MoneyStack() { BaseValue = 1, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 2, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 5, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 10, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 20, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 50, Quantity = 100 });

            // £ denominations
            atmVault.Add(new MoneyStack() { BaseValue = 100, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 100, Quantity = 100 });
            atmVault.Add(new MoneyStack() { BaseValue = 500, Quantity = 50 });
            atmVault.Add(new MoneyStack() { BaseValue = 1000, Quantity = 50 });
            atmVault.Add(new MoneyStack() { BaseValue = 2000, Quantity = 50 });
            atmVault.Add(new MoneyStack() { BaseValue = 5000, Quantity = 50 });
        }

        [TestMethod]
        public void PrimaryDispenserReturnsLeastPiecesOfMoney()
        {
            VaultA vault = new VaultA(atmVault);
            var cashDispenser = new PrimaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            decimal withdrawalAmount = 120m;

            var result  = atmTerminal.Withdraw(withdrawalAmount);

           var fiftyPoundCount = result.First(x => x.BaseValue == 5000).Quantity;
           var twentyPoundCount = result.First(x => x.BaseValue == 2000).Quantity;

            Assert.AreEqual(2, fiftyPoundCount);
            Assert.AreEqual(1, twentyPoundCount);
        }

        [TestMethod]
        public void SecondaryDispenserReturnsMostly20PoundNotes()
        {
            VaultA vault = new VaultA(atmVault);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £120.00 withdrawn
            var withdrawalAmount = 120;
            var result = atmTerminal.Withdraw(withdrawalAmount);

            int count = result.First(x => x.BaseValue == 2000).Quantity;

            Assert.AreEqual(6,count);
        }
        
        [TestMethod]
        public void SecondaryDispenserReturnsCorrectVaultBalanceBeforeWithdrawal()
        {
            // £3 in the vault
            var moneyStacks = new List<MoneyStack>();
            moneyStacks.Add(new MoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new MoneyStack() { BaseValue = 2, Quantity = 100 });

            VaultA vault = new VaultA(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            var result = atmTerminal.Balance;
            var expected = 3m;
            
            Assert.AreEqual(expected,result);
        }

        [TestMethod]
        public void SecondaryDispenserReturnsCorrectVaultBalanceAfterWithdrawal()
        {
            // £3 in the vault
            var moneyStacks = new List<MoneyStack>();
            moneyStacks.Add(new MoneyStack() { BaseValue = 1, Quantity = 100 });
            moneyStacks.Add(new MoneyStack() { BaseValue = 2, Quantity = 100 });

            VaultA vault = new VaultA(moneyStacks);
            var cashDispenser = new SecondaryCashDispenser(vault);
            var atmTerminal = new AtmTerminal(cashDispenser);

            // £1.20 taken from the vault
            var withdrawalAmount = 1.2m;
            var payload = atmTerminal.Withdraw(withdrawalAmount);
            var atmBalance = atmTerminal.Balance;
            var expected = 1.8m;
            
            Assert.AreEqual(expected, atmBalance);
        }
    }
}
