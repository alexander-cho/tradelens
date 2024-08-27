import pytest
from unittest.mock import patch

from pandas import Timestamp


@pytest.mark.parametrize("ipo_data, earnings_calendar, expected_status, expected_content", [
    (
        [
            ['symbol', 'name', 'ipoDate', 'priceRangeLow', 'priceRangeHigh', 'currency', 'exchange'],
            ['SOFI', 'SoFi Technologies, Inc.', '10', '11', 'USD', 'NASDAQ'],
            ['NET', 'Cloudflare, Inc.', '15', '18', 'USD', 'NYSE'],
            ['PLTR', 'Palantir Technologies, Inc.', '9', '11', 'USD', 'NYSE'],
        ],
        [
            {'date': '2024-06-10', 'epsActual': -0.07, 'epsEstimate': -0.0778, 'hour': 'bmo', 'quarter': 2, 'revenueActual': 22420000, 'revenueEstimate': 21778275, 'symbol': 'FCEL', 'year': 2024},
            {'date': '2024-06-12', 'epsActual': None, 'epsEstimate': 0.051, 'hour': 'bmo', 'quarter': 2, 'revenueActual': None, 'revenueEstimate': 4632840, 'symbol': 'CODA', 'year': 2024}
        ],
        200,
        [
            b'SoFi Technologies, Inc.',
            b'Cloudflare, Inc.',
            b'Palantir Technologies, Inc.',
            b'FCEL',
            b'CODA'
        ]
    ),
    (
        [],
        [],
        200,
        [
            b'No IPOs found',
            b'No earnings data available'
        ]
    )
])
@patch('modules.providers.finnhub_.Finnhub.get_earnings_calendar')
def test_earnings_ipos(
    mock_get_ipos_data,
    mock_get_earnings_calendar,
    client,
    ipo_data,
    earnings_calendar,
    expected_status,
    expected_content
) -> None:
    # Mock the API responses
    mock_get_ipos_data.return_value = ipo_data
    mock_get_earnings_calendar.return_value = earnings_calendar

    response = client.get('/earnings-ipos')

    assert response.status_code == expected_status

    for content in expected_content:
        assert content in response.data


@pytest.mark.parametrize("quarterly_gdp_data, cpi_data, expected_status, expected_content", [
    (
        [
            {
                Timestamp('2023-07-01 00:00:00'): 27610.128,
                Timestamp('2023-10-01 00:00:00'): 27956.998,
                Timestamp('2024-01-01 00:00:00'): 28255.928
            },
            {
                Timestamp('2024-01-01 00:00:00'): 3.0908849999999997,
                Timestamp('2024-02-01 00:00:00'): 3.153171,
                Timestamp('2024-03-01 00:00:00'): 3.477385
            },
            200,
            [
                b'GDP',
                b'CPI',
            ]
        ]
    )
])
@patch('modules.providers.federalreserve.FederalReserve.get_quarterly_gdp')
@patch('modules.providers.federalreserve.FederalReserve.get_cpi')
def test_macro(
    mock_cpi_data,
    mock_quarterly_gdp_data,
    client,
    cpi_data,
    quarterly_gdp_data,
    expected_status,
    expected_content
) -> None:
    mock_cpi_data.return_value = cpi_data
    mock_quarterly_gdp_data.return_value = quarterly_gdp_data

    response = client.get('/macro')

    assert response.status_code == expected_status

    for content in expected_content:
        assert content in response.data
