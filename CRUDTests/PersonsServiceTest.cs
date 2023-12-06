using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
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
            _countriesService = new CountriesService(false);
            _testOutHelper = testOutputHelper;

        }

        public List<PersonResponse> GenerateData()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "bahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Tmaeddy",
                Email = "chmadn@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Jonaedl",
                Email = "ahman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest> { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }
            return person_response_list_from_add;
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
            PersonAddRequest? personAddReuest = new PersonAddRequest() { PersonName = null };

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
            PersonAddRequest personAddReuest = new PersonAddRequest()
            {
                PersonName = "John"
            ,
                Email = "person@gmail.com",
                Address = "sample address",
                CountryID = Guid.NewGuid(),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
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
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

            //Asser
            Assert.Null(person_response_from_get);
        }

        [Fact]
        public void GetPersonByPersonID_withPersonID()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse = _countriesService.AddCountry(country_request);

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
            PersonResponse person_respone = _personService.AddPerson(person_request);
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_respone.PersonID);
            //Assert
            Assert.Equal(person_respone, person_response_from_get);
        }

        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPerson_EmptyList()
        {
            //Act 
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

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
                PersonName = "Jonaedl",
                Email = "rahman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest> { person_request_1, person_request_2, person_request_3 };
            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }


            foreach (PersonResponse person in person_response_list_from_add)
            {
                _testOutHelper.WriteLine(person.ToString());
            }



            List<PersonResponse> person_list_from_get = _personService.GetAllPersons();

            //Act
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

        #region GetFilteredPersons
        //If the search text is empty and searchBy is "PersonName", it should return all persons. 
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {

            GenerateData();

            // Act

            List<PersonResponse> person_list_from_search = _personService.GetFileteredPersons(nameof(Person.PersonName), "");

            List<PersonResponse> person_response_list_getall = _personService.GetAllPersons();
            _testOutHelper.WriteLine("Expected:");
            _testOutHelper.WriteLine("empty search default out put all data");
            foreach (PersonResponse person in person_response_list_getall)
            {

                _testOutHelper.WriteLine(person.ToString());
            }
            _testOutHelper.WriteLine("Actual:");
            foreach (PersonResponse person in person_list_from_search)
            {
                _testOutHelper.WriteLine(person.ToString());
            }



        }


        //First we will add few persons; and then we will search based on person name with some search string. It should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            GenerateData();


            // Act

            List<PersonResponse> person_list_from_search = _personService.GetFileteredPersons(nameof(Person.PersonName), "ma");

            _testOutHelper.WriteLine("Expected:");

            foreach (PersonResponse person in person_list_from_search)
            {
                _testOutHelper.WriteLine(person.ToString());
            }

            _testOutHelper.WriteLine("Actual:");

            foreach (PersonResponse person in person_list_from_search)
            {
                if (person.PersonName != null)
                {

                    if (person.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person, person_list_from_search);
                        _testOutHelper.WriteLine(person.ToString());
                    }
                }
            }


        }
        #endregion

        #region GetSortedOreder
        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        [Fact]
        public void GetSortedPersons()
        {
            List<PersonResponse> person_response_list_from_add = GenerateData();
            List<PersonResponse> allpersons = _personService.GetAllPersons();

            //Act
            List<PersonResponse> persons_list_from_sort = _personService.GetSortedPersons(allpersons, nameof(Person.Email), SortOrderOptions.DESC);

            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.Email).ToList();
            _testOutHelper.WriteLine("Expected:");

            foreach (PersonResponse person in person_response_list_from_add)
            {
                _testOutHelper.WriteLine(person.ToString());
            }

            _testOutHelper.WriteLine("Actual:");

            foreach (PersonResponse person in persons_list_from_sort)
            {
                _testOutHelper.WriteLine(person.ToString());
            }



            //Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {

                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);



            }


        }
        #endregion

        #region UpdatePerson

        //when UpdateRequest=null
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            }
            );

        }

        // invalid person id ,throw ArgumentException
        [Fact]

        public void UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personService.UpdatePerson(personUpdateRequest); });
        
           
        }

        //when UpdateRequestPerson.name=nul, throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "bahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse_fetch = _personService.AddPerson(person_request_1);
            
            PersonUpdateRequest personUpdateRequest= personResponse_fetch.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;

            //Assert 
            Assert.Throws<ArgumentException>(() => { 
            
                //Act
                _personService.UpdatePerson(personUpdateRequest); 
            });



        }

        //add new person and try to update the person name and email
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdateion()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "bahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse_from_add = _personService.AddPerson(person_request_1);

            PersonUpdateRequest personUpdateRequest = personResponse_from_add.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "William@gmail.com";

            //Act
            PersonResponse person_response_frome_update = _personService.UpdatePerson(personUpdateRequest);
            PersonResponse person_response_from_get=_personService.GetPersonByPersonID(person_response_frome_update.PersonID);
            //Assert
            Assert.Equal(person_response_from_get, person_response_frome_update);

       


        }
        #endregion

        #region DeletePerson

        //PersonId is Valid
        [Fact]
        public void DeletePersonID_Valid()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "bahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse_fetch = _personService.AddPerson(person_request_1);

            //Act
            //Assert
            Assert.True(_personService.DeletePerson(personResponse_fetch.PersonID));
            
        }

        [Fact]
        public void DeletePersonID_inValid()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest() { CountryName = "India" };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "KIds",
                Email = "bahaman@email.com",
                Gender = GenderOptions.Male,
                Address = "address of you",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("1999-03-03"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponse_fetch = _personService.AddPerson(person_request_1);

            //Act
            //Assert
            Assert.False(_personService.DeletePerson(Guid.NewGuid()));

        }
        #endregion
    }
}
