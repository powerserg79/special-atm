using System.Collections.Generic;
using System.Linq;

namespace CashMachine.Domain.Abstractions
{
    /// <summary>
    /// An ATM cash dispenser that will take money from the vault to be dispensed.
    /// </summary>
    public abstract class CashDispenser
    {
        public AtmVault Vault;

        protected CashDispenser(AtmVault vault)
        {
            Vault = vault;
        }

        /// <summary>
        /// Calculates the the fewest notes and coins to be dispensed.
        /// </summary>
        /// <param name="highestDenominationInBaseUnit">The largest currency denomination value to include eg. 2000 ie £20</param>
        /// <param name="amountToDispense">The amount to dispense expressed in the currencies fractional unit.</param>
        /// <returns></returns>
        public virtual IEnumerable<MoneyStack> CountMoneyStacksToDispense(int highestDenominationInBaseUnit, int amountToDispense)
        {
            var cashCollection = new List<MoneyStack>();

            if (amountToDispense < highestDenominationInBaseUnit)
                highestDenominationInBaseUnit = amountToDispense;

            var moneyVault = Vault.MoneyStacks.Where(x => x.BaseValue <= highestDenominationInBaseUnit).ToList();

            foreach (MoneyStack money in moneyVault)
            {
                int quantity = amountToDispense / money.BaseValue;

                quantity = money.Quantity > quantity ? quantity : money.Quantity;

                if (quantity == 0)
                    continue;

                int remainder = amountToDispense % (money.BaseValue * quantity);

                MoneyStack cash = Vault.Take(money, quantity);
                cashCollection.Add(cash);

                amountToDispense -= cash.Total;

                if (remainder <= 0)
                    break;
            }

            return cashCollection;
        }

        public abstract IEnumerable<MoneyStack> Dispense(decimal amount);
    }
}