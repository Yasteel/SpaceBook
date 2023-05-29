namespace Spacebook.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Spacebook.Models;
    public class AuthDbContext : IdentityDbContext<SpacebookUser>
    {
   
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }
}
}


