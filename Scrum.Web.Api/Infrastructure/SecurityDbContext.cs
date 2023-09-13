using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scrum.Web.Api.Identity;

namespace Scrum.Web.Api.Infrastructure;

public class SecurityDbContext : IdentityDbContext<ScrumUser>
{
    public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
    {
    }
}
