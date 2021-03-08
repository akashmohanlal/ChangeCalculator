using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeCalculator
{
    public class Program
    {
        //uniform definition from 1 penny to 5000 pennies or £50
        private static readonly List<int> CurrencyDefinition = new List<int> { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000, 2000, 5000 };

        private static void WriteOutput(int change, int currentValue)
        {
            //if the current value is > 50 we need to convert it from pennies to pounds
            //100 pennies = £1
            if (currentValue > 50)
            {
                Console.WriteLine($"{change} x £{currentValue / 100}");
            }
            else
            {
                Console.WriteLine($"{change} x {currentValue}p");
            }
        }

        /// <summary>
        /// The method recursively calculates the change by comparing the amount with a predefined currency list
        /// </summary>
        /// <param name="amount">The amount of change that is due</param>
        /// <param name="currencyDefinition">List of available currency notes and coins</param>
        private static void CalculateChange(int amount, List<int> availableCurrency)
        {
            //end of recursive behaviour
            if (amount == 0 || availableCurrency.Count == 0)
            {
                Console.WriteLine("");
            }
            else
            {
                int currentCurrencyValue = availableCurrency.First();

                //for the first line we want to display the line 'your change is...'
                if (currentCurrencyValue == CurrencyDefinition.OrderByDescending(a => a).First())
                {
                    Console.WriteLine("Your change is:");
                }

                int changeDue = amount / currentCurrencyValue;
                int remainingChangeDue = amount - (currentCurrencyValue * changeDue);

                //if change is due print with predefined format
                if (changeDue != 0)
                {
                    WriteOutput(changeDue, currentCurrencyValue);
                }

                //recursively iterate through the currency definition
                CalculateChange(remainingChangeDue, availableCurrency.Skip(1).ToList());
            }
        }

        /// <summary>
        /// This method handles the validation of the product and payment amounts
        /// </summary>
        /// <param name="productPriceStr">String that contains the product price</param>
        /// <param name="paymentAmountStr">String that contains the payment amount</param>
        /// <returns></returns>
        private static bool ValidateProductPriceAndPayment(string productPriceStr, string paymentAmountStr)
        {
            bool isValid = true;
            decimal productPrice = 0;
            decimal paymentAmount = 0;

            if (!decimal.TryParse(productPriceStr, out productPrice))
            {
                Console.WriteLine("It looks like the product price is not valid, please try again");
                isValid = false;
            }
            else if (!decimal.TryParse(paymentAmountStr, out paymentAmount))
            {
                Console.WriteLine("It looks like the payment amount is not valid, please try again");
                isValid = false;
            }
            else if (productPrice == 0)
            {
                Console.WriteLine("Everyone loves freebies!");
                isValid = false;
            }
            else if (productPrice < 0)
            {
                Console.WriteLine("Someone is paying you to take the products? Lucky you!");
                isValid = false;
            }
            else if (paymentAmount <= 0)
            {
                Console.WriteLine("I have no money, do you accept cryptocurrency?");
                isValid = false;
            }
            else if (paymentAmount < productPrice)
            {
                Console.WriteLine("Just a few pennies short...");
                isValid = false;
            }
            else if (paymentAmount == productPrice)
            {
                Console.WriteLine("No change due...");
                isValid = false;
            }

            return isValid;
        }

        public static void Main(string[] args)
        {
            string productPriceStr = "";
            string paymentAmountStr = "";
            bool quitApp = false;

            while (!quitApp)
            {
                bool inputValid = false;

                while (!inputValid)
                {
                    Console.WriteLine("Please enter the product price:");
                    productPriceStr = Console.ReadLine();
                    Console.WriteLine("Please enter the payment amount:");
                    paymentAmountStr = Console.ReadLine();

                    inputValid = ValidateProductPriceAndPayment(productPriceStr, paymentAmountStr);
                }

                //At this stage the input is valid
                decimal productPrice = decimal.Parse(productPriceStr);
                decimal paymentAmount = decimal.Parse(paymentAmountStr);

                int change = decimal.ToInt32((paymentAmount - productPrice) * 100);
                CalculateChange(change, CurrencyDefinition.OrderByDescending(a => a).ToList());

                //Application controller
                Console.WriteLine("To end enter 'q', otherwise press enter");
                if (Console.ReadLine() == "q")
                    return;
            }
        }
    }
}