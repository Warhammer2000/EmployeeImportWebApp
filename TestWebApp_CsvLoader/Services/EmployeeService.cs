using EmployeeImportApp.Models;
using EmployeeImportApp.Repository;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace EmployeeImportApp.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IMemoryCache cache, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _cache = cache;
            _logger = logger;
        }
        public async Task<int> ImportEmployeesAsync(Stream fileStream)
        {
            if (fileStream is null)
            {
                _logger.LogWarning("ImportEmployeesAsync: fileStream is null. Exiting method.");
                return 0;
            }

            var employees = new List<Employee>();

            _logger.LogInformation("ImportEmployeesAsync: Starting to read from the file stream.");

            using (var reader = new StreamReader(fileStream))
            {
                await reader.ReadLineAsync(); 

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');

                    var employee = new Employee
                    {
                        PayrollNumber = values[0],
                        FirstName = values[1],
                        LastName = values[2],
                        DateOfBirth = values[3],
                        PhoneNumber = values[4],
                        Mobile = values[5],
                        Address = values[6],
                        Address2 = values[7],
                        Postcode = values[8],
                        Email = values[9],
                        StartDate = values[10]
                    };

                    employees.Add(employee);
                }
            }

            _logger.LogInformation("ImportEmployeesAsync: Successfully read {EmployeeCount} employees from the file.", employees.Count);
            await _employeeRepository.AddEmployeesAsync(employees);
            _logger.LogInformation("ImportEmployeesAsync: Successfully added employees to the repository.");
            return employees.Count;
        }
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _employeeRepository.GetEmployeesAsync();
        }
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            _logger.LogInformation("GetEmployeeByIdAsync: Retrieving employee with ID {EmployeeId} from the repository.", id);

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("GetEmployeeByIdAsync: Employee with ID {EmployeeId} not found.", id);
            }
            return employee;
        }
        public async Task AddEmployeeAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await _employeeRepository.AddEmployeeAsync(employee);
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await _employeeRepository.UpdateEmployeeAsync(employee);
        }
        public async Task DeleteEmployeeAsync(int id)
        {
            _logger.LogInformation("DeleteEmployeeAsync: Attempting to delete employee with ID {EmployeeId}.", id);

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee != null)
            {
                await _employeeRepository.DeleteAsync(employee);
                _logger.LogInformation("DeleteEmployeeAsync: Successfully deleted employee with ID {EmployeeId}.", id);
            }
            else
            {
                _logger.LogWarning("DeleteEmployeeAsync: Employee with ID {EmployeeId} not found. No action taken.", id);
            }
        }
    }
}
