using ServiceContracts.DTO;
using System;
using System.Collections.Generic;


namespace ServiceContracts
{
    public interface IPersonsService
    {
        /// <summary>
        /// Add a new person into the list of persons
        /// </summary>
        /// <param name="personAddReuest"></param>
        /// <returns>Return the same person details, along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddReuest? personAddReuest);
        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();
    }
}
