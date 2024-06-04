using System.Text.Json;

namespace Phone_Book_Application;

public class PhoneBook
{
    private Dictionary<string, Contact> Contacts { get; }
    private const string FileName = @"C:\Users\Natia\RiderProjects\Phone Book Application\Phone Book Application\Data\contacts.json";
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions { WriteIndented = true };
        

    
    public PhoneBook()
    {
        Contacts = new Dictionary<string, Contact>();
        LoadContacts();
    }

    public void AddContact(string name, string phoneNumber)
    {
        if (!Contacts.ContainsKey(name))
        {
            Contact contact = new Contact(name, phoneNumber);
            Contacts.Add(name, contact);
            Console.WriteLine($"Contact with name {name} has been added.");
        }
        else
        {
            Console.WriteLine($"Contact with name {name} already exists.");
        }
        
        
    }
    
    public void RemoveContact(string name)
    {
        if (Contacts.ContainsKey(name))
        {
            Contacts.Remove(name);
            Console.WriteLine($"Contact with name {name} has been removed.");
        }
        else
        {
            Console.WriteLine($"Contact with name {name} does not exist.");
        }
        
    }

    public void ListContacts()
    {
        if (Contacts.Count == 0)
        {
            Console.WriteLine("There are no contacts in the phone book.");
            
        }
        else
        {
            foreach (var contact in Contacts.Values)
            {
                Console.WriteLine($"Name: {contact.Name} Phone number: {contact.PhoneNumber}");
            }
        }
    }

    public void SearchByName(string name)
    {
        if (Contacts.TryGetValue(name, out var contact))
        {
            Console.WriteLine($"Name: {name} Phone number: {contact.PhoneNumber}");
        }
        else
        {
            Console.WriteLine($"Contact with name {name} does not exist.");
        }
    }

    public void UpdateContact(string name, string phoneNumber)
    {
        if (Contacts.TryGetValue(name, out var contact))
        {
            Contacts[name] = contact with {PhoneNumber = phoneNumber};
            Console.WriteLine($"Contact with name {name} has been updated.");
        }
        else
        {
            AddContact(name, phoneNumber);
            Console.WriteLine($"Old contact with name {name} does not exist. new contact is created.");
        }
        
    }
    
  

    public void SaveContacts()
    {
        try
        {
            var directory = (Path.GetDirectoryName(FileName) ?? String.Empty);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var json = JsonSerializer.Serialize(Contacts, Options);
            File.WriteAllText(FileName, json);
            Console.WriteLine("Contacts have been saved.");

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving contacts: {e.Message}");
        }
        
    }
    
    private void LoadContacts()
    {
        try
        {
            if (File.Exists(FileName))
            {
                var json = File.ReadAllText(FileName);
                var contacts = JsonSerializer.Deserialize<Dictionary<string, Contact>>(json);
                if (contacts != null)
                {
                    foreach (var contact in contacts)
                    {
                        Contacts.Add(contact.Key, contact.Value);
                    }
                    Console.WriteLine("Contacts loaded successfully.");
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading contacts: {e.Message}");
        }
        
    }
    
}