namespace Cerealizer.Tests.TestObjects;

public class Product
{
    public string Name;
    public DateTime ExpiryDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public decimal Price;
    public string[] Sizes;

    public override bool Equals(object obj)
    {
        if (obj is Product)
        {
            Product p = (Product)obj;

            return (p.Name == Name && p.ExpiryDate == ExpiryDate && p.Price == Price);
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return (Name ?? string.Empty).GetHashCode();
    }
}