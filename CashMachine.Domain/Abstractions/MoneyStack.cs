namespace CashMachine.Domain.Abstractions
{
    /// <summary>
    /// This represents a group of money of the same value. eg A stack of £20 notes
    /// </summary>
    public abstract class MoneyStack
    {
        protected MoneyStack(string currencySymbol, string fractionalUnit)
        {
            CurrencySymbol = currencySymbol;
            FractionalUnit = fractionalUnit;
        }

        /// <summary>
        /// Currency Symbol
        /// </summary>
        public string CurrencySymbol { get; private set; }

        /// <summary>
        /// The unit for the base value
        /// </summary>
        public string FractionalUnit { get; private  set; }

        /// <summary>
        /// The currency denomination expressed in terms of its fractional unit
        /// </summary>
        public int BaseValue { get; set; }

        /// <summary>
        /// The numbers single units of money in the stack
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The total value of all the money in the stack expressed in terms of its fractional unit.
        /// </summary>
        public int Total
        {
            get
            {
                return BaseValue * Quantity;
            }
        }

    }
}