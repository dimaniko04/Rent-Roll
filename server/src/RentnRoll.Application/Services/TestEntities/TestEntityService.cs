using Microsoft.Extensions.Logging;

using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Contracts.TestEntities;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.TestEntities;

public class TestEntityService : ITestEntityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TestEntityService> _logger;
    private readonly IValidationService _validationService;
    private readonly ITestEntityRepository _testEntityRepository;

    public TestEntityService(
        IUnitOfWork unitOfWork,
        ILogger<TestEntityService> logger,
        IValidationService validationService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _testEntityRepository = unitOfWork
            .GetRepository<ITestEntityRepository>();
        _validationService = validationService;
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
            return Errors.TestEntity.NotFound(id);
        }

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return testEntityResponse;
    }

    public async Task<Result<TestEntityResponse>> CreateTestEntityAsync(
        CreateTestEntityRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var testEntity = request.ToDomain();

        await _testEntityRepository.AddAsync(testEntity);
        await _unitOfWork.SaveChangesAsync();

        var testEntityResponse = TestEntityResponse.FromDomain(testEntity);

        return testEntityResponse;
    }
}