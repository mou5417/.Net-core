using ServiceContracts;
using Xunit;
using ServiceContracts;
using Entities;
using ServiceContracts.DTO;
using Services;
using System.ComponentModel.DataAnnotations;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        //private fields
        private readonly IPersonsService _personService;
      
        //constructor
        public PersonsServiceTest()
        { 
            _personService = new PersonsService(); 
         
        }
        #region AddPerson
        // person valus = null
        [Fact]
        public void AddPerson_NullValue()
        {
            //Arrange
            PersonAddReuest? personAddReuest = null;

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
            PersonAddReuest? personAddReuest = new PersonAddReuest() { PersonName=null};

            //Act
            Assert.Throws<ArgumentNullException>(() =>
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
            PersonAddReuest? personAddReuest = new PersonAddReuest() { PersonName = "John"
            ,Email="person@gmail.com",Address ="sample address", CountryID = Guid.NewGuid(),Gender=ServiceContracts.Enums.GenderOptions.Male,
            DateOfBirth = DateTime.Parse("2000-01-01"),
            ReceiveNewsLetters=true
            };

            //Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAddReuest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();
            var emailChecker = new EmailAddressAttribute();
            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
            Assert.Throws<ArgumentException>(() =>
                emailChecker.IsValid(person_response_from_add.Email));
         
            
        }

        #endregion
    }
}
