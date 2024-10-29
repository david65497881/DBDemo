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

            // 檢查資料庫檔案是否存在；如果不存在，則建立
            if (!File.Exists("./database.db"))
            {
                SQLiteConnection.CreateFile("database.db");
            }

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

        }
    }
    
}
