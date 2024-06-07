using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Phone_Book_Application;

public class PhoneBook
{
    private Dictionary<string, Contact> Contacts { get; }
    private readonly string _connectionString;

    
    public PhoneBook(IConfiguration configuration)
    {
        Contacts = new Dictionary<string, Contact>();
        var projectDirectory = GetProjectDirectory();
        var databasePath = Path.Combine(projectDirectory, "phonebook.db");
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentException("Connection string is not specified");
        _connectionString = new SqliteConnectionStringBuilder(_connectionString)
        {
            DataSource = databasePath
        }.ConnectionString;
        CreateDatabaseAsync().Wait();
        LoadContacts().Wait();
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
    
    private async Task CreateDatabaseAsync()
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("CREATE TABLE IF NOT EXISTS Contacts " +
                                      "(Name VARCHAR(50) PRIMARY KEY, PhoneNumber VARCHAR(50) NOT NULL)");
    }

    public bool AddContact(string name, string phoneNumber)
    {
        if (!Contacts.ContainsKey(name))
        {
            Contact contact = new Contact(name, phoneNumber);
            Contacts.Add(name, contact); 
            SaveContacts(contact).Wait();
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
            DeleteContactFromDatabase(name).Wait();
            return true;
        }
        else
        {
            Console.WriteLine($"Contact with name {name} does not exist.");
            return false;
        }
        
    }

    private async Task DeleteContactFromDatabase(string name)
    {
        try
        {
    
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"DELETE FROM Contacts WHERE Name = @Name";

            await connection.ExecuteAsync(query, new { Name = name });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error removing contact from the database: {e.Message}");
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
            SaveContacts(Contacts[name]).Wait();
            return true;
        }
        else
        {
            AddContact(name, phoneNumber);
            Console.WriteLine($"Old contact with name {name} does not exist. new contact is created.");
            return false;
        }
        
    }
    
  

    private async Task SaveContacts(Contact contact)
    {
        try
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                    INSERT INTO Contacts (Name, PhoneNumber)
                    VALUES (@Name, @PhoneNumber)
                    ON CONFLICT (Name) DO UPDATE SET
                        PhoneNumber = excluded.PhoneNumber 
                ";
                
            await connection.ExecuteAsync(query, new { Name = contact.Name, PhoneNumber = contact.PhoneNumber });

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving contacts: {e.Message}");
        }
        
    }
    
    private async Task LoadContacts()
    {
        try
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            
            var query = @"
                SELECT Name, PhoneNumber
                FROM Contacts
            ";
            
            var contacts = await connection.QueryAsync<Contact>(query);

            foreach (var contact in contacts)
            {
                Contacts.Add(contact.Name, contact);
            }
            Console.WriteLine("Contacts loaded successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading contacts: {e.Message}");
        }
        
    }
    
}