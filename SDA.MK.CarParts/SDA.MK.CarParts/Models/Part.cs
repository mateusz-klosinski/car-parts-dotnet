using System;

namespace SDA.MK.CarParts.Models
{
	public class Part
	{
		public Guid Id { get; }
		public string Name { get; private set; }
		public decimal Price { get; private set; }

		private Part(Guid id, string name, decimal price)
		{
			Id = id;
			Name = name;
			Price = price;
		}

		public Part(string partName, decimal partPrice) : this(Guid.NewGuid(), partName, partPrice)
		{
			CheckInvariants(partName, partPrice);
		}

		public void Update(string name, decimal price)
		{
			CheckInvariants(name, price);
			Name = name;
			Price = price;
		}

		private void CheckInvariants(string name, decimal price)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			if (name.Length > 500)
			{
				throw new ArgumentException($"{nameof(name)} must have less than 500 characters");
			}

			if (price < 0)
			{
				throw new ArgumentException($"{nameof(price)} cannot be below 0!");
			}

			if (price > 1000000)
			{
				throw new ArgumentException($"{nameof(price)} that's way too much, don't you think? ;)");
			}
		} 
	}
}
