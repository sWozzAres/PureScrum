using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scrum.Web.Api.Identity;

namespace Scrum.Web.Api.Infrastructure;

public class SecurityDbContext(DbContextOptions<SecurityDbContext> options) : IdentityDbContext<ScrumUser>(options)
{
}
