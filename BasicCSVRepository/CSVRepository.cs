// Last updated 9/19/18 
// Author: Dave Bochichio
// https://davebochichio.com
// https://github.com/DaveEC42
using System;
using System.Collections.Generic;
using System.IO;

namespace BasicCSVRepository
{
    public class CSVRepository
    {
        private string _path;
        private List<Client> Clients;
        private int _nextItemId = 1;
        public string Path { get => _path; set => _path = value; }
        public int NextItemId { get => _nextItemId; }

        public CSVRepository(string filepath)
        {
            _path = filepath;
        }

        public OperationResult AddItem(Client client)
        {
            if (Clients == null)
            {
                Clients = new List<Client>();
            }
            var ret = new OperationResult();
            client.Id = _nextItemId++;
            var isValid = ValidateLineItem(client);

            if (isValid.Success)
            {

                Clients.Add(client);
            }
            else
            {
                return isValid;
            }
            return ret;
        }

        private OperationResult ValidateLineItem(Client lineItem)
        {
            var ret = new OperationResult();
            foreach (var item in Clients)
            {
                if (lineItem.Id == item.Id)
                {
                    ret.Success = false;
                    ret.Messages.Add("An error occurred while adding a line item to the repository.");
                    break;
                }
            }

            return ret;
        }

        public OperationResult DeleteItem(int id)
        {
            var ret = new OperationResult();
            for (int i = 0; i < Clients.Count; i++)
            {
                if (id == Clients[i].Id)
                {
                    Clients.Remove(Clients[i]);
                    break;
                }
            }
            return ret;
        }

        public Client GetItem(int id)
        {
            var clients = GetItems();
            foreach (var client in clients)
            {
                if (client.Id == id)
                {
                    return client;
                }
            }
            return null;
        }

        public IEnumerable<Client> GetItems()
        {
            if (Clients == null)
            {
                Load();
            }
            return Clients;
        }

        public OperationResult Load()
        {
            var ret = new OperationResult();
            Clients = new List<Client>();
            try
            {
                if (!File.Exists(Path))
                {
                    File.Create(Path);
                }
                using (var input = new StreamReader(Path))
                {
                    string line = string.Empty;
                    while ((line = input.ReadLine()) != null)
                    {
                        var newItem = new Client();
                        var values = line.Split(';');
                        newItem.Id = int.Parse(values[0]);
                        newItem.FirstName = values[1];
                        newItem.LastName = values[2];
                        newItem.StreetLineOne = values[3];
                        newItem.StreetLineTwo = values[4];
                        newItem.City = values[5];
                        newItem.State = values[6];
                        newItem.Zip = values[7];
                        Clients.Add(newItem);

                        if (newItem.Id >= _nextItemId)
                        {
                            _nextItemId = newItem.Id + 1;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                ret.Success = false;
                ret.Messages.Add("There was an error loading the repository: " + e.Message);
            }
            return ret;
        }

        public OperationResult UpdateItem(int id, Client client)
        {
            var ret = new OperationResult();
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id == id)
                {
                    client.Id = id;
                    Clients[i] = client;
                    break;
                }
            }

            return ret;
        }

        public OperationResult Save()
        {
            var ret = new OperationResult();
            try
            {
                if (!File.Exists(Path))
                {
                    File.Create(Path);
                }
                using (var output = new StreamWriter(Path))
                {
                    foreach (var item in Clients)
                    {
                        output.WriteLine(item.Id + ";" + item.FirstName + ";" + item.LastName + ";" + item.StreetLineOne + ";" + item.StreetLineTwo + ";" + item.City + ";" + item.State + ";" + item.Zip);

                    }
                }
            }
            catch (Exception e)
            {
                ret.Messages.Add("There was an error while saving the clients.");
                ret.Success = false;
            }
            return ret;

        }


        static void Main(string[] args)
        {
            var repo = new CSVRepository(@"c:\test\testrepo.csv");
            var client1 = new Client
            {
                FirstName = "John", LastName = "Smith", StreetLineOne = "123 Main Street", StreetLineTwo = "Apt 42",
                City = "White Plains", State = "NY", Zip = "10601"
            };
            var client2 = new Client
            {
                FirstName = "Jane", LastName = "Jones", StreetLineOne = "456 Windy Road", StreetLineTwo = "",
                City = "San Francisco", State = "CA", Zip = "94016"
            };
            repo.AddItem(client1);
            repo.AddItem(client2);
            repo.Save();
            var client3 = repo.GetItem(2);
            Console.WriteLine(client3.FirstName);
            Console.ReadKey();

        }
    }
}
