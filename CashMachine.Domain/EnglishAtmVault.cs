using System.Collections.Generic;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    /// <summary>
    /// An ATM vault to be used in England
    /// </summary>
    public class EnglishAtmVault : AtmVault
    {
        public override string CurrencyCode { get { return "GBP"; } }

        public EnglishAtmVault(IEnumerable<MoneyStack> money) : base(money)
        {
        }

        public override MoneyStack Take(MoneyStack moneyStack, int quantity)
        {
            var newEnglishMoneyStack = new EnglishMoneyStack();
            newEnglishMoneyStack.Quantity = quantity;
            newEnglishMoneyStack.BaseValue = moneyStack.BaseValue;
            
            moneyStack.Quantity -= quantity;

            return newEnglishMoneyStack;
        }
    }
}