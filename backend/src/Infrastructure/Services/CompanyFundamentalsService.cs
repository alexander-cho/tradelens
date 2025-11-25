using Core.Constants;
using Core.Interfaces;
using Core.Models;
using Core.Models.CompanyFundamentals;
using Infrastructure.Clients.Fmp;
using Infrastructure.Clients.Polygon;
using Infrastructure.Mappers;
using Infrastructure.Mappers.CompanyFundamentals;

namespace Infrastructure.Services;

public class CompanyFundamentalsService : ICompanyFundamentalsService
{
    private readonly IPolygonClient _polygonClient;
    private readonly IFmpClient _fmpClient;

    public CompanyFundamentalsService(IPolygonClient polygonClient, IFmpClient fmpClient)
    {
        _polygonClient = polygonClient;
        _fmpClient = fmpClient;
    }

    public async Task<RelatedCompaniesModel> GetRelatedCompaniesAsync(string ticker)
    {
        var relatedCompaniesDto = await _polygonClient.GetRelatedCompaniesAsync(ticker);
        if (relatedCompaniesDto == null)
        {
            throw new InvalidOperationException($"Related companies data for {ticker} was not available");
        }

        var relatedCompanies = RelatedCompaniesMapper.ToRelatedCompaniesDomainModel(relatedCompaniesDto);

        return relatedCompanies;
    }

    public async Task<CompanyFundamentalsResponse> GetCompanyFundamentalMetricsAsync(string ticker, string period,
        List<string> metric)
    {
        List<string> methodsToCall = [];

        foreach (var financialMetric in metric)
        {
            // if the value of that metric exists in endpointsToCall => continue
            // or else add that value to endpointsToCall
            if (methodsToCall.Contains(CompanyFundamentalMetricMappings.MetricToEndpoint[financialMetric]))
            {
                continue;
            }

            methodsToCall.Add(CompanyFundamentalMetricMappings.MetricToEndpoint[financialMetric]);
        }

        // for each service to call, find the appropriate method to invoke

        Task<IncomeStatement>? incomeStatementTask = null;
        Task<BalanceSheet>? balanceSheetTask = null;
        Task<CashFlowStatement>? cashFlowStatementTask = null;

        List<Task> nonNullMethodsList = new List<Task>();

        foreach (string service in methodsToCall)
        {
            switch (service)
            {
                case "IncomeStatement":
                    incomeStatementTask = GetIncomeStatementAsync(ticker, limit: 5, period);
                    nonNullMethodsList.Add(incomeStatementTask);
                    break;
                case "BalanceSheet":
                    balanceSheetTask = GetBalanceSheetAsync(ticker, limit: 5, period);
                    nonNullMethodsList.Add(balanceSheetTask);
                    break;
                case "CashFlow":
                    cashFlowStatementTask = GetCashFlowStatementAsync(ticker, limit: 5, period);
                    nonNullMethodsList.Add(cashFlowStatementTask);
                    break;
            }
        }

        await Task.WhenAll(nonNullMethodsList);

        // now we need to get the response data

        IncomeStatement? incomeStatementData = null;
        BalanceSheet? balanceSheetData = null;
        CashFlowStatement? cashFlowData = null;

        if (incomeStatementTask != null)
        {
            incomeStatementData = incomeStatementTask.Result;
        }

        if (balanceSheetTask != null)
        {
            balanceSheetData = balanceSheetTask.Result;
        }

        if (cashFlowStatementTask != null)
        {
            cashFlowData = cashFlowStatementTask.Result;
        }

        // // null coalescing to simplify above
        // incomeStatementData = incomeStatementTask?.Result;

        // now we want to return only the fields we want instead of the whole object

        // we must add metric data and append each one to this, to send as CompanyFundamentalsResponse at the end
        List<Metric> responseToSend = [];

        foreach (string financialMetric in metric)
        {
            // for each metric we have to find which dataset to parse
            var methodType = CompanyFundamentalMetricMappings.MetricToEndpoint[financialMetric];
            switch (methodType)
            {
                case "IncomeStatement":
                    switch (financialMetric)
                    {
                        case "revenue":
                            List<ValueDataAtEachPeriod> revenueData = [];
                            if (incomeStatementData != null)
                            {
                                foreach (var i in incomeStatementData.PeriodData)
                                {
                                    revenueData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.Revenue
                                    });
                                }
                            }

                            revenueData.Reverse();

                            // start shaping the response
                            Metric revenueMetric = new Metric
                            {
                                MetricName = "revenue",
                                Data = revenueData
                            };
                            
                            responseToSend.Add(revenueMetric);
                            break;
                        case "netIncome":
                            List<ValueDataAtEachPeriod> netIncomeData = [];
                            if (incomeStatementData != null)
                            {
                                foreach (var i in incomeStatementData.PeriodData)
                                {
                                    netIncomeData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.NetIncome
                                    });
                                }
                            }

