# CustomerRegistrationForm
# Description

This project is an application that allows an authorized (pre-defined) user to access a database and perform CRUD (Create, Read, Update, Delete) operations on registered users' records.
In addition to providing a user interface for these operations, the application also includes an API (ClientsController) that enables these actions programmatically.


# Requirements
## 1-Development Envoriment: 
Ensure that you have a suitable development environment for ASP.NET Core applications set up on your machine. You can download and install Visual Studio or use Visual Studio Code with the .NET SDK.
## 2-Database Schema and Initial Data
### I use localdatabase to make this project. You can configure your DB for 
### Database Schema

The application relies on a SQL Server database with the following schema:

- `clients` table:
  - `Id` (int, primary key): Unique identifier for clients.
  - `Name` (nvarchar(255), required): Client's name.
  - `Email` (nvarchar(255), required): Client's email address.
  - `Phone` (nvarchar(20), required): Client's phone number.
  - `Address` (nvarchar(255), required): Client's address.
  - `IdentityNumber` (nvarchar(11), required): Client's identity number.

### Initial Data

When setting up the application, you should populate the `clients` table with sample data to get started. Here's an example of initial data:

| Id | Name             | Email                | Phone       | Address               | IdentityNumber   |
|----|------------------|----------------------|-------------|-----------------------|------------------|
| 1  | John Doe         | john@example.com     | 1234567890  | 123 Main St, City     | 12345678901      |
| 2  | Jane Smith       | jane@example.com     | 9876543210  | 456 Elm St, Town      | 98765432109      |


## I used a local database to develop this application. I have outlined the fundamental requirements below. You can shape the rest according to your needs

**.NET Core SDK**: This application is built using .NET Core, so you need to have the .NET Core SDK installed on your machine. You can download it from [the official .NET website](https://dotnet.microsoft.com/download).

- **SQL Server Database**: The application relies on a SQL Server database for storing client information. You should have SQL Server installed on your local machine or have access to a SQL Server instance.

- **Database Configuration**:
  - Update the database connection string in the `appsettings.json` file of the application to match your SQL Server instance. Here's an example of the connection string format:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost\\MSSQLSERVER01;Database=mystore;Integrated Security=True"
    }
    ```
    Make sure to replace `"Server=localhost\\MSSQLSERVER01;Database=mystore;Integrated Security=True"` with the appropriate connection string for your SQL Server.

- **Sample Data**: For testing purposes, you may want to populate the `clients` table in the database with sample data. Refer to the "Database Schema and Initial Data" section for more information on the required schema and sample data.

- **JWT Secret Key**: The application uses a secret key for JWT token generation. You should replace the secret key in the `GenerateJwtToken` method of the `ClientsController` with your own secret key. Ensure that the secret key is kept confidential.
  ```csharp



## Installation

To run this project locally, follow these steps:

1. **Clone the Repository**: Start by cloning this repository to your local machine. You can do this by running the following command in your terminal:

   ```bash
   [git clone https://github.com/your-username/your-project.git](https://github.com/erkinavcii/CustomerRegistration).

Database Setup: This project uses a local database. You'll need to set up the database and update the connection string in the app's configuration. Follow these steps:

Create a local database instance.
Open the appsettings.json file in the project and update the DefaultConnection string with your database connection details, such as server name, database name, and authentication.
json
Copy code
"ConnectionStrings": {
    "DefaultConnection": "Server=your-server-name;Database=your-database-name;Trusted_Connection=True;"
}
# Build and Run:
 Once the database is set up and the connection string is updated, build and run the application using your preferred development environment. You can use Visual Studio, Visual Studio Code, or any other IDE you prefer for .NET Core development.

# API Endpoints:
 The API endpoints can be accessed at http://localhost:your-port/api/clients, where your-port is the port number your application is running on.

# Authentication: 
To use the API, you may need to authenticate. Refer to the API documentation or implementation for details on how to obtain and use authentication tokens.

# Start Developing: 
You're ready to start working with the application. Customize it according to your requirements and build your features on top of it.
#
Feel free to modify these installation instructions to match the specifics of your project. If you have any questions or encounter any issues during the setup, please don't hesitate to reach out for assistance.
