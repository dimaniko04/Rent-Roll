using RentnRoll.Application.Common.Request;
using RentnRoll.Application.Common.Responses;
using RentnRoll.Core.Common.Result;

namespace RentnRoll.Application.Interfaces.Services;

public interface ITestEntityService
{
    Task<List<TestEntityResponse>> GetAllTestEntitiesAsync();
    Task<Result<TestEntityResponse>> GetTestEntityAsync(Guid id);
    Task<TestEntityResponse> CreateTestEntityAsync(CreateTestEntityRequest request);
}