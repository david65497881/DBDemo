using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using System.IO;

namespace DBDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=database.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 查詢非管理職員工
                var nonManagerialEmployees = connection.Query<string>("SELECT name FROM Employees WHERE managerId IS NOT NULL");

                Console.WriteLine("非管理職員工：");
                foreach (var employee in nonManagerialEmployees)
                {
                    Console.WriteLine(employee);
                }
                Console.ReadLine();
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 查詢非管理職且薪資高於其主管的員工
                var result = connection.Query<string>(@"
                SELECT e1.name AS Employee
                FROM Employees e1
                JOIN Employees e2 ON e1.managerId = e2.id
                WHERE e1.salary > e2.salary
            ");

                Console.WriteLine("非管理職且薪資高於其主管的員工：");
                foreach (var employee in result)
                {
                    Console.WriteLine(employee);
                }
                Console.ReadLine();
            }

        }
    }
    
}
