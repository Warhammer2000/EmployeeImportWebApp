using EmployeeImportApp.Models;

namespace EmployeeImportApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int id);
        Task<List<Employee>> GetEmployeesAsync();
        Task AddEmployeeAsync(Employee employee);
        Task AddEmployeesAsync(IEnumerable<Employee> employees);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteAsync(Employee employee);
    }
}
