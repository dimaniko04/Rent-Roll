using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RentnRoll.Application.Common.Policies;
using RentnRoll.Application.Contracts.Tags.CreateTag;
using RentnRoll.Application.Contracts.Tags.GetAllTags;
using RentnRoll.Application.Contracts.Tags.UpdateTag;
using RentnRoll.Application.Services.Tags;
using RentnRoll.Domain.Constants;

namespace RentnRoll.Api.Controllers;

[ApiController]
[Route("api/businesses/{businessId:guid}/tags")]
[Authorize]
public class TagController : ApiController
{
    private readonly ITagService _tagService;

    public TagController(ITagService TagService)
    {
        _tagService = TagService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags(
        Guid businessId,
        [FromQuery] GetAllTagsRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _tagService
            .GetAllTagsAsync(businessId, request);

        return Ok(result);
    }

    [HttpGet("{tagId:guid}", Name = nameof(GetTagById))]
    public async Task<IActionResult> GetTagById(
        Guid businessId, Guid tagId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOrAdmin);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _tagService
            .GetTagByIdAsync(businessId, tagId);

        return result.Match(Ok, Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(
        Guid businessId,
        [FromBody] CreateTagRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _tagService
            .CreateTagAsync(businessId, request);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtRoute(
            nameof(GetTagById),
            new { businessId, tagId = result.Value!.Id },
            result.Value);
    }

    [HttpPut("{tagId:guid}")]
    public async Task<IActionResult> UpdateTag(
        Guid businessId,
        Guid tagId,
        [FromBody] UpdateTagRequest request)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _tagService
            .UpdateTagAsync(businessId, tagId, request);

        return result.Match(Ok, Problem);
    }

    [HttpDelete("{tagId:guid}")]
    public async Task<IActionResult> DeleteTag(
        Guid businessId, Guid tagId)
    {
        var authorizationResult = await AuthorizeForResource(
            businessId, Policy.OwnerOnly);
        if (authorizationResult.IsError)
            return Problem(authorizationResult.Errors);

        var result = await _tagService
            .DeleteTagAsync(businessId, tagId);

        return result.Match(Ok, Problem);
    }
}