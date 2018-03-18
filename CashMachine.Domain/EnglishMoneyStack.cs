using System;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    /// <summary>
    /// A money stack that will holds Englands native currency.
    /// </summary>
    public class EnglishMoneyStack : MoneyStack
    {
        public EnglishMoneyStack() : base("£", "p")
        {
         
        }
    }
}