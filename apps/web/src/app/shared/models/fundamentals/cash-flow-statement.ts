export type CashFlowStatement = {
  symbol: string;
  periodData: CashFlowStatementPeriod[];
}

export type CashFlowStatementPeriod = {
  fiscalYear: string;
  period: string;
  stockBasedCompensation: number;
  freeCashFlow: number;
}
