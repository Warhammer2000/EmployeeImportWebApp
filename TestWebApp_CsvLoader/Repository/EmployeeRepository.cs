using EmployeeImportApp.Db;
using EmployeeImportApp.Models;
using EmployeeImportApp.Repository;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly EmployeeDbContext _context;
    private readonly ILogger<EmployeeRepository> _logger;

    public EmployeeRepository(EmployeeDbContext context, ILogger<EmployeeRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<Employee> GetByIdAsync(int id)
    {
        return await _context.Employees.FindAsync(id);
    }
    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _context.Employees
            .AsNoTracking() 
            .OrderBy(e => e.LastName)
            .ToListAsync();
    }

    public async Task AddEmployeeAsync(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task AddEmployeesAsync(IEnumerable<Employee> employees)
    {
        if (employees is null || !employees.Any())
            throw new ArgumentNullException(nameof(employees), "The employees collection is null or empty.");

        await _context.Employees.AddRangeAsync(employees);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee), "The employees collection is null or empty.");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEmployeeAsync(Employee employee)
    {
        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
    }
}
