The College Management System is a web-based application built using ASP.NET Core MVC and Entity Framework Core (MySQL provider).
It helps manage different roles within a college—SuperAdmin, Admin (HOD), Professor, and Student—and supports features like:

Role-based login and dashboards
Adding professors and assigning subjects
Creating and submitting assignments
Tracking assignment completion status
Secure authentication using ASP.NET Identity
MySQL database integration via XAMPP
 Installation Steps
1. Prerequisites



Make sure the following tools are installed:

.NET 7 SDK or later
XAMPP (to run MySQL)
A code editor (Visual Studio or VS Code)
Internet connection (for NuGet package restore)
2. Clone or Download



Download this project or clone it using Git:

git clone https://github.com/your-username/CollegeManagementSystem.git
cd CollegeManagementSystem/CollegeManagementSystem

3. Set up MySQL Database
Start XAMPP and make sure MySQL is running.
Open phpMyAdmin.
Create a new database named college_db.
Update the connection string in appsettings.json if needed:
"ConnectionStrings": {
  "DefaultConnection": "server=127.0.0.1;port=3306;database=college_db;user=root;password=;"
}

(Leave password blank if your MySQL root has no password.)
4. Install Dependencies



Open the terminal in the project directory and run:

dotnet restore

5. Apply Entity Framework Migrations



Generate the database tables automatically:

dotnet ef migrations add InitialCreate
dotnet ef database update




If you don’t have the EF CLI tools installed:

dotnet tool install --global dotnet-ef

🚀 Running the Project



Once setup is done, run:

dotnet run




You’ll see an output like:

Now listening on: http://localhost:5000




Open your browser and visit:
 http://localhost:5000

 Default SuperAdmin Login
Email	Password
superadmin@college.local	Super@123



Use this account to:

Add HODs (Admins)
Manage professors
View system users
 Project Structure
CollegeManagementSystem/
│
├── Controllers/
├── Models/
├── Data/
├── Views/
├── ViewModels/
├── wwwroot/
│   └── uploads/
├── appsettings.json
└── Program.cs
