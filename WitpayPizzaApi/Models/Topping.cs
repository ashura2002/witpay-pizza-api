namespace WitpayPizzaApi.Models
{
    public class Topping
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        private readonly List<Pizza> _pizzas = new();
        public IReadOnlyCollection<Pizza> Pizzas => _pizzas.AsReadOnly();


        private Topping(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static Topping Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Topping name is required.", nameof(name));

            return new Topping(name);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Topping name is required.", nameof(name));

            Name = name;
        }
    }
}
