using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;

class Program
{
    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = (Factory[])ReadFactoriesFromExcel();

        WriteTanksToExcel(tanks);

        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        Console.WriteLine("Введите название резервуара ...");
        var tankToFind = Console.ReadLine();

        if (!String.IsNullOrEmpty(tankToFind))
        {
            var foundUnit = FindUnit(units, tanks, tankToFind);
            var factory = FindFactory(factories, foundUnit);
            if (foundUnit != null && factory != null)
            {
                Console.WriteLine($"{tankToFind} принадлежит установке {foundUnit.Name} и заводу {factory.Name}");
            }
            else
            {
                Console.WriteLine("Неверный ввод!");
            }
        }

        SerializeFiles(tanks);
        SerializeFiles(units);
        SerializeFiles(factories);

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
    }

    private static void WriteTanksToExcel(Tank[] tanks)
    {
        try
        {
            var excelFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources\\input.xlsx");

            Excel.Application app = new Excel.Application();
            Excel.Workbook workbook = app.Workbooks.Open(excelFileName);
            Excel._Worksheet worksheet = workbook.Sheets[1];
            Excel.Range cellRange = worksheet.UsedRange;

            var i = 6;
            foreach (var tank in tanks)
            {
                cellRange.Cells[i, 1].Value2 = tank.ID.ToString();
                cellRange.Cells[i, 2].Value2 = tank.Name;
                cellRange.Cells[i, 3].Value2 = tank.Description;
                cellRange.Cells[i, 4].Value2 = tank.Volume.ToString();
                cellRange.Cells[i, 5].Value2 = tank.MaxVolume.ToString();
                cellRange.Cells[i, 6].Value2 = tank.UnitId.ToString();

                i++;
            }
            workbook.Close(true);
            app.Quit();
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(ex.Message); 
        }
    }

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

    private static void SerializeFiles(object[] array)
    {
        var path = "output.json";
        foreach (var obj in array)
        {    
            using (var streamwriter = File.AppendText(path))
            {
                var strObj = JsonConvert.SerializeObject(obj);
                streamwriter.WriteLine(strObj);
            }
        }
    }

    // реализуйте этот метод, чтобы он возвращал массив резервуаров, согласно приложенным таблицам
    // можно использовать создание объектов прямо в C# коде через new, или читать из файла (на своё усмотрение)
    public static Tank[] GetTanks()
    {
        var tanks = new Tank[]
        {
            new Tank {ID=1, Name="Резервуар 1", Description="Надземный - вертикальный", Volume=1500, MaxVolume=2000, UnitId=1},
            new Tank {ID=2, Name="Резервуар 2", Description="Надземный - горизонтальный", Volume=2500, MaxVolume=3000, UnitId=1},
            new Tank {ID=3, Name="Дополнительный резервуар 24", Description="Надземный - горизонтальный", Volume=3000, MaxVolume=3000, UnitId=2},
            new Tank {ID=4, Name="Резервуар 35", Description="Надземный - вертикальный", Volume=3000, MaxVolume=3000, UnitId=2},
            new Tank {ID=5, Name="Резервуар 47", Description="Подземный - двустенный", Volume=4000, MaxVolume=5000, UnitId=2},
            new Tank {ID=6, Name="Резервуар 256", Description="Подводный", Volume=500, MaxVolume=500, UnitId=3},
        };
        return tanks;
    }
    // реализуйте этот метод, чтобы он возвращал массив установок, согласно приложенным таблицам
    public static Unit[] GetUnits()
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
    public static Factory[] GetFactories()
    {
        var factories = new Factory[]
        {
            new Factory {ID=1, Name="НПЗ№1", Description="Первый нефтеперерабатывающий завод"},
            new Factory {ID=2, Name="НПЗ№2", Description="Второй нефтеперерабатывающий завод"},
        };
        return factories;
    }

    // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
    // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
    // учтите, что по заданному имени может быть не найден резервуар
    public static Unit? FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        var tank = tanks.SingleOrDefault(t => t.Name == tankName);
        if (tank == null)
            return null;
        return units[tank.UnitId - 1];
    }

    // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
    public static Factory? FindFactory(Factory[] factories, Unit unit)
    {
        if (unit == null) return null;

        var factoryID = unit.FactoryID;
        if (factoryID <= factories.Length)
            return factories[factoryID - 1];
        else
            return null;
        }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] units)
    {
        var totalVolume = units.Sum(u => u.Volume);
        return totalVolume;
    }
}
