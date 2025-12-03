export type SeriesObservations = {
  id: string;
  title: string;
  frequency: string;
  units: string;
  observations: FredDataPoint[];
}

export type FredDataPoint = {
  date: string;
  // value of the observation. Nullable because the API returns "."
  // multiply by 1_000_000 since it's in millions?
  value: number | null;
  realtimeStart: string;
  realtimeEnd: string;
}
