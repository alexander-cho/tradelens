export interface OptionsChain {
  optionsChain: StrikePriceData[];
}

export interface StrikePriceData {
  symbol: string;
  description?: string;
  underlying?: string;
  strike: number;
  volume: number;
  openInterest: number;
  activity: number;
  expirationDate?: string;
  optionType?: string;
  last?: number;
  bid?: number;
  ask?: number;
  greeks?: Greeks;
  impliedVolatility?: ImpliedVolatility;
}

export interface Greeks {
  delta: number;
  gamma: number;
  theta: number;
  vega: number;
  rho: number;
  phi: number;
  updatedAt: string;
}

export interface ImpliedVolatility {
  bidIv: number;
  midIv: number;
  askIv: number;
  smvVol: number;
  updatedAt: string;
}
