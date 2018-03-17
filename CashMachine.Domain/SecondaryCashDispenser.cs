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
        public SecondaryCashDispenser(MoneyVault vault) : base(vault)
        {
        }

        public override IList<IMoneyStack> Dispense(decimal amount)
        {
            if (Vault.Balance < amount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }

            int seedDenomiation = 2000;

            return CalulateDispenseMoneyStacks(seedDenomiation, amount);
        }
    }
}