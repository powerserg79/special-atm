namespace CashMachine.Domain.Abstractions
{
    public interface IMoneyStack
    {
        /// <summary>
        /// The currency denomination
        /// </summary>
        int BaseValue { get; set; }

        /// <summary>
        /// The quantity of money items in the stack
        /// </summary>
        int Quantity { get; set; }

        /// <summary>
        /// The total of all the currency items in this money stack
        /// </summary>
        int Total { get; }

    }
}