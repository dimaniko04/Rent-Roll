using RentnRoll.Application.Contracts.TestEntities;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.TestEntities;

public interface ITestEntityService
{
    Task<List<TestEntityResponse>> GetAllTestEntitiesAsync();
    Task<Result<TestEntityResponse>> GetTestEntityAsync(Guid id);
    Task<TestEntityResponse> CreateTestEntityAsync(CreateTestEntityRequest request);
}