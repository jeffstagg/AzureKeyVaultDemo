using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureDataSecurity.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AzureDataSecurity.Controllers
{
    [Route("[controller]")]
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

        public IActionResult Index()
        {
            var people = _dbContext.People.ToList();
            return Ok(people);
        }
    }
}