using System.Threading.Tasks;
using AutoMapper;
using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    //[AllowAnonymous] ==> Defualt
    //[Authorize("Admin")]
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR for Creating object from class implement Interface IDepartmentRepository
        {
            //_departmentRepo = new DepartmentRepository();
            //_departmentRepo = departmentRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //get all Departments
        //BaseURL/Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) ///check server validation
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                await _unitOfWork.DepartmentRepository.AddAsync(MappedDepartment);
                int result = await _unitOfWork.CompleteAsync();

                if (result > 0)
                    TempData["Message"] = "Department Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }


        ///BaseURL/Department/Details/id
        public async Task<IActionResult> Details(int? id) {
            if (id is null) 
                return BadRequest(); //status code 400
            
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null) 
                return NotFound();
            var MappedDepartment = _mapper.Map<Department,DepartmentViewModel>(department);
            
            return View(MappedDepartment);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) {
            if (id is null)
                return BadRequest(); //status code 400
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(MappedDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id) //for security
            {
                return BadRequest();
            }

            if (ModelState.IsValid) ///check server validation
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(MappedDepartment);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }catch(System.Exception ex)
                {
                    //1. Log Exeption
                    //2. view in form
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
                
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id is null)
                return BadRequest(); //status code 400
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(MappedDepartment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id) {
                return BadRequest();
            }
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1. Log Exeption
                //2. view in form
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(departmentVM);
            }
        }
    }
}
