class Program
{
    static void Main(string[] args)
    {
        var tanks = GetTanks();
        var units = GetUnits();
        var factories = GetFactories();
        Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

        var foundUnit = FindUnit(units, tanks, "Резервуар 2");
        var factory = FindFactory(factories, foundUnit);

        Console.WriteLine($"Резервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

        var totalVolume = GetTotalVolume(tanks);
        Console.WriteLine($"Общий объем резервуаров: {totalVolume}");
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
    public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
    {
        var tank = tanks.SingleOrDefault(t => t.Name == tankName);
        return units[tank.UnitId - 1];
    }

    // реализуйте этот метод, чтобы он возвращал объект завода, соответствующий установке
    public static Factory FindFactory(Factory[] factories, Unit unit)
    {
        var factoryID = unit.FactoryID;
        return factories[factoryID];
    }

    // реализуйте этот метод, чтобы он возвращал суммарный объем резервуаров в массиве
    public static int GetTotalVolume(Tank[] units)
    {
        var totalVolume = units.Sum(u => u.Volume);
        return totalVolume;
    }
}

/// <summary>
/// Установка
/// </summary>
public class Unit
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int FactoryID { get; set; }
}

/// <summary>
/// Завод
/// </summary>
public class Factory
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

}

/// <summary>
/// Резервуар
/// </summary>
public class Tank
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Volume { get; set; }
    public int MaxVolume { get; set; }
    public int UnitId { get; set; }
}