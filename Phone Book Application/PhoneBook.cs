using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NLog;

namespace Phone_Book_Application;

public class PhoneBook
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private Dictionary<string, Contact> Contacts { get; }
    private readonly string _connectionString;

    public PhoneBook(IConfiguration configuration)
    {
        Logger.Debug("Initializing PhoneBook...");
        Contacts = new Dictionary<string, Contact>();
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                            ?? throw new ArgumentException("Connection string is not specified");
        Initialize().Wait();
    }

    private async Task Initialize()
    {
        try
        {
            Logger.Debug("Creating database and loading contacts...");
            await CreateDatabaseAsync();
            await LoadContacts();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error initializing phone book");
            throw;
        }
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
        Logger.Debug($"Attempting to add contact: {name}, {phoneNumber}");

        if (!Contacts.ContainsKey(name))
        {
            Contact contact = new Contact(name, phoneNumber);
            Contacts.Add(name, contact);
            SaveContacts(contact).Wait();
            Logger.Info($"Contact {name} has been added.");
            return true;
        }
        else
        {
            Logger.Warn($"Contact {name} already exists.");
            return false;
        }
    }

    public bool RemoveContact(string name)
    {
        Logger.Debug($"Attempting to remove contact: {name}");

        if (Contacts.ContainsKey(name))
        {
            Contacts.Remove(name);
            Logger.Info($"Contact {name} has been removed.");
            DeleteContactFromDatabase(name).Wait();
            return true;
        }
        else
        {
            Logger.Warn($"Contact {name} does not exist.");
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
            Logger.Error(e, $"Error removing contact {name} from the database");
        }
    }

    public void ListContacts()
    {
        Logger.Debug("Listing all contacts...");

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
        Logger.Debug($"Searching for contact: {name}");

        if (Contacts.TryGetValue(name, out var contact))
        {
            Console.WriteLine($"Name: {name} Phone number: {contact.PhoneNumber}");
            return true;
        }
        else
        {
            Logger.Warn($"Contact {name} does not exist.");
            return false;
        }
    }

    public bool UpdateContact(string name, string phoneNumber)
    {
        Logger.Debug($"Attempting to update contact: {name}, {phoneNumber}");

        if (Contacts.TryGetValue(name, out var contact))
        {
            Contacts[name] = contact with { PhoneNumber = phoneNumber };
            Logger.Info($"Contact {name} has been updated.");
            SaveContacts(Contacts[name]).Wait();
            return true;
        }
        else
        {
            AddContact(name, phoneNumber);
            Logger.Warn($"Old contact {name} does not exist, new contact is created.");
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
            Logger.Error(e, "Error saving contacts");
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
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error loading contacts");
        }
    }
}