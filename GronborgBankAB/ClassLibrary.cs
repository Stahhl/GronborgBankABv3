using System;

namespace GronborgBankAB
{
    public class Customer
    {
        public int Id { get; set; } //PK
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNummer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int EmployeeId { get; set; } //FK
    }
    public class Employee
    {
        public int Id { get; set; } //PK
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNummer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
    public class Account
    {
        public int AccountNumber { get; set; } //PK
        public int AccountTypeId { get; set; } //FK
        public decimal Balance { get; set; }
    }
    public class AccountType
    {
        public int Id { get; set; } //PK
        public decimal Interest { get; set; }
        public string AccountTypeName { get; set; }
    }
    public class PersonAccount
    {
        public int AccountNumber { get; set; } //FK
        public int CustmerId { get; set; } //FK
    }
    public class ColorManager
    {
        //Fields
        int colorIndex;

        ConsoleColor[] consoleColors = new ConsoleColor []
        {
            ConsoleColor.White,
            ConsoleColor.Red,
            ConsoleColor.Magenta,
            ConsoleColor.Yellow,
            ConsoleColor.Blue,
            ConsoleColor.Green
        };

        public ConsoleColor GetColor(bool reset)
        {
            if (reset == true || colorIndex >= consoleColors.Length)
                colorIndex = 0;

            ConsoleColor color = consoleColors[colorIndex];
            colorIndex++;

            return color;
        }
    }
}
