using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace WinFormsMVP.Model
{
    internal class CustomerXmlRepository : ICustomerRepository
    {
        private readonly string _xmlFilePath;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Customer>));
        private readonly Lazy<List<Customer>> _customers;

        public CustomerXmlRepository(string fullPath)
        {
            _xmlFilePath = fullPath + @"\customers.xml";

            if (!File.Exists(_xmlFilePath))
                CreateCustomerXmlStub();

            _customers = new Lazy<List<Customer>>(() =>
            {
                using (var reader = new StreamReader(_xmlFilePath))
                {
                    return (List<Customer>)_serializer.Deserialize(reader);
                }
            });
        }

        private void CreateCustomerXmlStub()
        {
            var stubCustomerList = new List<Customer> {
                new Customer {Name = "Dima", Address = "Lenina 63b", Phone = "+79600315449"},
                new Customer {Name = "Anton", Address = "Oktyaborskii 72", Phone = "+79600768211"},
                new Customer {Name = "Maria", Address = "Stroiteley, 5", Phone = "+79282472013"},
                new Customer {Name = "Anna", Address = "Sportivnaya, 23", Phone = "+79910358849"},
                new Customer {Name = "Gleb", Address = "Moskovskiy, 26", Phone = "+79093087810"},
                new Customer {Name = "Sveta", Address = "Kuznetckii, 115", Phone = "+79600315449"},
                new Customer {Name = "Lena", Address = "Iniciativnaya, 23", Phone = "+79673738660"},
                new Customer {Name = "Vika", Address = "Pionerskii,  126", Phone = "+77074370314"},
                new Customer {Name = "Veranika", Address = "Tihaci,  7", Phone = "+79063327418"}
            };
            SaveCustomerList(stubCustomerList);
        }

        private void SaveCustomerList(List<Customer> customers)
        {
            using (var writer = new StreamWriter(_xmlFilePath, false))
            {
                _serializer.Serialize(writer, customers);
            }
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers.Value;
        }

        public Customer GetCustomer(int id)
        {
            return _customers.Value[id];
        }

        public void SaveCustomer(int id, Customer customer)
        {
            _customers.Value[id] = customer;
            SaveCustomerList(_customers.Value);
        }
    }
}