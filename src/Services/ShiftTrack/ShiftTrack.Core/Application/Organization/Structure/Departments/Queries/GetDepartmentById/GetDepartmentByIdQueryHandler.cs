using AutoMapper;
using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.Interfaces;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentVM>
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;

        public GetDepartmentByIdQueryHandler(
            IMapper mapper,
            IDepartmentService departmentService)
        {
            _mapper = mapper;
            _departmentService = departmentService;
        }

        public async Task<DepartmentVM> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentService.GetById(request.Id, cancellationToken);

            return _mapper.Map<DepartmentVM>(department);
        }
    }
}
