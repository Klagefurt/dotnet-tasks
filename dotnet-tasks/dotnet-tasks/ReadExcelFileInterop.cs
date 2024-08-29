using Excel = Microsoft.Office.Interop.Excel;

namespace dotnet_tasks
{
    public class ReadExcelFileInterop
    {
        //Interop библиотека
        private static object[] ReadFactoriesFromExcel()
        {
            var excelFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input.xlsx");

            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(excelFileName);
            Excel._Worksheet worksheet = workbook.Sheets[1];
            Excel.Range cellRange = worksheet.UsedRange;

            var factories = new Factory[2];

            for (int i = 2; i <= 3; i++)
            {
                int id = 0;
                string name = "";
                string description = "";

                for (int j = 1; j <= 3; j++)
                {
                    if (j == 1)
                        id = (int)cellRange.Cells[i, j].Value2;
                    if (j == 2)
                        name = cellRange.Cells[i, j].Value2.ToString();
                    if (j == 3)
                        description = cellRange.Cells[i, j].Value2.ToString();
                }
                var factory = new Factory { ID = id, Name = name, Description = description };
                factories[id - 1] = factory;
            }
            workbook.Close(false);
            app.Quit();

            return factories;
        }
    }
}
