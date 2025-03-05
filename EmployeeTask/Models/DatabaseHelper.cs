using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeTask.Models
{
    public class DatabaseHelper
    {
        private readonly string connString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString;

        public int AddEmployee(Employee employee)
        {
            int employeeId = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    
                    string empQuery = @"INSERT INTO Employees (EmployeeName, Email, Phone, Department) 
                                        OUTPUT INSERTED.EmployeeID 
                                        VALUES (@Name, @Email, @Phone, @Department)";

                    using (SqlCommand cmd = new SqlCommand(empQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Name", employee.EmployeeName);
                        cmd.Parameters.AddWithValue("@Email", employee.Email);
                        cmd.Parameters.AddWithValue("@Phone", employee.Phone);
                        cmd.Parameters.AddWithValue("@Department", employee.Department);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            employeeId = Convert.ToInt32(result);
                            Console.WriteLine("Inserted EmployeeID: " + employeeId); 
                        }
                        else
                        {
                            throw new Exception("Failed to retrieve EmployeeID after insert.");
                        }
                    }

                    
                    if (employee.Dependents == null || employee.Dependents.Count == 0)
                    {
                        Console.WriteLine("No dependents found.");
                        transaction.Commit();
                        return employeeId;
                    }
                    else
                    {
                        Console.WriteLine($"Number of dependents: {employee.Dependents.Count}");
                    }

                    
                    foreach (var dep in employee.Dependents)
                    {
                        Console.WriteLine($"Inserting Dependent: {dep.DependentName}");

                        string depQuery = @"INSERT INTO Dependents (EmployeeID, DependentName, Relationship, Age) 
                                            VALUES (@EmployeeID, @DependentName, @Relationship, @Age)";

                        using (SqlCommand cmd = new SqlCommand(depQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                            cmd.Parameters.AddWithValue("@DependentName", dep.DependentName);
                            cmd.Parameters.AddWithValue("@Relationship", dep.Relationship);
                            cmd.Parameters.AddWithValue("@Age", dep.Age);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception($"Failed to insert dependent: {dep.DependentName}");
                            }
                        }
                    }

                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to error: " + ex.Message);
                    throw;
                }
            }

            return employeeId;
        }
    }
}
