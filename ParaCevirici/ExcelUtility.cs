using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaCevirici
{
    public class ExcelUtility
    {

        public static void ExportToExcel<T>(List<T> table)
        {
           
            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel.Workbooks.Add();

            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;
            workSheet.Name = DateTime.Now.Day.ToString()+".Day";
            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------

                var type = table.First().GetType();
                int count = 1;
                foreach (var prop in type.GetProperties())
                {
                    workSheet.Cells[1, count] = prop.Name;
                    count++;
                }


                // ------------------------------------------------
                // Populate sheet with some real data from "cars" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                foreach (var item in table)
                {
                    count = 1;
                    foreach (var prop in type.GetProperties())
                    {
                        workSheet.Cells[row, count] = type.GetProperty(prop.Name).GetValue(item);
                        count++;
                    }
                    row++;

                }

                // Apply some predefined styles for data to look nicely :)
                workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                var filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\Currencies";
                // Define filename
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                if (!Directory.Exists(filePath+ "\\" + DateTime.Now.Year))
                    Directory.CreateDirectory(filePath+ "\\" + DateTime.Now.Year);
                string fileName = string.Format(@"{0}\{1}-Month.xlsx", filePath +"\\"+ DateTime.Now.Year, DateTime.Now.Month);

                // Save this data as a file
                workSheet.SaveAs(fileName);

                // Display SUCCESS message
                Console.WriteLine(string.Format("The file '{0}' is saved successfully!", fileName));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception",
                    "There was a PROBLEM saving Excel file!\n" + exception.Message);
            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                // Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (workSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

                // Empty variables
                excel = null;
                workSheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
        }
    }
}
