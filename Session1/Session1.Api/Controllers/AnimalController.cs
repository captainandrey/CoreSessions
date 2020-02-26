using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AnimalsDbContext context;
        private readonly IMyService serivce;

        public AnimalController(AnimalsDbContext context, IMyService serivce)
        {
            this.context = context;
            this.serivce = serivce;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Animal>> Get(int id)
        {
            var animal = context.Animal.FirstOrDefault(a => a.Id == id);
            if (animal == null)
            {
                return NotFound();
            }
            return Ok(await Task.FromResult(animal));
        }

        [HttpGet]
        public async Task<ActionResult<List<Animal>>> GetAll()
        {
            var key = await serivce.GetMyKey();

            var animals = context.Animal.ToList();

            return Ok(await Task.FromResult(animals));
        }

        [HttpPost]
        public async Task<ActionResult> Post(Animal animal)
        {
            try
            {
                context.Animal.Add(animal);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
