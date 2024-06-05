using System.Text.Json;

namespace Phone_Book_Application;

public class PhoneBook
{
    private Dictionary<string, Contact> Contacts { get; }
    private static readonly string FileName = Path.Combine(GetProjectDirectory(), "Data", "contacts.json");
        

    
    public PhoneBook()
    {
        Contacts = new Dictionary<string, Contact>();
        LoadContacts();
    }
    
    private static string GetProjectDirectory()
    {
        var currentDirectory = AppContext.BaseDirectory;
        var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.FullName;
        if (projectDirectory == null)
        {
            throw new DirectoryNotFoundException("Project directory not found.");
        }
        return projectDirectory;
    }

    public bool AddContact(string name, string phoneNumber)
    {
        if (!Contacts.ContainsKey(name))
        {
            Contact contact = new Contact(name, phoneNumber);
            Contacts.Add(name, contact);
            Console.WriteLine($"Contact with name {name} has been added.");
            return true;
        }
        else
        {
            Console.WriteLine($"Contact with name {name} already exists.");
            return false;
        }
        
        
    }
    
    public bool RemoveContact(string name)
    {
        if (Contacts.ContainsKey(name))
        {
            Contacts.Remove(name);
            Console.WriteLine($"Contact with name {name} has been removed.");
            return true;
        }
        else
        {
            Console.WriteLine($"Contact with name {name} does not exist.");
            return false;
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

    public bool SearchByName(string name)
    {
        if (Contacts.TryGetValue(name, out var contact))
        {
            Console.WriteLine($"Name: {name} Phone number: {contact.PhoneNumber}");
            return true;
        }
        else
        {
            Console.WriteLine($"Contact with name {name} does not exist.");
            return false;
        }
    }

    public bool UpdateContact(string name, string phoneNumber)
    {
        if (Contacts.TryGetValue(name, out var contact))
        {
            Contacts[name] = contact with {PhoneNumber = phoneNumber};
            Console.WriteLine($"Contact with name {name} has been updated.");
            return true;
        }
        else
        {
            AddContact(name, phoneNumber);
            Console.WriteLine($"Old contact with name {name} does not exist. new contact is created.");
            return false;
        }
        
    }
    
  

    public bool SaveContacts()
    {
        try
        {
            var directory = (Path.GetDirectoryName(FileName) ?? String.Empty);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var json = Contacts.ToJson(writeIndented : true);
            File.WriteAllText(FileName, json);
            Console.WriteLine("Contacts have been saved.");
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving contacts: {e.Message}");
            return false;
        }
        
    }
    
    private bool LoadContacts()
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
                    return true;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading contacts: {e.Message}");
            return false;
        }
        return false;
        
    }
    
}