using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;


namespace Phone_Book_Application
{
    public class Program
    {
        public static async Task Main()
        {
            var configuration = GetConfiguration();
            PhoneBook phoneBook = new PhoneBook(configuration);
            PrintInstruction();
            await UserInteraction(phoneBook);
            
            phoneBook.SaveContacts();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        static async Task UserInteraction(PhoneBook phoneBook)
        {
            string command = GetCommand("\nEnter a command: ");

            switch (command)
            {
                case "add":
                    string name = ReadName("Please enter a name: ");
                    string phoneNumber = ReadPhoneNumber("Please enter a phone number: ");
                    phoneBook.AddContact(name, phoneNumber);
                    break;
                case "remove":
                    name = ReadName("Please enter a name: ");
                    phoneBook.RemoveContact(name);
                    break;
                case "list":
                    phoneBook.ListContacts();
                    break;
                case "search":
                    name = ReadName("Please enter a name: ");
                    phoneBook.SearchByName(name);
                    break;
                case "update":
                    name = ReadName("Please enter a name: ");
                    phoneNumber = ReadPhoneNumber("Please enter a phone number: ");
                    phoneBook.UpdateContact(name, phoneNumber);
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine("Please enter a valid command.");
                    break;
            }
            
            await Continue(phoneBook);
            
        }

        static async Task Continue(PhoneBook phoneBook)
        {
            Console.WriteLine("Do you need anything else? (yes/no)");
            string answer = (Console.ReadLine() ?? String.Empty).Trim().ToLower();
            
            while (!IsValidAnswer(answer))
            {
                Console.WriteLine("Please enter a valid answer (yes/no):");
                answer = (Console.ReadLine() ?? String.Empty).Trim().ToLower();
            }

            if (answer.Equals("yes"))
            {
                await UserInteraction(phoneBook);
            }else if (answer.Equals("no"))
            {
                Console.WriteLine("Goodbye!");
            }
        }

        static string ReadPhoneNumber(string prompt)
        {
            Console.WriteLine(prompt);
            string phoneNumber = (Console.ReadLine() ?? string.Empty).Trim();
            
            while (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Please, enter a phone number in the following format (+995XXXXXXXXX or XXXXXXXXX)");
                phoneNumber = (Console.ReadLine() ?? string.Empty).Trim();
            }
            return phoneNumber;
            
        }

        static string ReadName(string prompt)
        {
            Console.WriteLine(prompt);
            string name = (Console.ReadLine()?? string.Empty).Trim();
            
            while (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Please, enter a name");
                name = (Console.ReadLine()?? string.Empty).Trim();
            }
            return name;
        }



        static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(?:\+995\d{9}|\d{9})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        static void PrintInstruction()
        {
            Console.WriteLine("Phone Book Application");
            Console.WriteLine("----------------------");
            Console.WriteLine("Enter 'add' to add a contact");
            Console.WriteLine("Enter 'remove' to remove a contact");
            Console.WriteLine("Enter 'list' to list all contacts");
            Console.WriteLine("Enter 'search' to search contact with a name");
            Console.WriteLine("Enter 'update' to update existing contact's phone number");
            Console.WriteLine("Enter 'exit' to exit the application");
        }
        
        
        static bool IsValidCommand(string? response)
        {
            return response is "add" or "remove" or "list" or "exit" or "search" or "update";
        }

        static string GetCommand(string prompt)
        {
            Console.WriteLine(prompt);
            string response = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

            while (!IsValidCommand(response))
            {
                Console.WriteLine("Please, enter a valid command: add, remove, list, update, search or exit");  
                response = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            }
            
            return response;
        }

        static bool IsValidAnswer(string answer)
        {
            return answer is "yes" or "no";
        }


    }
    
    
    
}
