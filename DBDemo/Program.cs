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

                // 建立 Employees 資料表
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    id INTEGER PRIMARY KEY,
                    name VARCHAR(100),
                    salary INTEGER,
                    managerId INTEGER
                );";

                connection.Execute(createTableQuery);
            }

            Console.WriteLine("資料庫和資料表建立成功。");
            Console.ReadLine(); 
        }
    }
    
}
