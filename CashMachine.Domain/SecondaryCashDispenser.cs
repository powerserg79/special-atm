using System;
using System.Collections.Generic;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    /// <summary>
    /// A cash dispenser that dispenses mostly £20 notes
    /// </summary>
    public class SecondaryCashDispenser : CashDispenser
    {
        public SecondaryCashDispenser(AtmVault vault) : base(vault)
        {
        }

        /// <summary>
        /// Dispenses money from the vault.
        /// </summary>
        /// <param name="amount">The amount that the dispenser should dispense</param>
        /// <returns>A collection with as many £20 notes.</returns>
        public override IEnumerable<MoneyStack> Dispense(decimal amount)
        {
            int withdrawalAmount = (int)(amount * 100);

            if (Vault.Balance < withdrawalAmount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }

            int seedDenomiation = 2000;

            return CountMoneyStacksToDispense(seedDenomiation, withdrawalAmount);
        }
    }
}