using AipsCore.Domain.Common.Validation;
using AipsCore.Domain.Models.User.Validation;

namespace AipsCore.Domain.Models.User.External;

public record UserRole
{
    public string Name { get; init; }

    private UserRole(string Name)
    {
        this.Name = Name;
    }
    
    public static UserRole User => new("User");
    public static UserRole Admin => new("Admin");
    
    public static IEnumerable<UserRole> All() => [User, Admin];

    public static UserRole? FromString(string name)
    {
       return All().FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}