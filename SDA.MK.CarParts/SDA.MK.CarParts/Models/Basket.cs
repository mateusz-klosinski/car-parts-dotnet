namespace SDA.MK.CarParts.Models
{
	public class Basket
	{
		public Guid Id { get; }
		public Client Client { get; } = default!;
		public IReadOnlyCollection<BasketEntry> BasketEntries => basketEntries.AsReadOnly();
		public int TotalItems => basketEntries.Sum(e => e.Amount);
		public decimal Price => basketEntries.Sum(e => e.Price);
		public bool IsEmpty => !basketEntries.Any();

		private List<BasketEntry> basketEntries = new();

		public Basket(Guid id, Client client) : this(id)
		{
			Client = client ?? throw new ArgumentNullException(nameof(client));
		}

		private Basket(Guid id)
		{
			Id = id;
		}

		public void AddPart(Part part)
		{
			var existingEntry = basketEntries.FirstOrDefault(e => e.Part.Id == part.Id);

			if (existingEntry is not null)
			{
				existingEntry.ChangeAmount(existingEntry.Amount + 1);
				return;
			}

			var entry = new BasketEntry(Guid.NewGuid(), part, 1);
			basketEntries.Add(entry);
		}

		public void RemovePart(Part part)
		{
			var entry = basketEntries.FirstOrDefault(e => e.Part.Id == part.Id);

			if (entry is null)
			{
				return;
			}

			basketEntries.Remove(entry);
		}

		public void ChangeAmount(Guid partId, int amount)
		{
			var entry = basketEntries.FirstOrDefault(e => e.Part.Id == partId);

			if (entry is null)
			{
				return;
			}

			entry.ChangeAmount(amount);
		}

		public void Clear()
		{
			basketEntries.Clear();
		}
	}
}
