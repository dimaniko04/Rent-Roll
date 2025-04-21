using Microsoft.AspNetCore.Mvc;
using RentnRoll.Application.Interfaces.Services;

namespace RentnRoll.API.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
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
        var testEntity = await _testEntityService.GetTestEntityAsync(id);

        return Ok(testEntity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTestEntityRequest request)
    {
        var testEntity = await _testEntityService
            .CreateTestEntityAsync(request);

        return CreatedAtRoute(
            "GetById",
            new { id = testEntity.Id },
            testEntity);
    }

    [HttpGet("not-implemented")]
    public Task<IActionResult> NotImplemented()
    {
        throw new NotImplementedException("This is a test exception");
    }
}