export type ExpiryData = {
  expirations: FullExpiryList;
}

export type FullExpiryList = {
  date: string[];
}


export type MaxPainData = {
  callCashSums: CashSumAtStrike[];
  putCashSums: CashSumAtStrike[];
  totalCashSums: CashSumAtStrike[];
  maxPainValue: number;
}

type CashSumAtStrike = {
  price: number;
  totalCashValue: number;
}
