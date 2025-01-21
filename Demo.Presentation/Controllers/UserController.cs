using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Demo.DataAccess.Models;
using System.Linq;
using Demo.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
namespace Demo.Presentation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if (string.IsNullOrEmpty(SearchValue)) {
                var Users = await _userManager.Users.Select(
                u=>new UserViewModel()
                {
                    Id = u.Id,
                    FName = u.FName,
                    LName = u.LName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(u).Result,
                }).ToListAsync();
                return View(Users);
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(SearchValue);

                var MappedUser = new UserViewModel()
                {
                    Id = User.Id,
                    FName = User.FName,
                    LName = User.LName,
                    Email = User.Email,
                    PhoneNumber = User.PhoneNumber,
                    Roles = _userManager.GetRolesAsync(User).Result,
                };
                return View(new List<UserViewModel> { MappedUser });
            }
            
        }

        public async Task<IActionResult> Details(string Id, string ViewName="Details")
        {
            if (Id is null)
            {
                return BadRequest();
            }
            var User =  await _userManager.FindByIdAsync(Id);
            if (User is null) {
                return NotFound();
            }

            var MappedUser = _mapper.Map<User, UserViewModel>(User);

            return View(ViewName,MappedUser);
        }

        public async Task<IActionResult> Edit(string Id) {
            return await Details(Id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model,[FromRoute] string Id)
        {
            if (Id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid) {
                try
                {
                    //var MappedUser = _mapper.Map<UserViewModel, User>(model);
                    var User = await _userManager.FindByIdAsync(Id);
                    User.PhoneNumber = model.PhoneNumber;
                    User.FName = model.FName;
                    User.LName = model.LName;

                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty,ex.Message);
                }
                
            }
            return View(model);
        }
    }
}
