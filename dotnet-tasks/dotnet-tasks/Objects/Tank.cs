using CsvHelper.Configuration.Attributes;

/// <summary>
/// Резервуар
/// </summary>
public class Tank
{
    [Name("Id")]
    public int ID { get; set; }
    [Name("Name")]
    public string Name { get; set; }
    [Name("Description")]
    public string Description { get; set; }
    [Name("Volume")]
    public int Volume { get; set; }
    [Name("MaxVolume")]
    public int MaxVolume { get; set; }
    [Name("UnitId")]
    public int UnitId { get; set; }
}