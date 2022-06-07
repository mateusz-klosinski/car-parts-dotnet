using SDA.MK.CarParts.Models;

namespace SDA.MK.CarParts.Responses
{
	public record BasketResponse(Guid Id, IEnumerable<BasketEntryReponse> Entries, int TotalCount, decimal Price)
	{
		public static BasketResponse FromBasket(Basket basket)
		{
			var entries = basket.BasketEntries
				.Select(be => new BasketEntryReponse(
					be.Id, 
					new PartResponse(be.Part.Id, be.Part.Name, be.Part.Price),
					be.Amount, 
					be.Price
				));

			return new BasketResponse(basket.Id, entries, basket.TotalItems, basket.Price);
		}
	}
}
