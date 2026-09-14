namespace WitpayPizzaApi.Models
{
    public class Pizza
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        private readonly List<Topping> _toppings = new();
        public IReadOnlyCollection<Topping> Toppings => _toppings.AsReadOnly();


        private Pizza(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public static Pizza Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Pizza name is required.", nameof(name));
            return new Pizza(name);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Pizza name is required.", nameof(name));

            Name = name;
        }

        public void AddTopping(Topping topping)
        {
            _toppings.Add(topping);
        }

        public void RemoveTopping(Topping topping)
        {
            _toppings.Remove(topping);
        }
    }
}
