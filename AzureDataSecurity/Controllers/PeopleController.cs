using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureDataSecurity.Infrastructure;
using AzureDataSecurity.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace AzureDataSecurity.Controllers
{
    [Route("people")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private ApplicationDbContext _dbContext;

        public PeopleController(
            ApplicationDbContext dbContext
            )
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var people = _dbContext.People.ToList();

            return Ok(people);
        }

        [HttpPost]
        public IActionResult AddPerson(Person person)
        {
            var people = _dbContext.People;
            
            people.Add(person);

            _dbContext.SaveChanges();

            return Ok(person);
        }
    }
}