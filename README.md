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

- MSSQL
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
| 1  | John Doe         | john@example.com     | 1234567890  | 123 Main St, City     | 94685811754      |
| 2  | Jane Smith       | jane@example.com     | 9876543210  | 456 Elm St, Town      | 89500091428      |


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
**SecretKey="y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD"**
**var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("y9yPvg+2q3eFruhT6rGyTqApFp5PwWkD"));///32 bit long secretkey**
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
#####

# API USAGE

 ## User Authentication
- **Endpoint**  `/api/Clients/Login`
- **HTTPMethod** post
- **Description** Authenticates a user and issues a JWT token for authorization
- **Requested Parameters** 
-- **`username`**(string,required): User's name // Valid users for my code are "erkin-erkin" and "admin-admin." 
-- **`password`**(string,required): User's password
  -**Response** If authentication is succesfull, it returns a JWT token for other requests.
 ## Get Clients
- **Endpoint**  `/api/Clients/CheckToken`
- **HTTPMethod** post
- **Description** Checks validity of JWT token.
- **Requested Body** Should contain a string of JWT token.
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
- **Response** : If the token is valid, a success message is returned; otherwise, an unauthorized message is returned.

 ## Get Clients
- **Endpoint**  `/api/Clients/`
- **HTTPMethod** get
- **Description** Retrieves a list of clients from db.
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
## Get Client
- **Endpoint**  `/api/Clients/{id}`
- **HTTPMethod** get
- **Description** Retrieves a client by `ID` from db.
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
 ## Create Client
- **Endpoint**  `/api/Clients`
- **HTTPMethod** post
- **Description** Creates a new client and adds it to db.
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
- **Request Body** Should contain JSON object with valid informations of client.
## Update Client
- **Endpoint**  `/api/Clients/{id}`
- **HTTPMethod** put
- **Description** Updates an existing client's information in db.
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
- **Request Body** Should contain JSON object with updated informations of client.
 ## Delete Client
- **Endpoint**  `/api/Clients/{id}`
- **HTTPMethod** Delete
- **Description** Deletes a client from db by `ID`
- **Authorization** Requires a valid JWT token provided in `Authorization` HEADER
  
# Example API Usage 
## Updating a Client using Postman

To update a client using Postman, follow these steps:

1. Select the HTTP method "PUT."

2. Enter the URL for the client you want to update. The URL should be in the following format: `http://localhost:32387/api/Clients/1`, where `1` represents the ID of the client you want to update.

3. In the "Headers" section, add an "Authorization" header. Set its value to a string starting with "Bearer," followed by the JWT token you obtained in the previous steps.

4. Go to the "Body" section and select the "raw" option. Enter the updated client data in JSON format as shown below:

```json
{
    "name": "Updated Name",
    "email": "updated@example.com",
    "phone": "1234567890",
    "address": "Updated Address",
    "identityNumber": "40641039748"
}

Feel free to modify these installation instructions to match the specifics of your project. If you have any questions or encounter any issues during the setup, please don't hesitate to reach out for assistance.
