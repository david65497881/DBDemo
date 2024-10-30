using System;
using System.Data.SQLite;
using Dapper;
using System.IO;
using FastReport;
using FastReport.Export.PdfSimple;
using System.Text;
using System.Linq;

namespace DBDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=C:\Users\user\source\repos\DBDemo\DBDemo\bin\Debug\database.db;Version=3;";
            string reportPath = "report.frx";
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "ReportOutput.pdf");

            if (!File.Exists(reportPath))
            {
                Console.WriteLine("報表模板文件不存在。請確認 report.frx 存在於專案目錄中。");
                return;
            }

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var managerIdEmployees = connection.Query("SELECT name FROM Employees WHERE managerId IS NOT NULL").ToList();
                var higherSalaryEmployees = connection.Query(@"
                    SELECT e1.name 
                    FROM Employees e1
                    JOIN Employees e2 ON e1.managerId = e2.id
                    WHERE e1.salary > e2.salary").ToList();

                StringBuilder nonManagerEmployeesText = new StringBuilder();
                foreach (var employee in managerIdEmployees)
                {
                    nonManagerEmployeesText.AppendLine(employee.name);
                }

                StringBuilder higherSalaryEmployeesText = new StringBuilder();
                foreach (var employee in higherSalaryEmployees)
                {
                    higherSalaryEmployeesText.AppendLine(employee.name);
                }

                using (Report report = new Report())
                {
                    report.Load(reportPath);

                    // 找到 TextObject 並設置內容
                    var nonManagerTextObject = report.FindObject("Text3") as FastReport.TextObject; 
                    if (nonManagerTextObject != null)
                    {
                        nonManagerTextObject.Text = nonManagerEmployeesText.ToString();
                    }

                    var higherSalaryTextObject = report.FindObject("Text4") as FastReport.TextObject; 
                    if (higherSalaryTextObject != null)
                    {
                        higherSalaryTextObject.Text = higherSalaryEmployeesText.ToString();
                    }

                    report.Dictionary.Connections.Clear();
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
