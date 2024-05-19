namespace RabbitMQClient.Customers.API.Model
{
    public class Customer
    {
        public Customer(string name, string cpf, DateTime birthDate)
        {
            Name = name;
            Cpf = cpf;
            BirthDate = birthDate;
        }
        public string Name { get; private set; }
        public string Cpf { get; private set; }
        public DateTime BirthDate { get; private set; }
    }
}
