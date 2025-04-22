using Microsoft.Extensions.Logging;
using RentnRoll.Application.Common.Errors;
using RentnRoll.Application.Common.Request;
using RentnRoll.Application.Common.Responses;
using RentnRoll.Application.Interfaces.Services;
using RentnRoll.Core.Common;
using RentnRoll.Core.Common.Result;
using RentnRoll.Core.Entities;

namespace RentnRoll.Application.Services;

public class TestEntityService : ITestEntityService
{
    private readonly ILogger<TestEntityService> _logger;

    public TestEntityService(ILogger<TestEntityService> logger)
    {
        _logger = logger;
    }

    private readonly List<TestEntity> _testEntities = new List<TestEntity>();

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

    public async Task<Result<TestEntityResponse>> GetTestEntityAsync(Guid id)
    {
        var testEntity = _testEntities
            .FirstOrDefault(x => x.Id == id);
        await Task.CompletedTask;

        if (testEntity == null)
        {
            _logger.LogError(
                "Error: {Type} - Test Entity with ID {Id} not found.",
                ErrorType.NotFound, id);
            return Errors.TestEntity.NotFound(id);
        }

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return testEntityResponse;
    }
}