using System;
using System.Collections.Generic;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ServiceContracts.Enums;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countryService;
        //constructor 
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countryService = new CountriesService();
            if(initialize) 
            {
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("6f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                    PersonName = "John Doe",
                    Email = "john.doe@example.com",
                    DateOfBirth = DateTime.Parse("1990-05-15"),
                    Gender = "Male",
                    Address = "123 Main Street, Cityville",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("9c6d5c5c-4f47-48e7-ba6f-dcab69d6fc12")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("7f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                    PersonName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    DateOfBirth = DateTime.Parse("1985-08-22"),
                    Gender = "Female",
                    Address = "456 Oak Avenue, Townsville",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("8d6d5c5c-4f47-48e7-ba6f-dcab69d6fc12")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("8f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                    PersonName = "Sam Johnson",
                    Email = "sam.johnson@example.com",
                    DateOfBirth = DateTime.Parse("1982-11-10"),
                    Gender = "Male",
                    Address = "789 Pine Street, Villagetown",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("7d6d5c5c-4f47-48e7-ba6f-dcab69d6fc12")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("9f9619ff-8b86-d011-b42d-00cf4fc964ff"),
                    PersonName = "Emily Davis",
                    Email = "emily.davis@example.com",
                    DateOfBirth = DateTime.Parse("1995-03-18"),
                    Gender = "Female",
                    Address = "101 Cedar Lane, Hamletville",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("6d6d5c5c-4f47-48e7-ba6f-dcab69d6fc12")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("af9619ff-8b86-d011-b42d-00cf4fc964ff"),
                    PersonName = "Robert Brown",
                    Email = "robert.brown@example.com",
                    DateOfBirth = DateTime.Parse("1988-07-25"),
                    Gender = "Male",
                    Address = "202 Maple Avenue, Grovetown",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("5d6d5c5c-4f47-48e7-ba6f-dcab69d6fc12")
                });


            }
        }

        private PersonResponse ConverPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countryService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //personAddRequest = null?
           if (personAddRequest == null) { throw new ArgumentNullException(nameof(personAddRequest)); }

            //Model Validation
            ValidationHelper.ModelValidation(personAddRequest);
           
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
           return _persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse GetPersonByPersonID(Guid? personID)
        {
            if(personID==null) { return null; }
            Person? person=_persons.FirstOrDefault(temp=> temp.PersonID == personID);
            if (person == null)
            {
                return null;
            }
            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFileteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;//default value

            if (string.IsNullOrEmpty(searchBy)||string.IsNullOrEmpty(searchString)) { return matchingPersons; }
     

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    !string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp =>
                    !string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth!=null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    if (searchString.ToLower() == "male")
                    {
                        matchingPersons = allPersons.Where(temp =>
                       !string.IsNullOrEmpty(temp.Gender) && temp.Gender.Equals("Male")).ToList();
                        break;
                    }
                    else
                    {
                        matchingPersons = allPersons.Where(temp =>
                       !string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                        break;
                    }

                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    !string.IsNullOrEmpty(temp.CountryName) ? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp =>
                    !string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default: matchingPersons = allPersons;break;
            }

            return matchingPersons;

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allpersons, string sortBy, SortOrderOptions sortOrder)
        {
         
            PropertyInfo? property = typeof(PersonResponse).GetProperty(sortBy);
            if (sortBy == null)
            {
                return allpersons;
            }
            List<PersonResponse> sortedPersons = new List<PersonResponse>();
             switch (sortOrder)
            {
                case SortOrderOptions.DESC:
                   sortedPersons= allpersons.OrderByDescending(p => property?.GetValue(p)).ToList();
                    break;

                case SortOrderOptions.ASC:
                    sortedPersons= allpersons.OrderBy(p => property?.GetValue(p)).ToList();
                    break;
                   
            }
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }
            //validation personUpdateRequest
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update

            Person matchingPerson = _persons.FirstOrDefault(temp=>temp.PersonID==personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Person doesn't exist");
            }
            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            return matchingPerson.ToPersonResponse();

        }

        public bool DeletePerson(Guid? personID)
        {
            //TODO: check if "personID is not null.
            if(personID == null) { throw new ArgumentNullException(nameof(personID)); }
            //TODO: get the matching "Person" object from List<Person> based on PersonID.
            Person? matchingPerson=_persons.FirstOrDefault(temp=>temp.PersonID==personID);
            if (matchingPerson == null) { return false; }
            _persons.RemoveAll(temp=>temp.PersonID==personID);
            return true;
            //TODO: check if matching "Person" object is not null
            //TODO: Delete the matching "Person" object from List<Person>
            //Return Boolean value indicating whether person object was deleted or not
        }
    }
}
