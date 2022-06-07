using Microsoft.EntityFrameworkCore;
using SDA.MK.CarParts.Models;

namespace SDA.MK.CarParts
{
	public class Context : DbContext
	{
		public DbSet<Basket> Baskets { get; set; } = default!;
		public DbSet<Client> Clients { get; set; } = default!;
		public DbSet<Part> Parts { get; set; } = default!;
		public DbSet<BasketEntry> BasketEntries { get; set; } = default!;

		private readonly IConfiguration config;

		public Context(IConfiguration config)
		{
			this.config = config;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(config.GetConnectionString("Database"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var partBuilder = modelBuilder.Entity<Part>();
			partBuilder.ToTable("Parts");
			partBuilder.HasKey(p => p.Id);
			partBuilder.Property(p => p.Name).IsRequired().HasMaxLength(500);
			partBuilder.Property(p => p.Price).IsRequired().HasPrecision(7, 2);

			var clientBuilder = modelBuilder.Entity<Client>();
			clientBuilder.ToTable("Clients");
			clientBuilder.HasKey(c => c.Id);

			var basketEntryBuilder = modelBuilder.Entity<BasketEntry>();
			basketEntryBuilder.ToTable("BasketEntries");
			basketEntryBuilder.HasKey(be => be.Id);
			basketEntryBuilder.HasOne(be => be.Part);
			basketEntryBuilder.Property(be => be.Amount);

			var basketBuilder = modelBuilder.Entity<Basket>();
			basketBuilder.ToTable("Baskets");
			basketBuilder.HasKey(b => b.Id);
			basketBuilder.HasOne(b => b.Client);
			basketBuilder.HasMany(b => b.BasketEntries);
		}
	}
}
