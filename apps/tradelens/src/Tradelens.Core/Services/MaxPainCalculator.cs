using Tradelens.Core.Models;

namespace Tradelens.Core.Services;

public static class MaxPainCalculator
{
    public static double CalculateMaxPain(OptionsChainModel optionsChainModel)
    {
        List<CashSumAtPrice> totalSums = CalculateCashValuesTotal(optionsChainModel);
        double maxPainValue = totalSums.Min(x => x.TotalCashValue);
        double maxPainStrike = totalSums.First(x => x.TotalCashValue == maxPainValue).Price;
        return maxPainStrike;
    }

    // use same domain/model for adding calls and puts sums
    public static List<CashSumAtPrice> CalculateCashValuesTotal(OptionsChainModel optionsChainModel)
    {
        List<CashSumAtPrice> callSums = CalculateCallCashValues(optionsChainModel);
        List<CashSumAtPrice> putSums = CalculatePutCashValues(optionsChainModel);

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

    public static List<CashSumAtPrice> CalculateCallCashValues(OptionsChainModel optionsChainModel)
    {
        List<StrikePriceData> options = optionsChainModel.OptionsChain;
        HashSet<float> strikePrices = GetStrikePrices(optionsChainModel);

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

    public static List<CashSumAtPrice> CalculatePutCashValues(OptionsChainModel optionsChainModel)
    {
        List<StrikePriceData> options = optionsChainModel.OptionsChain;
        HashSet<float> strikePrices = GetStrikePrices(optionsChainModel);

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

    private static HashSet<float> GetStrikePrices(OptionsChainModel optionsChainModel)
    {
        List<StrikePriceData> options = optionsChainModel.OptionsChain;

        HashSet<float> strikePrices = new();

        for (int i = 0; i < options.Count; i++)
        {
            strikePrices.Add(options[i].Strike);
        }

        return strikePrices;
    }
}