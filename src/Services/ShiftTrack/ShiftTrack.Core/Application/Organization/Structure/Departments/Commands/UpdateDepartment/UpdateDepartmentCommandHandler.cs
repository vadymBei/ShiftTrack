using AutoMapper;
using Kernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentVM>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _dbContext;

        public UpdateDepartmentCommandHandler(
            IMapper mapper,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<DepartmentVM> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _dbContext.Departments
               .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (department == null)
                throw new EntityNotFoundException(typeof(Department), request.Id);

            department.Name = request.Name;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DepartmentVM>(department);
        }
    }
}
