using CsvHelper.Configuration.Attributes;

/// <summary>
/// Завод
/// </summary>
public class Factory
{
    [Name("Id")]
    public int ID { get; set; }
    [Name("Name")]
    public string Name { get; set; }
    [Name("Description")]
    public string Description { get; set; }
}