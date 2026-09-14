using System.ComponentModel.DataAnnotations;

namespace WitpayPizzaApi.DTOs
{
    public sealed record ResponsePizzaDTO(Guid Id, string Name, IReadOnlyCollection<ResponseToppingDTO> Toppings);

    public sealed record CreatePizzaWithToppingsRequest(
        [Required]
        string Name,
        [Required]
        IReadOnlyCollection<Guid> ToppingIds);

    public sealed record UpdatePizzaRequest(
        [Required]
        string Name);

    public sealed record UpdatePizzaToppingsRequest(
    IReadOnlyCollection<Guid> ToppingIds);
}   
