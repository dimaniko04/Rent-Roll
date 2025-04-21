using RentnRoll.Application.Responses;

namespace RentnRoll.Application.Interfaces.Services;

public interface ITestEntityService
{
    Task<List<TestEntityResponse>> GetAllTestEntitiesAsync();
    Task<TestEntityResponse> GetTestEntityAsync(Guid id);
    Task<TestEntityResponse> CreateTestEntityAsync(CreateTestEntityRequest request);
}