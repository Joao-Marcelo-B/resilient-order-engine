using ResilientOrderEngine.Domain.Enums;
using ResilientOrderEngine.Domain.Exceptions;

namespace ResilientOrderEngine.Domain.Entities;

public class Order : BaseEntity
{
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public string CustomerDocument { get; private set; }
    public string CustomerDeliveryAddress { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Invoice? Invoice { get; private set; }
    public OrderStatus Status { get; private set; }
    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    protected Order()
    { }

    public Order(string customerName, string customerEmail, string customerDocument, string customerDeliveryAddress)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new DomainException("O endereço de e-mail é obrigatório.");

        if (string.IsNullOrWhiteSpace(customerDocument))
            throw new DomainException("O documento federal é obrigatório.");

        if (string.IsNullOrWhiteSpace(customerDeliveryAddress))
            throw new DomainException("O endereço de entrega é obrigatório.");
        
        CustomerName = customerName.Trim();
        CustomerEmail = customerEmail.Trim().ToLowerInvariant();  
        CustomerDocument = customerDocument.Trim();
        CustomerDeliveryAddress = customerDeliveryAddress.Trim();
        TotalAmount = 0m;
        Status  = OrderStatus.Pending;
    }

    public void AddItem(string sku, string productName, decimal unitPrice, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Não é mais possível adicionar itens ao pedido");
        
        var item = new OrderItem(
            Id,
            sku, 
            productName, 
            unitPrice, 
            quantity);
        
        _items.Add(item);
        TotalAmount += unitPrice * quantity;
        SetUpdatedAt();
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("O pedido não pode ser alterado para pago.");

        if (!_items.Any())
            throw new DomainException("Não é posśivel pagar um pedido sem itens.");
        
        Status = OrderStatus.Paid;
        SetUpdatedAt();
    }

    public void MarkAsInvoiced(string invoiceNumber, string accessKey)
    {
        if (Status != OrderStatus.Paid)
            throw new DomainException("O pedido não pode ser alterado para faturado.");
        
        Invoice = new Invoice(Id, invoiceNumber, accessKey); 
        Status = OrderStatus.Invoiced;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Invoiced or OrderStatus.Canceled)
            throw new DomainException("O pedido não pode ser cancelado.");

        Status = OrderStatus.Canceled;
        SetUpdatedAt();
    }
}