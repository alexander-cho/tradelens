export interface OptionsChain {
  options: FullOptionsChain;
}

export interface FullOptionsChain {
  option: StrikePriceData[];
}

export interface StrikePriceData {
  symbol: string;
  description?: string;
  underlying?: string;
  strike: number;
  volume: number;
  openInterest: number;
  expirationDate?: string;
  optionType?: string;
  last?: number;
  bid?: number;
  ask?: number;
  greeks?: Greeks;
}

export interface Greeks {
  delta: number;
  gamma: number;
  theta: number;
  vega: number;
  rho: number;
  phi: number;
  smvVol: number;
  updatedAt: string;
}
