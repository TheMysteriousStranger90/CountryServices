﻿using CountryServices.Tests.Comparers;
using NUnit.Framework;

namespace CountryServices.Tests
{
    public class CountryCurrencyServiceTests
    {
        private const string ServiceUrl = "https://restcountries.com/v2";
        private CountryService countryService;

        [OneTimeSetUp]
        public void Setup()
        {
            this.countryService = new CountryService(ServiceUrl);
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCasesForCurrency))]
        public void GetLocalCurrencyByAlpha2Or3Code_ValidCountryCode(string countryCode, LocalCurrency expected)
        {
            var comparer = new LocalCurrencyComparer();
            var actual = this.countryService.GetLocalCurrencyByAlpha2Or3Code(countryCode);
            Assert.IsTrue(comparer.Equals(expected, actual));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("UPSS")]
        public void GetLocalCurrencyByAlpha2Or3Code_InvalidCountryCode_ThrowArgumentException(string? countryCode)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new CountryService(ServiceUrl).GetLocalCurrencyByAlpha2Or3Code(countryCode);
            });
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCasesForCurrency))]
        public async Task GetLocalCurrencyByAlpha2Or3CodeAsync_ValidCountryCode(string countryCode, LocalCurrency? expected)
        {
            var comparer = new LocalCurrencyComparer();
            var actual = await this.countryService.GetLocalCurrencyByAlpha2Or3CodeAsync(countryCode, CancellationToken.None);
            Assert.IsTrue(comparer.Equals(expected, actual));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("UPSS")]
        public void GetLocalCurrencyByAlpha2Or3CodeAsync_InvalidCountryCode_ThrowArgumentException(string? countryCode)
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                _ = await new CountryService(ServiceUrl).GetLocalCurrencyByAlpha2Or3CodeAsync(countryCode, CancellationToken.None);
            });
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCasesForCountryInfo))]
        public void GetCountryInfoByCapital_ValidCapitalName(string capitalName, CountryInfo expected)
        {
            var comparer = new CountryInfoComparer();
            var actual = this.countryService.GetCountryInfoByCapital(capitalName);
            Assert.IsTrue(comparer.Equals(expected, actual));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("UPSS")]
        public void GetCountryInfoByCapital_InvalidCapitalName_ThrowArgumentException(string? capitalName)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _ = new CountryService(ServiceUrl).GetCountryInfoByCapital(capitalName);
            });
        }

        [TestCaseSource(typeof(TestCasesData), nameof(TestCasesData.TestCasesForCountryInfo))]
        public async Task GetCountryInfoByCapitalAsync_ValidCapitalName(string capitalName, CountryInfo expected)
        {
            var comparer = new CountryInfoComparer();
            var actual = await this.countryService.GetCountryInfoByCapitalAsync(capitalName, CancellationToken.None);
            Assert.IsTrue(comparer.Equals(expected, actual));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("UPSS")]
        public void GetCountryInfoByCapitalAsync_InvalidCapitalName_ThrowArgumentException(string? capitalName)
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                _ = await new CountryService(ServiceUrl).GetCountryInfoByCapitalAsync(capitalName, CancellationToken.None);
            });
        }
    }
}
