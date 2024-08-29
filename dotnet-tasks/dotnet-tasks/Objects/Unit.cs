using CsvHelper.Configuration.Attributes;

/// <summary>
/// Установка
/// </summary>
public class Unit
{
    [Name("Id")]
    public int ID { get; set; }
    [Name("Name")]
    public string Name { get; set; }
    [Name("Description")]
    public string Description { get; set; }
    [Name("FactoryId")]
    public int FactoryID { get; set; }
}
