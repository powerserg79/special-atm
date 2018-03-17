using CashMachine.Domain.Abstractions;

namespace CashMachine.Domain
{
    public class MoneyStack : IMoneyStack
    {
        public int BaseValue { get; set; }
        public int Quantity { get; set; }

        public int Total
        {
            get
            {
                return BaseValue * Quantity;
            }
        }

    }
}