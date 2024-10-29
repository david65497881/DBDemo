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

                // 查詢資料表中的所有數據
                var employees = connection.Query("SELECT * FROM Employees");

                Console.WriteLine("Employees Table Data:");
                foreach (var employee in employees)
                {
                    Console.WriteLine(employee);
                }
                Console.ReadLine();
            }


        }
    }
    
}
