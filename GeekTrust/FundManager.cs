using GeekTrust.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekTrust
{
    public class FundManager
    {
        public const string CURRENT_PORTFOLIO = "CURRENT_PORTFOLIO";
        public const string CALCULATE_OVERLAP = "CALCULATE_OVERLAP";
        public const string ADD_STOCK = "ADD_STOCK";

        private IPortfolio Portfolio;
        private IAvailableFunds AvailableFunds;

        public FundManager(IAvailableFunds _AvailableFunds, IPortfolio _Portfolio)
        {
            AvailableFunds = _AvailableFunds;
            Portfolio = _Portfolio;
        }

        public void ProcessInputCommand(string _Input)
        {
            var splitInput = _Input.Split(null).ToList();
            var command = splitInput[0];
            splitInput.Remove(command); // remove the command from the split input so we are left with only parameters.

            switch (command)
            {
                case CURRENT_PORTFOLIO:
                    CurrentPortfolioInput(splitInput);
                    break;
                case CALCULATE_OVERLAP:
                    CalculateOverlapInput(splitInput);
                    break;
                case ADD_STOCK:
                    AddStockInput(splitInput);
                    break;
                default:
                    Console.WriteLine($"{command} is not a valid input.");
                    break;
            }
        }

        private void CurrentPortfolioInput(List<string> _Input)
        {
            if (!_Input.Any())
            {
                Console.WriteLine($"{CURRENT_PORTFOLIO} must include at least one argument.");
            }
            else
            {
                Portfolio.GetCurrentPortfolio( _Input, AvailableFunds);
            }
        }

        private void CalculateOverlapInput(List<string> _Input)
        {
            if (_Input.Count != 1)
            {
                Console.WriteLine($"{CALCULATE_OVERLAP} can only recieve one argument.");
            }
            else
            {
                Portfolio.CalculateOverlap(_Input.FirstOrDefault(), AvailableFunds);
            }
        }

        private void AddStockInput(List<string> _Input)
        {
            if (_Input.Count < 2)
            {
                Console.WriteLine($"{ADD_STOCK} must include a Fund name and a Stock name.");
            }
            else
            {
                string fundName = _Input[0];
                _Input.Remove(fundName);
                var fund = AvailableFunds.GetFundByName(fundName);
                fund?.AddStock(string.Join(" ", _Input));
            }
        }
    }
}
