using CashMachine.Domain.Abstractions;
using System;
using System.Collections.Generic;

namespace CashMachine.Domain
{
    /// <summary>
    /// Dispenses least pieces of money possible
    /// </summary>
    public class PrimaryCashDispenser : CashDispenser
    {
        public PrimaryCashDispenser(AtmVault vault) : base(vault)
        {
        }

        public override IEnumerable<MoneyStack> Dispense(decimal amount)
        {
            int withdrawalAmount = (int)(amount * 100);

            if (Vault.Balance < withdrawalAmount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }

            int seedDenomiation = 5000;

            return CalulateDispenseMoneyStacks(seedDenomiation, withdrawalAmount);
        }
    }
}