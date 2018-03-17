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
        public PrimaryCashDispenser(MoneyVault vault) : base(vault)
        {
        }

        public override IList<IMoneyStack> Dispense(decimal amount)
        {
            if ( Vault.Balance < amount)
            {
                throw new Exception("Insufficient funds to perform the transaction.");
            }
            
            int seedDenomiation = 5000;

            return CalulateDispenseMoneyStacks(seedDenomiation, amount);
        }
    }
}