using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countryService;
        //constructor 
        public PersonsService()
        {
            _persons = new List<Person>();
            _countryService = new CountriesService();
        }

        private PersonResponse ConverPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countryService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddReuest? personAddRequest)
        {
            //personAddRequest = null?
           if (personAddRequest == null) { throw new ArgumentNullException(nameof(personAddRequest)); }
           //personName = null?
           if (string.IsNullOrEmpty(personAddRequest.PersonName) || string.IsNullOrEmpty(personAddRequest.Email))
            {
                throw new ArgumentNullException("PersonName can't be blank");
            }
            var checker = new EmailAddressAttribute();
           if(!checker.IsValid(personAddRequest.Email)) 
            
            { throw new ArgumentException("Email address isn't valid"); }
            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(person);

            //convert the Person object into PersonResponse type
            return ConverPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
