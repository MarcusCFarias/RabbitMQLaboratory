using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQClient.Console
{
    internal class Person
    {
        public Person(string fullName, string document, DateTime birthDate)
        {
            FullName = fullName;
            Document = document;
            BirthDate = birthDate;
        }
        public string FullName { get; private set; }
        public string Document { get; private set; }
        public DateTime BirthDate { get; private set; }
    }
}
