namespace SDA.MK.CarParts.Responses
{
	public record BasketEntryReponse(Guid Id, PartResponse Part, int Amount, decimal Price);
}
