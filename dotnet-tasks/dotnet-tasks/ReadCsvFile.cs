using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace dotnet_tasks
{
    public class ReadCsvFile
    {
        List<Tank> tanks;
        List<Unit> units;
        List<Factory> factories;

        public ReadCsvFile()
        {
            tanks = new List<Tank>();
            units = new List<Unit>();
            factories = new List<Factory>();
        }

        public List<Tank> ReadTanks(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var reader = new StreamReader(path, Encoding.GetEncoding(1251));
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };
                using var csv = new CsvReader(reader, csvConfig);

                var records = csv.GetRecords<Tank>();
                foreach (var record in records)
                {
                    tanks.Add(new Tank
                    {
                        ID = record.ID,
                        Name = record.Name,
                        Description = record.Description,
                        Volume = record.Volume,
                        MaxVolume = record.MaxVolume,
                        UnitId = record.UnitId
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tanks;
        }

        public List<Factory> ReadFactories(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var reader = new StreamReader(path, Encoding.GetEncoding(1251));
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };
                using var csv = new CsvReader(reader, csvConfig);

                var records = csv.GetRecords<Factory>();
                foreach (var record in records)
                {
                    factories.Add(new Factory
                    {
                        ID = record.ID,
                        Name = record.Name,
                        Description = record.Description,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return factories;
        }

        public List<Unit> ReadUnits(string path)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var reader = new StreamReader(path, Encoding.GetEncoding(1251));
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-RU"))
                {
                    HasHeaderRecord = true,
                    Delimiter = ";"
                };
                using var csv = new CsvReader(reader, csvConfig);

                var records = csv.GetRecords<Unit>();
                foreach (var record in records)
                {
                    units.Add(new Unit
                    {
                        ID = record.ID,
                        Name = record.Name,
                        Description = record.Description,
                        FactoryID = record.FactoryID
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return units;
        }
    }
}