                            netIncomeData.Reverse();

                            // start shaping the response
                            Metric netIncomeMetric = new Metric
                            {
                                MetricName = "netIncome",
                                Data = netIncomeData
                            };
                            
                            responseToSend.Add(netIncomeMetric);
                            break;
                        case "grossProfit":
                            List<ValueDataAtEachPeriod> grossProfitData = [];
                            if (incomeStatementData != null)
                            {
                                foreach (var i in incomeStatementData.PeriodData)
                                {
                                    grossProfitData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.GrossProfit
                                    });
                                }
                            }

                            grossProfitData.Reverse();

                            // start shaping the response
                            Metric grossProfitMetric = new Metric
                            {
                                MetricName = "grossProfit",
                                Data = grossProfitData
                            };
                            
                            responseToSend.Add(grossProfitMetric);
                            break;
                    }
                    break;
                case "BalanceSheet":
                    switch (financialMetric)
                    {
                        case "totalAssets":
                            List<ValueDataAtEachPeriod> totalAssetsData = [];
                            if (balanceSheetData != null)
                            {
                                foreach (var i in balanceSheetData.PeriodData)
                                {
                                    totalAssetsData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.TotalAssets
                                    });
                                }
                            }

                            totalAssetsData.Reverse();

                            // start shaping the response
                            Metric totalAssetsMetric = new Metric
                            {
                                MetricName = "totalAssets",
                                Data = totalAssetsData
                            };
                            
                            responseToSend.Add(totalAssetsMetric);
                            break;
                        case "totalLiabilities":
                            List<ValueDataAtEachPeriod> totalLiabilitiesData = [];
                            if (balanceSheetData != null)
                            {
                                foreach (var i in balanceSheetData.PeriodData)
                                {
                                    totalLiabilitiesData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.TotalLiabilities
                                    });
                                }
                            }

                            totalLiabilitiesData.Reverse();

                            // start shaping the response
                            Metric totalLiabilitiesMetric = new Metric
                            {
                                MetricName = "totalLiabilities",
                                Data = totalLiabilitiesData
                            };
                            
                            responseToSend.Add(totalLiabilitiesMetric);
                            break;
                        case "totalStockholdersEquity":
                            List<ValueDataAtEachPeriod> totalStockholdersEquityData = [];
                            if (balanceSheetData != null)
                            {
                                foreach (var i in balanceSheetData.PeriodData)
                                {
                                    totalStockholdersEquityData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.TotalStockholdersEquity
                                    });
                                }
                            }

                            totalStockholdersEquityData.Reverse();

                            // start shaping the response
                            Metric totalStockholdersEquityMetric = new Metric
                            {
                                MetricName = "totalStockholdersEquity",
                                Data = totalStockholdersEquityData
                            };
                            
                            responseToSend.Add(totalStockholdersEquityMetric);
                            break;
                    }
                    break;
                case "CashFlow":
                    switch (financialMetric)
                    {
                        case "freeCashFlow":
                            List<ValueDataAtEachPeriod> freeCashFlowData = [];
                            if (cashFlowData != null)
                            {
                                foreach (var i in cashFlowData.PeriodData)
                                {
                                    freeCashFlowData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.FreeCashFlow
                                    });
                                }
                            }

                            freeCashFlowData.Reverse();

                            // start shaping the response
                            Metric freeCashFlowMetric = new Metric
                            {
                                MetricName = "freeCashFlow",
                                Data = freeCashFlowData
                            };
                            
                            responseToSend.Add(freeCashFlowMetric);
                            break;
                        case "stockBasedCompensation":
                            List<ValueDataAtEachPeriod> stockBasedCompensationData= [];
                            if (cashFlowData != null)
                            {
                                foreach (var i in cashFlowData.PeriodData)
                                {
                                    stockBasedCompensationData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.StockBasedCompensation
                                    });
                                }
                            }

                            stockBasedCompensationData.Reverse();

                            // start shaping the response
                            Metric stockBasedCompensationMetric = new Metric
                            {
                                MetricName = "stockBasedCompensation",
                                Data = stockBasedCompensationData
                            };
                            
                            responseToSend.Add(stockBasedCompensationMetric);
                            break;
                        case "cashAtEndOfPeriod":
                            List<ValueDataAtEachPeriod> cashAtEndOfPeriodData= [];
                            if (cashFlowData != null)
                            {
                                foreach (var i in cashFlowData.PeriodData)
                                {
                                    cashAtEndOfPeriodData.Add(new ValueDataAtEachPeriod
                                    {
                                        Period = i.Period,
                                        FiscalYear = i.FiscalYear,
                                        Value = i.CashAtEndOfPeriod
                                    });
                                }
                            }

                            cashAtEndOfPeriodData.Reverse();

                            // start shaping the response
                            Metric cashAtEndOfPeriodMetric = new Metric
                            {
                                MetricName = "cashAtEndOfPeriod",
                                Data = cashAtEndOfPeriodData
                            };
                            
                            responseToSend.Add(cashAtEndOfPeriodMetric);
                            break;
                    }
                    break;
            }
        }

        return new CompanyFundamentalsResponse
        {
            MetricData = responseToSend
        };
    }

    public async Task<IncomeStatement> GetIncomeStatementAsync(string ticker, int limit, string period)
    {
        var incomeStatementDto = await _fmpClient.GetIncomeStatementAsync(ticker, 5, period);
        var incomeStatements = IncomeStatementMapper.ToIncomeStatement(incomeStatementDto, ticker);

        return incomeStatements;
    }

    public async Task<BalanceSheet> GetBalanceSheetAsync(string ticker, int limit, string period)
    {
        var balanceSheetDto = await _fmpClient.GetBalanceSheetStatementAsync(ticker, 5, period);
        var balanceSheets = BalanceSheetMapper.ToBalanceSheet(balanceSheetDto, ticker);

        return balanceSheets;
    }

    public async Task<CashFlowStatement> GetCashFlowStatementAsync(string ticker, int limit, string period)
    {
        var cashFlowStatementDto = await _fmpClient.GetCashFlowStatementAsync(ticker, 5, period);
        var cashFlowStatements = CashFlowStatementMapper.ToCashFlowStatement(cashFlowStatementDto, ticker);

        return cashFlowStatements;
    }

    public async Task<IEnumerable<CompanyProfile>> GetCompanyProfileDataAsync(string symbol)
    {
        var companyProfileDto = await _fmpClient.GetCompanyProfileDataAsync(symbol);

        if (companyProfileDto == null)
        {
            throw new InvalidOperationException($"Company profile data for {symbol} was not available");
        }
        
        var companyProfile = CompanyProfileMapper.ToCompanyProfile(companyProfileDto);

        return companyProfile;
    }

    public async Task<IEnumerable<KeyMetricsTtm>> GetKeyMetricsTtmAsync(string symbol)
    {
        var keyMetricsTtmDto = await _fmpClient.GetKeyMetricsTtmAsync(symbol);

        if (keyMetricsTtmDto == null)
        {
            throw new InvalidOperationException($"Key metrics TTM data for {symbol} was not available");
        }
        
        var keyMetricsTtm = KeyMetricsMapper.ToKeyMetricsTtm(keyMetricsTtmDto);

        return keyMetricsTtm;
    }

    public async Task<IEnumerable<FinancialRatiosTtm>> GetFinancialRatiosTtmAsync(string symbol)
    {
        var financialRatiosTtmDto = await _fmpClient.GetFinancialRatiosTtmAsync(symbol);

        if (financialRatiosTtmDto == null)
        {
            throw new InvalidOperationException($"Company profile data for {symbol} was not available");
        }
        
        var financialRatiosTtm = FinancialRatiosMapper.ToFinancialRatiosTtm(financialRatiosTtmDto);

        return financialRatiosTtm;
    }
}