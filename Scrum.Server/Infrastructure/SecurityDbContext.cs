using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scrum.Server.Identity;

namespace Scrum.Server.Infrastructure;

public class SecurityDbContext : IdentityDbContext<ScrumUser>
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {
    }
}
