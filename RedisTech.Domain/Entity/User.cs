using RedisTech.Domain.Interfaces;

namespace RedisTech.Domain.Entity;

public class User : IEntityId<long>, IAuditable
{
    public long Id { get; set; }
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}. Логин: {Login}";
    }
}