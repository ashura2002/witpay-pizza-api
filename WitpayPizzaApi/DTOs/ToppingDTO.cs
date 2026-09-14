using System.ComponentModel.DataAnnotations;

namespace WitpayPizzaApi.DTOs
{
    public sealed record ResponseToppingDTO(
        Guid Id,
        string Name);

    public sealed record CreateToppingRequest(
        [Required]
        string Name);

    public sealed record UpdateToppingRequest(
        [Required]
        string Name);
}
