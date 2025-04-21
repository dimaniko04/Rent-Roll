using RentnRoll.Core.Entities;
using System.Security.Cryptography;
using System.Text;

public record CreateTestEntityRequest(
    string Name,
    string Secret)
{
    public TestEntity ToDomain()
    {
        return new TestEntity
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Hidden = ComputeHash(Secret)
        };
    }

    private string ComputeHash(string secret)
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
        byte[] hashBytes;

        using (SHA256 sha256 = SHA256.Create())
        {
            hashBytes = sha256.ComputeHash(secretBytes);
        }

        return GetHashString(hashBytes);
    }

    private string GetHashString(byte[] bytes)
    {
        var builder = new StringBuilder();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
};