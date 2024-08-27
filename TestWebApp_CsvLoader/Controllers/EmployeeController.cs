using EmployeeImportApp.Models;
using EmployeeImportApp.Repository;
using EmployeeImportApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace EmployeeImportApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(EmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        [OutputCache(Duration = 600)]
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetEmployeesAsync();
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ImportResult"] = "No file selected.";
                return RedirectToAction("Index");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var processedCount = await _employeeService.ImportEmployeesAsync(stream);
                    TempData["ImportResult"] = $"{processedCount} rows were successfully processed.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the import process.");
                TempData["ImportResult"] = "An error occurred during the import process.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
               await _employeeService.AddEmployeeAsync(employee);
                TempData["Success"] = "Новый сотрудник успешно создан.";
            }
            else
            {
                TempData["Error"] = "Ошибка при создании нового сотрудника.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee is null)
            {
                return NotFound();
            }
            employee.IsEditing = !employee.IsEditing;
            await _employeeService.UpdateEmployeeAsync(employee);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Dictionary<string, string> updatedData)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            foreach (var key in updatedData.Keys)
            {
                var value = updatedData[key];
                switch (key)
                {
                    case nameof(Employee.LastName):
                        employee.LastName = value;
                        break;
                    case nameof(Employee.FirstName):
                        employee.FirstName = value;
                        break;
                    case nameof(Employee.Email):
                        employee.Email = value;
                        break;
                    case nameof(Employee.PhoneNumber):
                        employee.PhoneNumber = value;
                        break;
                    case nameof(Employee.Mobile):
                        employee.Mobile = value;
                        break;
                    case nameof(Employee.Address):
                        employee.Address = value;
                        break;
                    case nameof(Employee.Postcode):
                        employee.Postcode = value;
                        break;
                    case nameof(Employee.DateOfBirth):
                        employee.DateOfBirth = value;
                        break;
                    case nameof(Employee.StartDate):
                        employee.StartDate = value;
                        break;
                }
            }
            employee.IsEditing = false;
            await _employeeService.UpdateEmployeeAsync(employee);
          
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
               throw new ArgumentNullException(nameof(employee));
            }

            await _employeeService.DeleteEmployeeAsync(id);

            TempData["Success"] = "Сотрудник успешно удален!";
            return RedirectToAction(nameof(Index));
        }
    }
}
