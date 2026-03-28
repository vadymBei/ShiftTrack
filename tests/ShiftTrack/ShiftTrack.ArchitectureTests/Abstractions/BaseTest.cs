using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Common.Abstractions;
using ShiftTrack.Infrastructure.Persistence;
using Assembly = System.Reflection.Assembly;

namespace ShiftTrack.ArchitectureTests.Abstractions;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(AuditableEntity).Assembly;
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