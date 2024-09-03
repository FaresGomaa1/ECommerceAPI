using ECommerceAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ECommerceContext : IdentityDbContext<User>
{
    #region Constructors

    public ECommerceContext(DbContextOptions<ECommerceContext> options)
        : base(options) { }

    #endregion

    #region DbSets

    public DbSet<Address> Addresses { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<ColorSizeProduct> ColorSizeProducts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Rate> Rates { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<WishList> WishLists { get; set; }

    #endregion



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Unique Indexes

        builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
        builder.Entity<Color>()
            .HasIndex(c => c.Name)
            .IsUnique();
        builder.Entity<Size>()
            .HasIndex(s => s.Name)
            .IsUnique();
        builder.Entity<Order>()
            .HasIndex(o => o.InvoiceNumber)
            .IsUnique();
        #endregion

        #region Composite Unique Indexes

        builder.Entity<Rate>()
            .HasIndex(r => new { r.UserId, r.ProductId })
            .IsUnique();
        builder.Entity<Product>()
                .HasIndex(p => new { p.Name, p.CategoryId })
                .IsUnique();
        builder.Entity<ColorSizeProduct>()
            .HasIndex(csp => new { csp.ProductId, csp.SizeId, csp.ColorId })
            .IsUnique();
        builder.Entity<Cart>()
            .HasIndex(c => new { c.UserId, c.ProductId, c.SizeId, c.ColorId })
            .IsUnique();
        builder.Entity<WishList>()
            .HasIndex(wl => new { wl.UserId, wl.ProductId })
            .IsUnique();
        builder.Entity<Photo>()
                .HasIndex(p => new { p.Url, p.ProductId })
                .IsUnique();

        #endregion

        #region Property Configurations

        builder.Entity<OrderDetail>()
            .Property(od => od.Price)
            .HasColumnType("decimal(18,2)");
        builder.Entity<Rate>()
            .Property(r => r.Value)
            .HasColumnType("decimal(18,2)");
        builder.Entity<Product>()
            .Property(csp => csp.Price)
            .HasColumnType("decimal(18,2)");

        #endregion

        #region Relationships

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        #endregion
    }
}