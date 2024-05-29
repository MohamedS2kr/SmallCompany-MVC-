using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepositor<T> where T : BaseEntity
    {
        /*
         * unit of work , Generic Repository اسمه Design pattern احنا قولنا اننا هنستخدم 
         * Generic Repository بس الي هنشتغل بيه دلوقتي هو 
         * هو نهج انت بتتبعه لحل مشكله معينه  Design pattern قالك ال
         * وعلشان نستخدم الRepository Design Pattern : 
         * Repositories - Interfaces : 2 Folder محتاج يكون عندي 
         * Repositories -> operations هحط هنا كل موديل هعمل عليه  
         * Interfaces ->  الي عندي operations لكل  Signature هحط 
         */

        private protected AppDbContext _Context;

        /*
         * Object الي هو حقنDependence injection بعتمد علي  Connection لما بجي افتح 
         * AppDbContext من  Create Object يعملي CLRانا كده بطلب من ال 
         * context في Reference ويرجعلي ال AppDbContext من Object هيروح يعملي CLR ال DepartmentRepository من Object فلما اعمل  
         * فهيستخدمه علي طول  Reference تانيه هو هيبقي فاهم انه معاه  operations فلما اعمل اي 
         *يروح يكريته ولو عنده يروح يستخدمه  Object علشان لو معندوش  CLR والي هيعمل كل ده هو 
         * Dependence injection  الي تسمحله يعمل Services مش هيعمل كده من دماغه لازم اضيف ال CLRوال
         * configuration Services في Startup ودي بتتضاف في 
         */
        public GenericRepository(AppDbContext context) //Object الي هو حقنDependence injection بعتمد علي  Connection لما بجي افتح 
        {
            _Context = context;
        }

        public async Task AddAsync(T entity)
        {
            if (entity is not null)
            {
               await _Context.AddAsync(entity);
            }
            
        }

        public void Delete(T entity)
        {
            if (entity is not null)
            {
                _Context.Remove(entity);
            }
        }

        /*
         * var result = _Context.Set<T>().Find(id); 
         * Error السطر ده كان جايب .
         *  DbSet<> الي هو واخدها دي عباره عن  T وده بسبب انه مش ضامن ان ال 
         * فضمنتله لماحطيته كشرط class لل2  parent وخليته BaseEntity فمن خلال الحركه بتاعة الكلاس الي اسمه 
         * interface , class علي ال
         */
        public async Task<T> GetAsync(int id)
        {
            var result =await _Context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee) )
            {
                return (IEnumerable<T>) await _Context.Employees.Include(E => E.Department).ToListAsync();
            }

            return await _Context.Set<T>().ToListAsync(); 
             
        }

        public void Update(T entity)
        {
            if (entity is not null)
            {
                _Context.Update(entity);
            }
           
        }
    }
}
