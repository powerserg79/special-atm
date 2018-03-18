namespace CashMachine.Domain.Abstractions
{
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
        /// The quantity of money items in the stack
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The total of all the currency items in this money stack
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