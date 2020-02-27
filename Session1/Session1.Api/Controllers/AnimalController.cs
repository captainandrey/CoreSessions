using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Session1.Api.Model;
using Session1.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalService animalService;
        private readonly ILogger<AnimalController> logger;

        public AnimalController(IAnimalService animalService, ILogger<AnimalController> logger)
        {
            this.animalService = animalService;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Animal>> Get(int id)
        {
            try
            {
                var animal = await animalService.Get(id);
                if (animal == null)
                {
                    return NotFound();
                }
                return Ok(animal);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Animal>>> GetAll()
        {
            try
            {
                var animals = await animalService.GetAll();

                return Ok(animals);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Animal>> Post(Animal animal)
        {
            try
            {
                var result = await animalService.Add(animal);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
