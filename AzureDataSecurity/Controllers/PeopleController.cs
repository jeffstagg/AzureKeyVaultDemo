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
        IDataProtector _dataProtector;

        public PeopleController(
            ApplicationDbContext dbContext,
            IDataProtectionProvider dataProtectionProvider
            )
        {
            _dbContext = dbContext;
            _dataProtector = dataProtectionProvider.CreateProtector("PersonalDataEncryption");
        }

        [HttpGet]
        public IActionResult Index()
        {
            var people = _dbContext.People.ToList();

            foreach (var person in people)
            {
                try
                {
                    person.Name = _dataProtector.Unprotect(person.Name);
                    person.Email = _dataProtector.Unprotect(person.Email);
                    person.Phone = _dataProtector.Unprotect(person.Phone);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            return Ok(people);
        }

        [HttpPost]
        public IActionResult AddPerson(Person person)
        {
            var people = _dbContext.People;

            try
            {
                person.Name = _dataProtector.Protect(person.Name);
                person.Email = _dataProtector.Protect(person.Email);
                person.Phone = _dataProtector.Protect(person.Phone);
            }
            catch { }
            
            people.Add(person);

            _dbContext.SaveChanges();

            return Ok(person);
        }
    }
}