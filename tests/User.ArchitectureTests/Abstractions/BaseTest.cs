using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ShiftTrack.Domain.Features.System.Auth.Models;
using User.Authentication.Application.Common.Interfaces;
using User.Authentication.Infrastructure.Persistence;
using Assembly = System.Reflection.Assembly;

namespace User.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Token).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IApplicationDbContext).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(ApplicationDbContext).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(Program).Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            DomainAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            PresentationAssembly)
        .Build();
}