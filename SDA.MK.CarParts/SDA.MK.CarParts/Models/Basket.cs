namespace SDA.MK.CarParts.Models
{
	public class Basket
	{
		public Guid Id { get; }
		public Client Client { get; }
		public IReadOnlyCollection<BasketEntry> BasketEntries { get; }

		private List<BasketEntry> basketEntries = new List<BasketEntry>();

	}
}
