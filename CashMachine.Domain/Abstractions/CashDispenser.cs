using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashMachine.Domain.Abstractions
{
    public abstract class CashDispenser
    {
        public MoneyVault Vault;

        protected CashDispenser(MoneyVault vault)
        {
            Vault = vault;
        }
        
        /// <summary>
        /// Calculates the the fewest notes and coins to be dispensed.
        /// </summary>
        /// <param name="highestDenominationInBaseUnit">The largest currency denomination value to include eg. 2000 ie £20</param>
        /// <param name="amount">The pound value to be withdrawn eg. 120.50</param>
        /// <returns></returns>
        public IList<IMoneyStack> CalulateDispenseMoneyStacks(int highestDenominationInBaseUnit, decimal amount)
        {
            List<IMoneyStack> cashCollection = new List<IMoneyStack>();
            int withdrawalAmount = (int)(amount * 100);

            if (withdrawalAmount < highestDenominationInBaseUnit)
                highestDenominationInBaseUnit = withdrawalAmount;

            var moneyVault = Vault.MoneyStacks.Where(x => x.BaseValue <= highestDenominationInBaseUnit).ToList();

            foreach (IMoneyStack money in moneyVault)
            {
                int quantity = withdrawalAmount / money.BaseValue;
                int remainder = withdrawalAmount % money.BaseValue;

                if (quantity == 0)
                    continue;

                var cash = new MoneyStack();
                cash.BaseValue = money.BaseValue;
                cash.Quantity = quantity;
                cashCollection.Add(cash);

                money.Quantity -= quantity;
                withdrawalAmount -= cash.Total;

                if (remainder == 0)
                    break;
            }

            return cashCollection;
        }

        public abstract IList<IMoneyStack> Dispense(decimal amount);

    }
}
