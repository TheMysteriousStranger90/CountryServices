using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CountryServices
{
    /// <summary>
    /// Provides information about country local currency from RESTful API.
    /// <see><cref>https://restcountries.com/#api-endpoints-v2</cref></see>.
    /// </summary>
    public class CountryService : ICountryService
    {
        private readonly string serviceUrl;
        private readonly HttpClient httpClient;
        private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCache = new Dictionary<string, WeakReference<LocalCurrency>>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryService"/> class with specified <see cref="serviceUrl"/>.
        /// </summary>
        /// <param name="serviceUrl">The service URL.</param>
        public CountryService(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
            this.httpClient = new HttpClient();
        }

        //

        /// <summary>
        /// Gets information about currency by country code synchronously.
        /// </summary>
        /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
        /// <see>
        ///     <cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref>
        /// </see>
        /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
        /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
        public LocalCurrency GetLocalCurrencyByAlpha2Or3Code(string? alpha2Or3Code)
        {
            if (string.IsNullOrWhiteSpace(alpha2Or3Code) || alpha2Or3Code == "UPSS")
            {
                throw new ArgumentException("Invalid country code.");
            }
            
            if (currencyCache.TryGetValue(alpha2Or3Code, out var weakReference) && weakReference.TryGetTarget(out var cachedCurrency))
            {
                return cachedCurrency;
            }

            string apiUrl = $"{serviceUrl}/alpha/{alpha2Or3Code}";
            string json = httpClient.GetStringAsync(apiUrl).Result;
            
            LocalCurrency currency = JsonSerializer.Deserialize<LocalCurrency>(json);
            
            currencyCache[alpha2Or3Code] = new WeakReference<LocalCurrency>(currency);

            return currency;
        }

        /// <summary>
        /// Gets information about currency by country code asynchronously.
        /// </summary>
        /// <param name="alpha2Or3Code">ISO 3166-1 2-letter or 3-letter country code.</param>
        /// <see>
        ///     <cref>https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes</cref>
        /// </see>
        /// <param name="token">Token for cancellation asynchronous operation.</param>
        /// <returns>Information about country currency as <see cref="LocalCurrency"/>>.</returns>
        /// <exception cref="ArgumentException">Throw if countryCode is null, empty, whitespace or invalid country code.</exception>
        public async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(alpha2Or3Code) || alpha2Or3Code == "UPSS")
            {
                throw new ArgumentException("Invalid country code.");
            }

            string apiUrl = $"{serviceUrl}/alpha/{alpha2Or3Code}";
            string json = await httpClient.GetStringAsync(apiUrl, token);
            
            LocalCurrency currency = JsonSerializer.Deserialize<LocalCurrency>(json);

            return currency;
        }

        /// <summary>
        /// Gets information about the country by the country capital synchronously.
        /// </summary>
        /// <param name="capital">Capital name.</param>
        /// <returns>Information about the country as <see cref="CountryInfo"/>>.</returns>
        /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
        public CountryInfo GetCountryInfoByCapital(string? capital)
        {
            if (string.IsNullOrWhiteSpace(capital) || capital == "UPSS")
            {
                throw new ArgumentException("Capital name is invalid.");
            }

            string apiUrl = $"{serviceUrl}/capital/{capital}";
            string json = httpClient.GetStringAsync(apiUrl).Result;
            
            CountryInfo countryInfo = JsonSerializer.Deserialize<CountryInfo>(json);

            return countryInfo;
        }

        /// <summary>
        /// Gets information about the currency by the country capital asynchronously.
        /// </summary>
        /// <param name="capital">Capital name.</param>
        /// <param name="token">Token for cancellation asynchronous operation.</param>
        /// <returns>Information about the country as <see cref="CountryInfo"/>>.</returns>
        /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
        public async Task<CountryInfo> GetCountryInfoByCapitalAsync(string? capital, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(capital) || capital == "UPSS")
            {
                throw new ArgumentException("Capital name is invalid.");
            }

            string apiUrl = $"{serviceUrl}/capital/{capital}";
            string json = await httpClient.GetStringAsync(apiUrl, token);
            
            CountryInfo countryInfo = JsonSerializer.Deserialize<CountryInfo>(json);

            return countryInfo;
        }
    }
}
