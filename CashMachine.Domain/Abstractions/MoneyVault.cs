using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CashMachine.Domain.Abstractions
{
    public abstract class AtmVault
    {
        public IEnumerable<MoneyStack> MoneyStacks { get; set; }

        public abstract string CurrencyCode { get; }

        protected AtmVault(IEnumerable<MoneyStack> money)
        {
            this.MoneyStacks = money.OrderByDescending(x => x.BaseValue);
        }

        /// <summary>
        /// This will return a new instance of the money stack and reduce that stack from the stacks of money in the vaults main supply
        /// </summary>
        /// <param name="englishMoneyStack"></param>
        /// <returns></returns>
        public abstract MoneyStack Take(MoneyStack moneyStack, int quantity);
     
        /// <summary>
        /// The balance of the cash dispenser in the base unit
        /// </summary>
        public int Balance
        {
            get
            {
                return MoneyStacks.Sum(x => x.Total);
            }
        }
    }
}