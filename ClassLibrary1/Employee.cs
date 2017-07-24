using System;

namespace ClassLibrary
{
    public class Employee : Entity
    {
        public string Name { get; set; }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}