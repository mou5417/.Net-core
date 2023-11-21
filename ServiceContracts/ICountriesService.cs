using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulation 
    /// Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest">object to add</param>
        /// <returns>Returns the country object after adding it(including newly generated country id)</returns>
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
        /// <summary>
        /// Returns a country object based on the given country id
        /// </summary>
        /// <param name="CountryID">CountryID (guid) to search </param>
        /// <returns>Matching country as CountryResponse object</returns>
        CountryResponse? GetCountryByCountryID(Guid? CountryID);
        
    }
}