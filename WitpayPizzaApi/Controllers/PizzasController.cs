using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WitpayPizzaApi.Data;
using WitpayPizzaApi.DTOs;
using WitpayPizzaApi.Models;

namespace WitpayPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {
        private readonly WitpayDbContext _context;

        public PizzasController(WitpayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ResponsePizzaDTO>>> GetAllPizzas(
            CancellationToken cancellationToken)
        {
            var pizzas = await _context.Pizzas
                .Select(p => new ResponsePizzaDTO(
                    p.Id,
                    p.Name,
                    p.Toppings.Select(t => new ResponseToppingDTO(
                        t.Id,
                        t.Name))
                        .ToList()))
                .ToListAsync(cancellationToken);

            return Ok(pizzas);
        }

        [HttpPost]
        public async Task<ActionResult<ResponsePizzaDTO>> CreatePizzaWithToppings(
            [FromBody] CreatePizzaWithToppingsRequest request,
            CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exists = await _context.Pizzas
                .AnyAsync(p => p.Name == name, cancellationToken);

            if (exists)
                return Conflict("Pizza already exists.");

            var toppings = await _context.Toppings
                .Where(t => request.ToppingIds.Contains(t.Id))
                .ToListAsync(cancellationToken);

            if (toppings.Count != request.ToppingIds.Count)
                return BadRequest("One or more toppings were not found.");

            var pizza = Pizza.Create(name);

            foreach (var topping in toppings)
            {
                pizza.AddTopping(topping);
            }

            _context.Pizzas.Add(pizza);

            await _context.SaveChangesAsync(cancellationToken);

            var response = new ResponsePizzaDTO(
                pizza.Id,
                pizza.Name,
                pizza.Toppings.Select(t => new ResponseToppingDTO(
                    t.Id,
                    t.Name))
                    .ToList());

            return StatusCode(StatusCodes.Status201Created, response);
        }


        [HttpPut("{pizzaId:guid}")]
        public async Task<ActionResult<ResponsePizzaDTO>> UpdatePizzaDetails(
             [FromRoute] Guid pizzaId,
             [FromBody] UpdatePizzaRequest request,
             CancellationToken cancellationToken)
        {
            var pizza = await _context.Pizzas
                .FirstOrDefaultAsync(p => p.Id == pizzaId, cancellationToken);

            if (pizza is null)
            {
                return NotFound("Pizza not found.");
            }

            var name = request.Name.Trim();

            var exists = await _context.Pizzas
                .AnyAsync(p => p.Name == name && p.Id != pizzaId, cancellationToken);

            if (exists)
            {
                return Conflict("Pizza name already exists.");
            }

            pizza.UpdateName(name);

            await _context.SaveChangesAsync(cancellationToken);

            var response = new ResponsePizzaDTO(
                pizza.Id,
                pizza.Name,
                pizza.Toppings.Select(t =>
                    new ResponseToppingDTO(
                        t.Id,
                        t.Name))
                    .ToList());

            return Ok(response);
        }


        [HttpPut("{pizzaId:guid}/toppings")]
        public async Task<ActionResult<ResponsePizzaDTO>> UpdatePizzaToppings(
             [FromRoute] Guid pizzaId,
             [FromBody] UpdatePizzaToppingsRequest request,
             CancellationToken cancellationToken)
        {
            var pizza = await _context.Pizzas
                .Include(p => p.Toppings)// need to load the existing toppings of the pizza to know what exactly what should we remove
                .FirstOrDefaultAsync(p => p.Id == pizzaId, cancellationToken);

            if (pizza is null)
            {
                return NotFound("Pizza not found.");
            }

            var toppings = await _context.Toppings
                .Where(t => request.ToppingIds.Contains(t.Id))
                .ToListAsync(cancellationToken);

            if (toppings.Count != request.ToppingIds.Count)
            {
                return BadRequest("One or more toppings were not found.");
            }

            // .ToList() creates a copy of the collection in memory
            // this prevents a "Collection was modified" exception while loop removing items
            foreach (var topping in pizza.Toppings.ToList())
            {
                pizza.RemoveTopping(topping);
            }

            // add the new set of toppings
            foreach (var topping in toppings)
            {
                pizza.AddTopping(topping);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var response = new ResponsePizzaDTO(
                pizza.Id,
                pizza.Name,
                pizza.Toppings.Select(t =>
                    new ResponseToppingDTO(
                        t.Id,
                        t.Name))
                    .ToList());

            return Ok(response);
        }

        [HttpDelete("{pizzaId:guid}")]
        public async Task<ActionResult> DeletePizza([FromRoute] Guid pizzaId, CancellationToken cancellationToken)
        {
            var pizza = await _context.Pizzas.FirstOrDefaultAsync(p => p.Id == pizzaId, cancellationToken);
            if (pizza is null)
            {
                return NotFound("Pizza not found.");
            }

            _context.Pizzas.Remove(pizza);
            await _context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}