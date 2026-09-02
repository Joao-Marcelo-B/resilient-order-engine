using ResilientOrderEngine.Domain.Exceptions;

namespace ResilientOrderEngine.Domain.Entities;

public class Invoice : BaseEntity
{
    public Guid OrderId { get; private set; }
    public string InvoiceNumber { get; private set; }
    public string AccessKey { get; private set; }
    public DateTimeOffset IssuedAt { get; private set; }
    
    protected Invoice() { }
  
    public Invoice(Guid orderId, string invoiceNumber, string accessKey)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("O identificador do pedido é obrigatório.");
  
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new DomainException("O número da nota fiscal é obrigatório.");
  
        if (string.IsNullOrWhiteSpace(accessKey) || accessKey.Length != 44)
            throw new DomainException("A chave de acesso da NF-e deve possuir exatamente 44 caracteres.");           
  
        OrderId = orderId;
        InvoiceNumber = invoiceNumber.Trim();
        AccessKey = accessKey.Trim();
        IssuedAt = DateTimeOffset.UtcNow;
    }

}