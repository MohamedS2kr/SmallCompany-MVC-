using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        // private IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork; // main Repository and he Contain all of Repository
        private readonly IMapper mapper;

        //private IDepartmentRepository _DepartmentRepository;
        public EmployeeController(IUnitOfWork unitOfWork ,IMapper mapper/*, IDepartmentRepository departmentRepository*//*IEmployeeRepository employeeRepository*/)
        {
            
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            //_DepartmentRepository = departmentRepository;
        }
        /*
         * View في ال employee لو عايز ابعت داتا تانيه غير ال 
         * View الي  Action من ال Data  و بستخدمهم علشان انقل 
         * View`s Dictionary  وده بيتم من خلال Extra Information  زي ماتقول كده عايز انقل 
         * View`s Dictionary : Controller انا بورثهم من خلال الكلاس الي اسمه  property عندي 3
         *
         * 1.ViewData -> Property Inherited From Class Controller : Dictionary
         *  ViewData["Message"] = "Hello ViewData";
         * 
         * 2.ViewBag  -> Property Inherited From Class Controller : dynamic -> detect datatype in run time 
         * ViewBag.Hamada = "Hello ViewBag";
         * 
         * 3.TempData -> Transfer Information From Request To Another .. View الي View الي هو بنقل داتا من
         * TempData["Message"] = "Employee Added!";
         */
        public async Task<IActionResult> Index(string SearchInput)
        {
            var employee = Enumerable.Empty<Employee>();
            
            if (string.IsNullOrEmpty(SearchInput))
            {
                employee = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employee =  _unitOfWork.EmployeeRepository.GetByName(SearchInput.ToLower());
            }
            /*
             * View`s Dictionary : Controller انا بورثهم من خلال الكلاس الي اسمه  property عندي 3
             * 1.ViewData -> Property Inherited From Class Controller : Dictionary
             * ومكان ما هحطها هيجيبلي القيمه @ViewData["Message"]  هقوله عندك  View ولو عايز اتعامل معاها في  
             //ViewData["Message"] = "Hello ViewData";

            //ViewData["Message"] = _employeeRepository.GetAll();

            // 2. ViewBag -> Property Inherited From Class Controller : dynamic -> detect datatype in run time

            //ViewBag.Hamada = "Hello ViewBag";
             
             
             */

            var result = mapper.Map<IEnumerable<EmployeeViewModel>>(employee);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] = _DepartmentRepository.GetAll();
            
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Create(EmployeeViewModel model)
        {
            if(ModelState.IsValid)
            {

                string FileName =  DocumentSettings.UploadFile(model.Image, "Images");
                model.ImageName = FileName;
                var employee = mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                int count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Added!";
                }
                else
                {
                    TempData["Message"] = "Employee Dose Not Added!";
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id,string ViewName ="Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            var result = mapper.Map<EmployeeViewModel>(employee);
            if (employee == null)
            {
                return NotFound();
            }
            return View(ViewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //ViewData["Departments"] = _DepartmentRepository.GetAll();

            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel model)
        {

            if (model is not null)
            {
                string FileName = DocumentSettings.UploadFile(model.Image, "Images");
                model.ImageName = FileName;
                var employee = mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(employee);
                int count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel model)
        {
            
            var employee = mapper.Map<Employee>(model);
            _unitOfWork.EmployeeRepository.Delete(employee);
            int count =await _unitOfWork.CompleteAsync();
            if (count > 0 )
            {
                if(model.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
