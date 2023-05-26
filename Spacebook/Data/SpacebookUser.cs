using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Spacebook.Data;

public class SpacebookUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public string? Bio { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }
}

