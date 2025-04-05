using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentVM>
{
    private readonly IMapper _mapper;
    private readonly IUnitService _unitService;
    private readonly IDepartmentService _departmentService;
    private readonly IApplicationDbContext _applicationDbContext;

    public CreateDepartmentCommandHandler(
        IMapper mapper,
        IUnitService unitService,
        IDepartmentService departmentService,
        IApplicationDbContext applicationDbContext)
    {
        _mapper = mapper;
        _unitService = unitService;
        _departmentService = departmentService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<DepartmentVM> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        await _unitService.GetById(request.UnitId, cancellationToken);

        var department = new Department()
        {
            Name = request.Name,
            UnitId = request.UnitId
        };

        await _applicationDbContext.Departments.AddAsync(department, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        department = await _departmentService.GetById(department.Id, cancellationToken);

        return _mapper.Map<DepartmentVM>(department);
    }
}