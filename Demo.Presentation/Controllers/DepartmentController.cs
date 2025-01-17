using AutoMapper;
using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
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
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid) ///check server validation
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Add(MappedDepartment);
                int result = _unitOfWork.Complete();

                if (result > 0)
                    TempData["Message"] = "Department Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }


        ///BaseURL/Department/Details/id
        public IActionResult Details(int? id) {
            if (id is null) 
                return BadRequest(); //status code 400
            
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department is null) 
                return NotFound();
            var MappedDepartment = _mapper.Map<Department,DepartmentViewModel>(department);
            
            return View(MappedDepartment);
        }

        [HttpGet]
        public IActionResult Edit(int? id) {
            if (id is null)
                return BadRequest(); //status code 400
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(MappedDepartment);
        }

        [HttpPost]
        public IActionResult Edit(DepartmentViewModel departmentVM, [FromRoute] int id)
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
                    _unitOfWork.Complete();
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

        public IActionResult Delete(int? id) {
            if (id is null)
                return BadRequest(); //status code 400
            var department = _unitOfWork.DepartmentRepository.GetById(id.Value);
            if (department is null)
                return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(MappedDepartment);
        }

        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentVM, [FromRoute] int id)
        {
            if (id != departmentVM.Id) {
                return BadRequest();
            }
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                _unitOfWork.DepartmentRepository.Delete(MappedDepartment);
                _unitOfWork.Complete();
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
