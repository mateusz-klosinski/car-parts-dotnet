namespace SDA.MK.CarParts.Requests
{
	public record ChangePartAmount(Guid ClientId, Guid PartId, int Amount);
}
