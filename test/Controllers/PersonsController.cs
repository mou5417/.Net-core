﻿using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace GRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsService _personsService;

        //constructor

        public PersonsController(IPersonsService personsService)
        {
            _personsService = personsService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy,string? searchString )
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName),"Person Name" },
                { nameof(PersonResponse.Email),"Email" },
                { nameof(PersonResponse.DateOfBirth),"Date of Birth" },
                { nameof(PersonResponse.Gender),"Gender" },
                { nameof(PersonResponse.CountryID),"Country" },
                { nameof(PersonResponse.Address),"Address" }
            };
            ViewBag.CurrentllySearchBy=searchBy;
            ViewBag.CurrentllySearchString=searchString;
            List<PersonResponse> persons = _personsService.GetFileteredPersons(searchBy, searchString);
            return View(persons);//Views/Persons/Index.cshtml
        }
    }
}
