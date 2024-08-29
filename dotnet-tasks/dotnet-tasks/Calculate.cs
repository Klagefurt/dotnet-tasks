using dotnet_tasks.Exceptions;
using OfficeOpenXml;

namespace dotnet_tasks
{
    public class Calculate
    {
        public IReadOnlyCollection<Tank> Tanks { get; set; }
        public IReadOnlyCollection<Unit> Units { get; set; }
        public IReadOnlyCollection<Factory> Factories {  get; set; }

        public void ReadData(string inputCommand = "")
        {
            if (inputCommand == "readFromExcel")
            {
                Console.WriteLine("Reading from excel...");

                IReadOnlyCollection<Factory> factories;
                IReadOnlyCollection<Unit> units;
                IReadOnlyCollection<Tank> tanks;
                GetCollectionsFromExcel(out factories, out units, out tanks);

                Factories = factories;
                Units = units;
                Tanks = tanks;
            }
            else if (inputCommand == "readFromCSV")
            {
                Console.WriteLine("Reading from csv...");

                IReadOnlyCollection<Factory> factories;
                IReadOnlyCollection<Unit> units;
                IReadOnlyCollection<Tank> tanks;
                GetCollectionsFromCSV(out factories, out units, out tanks);

                Factories = factories;
                Units = units;
                Tanks = tanks;
            }
            else
            {
                Console.WriteLine("Reading random data...");
                Tanks = GetTanks();
                Units = GetUnits();
                Factories = GetFactories();
            }
        }

        private void GetCollectionsFromCSV(out IReadOnlyCollection<Factory> factories, 
                                           out IReadOnlyCollection<Unit> units, 
                                           out IReadOnlyCollection<Tank> tanks)
        {
            var csvFactoriesFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input-factories.csv");
            var csvTanksFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input-tanks.csv");
            var csvUnitsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input-units.csv");

            var readCsvFile = new ReadCsvFile();
            factories = readCsvFile.ReadFactories(csvFactoriesFile);
            units = readCsvFile.ReadUnits(csvUnitsFile);
            tanks = readCsvFile.ReadTanks(csvTanksFile);          
        }

        private IReadOnlyCollection<Tank> GetTanks()
        {
            var tanks = new Tank[]
            {
            new Tank {ID=1, Name="Резервуар 1", Description="Надземный - вертикальный", Volume=1500, MaxVolume=2000, UnitId=1},
            new Tank {ID=2, Name="Резервуар 2", Description="Надземный - горизонтальный", Volume=2500, MaxVolume=3000, UnitId=1},
            new Tank {ID=3, Name="Дополнительный резервуар 24", Description="Надземный - горизонтальный", Volume=3000, MaxVolume=3000, UnitId=2},
            new Tank {ID=4, Name="Резервуар 35", Description="Надземный - вертикальный", Volume=3000, MaxVolume=3000, UnitId=2},
            new Tank {ID=5, Name="Резервуар 47", Description="Подземный - двустенный", Volume=4000, MaxVolume=5000, UnitId=2},
            new Tank {ID=6, Name="Резервуар 256", Description="Подводный", Volume=500, MaxVolume=500, UnitId=3},
            new Tank {ID=7, Name="Резервуар 1", Description="Надземный - вертикальный", Volume=1570, MaxVolume=2100, UnitId=1},

            };
            return tanks;
        }
        // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
        private IReadOnlyCollection<Unit> GetUnits()
        {
            var units = new Unit[]
            {
            new Unit {ID=1, Name="ГФУ-2", Description="Газофракционирующая установка", FactoryID=1},
            new Unit {ID=2, Name="АВТ-6", Description="Атмосферно-вакуумная трубчатка", FactoryID=1},
            new Unit {ID=3, Name="АВТ-10", Description="Атмосферно-вакуумная трубчатка", FactoryID=2},
            };
            return units;
        }
        // реализуйте этот метод, чтобы он возвращал массив заводов, согласно приложенным таблицам
        private IReadOnlyCollection<Factory> GetFactories()
        {
            var factories = new Factory[]
            {
            new Factory {ID=1, Name="НПЗ№1", Description="Первый нефтеперерабатывающий завод"},
            new Factory {ID=2, Name="НПЗ№2", Description="Второй нефтеперерабатывающий завод"},
            };
            return factories;
        }

