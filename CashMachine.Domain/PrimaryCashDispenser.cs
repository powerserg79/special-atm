using CashMachine.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace CashMachine.Domain
{
    /// <summary>
    /// A cash dispenser that dispenses the fewest pieces of money possible.
    /// </summary>
    public class PrimaryCashDispenser : CashDispenser
    {
        public PrimaryCashDispenser(AtmVault vault) : base(vault)
        {
        }

        /// <summary>
        /// Dispenses money from the vault.
        /// </summary>
        /// <param name="amount">The amount that the dispenser should dispense</param>
        /// <returns>A collection with the fewest pieces of money possible.</returns>
        public override IEnumerable<MoneyStack> Dispense(decimal amount)
        {
            int withdrawalAmount = (int)(amount * 100);

            if (Vault.Balance < withdrawalAmount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }

            int seedDenomiation = 5000;

            return CountMoneyStacksToDispense(seedDenomiation, withdrawalAmount);
        }
    }
}