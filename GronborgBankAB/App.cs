using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 'App' handels all output / input
/// </summary>
namespace GronborgBankAB
{
    class App
    {
        //Properties
        public static string ConnectionString { get; private set; }

        //Fields
        private DataAccess _dataAccess;
        private ColorManager _colorManager;

        public void Init()
        {
            ConnectionString = "Server=(localdb)\\mssqllocaldb; Database=BankDB";
            _dataAccess = new DataAccess();
            _colorManager = new ColorManager();
            _dataAccess.CheckConnection();
            //WriteLine($"Connection is: {_dataAccess.CheckConnection()}", ConsoleColor.Green);    
        }
        public void Run()
        {
            bool keepGoing = true;

            while (keepGoing)
            {
                Console.Clear();
                WriteLine("Välkommen till Grönborg Bank!\n", ConsoleColor.Yellow);
                WriteLine("Vad vill du göra? Mata in modulnummer:\n", ConsoleColor.Yellow);
                WriteLine("1 - Lista alla kunder", ConsoleColor.White);
                WriteLine("2 - Lista alla anställda", ConsoleColor.White);
                WriteLine("3 - Lista alla konton", ConsoleColor.White);
                WriteLine("4 - Sök kund efter ID", ConsoleColor.White);
                WriteLine("7 - Kör sparroboten", ConsoleColor.White);
                WriteLine("10 - Gör överföring", ConsoleColor.White);
                WriteLine("11 - Uttag/insättning", ConsoleColor.White);
                WriteLine("12 - Avsluta", ConsoleColor.White);


                string input = GreenInput();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        ListCustomers(_dataAccess.GetAllCustomers(), true);
                        break;
                    case "2":
                        Console.Clear();
                        ListEmployees(_dataAccess.GetAllEmployees(), true);
                        break;
                    case "3":
                        Console.Clear();
                        ListAccounts(_dataAccess.GetAllAccounts(), true);
                        break;
                    case "4":
                        Console.Clear();
                        ChooseCustomerById();
                        break;
                    case "7":
                        CalculateFutureBalance();
                        break;
                    case "10":
                        PerformTransaction();
                        break;
                    case "11":
                        UpdateBalance();
                        break;
                    case "12":
                        keepGoing = false;
                        break;
                    default:
                        WriteLine("Invalid input: ", ConsoleColor.Red);
                        Console.ReadLine();
                        Run();
                        break;
                }
            
            }
        }
        void ChooseCustomerById()
        {
            WriteLine("Mata in ID:et på den kunden du vill editera eller RETUR för att gå tillbaka: ", ConsoleColor.Yellow);

            bool parsed = int.TryParse(GreenInput(), out int input);

            if (parsed == false || _dataAccess.GetCustomerById(input) == null)
            {
                Run();
            }
            else
            {
                Console.Clear();
                ShowDetailedCustomer(_dataAccess.GetCustomerById(input));
            }
        }
        void ShowDetailedCustomer(Customer customer)
        {
            List<Customer> customerList = new List<Customer>();
            List<Account> accountList = _dataAccess.GetAccountsByCustomerId(customer.Id);

            customerList.Add(customer);

            WriteLine("Kund: \n", ConsoleColor.Yellow);
            ListCustomers(customerList, false);
            WriteLine("\nKonton: \n", ConsoleColor.Yellow);
            ListAccounts(accountList, false);

            WriteLine("\nMata in 'edit' för att editera kunden: ", ConsoleColor.Yellow);
            WriteLine("\nMata in kontonummer för att se utvecklingen över 40 år: ", ConsoleColor.Yellow);
            WriteLine("\nMata in RETUR för att gå tillbaka: ", ConsoleColor.Yellow);

            string input = GreenInput().ToUpper();

            if(int.TryParse(input, out int result) == true)
            {
                var acc = accountList.Where(x => x.AccountNumber == result).ToList()[0];
                var accType = _dataAccess.GetAccountTypeByAccountTypeId(acc.AccountTypeId);
                CalculateFutureBalanceAuto(acc.Balance, accType.Interest);
                Console.ReadLine();
            }
            if (input == "EDIT")
            {
                Console.Clear();
                EditCustomer(customer);
            }
            else
            {
                Run();
            }
        }
        void ListAccounts(List<Account> accounts, bool stop)
        {
            #region header
            Write("Konto nummer: ", ConsoleColor.White, 5);
            Write("Konto typ: ", ConsoleColor.White, 20);
            Write("Saldo SEK: ", ConsoleColor.White, 20);
            Write("Ränta: ", ConsoleColor.White, 5);
            Console.WriteLine();
            #endregion

            foreach (var account in accounts)
            {
                ////A - AccontNumber, AT - AccountTypeName, A - Balance, AT - Interest
                var accountType = _dataAccess.GetAccountTypeByAccountTypeId(account.AccountTypeId);

                Write(account.AccountNumber.ToString(), _colorManager.GetColor(true), 15);
                Write(accountType.AccountTypeName.ToString(), _colorManager.GetColor(false), 20);
                Write(account.Balance.ToString(), _colorManager.GetColor(false), 20);
                Write(string.Format("{0:P2}", accountType.Interest), _colorManager.GetColor(false), 5);


                Console.WriteLine();
            }
            if(stop == true)
            {
                WriteLine("\nRETUR för att gå tillbaka: ", ConsoleColor.Yellow);
                Console.ReadLine();
                Run();
            }
        }
        void ListCustomers(List<Customer> customers, bool stop)
        {
            #region header
            Write("ID: ", ConsoleColor.White, 5);
            Write("Förnamn: ", ConsoleColor.White, 5);
            Write("Efternamn: ", ConsoleColor.White, 5);
            Write("Personnummer: ", ConsoleColor.White, 5);
            Write("Adress: ", ConsoleColor.White, 5);
            Write("Telefon: ", ConsoleColor.White, 5);
            Write("Email: ", ConsoleColor.White, 25);
            Write("Ansvarig: ", ConsoleColor.White, 5);
            Console.WriteLine();
            #endregion

            //List customers
            foreach (var customer in customers)
            {
                Employee employee = _dataAccess.GetEmployeeById(customer.EmployeeId);

                Write(customer.Id.ToString(), _colorManager.GetColor(true), 5);
                Write(customer.FirstName, _colorManager.GetColor(false), 10);
                Write(customer.LastName, _colorManager.GetColor(false), 10);
                Write(customer.PersonNummer, _colorManager.GetColor(false), 10);
                Write(customer.Address, _colorManager.GetColor(false), 10);
                Write(customer.Phone, _colorManager.GetColor(false), 10);
                Write(customer.Email, _colorManager.GetColor(false), 25);
                Write(employee.FirstName + " " + employee.LastName, _colorManager.GetColor(false), 5);

                Console.WriteLine();
            }

            if(stop == true)
            {
                WriteLine("\nMata in ID:et på den kunden du vill editera eller RETUR för att gå tillbaka: ", ConsoleColor.Yellow);

                bool parsed = int.TryParse(GreenInput().ToUpper(), out int input);

                if (parsed == false || _dataAccess.GetCustomerById(input) == null)
                {
                    Run();
                }
                else
                {
                    Console.Clear();
                    ShowDetailedCustomer(_dataAccess.GetCustomerById(input));
                }
            }
        }
        void ListEmployees(List<Employee> employees, bool stop)
        {
            #region header
            Write("ID: ", ConsoleColor.White, 5);
            Write("Förnamn: ", ConsoleColor.White, 5);
            Write("Efternamn: ", ConsoleColor.White, 5);
            Write("Personnummer: ", ConsoleColor.White, 5);
            Write("Adress: ", ConsoleColor.White, 5);
            Write("Telefon: ", ConsoleColor.White, 5);
            Write("Email: ", ConsoleColor.White, 25);
            Console.WriteLine();
            #endregion

            //List customers
            foreach (var employee in employees)
            {
                Write(employee.Id.ToString(), _colorManager.GetColor(true), 5);
                Write(employee.FirstName, _colorManager.GetColor(false), 10);
                Write(employee.LastName, _colorManager.GetColor(false), 10);
                Write(employee.PersonNummer, _colorManager.GetColor(false), 10);
                Write(employee.Address, _colorManager.GetColor(false), 10);
                Write(employee.Phone, _colorManager.GetColor(false), 10);
                Write(employee.Email, _colorManager.GetColor(false), 25);

                Console.WriteLine();
            }

            if(stop == true)
            {
                WriteLine("\nRETUR för att gå tillbaka: ", ConsoleColor.Yellow);
                Console.ReadLine();
                Run();
            }
        }
        void EditCustomer(Customer customer)
        {
            Employee employee = _dataAccess.GetEmployeeById(customer.EmployeeId);
            WriteLine($"1 - Förnamn:      {customer.FirstName}", ConsoleColor.White);
            WriteLine($"2 - Efternamn:    {customer.LastName}", ConsoleColor.White);
            WriteLine($"3 - PersonNummer: {customer.PersonNummer}", ConsoleColor.White);
            WriteLine($"4 - Adress:       {customer.Address}", ConsoleColor.White);
            WriteLine($"5 - Phone:        {customer.Phone}", ConsoleColor.White);
            WriteLine($"6 - Email:        {customer.Email}", ConsoleColor.White);
            WriteLine($"7 - Ansvarig:     {employee.FirstName} {employee.LastName}", ConsoleColor.White);

            WriteLine("Mata in nummret på vad du vill ändra: ", ConsoleColor.Yellow);
            WriteLine("Mata in 'save' för att spara ändringarna: ", ConsoleColor.Yellow);
            WriteLine("Mata in 'exit' för att gå tillbaka utan att spara: ", ConsoleColor.Yellow);

            switch (GreenInput().ToUpper())
            {
                case "1":
                    WriteLine("Mata in nytt förnamn: ", ConsoleColor.Yellow);
                    customer.FirstName = GreenInput();
                    break;
                case "2":
                    WriteLine("Mata in nytt efternamn: ", ConsoleColor.Yellow);
                    customer.LastName = GreenInput();
                    break;
                case "3":
                    WriteLine("Mata in nytt personnummer: ", ConsoleColor.Yellow);
                    customer.FirstName = GreenInput();
                    break;
                case "4":
                    WriteLine("Mata in ny adress: ", ConsoleColor.Yellow);
                    customer.FirstName = GreenInput();
                    break;
                case "5":
                    WriteLine("Mata in nytt telefonnummer: ", ConsoleColor.Yellow);
                    customer.FirstName = GreenInput();
                    break;
                case "6":
                    WriteLine("Mata in ny email: ", ConsoleColor.Yellow);
                    customer.FirstName = GreenInput();
                    break;
                case "7":
                    Console.Clear();
                    ListEmployees(_dataAccess.GetAllEmployees(), false);
                    WriteLine("Mata in ID:et på nya ansvarige: ", ConsoleColor.Yellow);
                    customer.EmployeeId = int.Parse(GreenInput());
                    break;
                case "EXIT":
                    Run();
                    break;
                case "SAVE":
                    _dataAccess.UpdateCustomer(customer);
                    Run();
                    break;
                default:
                    Console.Clear();
                    WriteLine("Invalid input", ConsoleColor.Red);
                    EditCustomer(customer);
                    break;
            }
            EditCustomer(customer);
        }
        void EditEmployee(Employee employee)
        {

        }
        private void PerformTransaction()
        {
            List<Account> accountList = _dataAccess.GetAllAccounts();
            ListAccounts(accountList, false);

            Console.Write("Mata in kontonumret på det konto du vill skicka pengar från: ");
            int accountNumberFrom = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Account accountFrom = _dataAccess.GetAccountByAccountNumber(accountNumberFrom);

            Console.Write("Mata in kontonumret på det konto du vill skicka pengar till: ");
            int accountNumberTo = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Account accountTo = _dataAccess.GetAccountByAccountNumber(accountNumberTo);

            Console.Write("Belopp att överföra (SEK): ");
            decimal amount = decimal.Parse(Console.ReadLine());
            Console.WriteLine();

            ListAccounts(new List<Account> { accountFrom, accountTo }, false);

            if (_dataAccess.PerformTransaction(accountFrom, accountTo, amount))
            {
                Console.WriteLine("SUCCESS");
            }

            Account accountFrom2 = _dataAccess.GetAccountByAccountNumber(accountNumberFrom);
            Account accountTo2 = _dataAccess.GetAccountByAccountNumber(accountNumberTo);
            ListAccounts(new List<Account> { accountFrom2, accountTo2 }, false);
            Console.ReadLine();
        }

