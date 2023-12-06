using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        /// <summary>
        /// Add a new person into the list of persons
        /// </summary>
        /// <param name="personAddReuest"></param>
        /// <returns>Return the same person details, along with newly generated PersonID</returns>
        
        PersonResponse AddPerson(PersonAddRequest? person_request);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID"></param>
        /// <returns>Return matching person object</returns>
        PersonResponse? GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Retrun all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Retruns all matching persons based on the given search field and search string</returns>
        List<PersonResponse> GetFileteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allpersons">Represents list of persons to sort</param> 
        /// <param name="sortBy">Name of the property(key),based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updat the specified person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person object after updation</returns>
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        bool DeletePerson(Guid? personID);
    }
}
