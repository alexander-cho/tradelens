using Core.DTOs.Options;
using Core.DTOs.Tradier;

namespace Infrastructure.Helpers;

public static class MaxPainHelpers
{
    public static double CalculateMaxPain(OptionsData optionsData)
    {
        List<CashSumAtPrice> totalSums = CalculateCashValuesTotal(optionsData);
        double maxPainValue = totalSums.Min(x => x.TotalCashValue);
        double maxPainStrike = totalSums.First(x => x.TotalCashValue == maxPainValue).Price;
        return maxPainStrike;
    }

    // use same DTO for adding calls and puts sums
    public static List<CashSumAtPrice> CalculateCashValuesTotal(OptionsData optionsData)
    {
        List<CashSumAtPrice> callSums = CalculateCallCashValues(optionsData);
        List<CashSumAtPrice> putSums = CalculatePutCashValues(optionsData);

        var totalAtEachPrice = new List<CashSumAtPrice>();

        for (int i = 0; i < callSums.Count; i++)
        {
            totalAtEachPrice.Add(new CashSumAtPrice
            {
                Price = callSums[i].Price,
                TotalCashValue = callSums[i].TotalCashValue + putSums[i].TotalCashValue
            });
        }

        return totalAtEachPrice;
    }

    public static List<CashSumAtPrice> CalculateCallCashValues(OptionsData optionsData)
    {
        List<StrikePriceData> options = optionsData.Options.Option;
        HashSet<float> strikePrices = GetStrikePrices(optionsData);

        var cashAtEachPrice = new List<CashSumAtPrice>();

        // each available strike price represents a hypothetical close, to calculate intrinsic value
        foreach (var hypotheticalClose in strikePrices)
        {
            double totalCallValue = 0;

            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].OptionType == "call")
                {
                    double strike = options[i].Strike;
                    int openInterest = options[i].OpenInterest;

                    double intrinsicValue;

                    // could potentially replace this with more concise max() function
                    if ((hypotheticalClose - strike) * openInterest * 100 < 0)
                    {
                        intrinsicValue = 0;
                    }
                    else
                    {
                        intrinsicValue = (hypotheticalClose - strike) * openInterest * 100;
                    }

                    totalCallValue += intrinsicValue;
                }
            }

            cashAtEachPrice.Add(new CashSumAtPrice
            {
                Price = hypotheticalClose,
                TotalCashValue = totalCallValue
            });
        }

        return cashAtEachPrice;
    }

    public static List<CashSumAtPrice> CalculatePutCashValues(OptionsData optionsData)
    {
        List<StrikePriceData> options = optionsData.Options.Option;
        HashSet<float> strikePrices = GetStrikePrices(optionsData);

        var cashAtEachPrice = new List<CashSumAtPrice>();

        foreach (var hypotheticalClose in strikePrices)
        {
            double totalPutValue = 0;

            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].OptionType == "put")
                {
                    double strike = options[i].Strike;
                    int openInterest = options[i].OpenInterest;

                    double intrinsicValue;

                    if ((strike - hypotheticalClose) * openInterest * 100 < 0)
                    {
                        intrinsicValue = 0;
                    }
                    else
                    {
                        intrinsicValue = (strike - hypotheticalClose) * openInterest * 100;
                    }

                    totalPutValue += intrinsicValue;
                }
            }

            cashAtEachPrice.Add(new CashSumAtPrice
            {
                Price = hypotheticalClose,
                TotalCashValue = totalPutValue
            });
        }

        return cashAtEachPrice;
    }

    private static HashSet<float> GetStrikePrices(OptionsData optionsData)
    {
        List<StrikePriceData> options = optionsData.Options.Option;

        HashSet<float> strikePrices = new();

        for (int i = 0; i < options.Count; i++)
        {
            strikePrices.Add(options[i].Strike);
        }

        return strikePrices;
    }
}