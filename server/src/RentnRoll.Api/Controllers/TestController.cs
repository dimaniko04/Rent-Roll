using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Contracts.TestEntities;
using RentnRoll.Application.Services.TestEntities;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ApiController
{
    private readonly ITestEntityService _testEntityService;

    public TestController(ITestEntityService testEntityService)
    {
        _testEntityService = testEntityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var testEntities = await _testEntityService.GetAllTestEntitiesAsync();

        return Ok(testEntities);
    }

    [HttpGet("{id:guid}", Name = "GetById")]
    public async Task<IActionResult> Get(Guid id)
    {
        var testEntityResult = await _testEntityService.GetTestEntityAsync(id);

        return testEntityResult.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTestEntityRequest request)
    {
        var testEntity = await _testEntityService
            .CreateTestEntityAsync(request);

        if (testEntity.IsError)
        {
            return Problem(testEntity.Errors);
        }

        return CreatedAtRoute(
            "GetById",
            new { id = testEntity.Value!.Id },
            testEntity);
    }

    [HttpGet("not-implemented")]
    public Task<IActionResult> NotImplemented()
    {
        throw new NotImplementedException("This is a test exception");
    }
}