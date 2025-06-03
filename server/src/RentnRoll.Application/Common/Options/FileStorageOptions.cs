namespace RentnRoll.Application.Common.Options;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";

    public string UploadRoot { get; set; } = null!;
    public string UploadSubFolder { get; set; } = null!;
}