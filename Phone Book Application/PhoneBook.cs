namespace Phone_Book_Application;

public class PhoneBook
{
    public Dictionary<string, Contact> Contacts { get; }
    public const string FileName = "Data/contacts.json";
    
    public PhoneBook()
    {
        Contacts = new Dictionary<string, Contact>();
        LoadContacts();
    }

    public void AddContact(string name, string phoneNumber)
    {
        
    }
    
    public void RemoveContact(string name)
    {
        
    }

    public void ListContacts()
    {
        
    }

    public void SaveContacts()
    {
        
    }
    
    public void LoadContacts()
    {
        
    }
    
}