using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
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

    try
    {
        string apiUrl = $"{serviceUrl}/alpha/{Uri.EscapeDataString(alpha2Or3Code)}";
        string json = httpClient.GetStringAsync(apiUrl).Result;

        // Parse the JSON response into an object.
        var jsonObject = JsonDocument.Parse(json).RootElement;

        string name = jsonObject.GetProperty("name").GetString();

        // Access the currencies property as an object.
        var currencies = jsonObject.GetProperty("currencies");

        string currencyCode = currencies.EnumerateObject()
            .SelectMany(prop => prop.Value.EnumerateObject())
            .Where(prop => prop.Name.Equals("code"))
            .Select(prop => prop.Value.GetString())
            .FirstOrDefault();

        string currencySymbol = currencies.EnumerateObject()
            .SelectMany(prop => prop.Value.EnumerateObject())
            .Where(prop => prop.Name.Equals("symbol"))
            .Select(prop => prop.Value.GetString())
            .FirstOrDefault();

        LocalCurrency localCurrency = new LocalCurrency
        {
            CountryName = name,
            CurrencyCode = currencyCode,
            CurrencySymbol = currencySymbol
        };

        // Cache the currency
        currencyCache[alpha2Or3Code] = new WeakReference<LocalCurrency>(localCurrency);

        return localCurrency;
    }
    catch (HttpRequestException ex)
    {
        throw new ApplicationException("Failed to retrieve country info. Check your network connection.", ex);
    }
    catch (JsonException ex)
    {
        throw new JsonException("Failed to parse JSON response.", ex);
    }
    catch (Exception ex)
    {
        throw new ApplicationException("An error occurred while retrieving country info.", ex);
    }
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
                throw new ArgumentException("Invalid capital name.");
            }

            try
            {
                string apiUrl = $"{serviceUrl}/capital/{Uri.EscapeDataString(capital)}";
                string json = httpClient.GetStringAsync(apiUrl).Result;

                // Parse the JSON response into an array of JSON elements.
                JsonDocument doc = JsonDocument.Parse(json);

                // Assuming the JSON response is an array, loop through the elements.
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    // Extract the properties you need from each element.
                    string name = element.GetProperty("name").GetString();
                    string capitalName = element.GetProperty("capital").GetString();
                    double area = element.GetProperty("area").GetDouble();
                    int population = element.GetProperty("population").GetInt32();
                    string flag = element.GetProperty("flag").GetString();

                    // Create and return a CountryInfo object.
                    CountryInfo countryInfo = new CountryInfo
                    {
                        Name = name,
                        CapitalName = capitalName,
                        Area = area,
                        Population = population,
                        Flag = flag
                    };

                    return countryInfo;
                }

                throw new JsonException("No valid country information found in the JSON array.");
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to retrieve country info. Check your network connection.", ex);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to parse JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving country info.", ex);
            }
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
                throw new ArgumentException("Invalid capital name.");
            }

            try
            {
                string apiUrl = $"{serviceUrl}/capital/{Uri.EscapeDataString(capital)}";
                string json = await httpClient.GetStringAsync(apiUrl);

                // Parse the JSON response into an array of JSON elements.
                JsonDocument doc = JsonDocument.Parse(json);

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    // Extract the properties you need from each element.
                    string name = element.GetProperty("name").GetString();
                    string capitalName = element.GetProperty("capital").GetString();
                    double area = element.GetProperty("area").GetDouble();
                    int population = element.GetProperty("population").GetInt32();
                    string flag = element.GetProperty("flag").GetString();

                    CountryInfo countryInfo = new CountryInfo
                    {
                        Name = name,
                        CapitalName = capitalName,
                        Area = area,
                        Population = population,
                        Flag = flag
                    };

                    return countryInfo;
                }

                throw new JsonException("No valid country information found in the JSON array.");
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("Failed to retrieve country info. Check your network connection.", ex);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to parse JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving country info.", ex);
            }
        }

    }
}
