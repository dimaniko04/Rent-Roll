namespace RentnRoll.Domain.Common.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
}