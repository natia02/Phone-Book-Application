# Phone Book Application

## Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Commands](#commands)
- [Contact](#contact)

## Introduction
The **Phone Book Application** is a simple console-based application that allows users to manage their contacts. Users can add, remove, list, search, and update contact information, and the contact data is saved to a JSON file for persistence.

## Features
- **Add Contacts:** Add new contacts with a name and phone number.
- **Remove Contacts:** Remove existing contacts by name.
- **List Contacts:** Display all stored contacts.
- **Search Contacts:** Search for a contact by name.
- **Update Contacts:** Update the phone number of an existing contact.
- **Persistent Storage:** Contacts are saved to a JSON file and loaded when the application starts.

## Installation
1. **Clone the repository:**
    ```sh
    git clone https://github.com/natia02/Phone-Book-Application.git
    ```
2. **Navigate to the project directory:**
    ```sh
    cd phone-book-application
    ```
3. **Open the project in your preferred C# IDE (e.g., Visual Studio, JetBrains Rider).**
4. **Build the project to restore dependencies.**

## Usage
1. **Run the application:**
    - Execute the `Main` method in the `Program` class.
    - The application will load existing contacts from `contacts.json` if the file exists.

2. **Follow the on-screen instructions to interact with the application.**

## Commands
- **`add`**: Add a new contact.
    - Prompts for the contact's name and phone number.
- **`remove`**: Remove an existing contact.
    - Prompts for the contact's name.
- **`list`**: List all contacts.
    - Displays all stored contacts with their details.
- **`search`**: Search for a contact by name.
    - Prompts for the contact's name and displays the corresponding contact if found.
- **`update`**: Update an existing contact's phone number.
    - Prompts for the contact's name and new phone number.
- **`exit`**: Exit the application.

### Example Usage
1. **Add a Contact:**
    ```
    Enter a command: add
    Please enter a name: John Doe
    Please enter a phone number: +995123456789
    Contact with name John Doe has been added.
    ```

2. **List Contacts:**
    ```
    Enter a command: list
    Name: John Doe Phone number: +995123456789
    ```

3. **Search for a Contact:**
    ```
    Enter a command: search
    Please enter a name: John Doe
    Name: John Doe Phone number: +995123456789
    ```

4. **Update a Contact:**
    ```
    Enter a command: update
    Please enter a name: John Doe
    Please enter a phone number: +995987654321
    Contact with name John Doe has been updated.
    ```

5. **Remove a Contact:**
    ```
    Enter a command: remove
    Please enter a name: John Doe
    Contact with name John Doe has been removed.
    ```

6. **Exit the Application:**
    ```
    Enter a command: exit
    Goodbye!
    ```
