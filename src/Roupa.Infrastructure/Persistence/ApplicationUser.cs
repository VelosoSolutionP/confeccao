using Microsoft.AspNetCore.Identity;

namespace Roupa.Infrastructure.Persistence;

public class ApplicationUser : IdentityUser
{
    public string NomeCompleto { get; set; } = default!;
}