        private void UpdateBalance()
        {
            WriteLine("Vad vill du göra? Mata in modulnummer:\n", ConsoleColor.Yellow);
            WriteLine("1 - Uttag", ConsoleColor.White);
            WriteLine("2 - Insättning", ConsoleColor.White);

            string userInput = GreenInput();
            bool uttag = true;
            bool validInput = false;

            if (userInput == "1")
            {
                uttag = true;
                validInput = true;
            }
            else if (userInput == "2")
            {
                uttag = false;
                validInput = true;
            }

            if (validInput)
            {
                List<Account> accountList = _dataAccess.GetAllAccounts();
                ListAccounts(accountList, false);

                if (uttag)
                {
                    Console.Write("Mata in kontonumret på det konto du vill genomföra uttaget på: ");
                }
                else
                {
                    Console.Write("Mata in kontonumret på det konto du vill genomföra insättningen på: ");
                }
                int accountNumber = int.Parse(GreenInput());
                Console.WriteLine();
                Account account = _dataAccess.GetAccountByAccountNumber(accountNumber);
                ListAccounts(new List<Account> { account }, false);

                Console.Write("Belopp: ");
                decimal amount = decimal.Parse(GreenInput());
                Console.WriteLine();

                if (uttag)
                {
                    amount = -amount;
                }

                _dataAccess.UpdateBalance(account, amount);

                Account account2 = _dataAccess.GetAccountByAccountNumber(accountNumber);
                ListAccounts(new List<Account> { account2 }, false);

            }
        }

