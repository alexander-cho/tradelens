using Core.DTOs.Options;
using Core.DTOs.Tradier;

namespace Infrastructure.Helpers;

public static class CashValueHelpers
{
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

    public static PutCashSumData CalculatePutCashValues(OptionsData optionsData)
    {
        List<StrikePriceData> options = optionsData.Options.Option;

        PutCashSumData putCashSumData = new PutCashSumData
        {
            PutCashSumList = new List<CashSumAtStrike>()
        };

        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].OptionType == "put")
            {
                double strike = options[i].Strike;
                int openInterest = options[i].OpenInterest;
                double putCashSum = 0;

                for (int j = 0; j < options.Count; j++)
                {
                    double putCashValue;
                    double hypotheticalClose = options[j].Strike;

                    if ((strike - hypotheticalClose) * openInterest * 100 < 0)
                    {
                        putCashValue = 0;
                    }
                    else
                    {
                        putCashValue = (hypotheticalClose - strike) * openInterest * 100;
                    }

                    putCashSum += putCashValue;
                }

                putCashSumData.PutCashSumList.Add(new CashSumAtStrike
                {
                    Strike = strike,
                    CashSum = putCashSum
                });
            }
        }

        return putCashSumData;
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

    public static double CalculateMaxPain()
    {
        return 0;
    }
}