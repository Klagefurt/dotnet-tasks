using Excel = Microsoft.Office.Interop.Excel;
using Cocona;
using System.Text.Json;
using System.Text.Json.Nodes;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text.Encodings.Web;

namespace dotnet_tasks
{
    class Program
    {
        static bool ToSerialize = true;
        static void Main(string[] args)
        {
            var calculate = new Calculate();

            var builder = CoconaApp.CreateBuilder();
            var app = builder.Build();

            app.AddCommand(([Option(Description = "Write 'readFromExcel' or 'readFromCSV' if you want to")] string? command,
                [Option(Description = "Write 'true' if you want to serialize data")] string? toSerialize) =>
            {
                calculate.ReadData(command);
                if (toSerialize == "true") SetToSerialize();
            });

            app.Run();

            var units = calculate.Units;
            var tanks = calculate.Tanks;
            var factories = calculate.Factories;

            Console.WriteLine($"Количество резервуаров: {calculate.Tanks.Count}, установок: {calculate.Units.Count}");

            Console.WriteLine("Введите название резервуара ...");

            var tankToFind = Console.ReadLine();

            if (!string.IsNullOrEmpty(tankToFind))
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
       
            if (ToSerialize)
            {
                var jsonData = CreateJsonObject(units, tanks, factories);
                SerializeData(jsonData);
            }

            var totalVolume = GetTotalVolume(tanks);
            Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
        }

        private static JsonObject CreateJsonObject(IReadOnlyCollection<Unit> units, IReadOnlyCollection<Tank> tanks, IReadOnlyCollection<Factory> factories)
        {
            var jsonArrayUnits = new JsonArray { units.ToArray() };
            var jsonArrayTanks = new JsonArray { tanks.ToArray() };
            var jsonArrayFactories = new JsonArray { factories.ToArray() };

            var complexValue = new JsonObject
            {
                ["tanks"] = jsonArrayUnits,
                ["units"] = jsonArrayTanks,
                ["factories"] = jsonArrayFactories
            };
            return complexValue;
        }

        private static void SerializeData(JsonObject jsonObject)
        {
            var path = "output.json";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var jsonString = JsonSerializer.Serialize(jsonObject, options);

            using StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(jsonString);
        }

        private static void SetToSerialize() => ToSerialize = true;

        // реализуйте этот метод, чтобы он возвращал установку (Unit), которой
        // принадлежит резервуар (Tank), найденный в массиве резервуаров по имени
        // учтите, что по заданному имени может быть не найден резервуар
        private static Unit? FindUnit(IEnumerable<Unit> units, IEnumerable<Tank> tanks, string tankName)
        {
            var tank = tanks.FirstOrDefault(t => t.Name == tankName);
            if (tank == null)
                return null;
            return units.FirstOrDefault(u => u.ID == tank.UnitId);
        }

        // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
        private static Factory? FindFactory(IEnumerable<Factory> factories, Unit unit)
        {
            if (unit == null) return null;

            var factoryID = unit.FactoryID;
            var factory = factories.FirstOrDefault(f => f.ID == factoryID);
            return factory;
        }

        // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
        private static int GetTotalVolume(IEnumerable<Tank> tanks)
        {
            var totalVolume = tanks.Sum(u => u.Volume);
            return totalVolume;
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
    }
}