        private IReadOnlyCollection<Factory> ReadFactories(ExcelWorksheet worksheet)
        {
            var factories = new List<Factory>();

            int row = 2;
            int col = 1;

            if (worksheet.Cells[row, col] == null || worksheet.Cells[row, col].Value == null)
                throw new WorksheetEmptyException("Стартовая ячейка пустая!");

            while (true)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue == null) 
                    break;

                Factory factory = new();
                factory.ID = int.Parse(cellValue.ToString());
                factory.Name = worksheet.Cells[row, col + 1].Value.ToString();
                factory.Description = worksheet.Cells[row, col + 2].Value.ToString();
                factories.Add(factory);
                row++;
            }
            return factories;
        }

        private IReadOnlyCollection<Unit> ReadUnits(ExcelWorksheet worksheet)
        {
            var units = new List<Unit>();

            int row = 2;
            int col = 1;

            if (worksheet.Cells[row, col] == null || worksheet.Cells[row, col].Value == null)
                throw new WorksheetEmptyException("Стартовая ячейка пустая!");

            while (true)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue == null)
                    break;

                Unit unit = new();
                unit.ID = int.Parse(cellValue.ToString());
                unit.Name = worksheet.Cells[row, col + 1].Value.ToString();
                unit.Description = worksheet.Cells[row, col + 2].Value.ToString();
                unit.FactoryID = int.Parse(worksheet.Cells[row, col + 3].Value.ToString());
                units.Add(unit);
                row++;
            }
            return units;
        }

        private IReadOnlyCollection<Tank> ReadTanks(ExcelWorksheet worksheet)
        {
            var tanks = new List<Tank>();

            int row = 2;
            int col = 1;

            if (worksheet.Cells[row, col] == null || worksheet.Cells[row, col].Value == null)
                throw new WorksheetEmptyException("Стартовая ячейка пустая!");

            while (true)
            {
                var cellValue = worksheet.Cells[row, col].Value;
                if (cellValue == null)
                    break;

                Tank tank = new();
                tank.ID = int.Parse(cellValue.ToString());
                tank.Name = worksheet.Cells[row, col + 1].Value.ToString();
                tank.Description = worksheet.Cells[row, col + 2].Value.ToString();
                tank.Volume = int.Parse(worksheet.Cells[row, col + 3].Value.ToString());
                tank.MaxVolume = int.Parse(worksheet.Cells[row, col + 4].Value.ToString());
                tank.UnitId = int.Parse(worksheet.Cells[row, col + 5].Value.ToString());
                tanks.Add(tank);
                row++;
            }
            return tanks;
        }

        private void GetCollectionsFromExcel(out IReadOnlyCollection<Factory> factories,
                                out IReadOnlyCollection<Unit> units,
                                out IReadOnlyCollection<Tank> tanks)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var excelFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input.xlsx");
            var file = new FileInfo(excelFileName);

            if (!file.Exists) 
            {
                throw new InvalidPathException("Excel-файл с таким именем по этому адресу не существует!");
            }

            using (var package = new ExcelPackage(file))
            {
                if (package.Workbook?.Worksheets?.Count <= 0)
                    throw new WorksheetEmptyException("В книге нет 0-го листа");

                var worksheet0 = package.Workbook.Worksheets[0];
                factories = ReadFactories(worksheet0);

                if (package.Workbook?.Worksheets?.Count <= 1)
                    throw new WorksheetEmptyException("В книге нет 1-го листа");

                var worksheet1 = package.Workbook.Worksheets[1];
                units = ReadUnits(worksheet1);

                if (package.Workbook?.Worksheets?.Count <= 2)
                    throw new WorksheetEmptyException("В книге нет 2-го листа");

                var worksheet2 = package.Workbook.Worksheets[2];
                tanks = ReadTanks(worksheet2);
            }
        }    
    }
}
