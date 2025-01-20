using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Demo.BusinessLogic.Interfaces;
using Demo.DataAccess.Models;
using Demo.Presentation.Helpers;
using Demo.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Presentation.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork; 
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue)) 
                 employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            else
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
                
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);

        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
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

                employeeVM.ImageName = DocumentSettings.UplaodFile(employeeVM.Image,"Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Details(int? id) {
            if (id is null)
                return BadRequest(); //status code 400

            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(MappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(MappedEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id) //for security
            {
                return BadRequest();
            }

            if (ModelState.IsValid) ///check server validation
            {
                try
                {
                    if(employeeVM.ImageName is not null)
                    {
                        employeeVM.ImageName = DocumentSettings.UplaodFile(employeeVM.Image, "Images");
                    }
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest(); //status code 400
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(MappedEmployee);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                var res = await _unitOfWork.CompleteAsync();
                //delete image
                if (res > 0 && employeeVM.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                }
                
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
