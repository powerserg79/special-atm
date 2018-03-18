using System;
using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    public class EnglishMoneyStack : MoneyStack
    {

        public EnglishMoneyStack() : base("£", "p")
        {
         
        }
    }
}