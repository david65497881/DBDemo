using System;
using System.Data.SQLite;
using Dapper;
using System.IO;
using FastReport;
using FastReport.Export.PdfSimple;
using System.Linq;

namespace DBDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            // 使用完整的資料庫路徑
            string connectionString = @"Data Source=C:\Users\user\source\repos\DBDemo\DBDemo\bin\Debug\database.db;Version=3;";

            string reportPath = "report.frx"; // 報表模板的路徑
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportOutput.pdf");

            // 檢查報表模板是否存在
            if (!File.Exists(reportPath))
            {
                Console.WriteLine("報表模板文件不存在。請確認 report.frx 存在於專案目錄中。");
                return;
            }

            // 使用 using 確保 SQLiteConnection 在使用後會自動關閉並釋放資源
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // 查詢資料並註冊為報表使用的數據源
                var managerIdEmployees = connection.Query("SELECT name FROM Employees WHERE managerId IS NOT NULL").ToList();
                var higherSalaryEmployees = connection.Query(@"
                                                              SELECT e1.name 
                                                              FROM Employees e1
                                                              JOIN Employees e2 ON e1.managerId = e2.id
                                                              WHERE e1.salary > e2.salary
                                                              ").ToList();

                // 初始化報表
                using (Report report = new Report())
                {
                    report.Load(reportPath);

                    // 註冊數據源，命名為 "NonManagerEmployees" 和 "HigherSalaryEmployees"
                    report.RegisterData(managerIdEmployees, "NonManagerEmployees");
                    report.RegisterData(higherSalaryEmployees, "HigherSalaryEmployees");

                    // 確認刪除報表模板中的數據連接
                    report.Dictionary.Connections.Clear();

                    // 準備並匯出報表
                    report.Prepare();
                    using (PDFSimpleExport pdfExport = new PDFSimpleExport())
                    {
                        report.Export(pdfExport, outputPath);
                        Console.WriteLine($"報表已匯出至 {outputPath}");
                    }
                }
            }


            Console.ReadLine();
        }
    }
}
