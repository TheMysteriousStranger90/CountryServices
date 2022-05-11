﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CountryServices
{
    /// <summary>
    /// Provides information about country local currency from RESTful API
    /// <see>
    ///     <cref>https://restcountries.com/#api-endpoints-v2</cref>
    /// </see>
    /// </summary>
    public class CountryService : ICountryService
    {
        private const string? serviceUrl = "https://restcountries.com/v2";

        private readonly Dictionary<string, WeakReference<LocalCurrency>> currencyCountries =
            new Dictionary<string, WeakReference<LocalCurrency>>();
        //TODO: Add code if needed here.

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
            //TODO: Use WebClient class.
            throw new NotImplementedException();
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
        public async Task<LocalCurrency> GetLocalCurrencyByAlpha2Or3CodeAsync(string? alpha2Or3Code,
            CancellationToken token)
        {
            //TODO: Use HttpClient class. Notice the difference from WebClient class. In the future, in a similar situation, use only HttpClient.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets information about the country by the country capital synchronously.
        /// </summary>
        /// <param name="capital">Capital name.</param>
        /// <returns>Information about the country as <see cref="CountryInfo"/>>.</returns>
        /// <exception cref="ArgumentException">Throw if the capital name is null, empty, whitespace or nonexistent.</exception>
        public CountryInfo GetCountryInfoByCapital(string? capital)
        {
            //TODO: Use WebClient class.
            throw new NotImplementedException();
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
            //TODO: Use HttpClient class. Notice the difference from WebClient class. In the future, in a similar situation, use only HttpClient.
            throw new NotImplementedException();
        }
    }
}