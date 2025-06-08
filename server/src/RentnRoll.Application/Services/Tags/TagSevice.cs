using RentnRoll.Application.Common.AppErrors;
using RentnRoll.Application.Common.Interfaces.Repositories;
using RentnRoll.Application.Common.Interfaces.UnitOfWork;
using RentnRoll.Application.Common.UserContext;
using RentnRoll.Application.Contracts.Tags.CreateTag;
using RentnRoll.Application.Contracts.Tags.GetAllTags;
using RentnRoll.Application.Contracts.Tags.Response;
using RentnRoll.Application.Contracts.Tags.UpdateTag;
using RentnRoll.Application.Services.Validation;
using RentnRoll.Application.Specifications.Tags;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Tags;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITagRepository _tagRepository;
    private readonly IValidationService _validationService;
    private readonly ICurrentUserContext _currentUserContext;

    public TagService(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        ICurrentUserContext currentUserContext)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
        _currentUserContext = currentUserContext;
        _tagRepository = unitOfWork
            .GetRepository<ITagRepository>();
    }

    public async Task<ICollection<TagResponse>> GetAllTagsAsync(
        Guid businessId,
        GetAllTagsRequest request)
    {
        var specification = new GetAllTagsRequestSpec(
            businessId, request);
        var tags = await _tagRepository
            .GetAllAsync(specification);

        var tagResponses = tags
            .Select(TagResponse.FromTag)
            .ToList();

        return tagResponses;
    }

    public async Task<Result<TagDetailResponse>> GetTagByIdAsync(
        Guid businessId,
        Guid id)
    {
        var specification = new TagIdSpec(
            businessId, id);
        var tag = await _tagRepository
            .GetSingleAsync(specification);

        if (tag == null)
            return Errors.Tags.NotFound;

        var tagResponse = TagDetailResponse.FromTag(tag);

        return tagResponse;
    }

    public async Task<Result<TagDetailResponse>> CreateTagAsync(
        Guid businessId,
        CreateTagRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var existingTagSpec = new TagNameSpec(
            businessId, request.Name);
        var existingTag = await _tagRepository
            .GetSingleAsync(existingTagSpec);

        if (existingTag is not null)
        {
            return Errors.Tags.AlreadyExists(request.Name);
        }

        var tag = request.ToTag(businessId);
        await _tagRepository.CreateAsync(tag);
        await _unitOfWork.SaveChangesAsync();

        var tagResponse = TagDetailResponse.FromTag(tag);

        return tagResponse;
    }

    public async Task<Result<TagDetailResponse>> UpdateTagAsync(
        Guid businessId,
        Guid id,
        UpdateTagRequest request)
    {
        var validationResult = await _validationService
            .ValidateAsync(request);
        if (validationResult.IsError)
            return validationResult.Errors;

        var specification = new TagIdSpec(
            businessId, id);
        var tag = await _tagRepository
            .GetSingleAsync(specification, trackChanges: true);

        if (tag is null)
        {
            return Errors.Tags.NotFound;
        }

        request.UpdateTag(tag);
        await _unitOfWork.SaveChangesAsync();

        var tagResponse = TagDetailResponse.FromTag(tag);

        return tagResponse;
    }

    public async Task<Result> DeleteTagAsync(
        Guid businessId, Guid id)
    {
        var specification = new TagIdSpec(
            businessId, id);
        var tag = await _tagRepository
            .GetSingleAsync(specification);

        if (tag is null)
            return Result.Failure([Errors.Tags.NotFound]);

        _tagRepository.Delete(tag);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}