using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WitpayPizzaApi.Data;
using WitpayPizzaApi.DTOs;
using WitpayPizzaApi.Models;

namespace WitpayPizzaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToppingsController : ControllerBase
    {
        private readonly WitpayDbContext _context;

        public ToppingsController(WitpayDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ResponseToppingDTO>>> GetAllToppings(CancellationToken cancellationToken)
        {
            var toppings = await _context.Toppings
                .Select(t => new ResponseToppingDTO(t.Id, t.Name))
                .ToListAsync(cancellationToken);

            return Ok(toppings);
        }


        [HttpPost]
        public async Task<ActionResult<ResponseToppingDTO>> CreateTopping([FromBody] CreateToppingRequest request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

            var exists = await _context.Toppings.AnyAsync(t => t.Name == name, cancellationToken);
            if (exists) return Conflict("Topping name already exists.");

            var topping = Topping.Create(name);
            _context.Toppings.Add(topping);

            await _context.SaveChangesAsync(cancellationToken);
            var response = new ResponseToppingDTO(topping.Id, topping.Name);

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPut("{toppingId:guid}")]
        public async Task<ActionResult<ResponseToppingDTO>> UpdateTopping([FromRoute] Guid toppingId, [FromBody] UpdateToppingRequest request,
            CancellationToken cancellationToken)
        {
            var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.Id == toppingId, cancellationToken);

            if (topping is null) return NotFound("Topping not found.");

            var name = request.Name.Trim();

            var exists = await _context.Toppings.AnyAsync(t => t.Name == name && t.Id != toppingId, cancellationToken);
            if (exists) return Conflict("Topping name already exists.");

            topping.UpdateName(name);

            await _context.SaveChangesAsync(cancellationToken);
            var response = new ResponseToppingDTO(topping.Id, topping.Name);
            return Ok(response);
        }

        [HttpDelete("{toppingId:guid}")]
        public async Task<IActionResult> DeleteTopping([FromRoute] Guid toppingId, CancellationToken cancellationToken)
        {
            var topping = await _context.Toppings
                .FirstOrDefaultAsync(t => t.Id == toppingId, cancellationToken);

            if (topping is null)
                return NotFound("Topping not found.");

            _context.Toppings.Remove(topping);

            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

    }
}
