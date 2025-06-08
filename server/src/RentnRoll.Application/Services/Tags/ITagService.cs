using RentnRoll.Application.Contracts.Tags.CreateTag;
using RentnRoll.Application.Contracts.Tags.GetAllTags;
using RentnRoll.Application.Contracts.Tags.Response;
using RentnRoll.Application.Contracts.Tags.UpdateTag;
using RentnRoll.Domain.Common;

namespace RentnRoll.Application.Services.Tags;

public interface ITagService
{
    Task<ICollection<TagResponse>> GetAllTagsAsync(
        Guid businessId,
        GetAllTagsRequest request);

    Task<Result<TagDetailResponse>> GetTagByIdAsync(
        Guid businessId,
        Guid id);

    Task<Result<TagDetailResponse>> CreateTagAsync(
        Guid businessId,
        CreateTagRequest request);

    Task<Result<TagDetailResponse>> UpdateTagAsync(
        Guid businessId,
        Guid id,
        UpdateTagRequest request);

    Task<Result> DeleteTagAsync(
        Guid businessId,
        Guid id);
}