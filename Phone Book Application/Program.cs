using System;
using System.Text.RegularExpressions;


namespace Phone_Book_Application
{
    public class Program
    {
        public static void Main()
        {
            PhoneBook phoneBook = new PhoneBook();
            PrintInstruction();
            UserInteraction(phoneBook);
            
        }
        
        static void UserInteraction(PhoneBook phoneBook)
        {
            string command = GetCommand("\nEnter a command: ");

            if (command.Equals("add"))
            {
            }
            
            if (command.Equals("remove"))
            {
                
            }
            
            if (command.Equals("list"))
            {
                
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
            Console.WriteLine("Enter 'exit' to exit the application");
        }
        
        
        static bool IsValidCommand(string? response)
        {
            return response is "add" or "remove" or "list" or "exit";
        }

        static string GetCommand(string prompt)
        {
            Console.WriteLine(prompt);
            string response = (Console.ReadLine() ?? string.Empty).Trim().ToLower();

            while (!IsValidCommand(response))
            {
                Console.WriteLine("Please, enter a valid command: add, remove, list or exit");  
                response = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
            }
            
            return response;
        }


    }
    
    
    
}
