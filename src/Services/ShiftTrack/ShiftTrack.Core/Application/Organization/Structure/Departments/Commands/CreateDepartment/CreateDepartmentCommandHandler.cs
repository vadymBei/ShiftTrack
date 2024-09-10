using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, DepartmentVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _applicationDbContext;

        public CreateDepartmentCommandHandler(
            IMapper mapper,
            IApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<DepartmentVM> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {

            var department = new Department()
            {
                Name = request.Name
            };

            await _applicationDbContext.Departments.AddAsync(department, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DepartmentVM>(department);
        }
    }
}
