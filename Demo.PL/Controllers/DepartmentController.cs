using Microsoft.AspNetCore.Mvc;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
using AutoMapper;
using Demo.PL.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private IDepartmentRepository _departmentRepository; // Null

        private readonly IMapper mapper;

        public DepartmentController(/*IDepartmentRepository departmentRepository*/IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR Create Object From DepartmentRepository
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;

            this.mapper = mapper;
        }
        //One Action Or One View -> يعرضلي كل الاقسام الي عندي 
        public async Task<IActionResult> Index(string SearchInput)
        {
            var department = Enumerable.Empty<Department>();
            if (string.IsNullOrEmpty(SearchInput))
            {
                department =await _unitOfWork.DepartmentRepository.GetAllAsync();
            }
            else
            {
                department = _unitOfWork.DepartmentRepository.GetByName(SearchInput.ToLower());
            }
            var result = mapper.Map<IEnumerable<DepartmentViewModel>>(department);
            return View(result);
        }

        //Action for Create Department
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = mapper.Map<Department>(model);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        /*
         * Action في اكتر من  Details علشان بستخدم نفس الكود الي 
         * { string ViewName = "Details" } اسمه  parameter فقدر من خلال اني ادي لاول اكشن عندي اديها 
         * باسم الاكشن الاولي Default value واديته 
         * { return Details(id, "Edit") } وبعد كده بروح لكل الاكشن التانيه وبقوله 
         * ID,ViewName وانا فوق محدد انها بتاخد حاجتين  Details بس للاكشن الي اسمها  return بعمل 
         * Edit وينفذ لما يتنادي علي  Details الي جوه بس من خلال ال Logic فيخش يستخد ال
         */
        public async Task<IActionResult> Details(int? id , string ViewName = "Details")
        {
            if(id  == null)
            {
                return BadRequest();
            }
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            var result = mapper.Map<DepartmentViewModel>(department);
            return View(ViewName, result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepository.Get(id.Value);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            // return View(department);
            return await Details(id, "Edit");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id,DepartmentViewModel model)
        {
           var department = mapper.Map<Department>(model);
            _unitOfWork.DepartmentRepository.Update(department);
            var Count =await _unitOfWork.CompleteAsync();
            if (Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRepository.Get(id.Value);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //return View(department);

            return await Details (id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel model)
        {
            var result = mapper.Map<Department>(model);
             _unitOfWork.DepartmentRepository.Delete(result);
            var Count = await _unitOfWork.CompleteAsync();
            if (Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
