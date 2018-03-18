using CashMachine.Domain.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace CashMachine.Domain
{
    /// <summary>
    /// A Cash Machine
    /// </summary>
    public class AtmTerminal
    {
        private CashDispenser cashDispenser;

        public AtmTerminal(CashDispenser cashDispenser)
        {
            this.cashDispenser = cashDispenser;
        }

        /// <summary>
        /// The balance of the ATM in pounds
        /// </summary>
        public decimal Balance
        {
            get
            {
                return (decimal)cashDispenser.Vault.Balance / 100;
            }
        }

        /// <summary>
        /// Withdraw money from the ATM's vault
        /// </summary>
        /// <param name="amount">The amount to withdraw in pounds</param>
        /// <returns>Collection of money grouped by denomination</returns>
        public IEnumerable<MoneyStack> Withdraw(decimal amount)
        {
            return cashDispenser.Dispense(amount);
        }
    }
}