using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _empolyeeRepo;

        public EmployeeController(IEmployeeRepository empolyeeRepo)
        {
            _empolyeeRepo = empolyeeRepo;
        }
        public IActionResult Index()
        {
            var employees = _empolyeeRepo.GetAll();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _empolyeeRepo.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Details(int? id) {
            if (id is null)
                return BadRequest(); //status code 400

            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee, [FromRoute] int id)
        {
            if (id != employee.Id) //for security
            {
                return BadRequest();
            }

            if (ModelState.IsValid) ///check server validation
            {
                try
                {
                    _empolyeeRepo.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1. Log Exeption
                    //2. view in form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employee);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();
            return View(employee);
        }

        [HttpPost]
        public IActionResult Delete(Employee employee, [FromRoute] int id)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            try
            {
                _empolyeeRepo.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1. Log Exeption
                //2. view in form
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employee);
            }
        }
    }
}
