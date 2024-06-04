using System.Text.Json;

namespace Phone_Book_Application;

public class PhoneBook
{
    private Dictionary<string, Contact> Contacts { get; }
    private const string FileName = "Data/contacts.json";
    
    public PhoneBook()
    {
        Contacts = new Dictionary<string, Contact>();
        LoadContacts();
    }

    public void AddContact(string name, string phoneNumber)
    {
        if (Contacts.ContainsKey(name))
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

    public void SaveContacts()
    {
        try
        {
            var json = JsonSerializer.Serialize(Contacts);
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