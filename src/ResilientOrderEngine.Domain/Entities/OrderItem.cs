using ResilientOrderEngine.Domain.Exceptions;

namespace ResilientOrderEngine.Domain.Entities;

public class OrderItem : BaseEntity
{
    public string Sku { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    protected OrderItem()
    { }
    
    public OrderItem(Guid orderId, string sku, string productName, decimal unitPrice, int quantity)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("O pedido é obrigatório."); 
            
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("O SKU do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("O nome do produto é obrigatório.");

        if (unitPrice <= 0)
            throw new DomainException("O preço unitário do produto deve ser maior que zero.");

        if (quantity <= 0)
            throw new DomainException("A quantidade do item deve ser maior que zero.");
        
        OrderId = orderId;
        Sku = sku.Trim().ToUpper();
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}