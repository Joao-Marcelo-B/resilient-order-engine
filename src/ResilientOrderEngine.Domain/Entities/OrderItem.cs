using ResilientOrderEngine.Domain.Exceptions;

namespace ResilientOrderEngine.Domain.Entities;

public class OrderItem : BaseEntity
{
    public string Sku { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    protected OrderItem()
    { }
    
    public OrderItem(string sku, string productName, decimal unitPrice, int quantity)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("O SKU do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("O nome do produto é obrigatório.");

        if (unitPrice <= 0)
            throw new DomainException("O preço unitário do produto deve ser maior que zero.");

        if (quantity <= 0)
            throw new DomainException("A quantidade do item deve ser maior que zero.");
        
        Sku = sku.Trim().ToUpper();
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}