namespace Tradelens.Infrastructure.Clients.Finnhub.DTOs;

public record SecFilingDto(
    string Cik
);

//     
// {
// "accessNumber": "0001193125-20-050884",
// "symbol": "AAPL",
// "cik": "320193",
// "form": "8-K",
// "filedDate": "2020-02-27 00:00:00",
// "acceptedDate": "2020-02-27 06:14:21",
// "reportUrl": "https://www.sec.gov/ix?doc=/Archives/edgar/data/320193/000119312520050884/d865740d8k.htm",
// "filingUrl": "https://www.sec.gov/Archives/edgar/data/320193/000119312520050884/0001193125-20-050884-index.html"
// },