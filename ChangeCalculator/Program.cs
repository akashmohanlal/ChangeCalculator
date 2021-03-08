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

        /// <summary>
        /// Helper method that formats the output
        /// </summary>
        /// <param name="change">The amount of change due</param>
        /// <param name="currentValue">The current currency value</param>
        /// <returns>A formatted string that contains the change details</returns>
        private static string FormatOutput(int change, int currentValue)
        {
            //if the current value is > 50 we need to convert it from pennies to pounds
            //100 pennies = £1
            if (currentValue > 50)
            {
                return $"{change} x £{currentValue / 100}";
            }

            return $"{change} x {currentValue}p";
        }

        /// <summary>
        /// The method calculates the change due by iterating through the currency definition
        /// </summary>
        /// <param name="productPriceStr">String that contains the product price</param>
        /// <param name="paymentAmountStr">String that contains the payment amount</param>
        /// <returns>A list of strings that contains all the change broken down by currency values</returns>
        private static List<string> CalculateChange(string productPriceStr, string paymentAmountStr)
        {
            decimal productPrice = decimal.Parse(productPriceStr);
            decimal paymentAmount = decimal.Parse(paymentAmountStr);
            int change = decimal.ToInt32((paymentAmount - productPrice) * 100);

            if (change == 0)
            {
                return new List<string> { "No change due" };
            }

            var output = new List<string> { "Your change is:" };

            CurrencyDefinition.OrderByDescending(a => a).ToList()
                .ForEach(currentCurrencyValue =>
                {
                    int changeDue = change / currentCurrencyValue;
                    change -= (currentCurrencyValue * changeDue);

                    //if change is due format the output
                    if (changeDue != 0)
                        output.Add(FormatOutput(changeDue, currentCurrencyValue));
                    
                });

            return output;
        }

        /// <summary>
        /// This method handles the validation of the product and payment amounts
        /// </summary>
        /// <param name="productPriceStr">String that contains the product price</param>
        /// <param name="paymentAmountStr">String that contains the payment amount</param>
        /// <returns>A boolean value specifying whether the input is valid</returns>
        private static string ValidateProductPriceAndPayment(string productPriceStr, string paymentAmountStr)
        {
            decimal productPrice = 0;
            decimal paymentAmount = 0;

            if (!decimal.TryParse(productPriceStr, out productPrice))
                return "It looks like the product price is not valid, please try again";

            if (!decimal.TryParse(paymentAmountStr, out paymentAmount))
                return "It looks like the payment amount is not valid, please try again";

            if (productPrice == 0)
                return "Everyone loves freebies!";

            if (productPrice < 0)
                return "Someone is paying you to take the products? Lucky you!";

            if (paymentAmount <= 0)
                return "I have no money, do you accept cryptocurrency?";

            if (paymentAmount < productPrice)
                return "Just a few pennies short...";

            return string.Empty;
        }

        public static void Main(string[] args)
        {
            string productPriceStr = "";
            string paymentAmountStr = "";
            bool quitApp = false;

            Console.WriteLine("Welcome to the Change Calculator");
            Console.WriteLine($"Note: Please use a dot '.' to enter decimal values{Environment.NewLine}");

            while (!quitApp)
            {
                bool inputValid = false;

                while (!inputValid)
                {
                    Console.WriteLine("Please enter the product price:");
                    productPriceStr = Console.ReadLine();
                    Console.WriteLine("Please enter the payment amount:");
                    paymentAmountStr = Console.ReadLine();

                    string validationMsg = ValidateProductPriceAndPayment(productPriceStr, paymentAmountStr);

                    inputValid = String.IsNullOrEmpty(validationMsg);

                    if (!inputValid)
                        Console.WriteLine(validationMsg);
                }

                //At this stage input is valid so calculate the change
                CalculateChange(productPriceStr, paymentAmountStr)
                    .ForEach(line => Console.WriteLine(line));

                //Application controller
                Console.WriteLine("To end enter 'q', otherwise press enter");
                if (Console.ReadLine() == "q")
                    return;
            }
        }
    }
}