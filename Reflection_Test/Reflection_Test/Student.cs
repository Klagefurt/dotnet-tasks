namespace TestNamespace
{
    public class Student
    {
        private int _temp = 7;

        public string Name { get; set; }
        [My(Number = 11)]
        public int Age { get; set; }
    }
}