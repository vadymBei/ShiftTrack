using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using User.Authentication.Core.Application.Common.Interfaces;

namespace User.Authentication.Core.Infrastructure
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ShiftTrack.Authentication.Models.User>, 
        IApplicationDbContext
    {
        public ApplicationDbContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
