using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
using Demo.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;
        public DepartmentController(IDepartmentRepository departmentRepo) // Ask CLR for Creating object from class implement Interface IDepartmentRepository
        {
            //_departmentRepo = new DepartmentRepository();
            _departmentRepo = departmentRepo;
        }

        //get all Departments
        //BaseURL/Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepo.GetAll(); 
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid) ///check server validation
            {
                _departmentRepo.Add(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        ///BaseURL/Department/Details/id
        public IActionResult Details(int? id) {
            if (id is null) 
                return BadRequest(); //status code 400
            
            var department = _departmentRepo.GetById(id.Value);
            if (department is null) 
                return NotFound();
            
            return View(department);
        }

        [HttpGet]
        public IActionResult Edit(int? id) {
            if (id is null)
                return BadRequest(); //status code 400
            var department = _departmentRepo.GetById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }

        [HttpPost]
        public IActionResult Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id) //for security
            {
                return BadRequest();
            }

            if (ModelState.IsValid) ///check server validation
            {
                try
                {
                    _departmentRepo.Update(department);
                    return RedirectToAction(nameof(Index));
                }catch(System.Exception ex)
                {
                    //1. Log Exeption
                    //2. view in form
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
                
            }
            return View(department);
        }
    }
}
