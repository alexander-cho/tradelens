export type IncomeStatement = {
  symbol: string;
  periodData: IncomeStatementPeriod[];
}

export type IncomeStatementPeriod = {
  fiscalYear: string;
  period: string;
  revenue: number;
  grossProfit: number;
  netIncome: number;
}
