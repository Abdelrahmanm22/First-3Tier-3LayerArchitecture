using Demo.BusinessLogic.Interfaces;
using Demo.BusinessLogic.Repositories;
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
    }
}
