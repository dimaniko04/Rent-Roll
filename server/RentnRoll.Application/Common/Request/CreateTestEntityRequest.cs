using RentnRoll.Core.Entities;
using System.Security.Cryptography;
using System.Text;

namespace RentnRoll.Application.Common.Request;

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
            Hidden = ComputeHash()
        };
    }

    private string ComputeHash()
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(Secret);
        byte[] bytes;

        using (SHA256 sha256 = SHA256.Create())
        {
            bytes = sha256.ComputeHash(secretBytes);
        }

        var builder = new StringBuilder();

        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }

        return builder.ToString();
    }

};