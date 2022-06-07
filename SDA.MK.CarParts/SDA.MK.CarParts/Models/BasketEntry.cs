namespace SDA.MK.CarParts.Models
{
	public class BasketEntry
	{
		public Guid Id { get; }
		public Part Part { get; } = default!;
		public int Amount { get; private set; }
		public decimal Price => Part.Price * Amount;

		private BasketEntry(Guid id, int amount)
		{
			Id = id;
			Amount = amount;
		}

		public BasketEntry(Guid id, Part part, int amount) : this(id, amount)
		{
			CheckInvariants(amount);

			Part = part ?? throw new ArgumentNullException(nameof(part));
		}

		public void ChangeAmount(int amount)
		{
			CheckInvariants(amount);
			Amount = amount;
		}

		private static void CheckInvariants(int amount)
		{
			if (amount < 1 || amount > 100)
			{
				throw new ArgumentOutOfRangeException(nameof(amount));
			}
		}
	}
}
