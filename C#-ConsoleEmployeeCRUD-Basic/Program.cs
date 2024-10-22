using System;
using Microsoft.Data.Sqlite;

namespace EmployeeCRUDApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("\n ----Employee Management----");
                Console.WriteLine("1.Create Employee");
                Console.WriteLine("2.List Employees");
                Console.WriteLine("3.Update Employee");
                Console.WriteLine("4.Delete Employee");
                Console.WriteLine("5.Exit");
                Console.Write("select an option: ");
                string option = Console.ReadLine();
                using (var connection = OpenConnection())
                {
                    switch (option)
                    {
                        case "1":
                            Console.Clear();
                            CreateEmployee(connection);
                            break;

                        case "2":
                            Console.Clear();
                            ListEmployees(connection);
                            break;

                        case "3":
                            Console.Clear();
                            UpdateEmployee(connection);
                            break;

                        case "4":
                            Console.Clear();
                            DeleteEmployee(connection);
                            break;

                        case "5":

                            exit = true;
                            break;

                        default:

                            Console.WriteLine("Invalid option.Please try again.");
                            break;

                    }

                    if (!exit)
                    {
                        Console.WriteLine("\n Press Enter to continue....");
                        Console.ReadLine();
                    }
                }
            }
        }

        static SqliteConnection OpenConnection()
        {
            var connection = new SqliteConnection("Data Source=employee.db;");
            connection.Open();
            string CreateTableQuery = @"CREATE TABLE IF NOT EXISTS Employees(
                EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                DateOfBirth TEXT NOT NULL)";

            using (var command = new SqliteCommand(CreateTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
            return connection;
        }


        //create Employee
        static void CreateEmployee(SqliteConnection connection)
        {
            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter Last Name:");
            string lastName = Console.ReadLine();
            Console.Write("Enter Date of Birth (yyyy-mm-dd):");
            string dob = Console.ReadLine();

            string insertQuery = "INSERT INTO Employees(FirstName,LastName,DateOfBirth) VALUES(@FirstName,@LastName,@DateOfBirth)";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@DateOfBirth", dob);
                command.ExecuteNonQuery();
            }

           
            Console.WriteLine("Employee Created Successfully");
        }

        private static void CreateEmployee(SqliteConnection connection, string? firstName, string? lastName, string? dob)
        {
            throw new NotImplementedException();
        }


        //list of All Employees
        static void ListEmployees(SqliteConnection connection)
        {
            string selectQuery = "SELECT * FROM Employees";
            using (var command = new SqliteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("\n----Employee List ----");
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["EmployeeID"]}, Name: {reader["FirstName"]}{reader["LastName"]},DOB: {reader["DateOfBirth"]}");
                    }
                }
            }
        }

        //Update Employee
        static void UpdateEmployee(SqliteConnection connection)
        {
            Console.Write("Enter Employee ID to update: ");
            int employeeID = int.Parse(Console.ReadLine());
            Console.Write("Enter New First Name: ");
            string newFirstName = Console.ReadLine();
            Console.Write("Enter New Last Name: ");
            string newLastName = Console.ReadLine();
            string updateQuery = @"UPDATE Employees
                             SET FirstName=@FirstName,LastName=@LastName 
                             WHERE EmployeeID=@EmployeeID";

            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@FirstName", newFirstName);
                command.Parameters.AddWithValue("@LastName", newLastName);
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee updated successfully");

                }
                else
                {
                    Console.WriteLine("Employee not found");
                }
            }
        }


        //Delete Employee
        static void DeleteEmployee(SqliteConnection connection)
        {
            Console.Write("Enter Employee ID to delete: ");
            int employeeID = int.Parse(Console.ReadLine());
            string deleteQuery = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
            using (var command = new SqliteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", employeeID);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee deleted successfully");
                }
                else
                {
                    Console.WriteLine("Employee not found");
                }
            }
        }
    }
}