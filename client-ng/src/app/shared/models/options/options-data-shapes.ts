export interface OptionChainRow {
  strike: number;
  call?: {
    last?: number;
    bid?: number;
    ask?: number;
    volume: number;
    openInterest: number;
  };
  put?: {
    last?: number;
    bid?: number;
    ask?: number;
    volume: number;
    openInterest: number;
  };
}

export interface OptionMetricData {
  metricType: string;
  strike: number;
  optionType: string | undefined;
  data: string | number | Object | undefined;
}
