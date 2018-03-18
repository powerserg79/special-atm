using System;
using System.Collections.Generic;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    /// <summary>
    /// Dispenses mostly £20 notes
    /// </summary>
    public class SecondaryCashDispenser : CashDispenser
    {
        public SecondaryCashDispenser(AtmVault vault) : base(vault)
        {
        }

        public override IEnumerable<MoneyStack> Dispense(decimal amount)
        {
            int withdrawalAmount = (int)(amount * 100);

            if (Vault.Balance < withdrawalAmount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }

            int seedDenomiation = 2000;

            return CalulateDispenseMoneyStacks(seedDenomiation, withdrawalAmount);
        }
    }
}