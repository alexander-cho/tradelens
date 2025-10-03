namespace Core.Models;

public class MaxPain
{
    
}

public class CallsAndPutsCashSums
{
    public required List<CashSumAtPrice> CallCashSums { get; set; }
    public required List<CashSumAtPrice> PutCashSums { get; set; }
    public required List<CashSumAtPrice> TotalCashSums { get; set; }
    public required double MaxPainValue { get; set; }
}

public class CashSumAtPrice
{
    public float Price { get; set; }
    public double TotalCashValue { get; set; }
}

public class CallCashSumData
{
    public required List<CashSumAtStrike> CallCashSumList { get; set; }
}

public class PutCashSumData
{
    public required List<CashSumAtStrike> PutCashSumList { get; set; }
}

public class CashSumAtStrike
{
    public double Strike { get; set; }
    public double CashSum { get; set; }
}