namespace TestNamespace
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MyAttribute : Attribute 
    { 
        public int Number { get; set; }
    }
}
