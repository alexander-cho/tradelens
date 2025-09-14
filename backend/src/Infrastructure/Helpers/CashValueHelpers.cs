using Core.DTOs.Tradier;

namespace Infrastructure.Helpers;

public static class CashValueHelpers
{
    public static List<double> CalculateCallCashValues(OptionsData optionsData)
    {
        List<StrikePriceData> options = optionsData.Options.Option;
        List<double> callCashValues = new List<double>();

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].OptionType == "call")
            {
                double strike = options[i].Strike;
                int openInterest = options[i].OpenInterest;
                double callCashSum = 0;
                
                // each available strike price represents a hypothetical close, to calculate intrinsic value
                for (int j = 0; j < options.Count; j++)
                {
                    double callCashValue;
                    double hypotheticalClose = options[j].Strike;

                    // could potentially replace this with more concise max() function
                    if ((hypotheticalClose - strike) * openInterest * 100 < 0)
                    {
                        callCashValue = 0;
                    }
                    else
                    {
                        callCashValue = (hypotheticalClose - strike) * openInterest * 100;
                    }

                    callCashSum += callCashValue;
                }

                callCashValues.Add(callCashSum);
            }
        }
        
        // have to return this as a dict with each strike and corresponding cash value
        return callCashValues;
    }

    public static List<int> CalculatePutCashValues()
    {
        return [];
    }

    public static double CalculateMaxPain()
    {
        return 0;
    }
}