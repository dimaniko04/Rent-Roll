using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.Errors;
using RentnRoll.Application.Common.Interfaces.Persistence.Repositories;
using RentnRoll.Application.Common.Interfaces.Persistence.UnitOfWork;
using RentnRoll.Application.Common.Interfaces.Services;
using RentnRoll.Application.Common.Request;
using RentnRoll.Application.Common.Responses;
using RentnRoll.Core.Common;

namespace RentnRoll.Application.Services;

public class TestEntityService : ITestEntityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TestEntityService> _logger;
    private readonly ITestEntityRepository _testEntityRepository;

    public TestEntityService(
        ILogger<TestEntityService> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _testEntityRepository = unitOfWork
            .GetRepository<ITestEntityRepository>();
    }

    public async Task<List<TestEntityResponse>> GetAllTestEntitiesAsync()
    {
        var testEntities = await _testEntityRepository
            .GetAllAsync();

        return testEntities
            .Select(TestEntityResponse.FromDomain)
            .ToList();
    }

    public async Task<Result<TestEntityResponse>> GetTestEntityAsync(Guid id)
    {
        var testEntity = await _testEntityRepository
            .GetByIdAsync(id);

        if (testEntity == null)
        {
            _logger.LogError(
                "Error: {Type} - Test Entity with ID {Id} not found.",
                ErrorType.NotFound, id);
            return AppErrors.TestEntity.NotFound(id);
        }

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return testEntityResponse;
    }

    public async Task<TestEntityResponse> CreateTestEntityAsync(
        CreateTestEntityRequest request)
    {
        var testEntity = request.ToDomain();

        await _testEntityRepository.AddAsync(testEntity);
        await _unitOfWork.SaveChangesAsync();

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return testEntityResponse;
    }
}