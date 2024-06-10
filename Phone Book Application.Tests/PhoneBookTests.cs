using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;
using NSubstitute;

namespace Phone_Book_Application.Tests;

[TestFixture]
public class PhoneBookTests
{
    private PhoneBook _phoneBook;
    private SqliteConnection _connection;
    private StringWriter _stringWriter;
    private TextWriter _originalOutput;
    
    
    [SetUp]
    public void SetUp()
    {
        var inMemoryConnectionString = new SqliteConnectionStringBuilder { DataSource = ":memory:" }.ToString();
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration.GetConnectionString("DefaultConnection").Returns(inMemoryConnectionString);

        try
        {
            _phoneBook = new PhoneBook(mockConfiguration);
            _connection = new SqliteConnection(inMemoryConnectionString);
            _connection.Open();

            _connection.Execute("CREATE TABLE Contacts (Name VARCHAR(50) PRIMARY KEY, PhoneNumber VARCHAR(50) NOT NULL)");
            
            _stringWriter = new StringWriter();
            _originalOutput = Console.Out;
            Console.SetOut(_stringWriter);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Dispose();
        _stringWriter.Dispose();
        Console.SetOut(_originalOutput);
    }

    [Test]
    public void AddContact_True()
    {
        var result = _phoneBook.AddContact("John", "+995555555555");
        Assert.That(result, Is.True);
    }

    [Test]
    public void AddDuplicateContact_False()
    {
        _phoneBook.AddContact("Natia Pruidze", "+995123456789");
        var result = _phoneBook.AddContact("Natia Pruidze", "+995987654321");
        Assert.That(result, Is.False);
    }

    [Test]
    public void RemoveContact_True()
    {
        _phoneBook.AddContact("Natia Pruidze", "+995123456789");
        var result = _phoneBook.RemoveContact("Natia Pruidze");
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void RemoveNonExistingContact_False()
    {
        var result = _phoneBook.RemoveContact("Jane");
        Assert.That(result, Is.False);
    }

    [Test]
    public void SearchByName_True()
    {
        _phoneBook.AddContact("John", "+995123456789");
        var result = _phoneBook.SearchByName("John");
        Assert.That(result, Is.True);
    }

    [Test]
    public void SearchNonExistingContactByName_False()
    {
        var result = _phoneBook.SearchByName("Jane");
        Assert.That(result, Is.False);
    }

    [Test]
    public void UpdateContact_True()
    {
        _phoneBook.AddContact("John", "+995123456789");
        var result = _phoneBook.UpdateContact("John", "+995987654321");
        Assert.That(result, Is.True);
    }

    [Test]
    public void UpdateNonExistingContact_False()
    {
        var result = _phoneBook.UpdateContact("Jane", "+995123456789");
        Assert.That(result, Is.False);;
    }

    [Test]
    public void ListContacts_Prints()
    {
        _phoneBook.AddContact("John", "+995123456789");
        _phoneBook.AddContact("Jane", "+995987654321");

        using var sw = new StringWriter();
        Console.SetOut(sw);
        _phoneBook.ListContacts();

        var result = sw.ToString();
        Assert.That(result.Contains("Name: John Phone number: +995123456789"), Is.True);
        Assert.That(result.Contains("Name: Jane Phone number: +995987654321"), Is.True);
    }
}