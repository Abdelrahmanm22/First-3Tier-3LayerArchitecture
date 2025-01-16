using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Models;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _empolyeeRepo;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository empolyeeRepo,IDepartmentRepository departmentRepo,IMapper mapper)
        {
            _empolyeeRepo = empolyeeRepo;
            _departmentRepo = departmentRepo;
            _mapper = mapper;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue)) 
                 employees = _empolyeeRepo.GetAll();
            else
                employees = _empolyeeRepo.GetEmployeesByName(SearchValue);
                
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);

        }

        public IActionResult Create()
        {
            ViewBag.Departments =  _departmentRepo.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                ///Munual Mapping (not recommended)
                ///var MappedEmployee = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Age = employeeVM.Age,
                ///    Address = employeeVM.Address,
                ///    PhoneNumber = employeeVM.PhoneNumber,
                ///    DepartmentId = employeeVM.DepartmentId,
                ///};
                ///using casting
                ///Employee employee = (Employee)employeeVM; //need to implement operator overloading in Model Employee!!!!!

                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _empolyeeRepo.Add(MappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public IActionResult Details(int? id) {
            if (id is null)
                return BadRequest(); //status code 400

            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(MappedEmployee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            ViewBag.Departments = _departmentRepo.GetAll();
            return View(MappedEmployee);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id) //for security
            {
                return BadRequest();
            }

            if (ModelState.IsValid) ///check server validation
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _empolyeeRepo.Update(MappedEmployee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    //1. Log Exeption
                    //2. view in form
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = _empolyeeRepo.GetById(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(MappedEmployee);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _empolyeeRepo.Delete(MappedEmployee);
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception ex)
            {
                //1. Log Exeption
                //2. view in form
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);
            }
        }
    }
}
