using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager , IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SerchValue)
		{
			if(string.IsNullOrWhiteSpace(SerchValue))
			{
				var users = await _userManager.Users.Select(
					 U => new UserViewModel()
					 {
						 Id = U.Id,
						 FName = U.Fname,
						 LName = U.Lname,
						 Email = U.Email,
						 PhoneNumber = U.PhoneNumber,
						 Roles =  _userManager.GetRolesAsync(U).Result,
					 }).ToListAsync();

				return View(users);
			}
			else
			{
				var Users  = await _userManager.FindByEmailAsync(SerchValue);
				var MappedUser = new UserViewModel()
				{
					Id = Users.Id,
					FName = Users.Fname,
					LName = Users.Lname,
					Email = Users.Email,
					PhoneNumber = Users.PhoneNumber,
					Roles = _userManager.GetRolesAsync(Users).Result,
				};
				return View(new List<UserViewModel> { MappedUser });
			}
		}

		public async Task<IActionResult> Details(string id , string ViewName = "Details")
		{
            if (id is null)
				return BadRequest();
			var users = await _userManager.FindByIdAsync(id);
			if(users is null)
				return NotFound();
			var MappedUser = _mapper.Map<ApplicationUser,UserViewModel>(users);
			return View(ViewName, MappedUser);
        }
		[HttpGet]
		public async Task<IActionResult> Edit(string Id)
		{
			return await Details(Id,"Edit");
		}
		[HttpPost]
		public async Task<IActionResult> Edit (UserViewModel model,[FromRoute]string Id) 
		{ 
			if( Id != model.Id)
				return BadRequest();
			
			if(ModelState.IsValid)
			{
				try
				{
					var User = await _userManager.FindByIdAsync(Id);
					User.PhoneNumber = model.PhoneNumber;
					User.Fname = model.FName;
					User.Lname = model.LName;

					await _userManager.UpdateAsync(User);
					return RedirectToAction(nameof(Index));
				}
				catch(Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}
			return View(model);
		}

		public async Task<IActionResult> Delete ([FromRoute] string id)
		{
			return await Details(id, "Delete");
		}
		[HttpPost]
		public async Task< IActionResult> ConfirmDelete(string id)
		{
			try
			{
				var user =await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch(Exception ex) 
			{
                ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
            }
        }
	}

}