        private void CalculateFutureBalance()
        {
            Console.Write("Skriv in startsumma: ");
            decimal currentBalance = decimal.Parse(Console.ReadLine());
            Console.Write("Skriv in ränta i procent: ");
            decimal interest = decimal.Parse(Console.ReadLine().Replace(',', '.')) / 100;
            Console.Write("Skriv in hur mycket du planerar att sätta in per månad: ");
            decimal monthlySavings = decimal.Parse(Console.ReadLine());
            Console.Write("Skriv in antal år du tänkt spara: ");
            int numberOfYears = int.Parse(Console.ReadLine());


            List<string> yearByYear = SparRobot.CalculateFutureBalance(currentBalance, interest, monthlySavings, numberOfYears);

            foreach (var year in yearByYear)
            {
                Console.WriteLine(year);
            }
            Console.ReadLine();
        }

        private void CalculateFutureBalanceAuto(decimal currentBalance, decimal interest)
        {
            List<string> yearByYear = SparRobot.CalculateFutureBalance(currentBalance, interest, 0, 40);

            foreach (var year in yearByYear)
            {
                Console.WriteLine(year);
            }
        }

        private string GreenInput()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string input = Console.ReadLine();
            Console.ResetColor();
            return input;
        }
        private void Write(string msg, ConsoleColor color, int padAmount)
        {
            Console.ForegroundColor = color;
            Console.Write($"{msg.PadRight(padAmount)}\t");
            Console.ResetColor();
        }
        private void WriteLine(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
