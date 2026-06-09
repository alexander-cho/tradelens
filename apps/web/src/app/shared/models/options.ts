export type ExpiryData = {
  expirations: FullExpiryList;
}

export type FullExpiryList = {
  date: string[];
}


export type CallsAndPutsCashSums = {
  callCashSums: CashSumAtPrice[];
  putCashSums: CashSumAtPrice[];
  totalCashSums: CashSumAtPrice[];
  maxPainValue: number;
}

type CashSumAtPrice = {
  price: number;
  totalCashValue: number;
}
