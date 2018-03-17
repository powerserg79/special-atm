using System.Collections.Generic;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    public class VaultA : MoneyVault
    {
        public VaultA(IEnumerable<IMoneyStack> money) : base(money)
        {
        }
    }
}