using System.Collections.Generic;
using System.Linq;

namespace CashMachine.Domain.Abstractions
{
    public abstract class MoneyVault
    {
        public IEnumerable<IMoneyStack> MoneyStacks { get; set; }
        
        public MoneyVault(IEnumerable<IMoneyStack> money)
        {
            this.MoneyStacks = money.OrderByDescending(x => x.BaseValue);
        }

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