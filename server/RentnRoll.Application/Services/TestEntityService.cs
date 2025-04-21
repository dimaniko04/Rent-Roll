using RentnRoll.Application.Interfaces.Services;
using RentnRoll.Application.Responses;
using RentnRoll.Core.Entities;

namespace RentnRoll.Application.Services;

public class TestEntityService : ITestEntityService
{
    private List<TestEntity> _testEntities = new List<TestEntity>();

    public Task<TestEntityResponse> CreateTestEntityAsync(
        CreateTestEntityRequest request)
    {
        var testEntity = request.ToDomain();
        _testEntities.Add(testEntity);

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return Task.FromResult(testEntityResponse);
    }

    public Task<List<TestEntityResponse>> GetAllTestEntitiesAsync()
    {
        var testEntityResponses = _testEntities
            .Select(TestEntityResponse.FromDomain)
            .ToList();

        return Task.FromResult(testEntityResponses);
    }

    public Task<TestEntityResponse> GetTestEntityAsync(Guid id)
    {
        var testEntity = _testEntities
            .FirstOrDefault(x => x.Id == id);

        if (testEntity == null)
        {
            throw new KeyNotFoundException($"TestEntity with ID {id} not found.");
        }

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return Task.FromResult(testEntityResponse);
    }
}