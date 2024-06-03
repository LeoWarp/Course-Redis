using RedisTech.Domain.Interfaces;

namespace RedisTech.Domain.Entity;

public class Settings : IEntityId<long>
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
}