export type ExpirationDates = {
  expiryDate: string[];
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
