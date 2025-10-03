using Core.Models;

namespace Core.Services;

public static class MaxPainCalculator
{
    public static double CalculateMaxPain(OptionsChain optionsChain)
    {
        List<CashSumAtPrice> totalSums = CalculateCashValuesTotal(optionsChain);
        double maxPainValue = totalSums.Min(x => x.TotalCashValue);
        double maxPainStrike = totalSums.First(x => x.TotalCashValue == maxPainValue).Price;
        return maxPainStrike;
    }

    // use same DTO for adding calls and puts sums
    public static List<CashSumAtPrice> CalculateCashValuesTotal(OptionsChain optionsChain)
    {
        List<CashSumAtPrice> callSums = CalculateCallCashValues(optionsChain);
        List<CashSumAtPrice> putSums = CalculatePutCashValues(optionsChain);

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

    public static List<CashSumAtPrice> CalculateCallCashValues(OptionsChain optionsChain)
    {
        List<StrikePriceData> options = optionsChain.Options.Option;
        HashSet<float> strikePrices = GetStrikePrices(optionsChain);

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

    public static List<CashSumAtPrice> CalculatePutCashValues(OptionsChain optionsChain)
    {
        List<StrikePriceData> options = optionsChain.Options.Option;
        HashSet<float> strikePrices = GetStrikePrices(optionsChain);

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

    private static HashSet<float> GetStrikePrices(OptionsChain optionsChain)
    {
        List<StrikePriceData> options = optionsChain.Options.Option;

        HashSet<float> strikePrices = new();

        for (int i = 0; i < options.Count; i++)
        {
            strikePrices.Add(options[i].Strike);
        }

        return strikePrices;
    }
}