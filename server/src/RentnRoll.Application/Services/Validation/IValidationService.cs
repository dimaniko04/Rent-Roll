using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Validation;

public interface IValidationService
{
    Task<Result<T>> ValidateAsync<T>(T entity);
}