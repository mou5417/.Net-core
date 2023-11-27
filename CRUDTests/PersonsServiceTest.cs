using ServiceContracts;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using System.ComponentModel.DataAnnotations;
using ServiceContracts.Enums;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        //private fields
        private readonly IPersonsService _personService;
      private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutHelper;
        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        { 
            _personService = new PersonsService(); 
            _countriesService = new CountriesService(); 
            _testOutHelper = testOutputHelper;

        }
       
   
        #region AddPerson
        // person valus = null
        [Fact]
        public void AddPerson_NullValue()
        {
            //Arrange
            PersonAddRequest? personAddReuest = null;

            //Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personService.AddPerson(personAddReuest);
            }
            );
        }

        // personName valus = null
        [Fact]
        public void AddPerson_NameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddReuest = new PersonAddRequest() { PersonName=null};

            //Act
            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddReuest);
            }
            );
        }

        // insert person into persons list and return object of PersonResponse includes newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest personAddReuest = new PersonAddRequest() { PersonName = "John"
            ,Email="person@gmail.com",Address ="sample address", CountryID = Guid.NewGuid(),Gender=ServiceContracts.Enums.GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters=true
            };

            //Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAddReuest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();
            
            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        
            
        }

        #endregion

        #region GetPersonByPersonID

        //PersonID=null? return null

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange 
            Guid? personID = null;
            
            //Act
            PersonResponse? person_response_from_get= _personService.GetPersonByPersonID(personID);

            //Asser
            Assert.Null(person_response_from_get);
        }

        [Fact]
        public void GetPersonByPersonID_withPersonID()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse= _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "personone",
                Email = "sample@gmail.com",
                Address = "address",
                CountryID = countryResponse.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };
            PersonResponse person_respone=_personService.AddPerson(person_request);
            PersonResponse? person_response_from_get=_personService.GetPersonByPersonID(person_respone.PersonID);
            //Assert
            Assert.Equal(person_respone, person_response_from_get);
        }

        #endregion
        #region GetAllPersons
        [Fact]
        public void GetAllPerson_EmptyList()
        {
           //Act 
           List<PersonResponse> persons_from_get=_personService.GetAllPersons();
           
            //Assert
            Assert.Empty(persons_from_get);
            
        }

        //First, we will add few persons; then call GetAllPersons(), it should return the same persons that were added
        [Fact]

        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "rahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Teddy",
                Email = "rahmadn@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Jonaedl", Email = "rahman@email.com", Gender = GenderOptions.Male, Address = "address of you",
                CountryID = country_response_1.CountryID, DateOfBirth = DateTime.Parse("1999-03-03"), ReceiveNewsLetters = true
            };
            
            List<PersonAddRequest> person_requests = new List<PersonAddRequest> { person_request_1, person_request_2, person_request_3 };
            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();
            
            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            foreach (PersonResponse person in person_response_list_from_add)
            {
                _testOutHelper.WriteLine(person.ToString());
            }
            //Act


            List<PersonResponse> person_list_from_get = _personService.GetAllPersons();

            //print person_response_list_from_get
            _testOutHelper.WriteLine("Actual:");
            foreach (PersonResponse person in person_list_from_get)
            {
                _testOutHelper.WriteLine(person.ToString());
            }

            //Assert
            _testOutHelper.WriteLine("Expected:");
            foreach (PersonResponse person in person_response_list_from_add)
            {
                Assert.Contains(person, person_list_from_get);
            }

        }
        #endregion
    }
}
