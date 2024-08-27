# Employee Management Web Application

## Overview

This project is a web-based application built using ASP.NET MVC. It allows users to import, manage, and interact with employee data stored in a SQL Server database. The application provides functionalities for importing employees from a CSV file, editing and deleting employee records directly from a DataTable-powered grid, and displaying meaningful error messages with logging and middleware handling.

## Features

### 1. CSV File Import

- Users can upload a CSV file containing employee data.
- The application parses the file, inserts the data into the SQL Server database, and displays the newly added employees in a table.
- Feedback is provided to the user on how many rows were successfully processed.

### 2. Data Management

- **Edit:** Users can edit employee data directly within the grid. The grid allows inline editing of employee details such as Last Name, First Name, Email, and other fields.
- **Delete:** Users can delete employee records from the grid. Upon deletion, the corresponding record is removed from the database.

### 3. Rich User Interaction with DataTables

- The application uses DataTables for displaying employee data.
- **Sorting:** Columns can be sorted in ascending or descending order.
- **Searching:** Users can search through the data displayed in the grid.
- **Pagination:** DataTables automatically handles pagination to improve the performance of data display.
- **Localization:** The grid supports Russian localization, ensuring the interface is user-friendly for Russian-speaking users.

### 4. Middleware and Error Handling

- The application uses custom middleware to handle errors globally. This middleware intercepts exceptions and logs them using Serilog, providing users with clear error messages and ensuring that the application continues running smoothly.
- A centralized error-handling strategy is implemented to avoid repetitive try-catch blocks scattered throughout the code.

### 5. Logging and Caching

- **Logging:** Serilog is integrated for logging important application events and errors, making it easier to diagnose and troubleshoot issues.
- **Caching:** The application utilizes caching mechanisms to reduce database load and speed up page load times by storing frequently accessed data in memory.

## Installation and Setup

### 1. Prerequisites

- .NET SDK 6.0 or higher
- SQL Server (any version compatible with Entity Framework Core)
- Visual Studio or any preferred IDE for .NET development

### 2. Database Setup

- Ensure SQL Server is installed and running.

- Create a new database (e.g., `EmployeeDB`).

- Run the Entity Framework Core migrations to set up the 

  ```
  Employees
  ```

   table. If you haven't set up migrations yet, you can add a migration and update the database using the following commands in the Package Manager Console:

  ```
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

- Update the connection string in 

  ```
  appsettings.json
  ```

   to point to your SQL Server instance:

  ```
  "ConnectionStrings": {
    "EmployeeDbContext": "Server=YOUR_SERVER_NAME;Database=EmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
  ```

### 3. Dependencies

- Restore NuGet packages:

  ```
  dotnet restore
  ```

- Ensure the following packages are installed via NuGet:

  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.SqlServer
  - Microsoft.EntityFrameworkCore.Tools
  - Serilog.AspNetCore
  - Serilog.Sinks.File

### 4. Running the Application

- Build and run the application:

  ```
  dotnet run
  ```

- Open a browser and navigate to `https://localhost:5001` (or the appropriate port configured in your `launchSettings.json`).

## Project Structure

### 1. Controllers

- **EmployeeController.cs**: Handles all the HTTP requests related to employees, including importing data, updating records, and deleting employees.

### 2. Models

- **Employee.cs**: Represents the data structure for an employee, including fields such as `LastName`, `FirstName`, `Email`, `PhoneNumber`, `Mobile`, etc.

### 3. Views

- **Employee/Index.cshtml**: Main view displaying the employee grid with options to import, edit, and delete employee data.

### 4. Middleware

- **ErrorHandlingMiddleware.cs**: Custom middleware that intercepts exceptions globally, logs them using Serilog, and displays user-friendly error pages.

### 5. Services

- **EmployeeService.cs**: Contains business logic for processing employee data, including methods for importing, updating, and deleting employees.

### 6. Repositories

- **EmployeeRepository.cs**: Implements data access logic, interacting with the SQL Server database through Entity Framework Core.

### 7. Static Files

- **wwwroot/js/russia/ru.json**: JSON file containing Russian translations for DataTables, ensuring the UI is fully localized.





## Detailed Features and Implementations

### 1. Inline Editing and Updates

- Employee data is editable directly in the DataTables grid.
- Each row includes an "Edit" button, which allows users to modify the data.
- Upon clicking "Save," the changes are sent to the server via AJAX and saved to the database.

### 2. Adding New Employees

- A button labeled "Create" allows users to add a new row to the table.
- The user can fill in the details directly in the grid, and clicking "Save" will send the data to the server to create a new employee record.

### 3. Data Validation

- The application performs server-side validation on the incoming data to ensure it meets the required format before saving it to the database.

### 4. Deletion with Confirmation

- Users can delete employees directly from the table. A confirmation prompt ensures that records are not deleted accidentally.

### 5. Custom Error Pages

- In case of an error, users are redirected to a custom error page that provides details about the issue. This ensures that users are informed of any problems without exposing sensitive server information.

### 6. Logging and Monitoring

- All critical operations and exceptions are logged using Serilog, which writes logs to a file. This is crucial for monitoring application performance and debugging issues.

### Future Enhancements

- **User Authentication and Authorization**: Implementing roles to restrict access to certain actions (e.g., only administrators can delete or update records).
- **Enhanced UI/UX**: Improving the front-end experience with additional interactive elements and user feedback.
- **Automated Testing**: Expanding the test coverage with unit and integration tests to ensure the reliability of the application.





## Conclusion

This Employee Management Web Application is designed with real-world use in mind, offering robust features for managing employee data. The combination of ASP.NET MVC, Entity Framework Core, and DataTables, along with best practices such as logging, error handling, and caching, makes this application a solid foundation for further development and deployment in a production environment.