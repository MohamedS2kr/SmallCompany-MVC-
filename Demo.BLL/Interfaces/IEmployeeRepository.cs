using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Models;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepositor<Employee>
    {
        IEnumerable<Employee> GetByName(string name);
    }
}
