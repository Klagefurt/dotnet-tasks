using System;
using System.Reflection;

namespace TestNamespace
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            //var type = Type.GetType("TestNamespace.Student"); //здесь лежит метоописание типа

            var student = new Student();
            Type type = student.GetType();
            var properties = type.GetProperties();

            //Type type = typeof(Student);
            //ConstructorInfo constructorInfo = type.GetConstructor( new Type[] {});
            //object student = constructorInfo.Invoke(new object[] { });

            //var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            //var members = type.GetMembers(); // метоописания ВСЕХ членов типа
            //var members = student.GetMembers(BindingFlags.NonPublic | BindingFlags.Instance); //выбираем приватные и экземалярные поля и свойства

            foreach ( PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof (MyAttribute), false );

                if (attributes.Length > 0)
                {
                    var attribute = (MyAttribute) attributes[0];
                    Console.WriteLine("Property name - {0}, attribute value - {1}", property.Name, attribute.Number);
                }


                //if (fieldInfo.Name == "_temp")
                //{
                //    var value = fieldInfo.GetValue(student);
                //    Console.WriteLine($"Before - {value}");

                //    fieldInfo.SetValue(student, 67);

                //    value = fieldInfo.GetValue(student);
                //    Console.WriteLine($"After - {value}");
                //}
            }
        }
    }
}
