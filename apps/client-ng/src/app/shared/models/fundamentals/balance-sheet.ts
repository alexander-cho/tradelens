export type BalanceSheet = {
  symbol: string;
  periodData: BalanceSheetPeriod[];
}

export type BalanceSheetPeriod = {
  fiscalYear: string;
  period: string;
  totalAssets: number;
}
