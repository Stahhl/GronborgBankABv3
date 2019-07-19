using System;
using System.Collections.Generic;
using System.Linq;

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
        }
        public void Run()
        {
            Console.Clear();
            WriteLine("---------------------------------------------------", ConsoleColor.Yellow);

            WriteLine("| $$$" + "Välkommen till Grönborg Bank!".PadLeft(36) + "$$$ |".PadLeft(10), ConsoleColor.Yellow);
            WriteLine("---------------------------------------------------", ConsoleColor.Yellow);

            Console.WriteLine();

            WriteLine("Vad vill du göra? Mata in modulnummer:\n", ConsoleColor.Yellow);
            WriteLine("1 - Lista alla kunder", ConsoleColor.White);
            WriteLine("2 - Lista alla anställda", ConsoleColor.White);
            WriteLine("3 - Lista alla konton", ConsoleColor.White);
            WriteLine("4 - Sök kund efter ID", ConsoleColor.White);
            WriteLine("5 - Lägg till kund", ConsoleColor.White);
            WriteLine("6 - Lägg till anställd", ConsoleColor.White);
            WriteLine("7 - Kör sparroboten", ConsoleColor.White);
            WriteLine("8 - Lägg till konto hos kund", ConsoleColor.White);
            WriteLine("9 - Lista alla kontotyper", ConsoleColor.White);
            WriteLine("10 - Gör överföring", ConsoleColor.White);
            WriteLine("11 - Uttag/insättning", ConsoleColor.White);
            WriteLine("14 - Ta bort kund", ConsoleColor.White);
            WriteLine("15 - Ta bort anställd", ConsoleColor.White);
            WriteLine("20 - Avsluta", ConsoleColor.White);

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
                case "5":
                    Console.Clear();
                    AddCustomer(new Customer(), 0);
                    break;
                case "6":
                    Console.Clear();
                    AddEmployee(new Employee(), 0);
                    break;
                case "7":
                    CalculateFutureBalance();
                    break;
                case "8":
                    Console.Clear();
                    AddAccountToCustomer(null, new Account(), new AccountType(), 0);
                    break;
                case "9":
                    Console.Clear();
                    ListAccountTypes(_dataAccess.GetAllAccountTypes(), true);
                    break;
                case "10":
                    PerformTransaction();
                    break;
                case "11":
                    UpdateBalance();
                    break;
                case "14":
                    Console.Clear();
                    RemoveCustomer();
                    break;
                case "15":
                    Console.Clear();
                    RemoveEmployee();
                    break;
                case "20":
                    Environment.Exit(0);
                    break;
                default:
                    WriteLine("Invalid input: ", ConsoleColor.Red);
                    Console.ReadLine();
                    Run();
                    break;
            }
        }
        private void RemoveEmployee()
        {
            Console.Clear();
            ListEmployees(_dataAccess.GetAllEmployees(), false);
            Console.WriteLine();

            Console.Write("Mata in ID:et på den anställda du vill ta bort: ");

            string input = GreenInput();

            if (int.TryParse(input, out int result) == true)
            {
                Employee emp = _dataAccess.GetEmployeeById(result);
                if (emp != null)
                {
                    ListEmployees(new List<Employee> { emp }, false);
                    Console.WriteLine();
                    Console.WriteLine("Vill du ta bort denna anställda?");
                    Console.WriteLine("1) Ta bort");
                    Console.WriteLine("9) Avbryt");

                    string userAnswer = Console.ReadLine();

                    if (userAnswer == "1")
                    {
                        if (_dataAccess.RemoveEmployee(emp.Id))
                        {
                            Console.WriteLine($"Successfully removed employee, press enter to continue");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Removal of employee failed, press enter to continue");
                            Console.ReadLine();
                        }

                    }
                    if (userAnswer == "9")
                    {

                    }

                }
                else
                {
                    Console.Write($"Det finns ingen anställd med ID \"{input}\"");
                }
            }
            Run();
        }
        private void RemoveCustomer()
        {
            Console.Clear();
            ListCustomers(_dataAccess.GetAllCustomers(), false);
            Console.WriteLine();

            Console.Write("Mata in ID:et på den kund du vill ta bort: ");

            string input = GreenInput();

            if (int.TryParse(input, out int result) == true)
            {
                Customer customer = _dataAccess.GetCustomerById(result);
                if (customer != null)
                {
                    ListCustomers(new List<Customer> { customer }, false);
                    Console.WriteLine();
                    Console.WriteLine("Vill du ta bort denna kund?");
                    Console.WriteLine("1) Ta bort");
                    Console.WriteLine("9) Avbryt");

                    string userAnswer = Console.ReadLine();

                    if (userAnswer == "1")
                    {
                        if (_dataAccess.RemoveCustomer(customer.Id))
                        {
                            Console.WriteLine($"Kunden borttagen, tryck enter för att fortsätta");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Borttagning misslyckades, tryck enter för att fortsätta");
                            Console.ReadLine();
                        }

                    }
                    else if (userAnswer == "9")
                    {

                    }
                }
                else
                {
                    Console.Write($"Det finns ingen anställd med ID \"{input}\". Tryck enter för att fortsätta");
                    Console.ReadLine();
                }
            }
            Run();
        }
        private void AddAccountToCustomer(Customer customer, Account account, AccountType accountType, int index)
        {
            if(customer == null)
            {
                Console.Write("Mata in ID:et på kunden du vill använda: ");
                if(int.TryParse(GreenInput(), out int result1) == true)
                {
                    customer = _dataAccess.GetCustomerById(result1);
                    if (customer == null)
                    {
                        WriteLine("Invalid input! ", ConsoleColor.Red);
                        AddAccountToCustomer(customer, new Account(), new AccountType(), 0);
                    }
                }
                Console.Clear();
            }

            WriteLine("Kund:\n", ConsoleColor.Yellow);
            ListCustomers(new List<Customer> { customer }, false);
            WriteLine("\nKonto:\n", ConsoleColor.Yellow);
            WriteLine($"Kontotyp: {accountType.AccountTypeName}", ConsoleColor.White);
            WriteLine($"Saldo:    {account.Balance}", ConsoleColor.White);
            WriteLine($"Ränta:    {accountType.Interest}", ConsoleColor.White);
            Console.WriteLine();
            //Kontotyp, ränta
            if(index == 0)
            {
                WriteLine($"Mata in ID:et på kontotypen det nya konto bör vara: \n", ConsoleColor.Yellow);
                ListAccountTypes(_dataAccess.GetAllAccountTypes(), false);
                string input = GreenInput();
                if(int.TryParse(input, out int accType) == true && _dataAccess.GetAccountTypeByAccountTypeId(accType) != null)
                {
                    accountType.AccountTypeName = _dataAccess.GetAccountTypeByAccountTypeId(accType).AccountTypeName;
                    accountType.Interest = _dataAccess.GetAccountTypeByAccountTypeId(accType).Interest;
                    account.AccountTypeId = _dataAccess.GetAccountTypeByAccountTypeId(accType).Id;
                    index++;
                    Console.Clear();
                    AddAccountToCustomer(customer, account, accountType, index);
                }
                else
                {
                    Console.Clear();
                    WriteLine("Invalid output", ConsoleColor.Red);
                    AddAccountToCustomer(customer, account, accountType, 0);
                }
            }
            //saldo
            if(index == 1)
            {
                Write($"Mata in saldo i det nya kontot: ", ConsoleColor.Yellow, 0);
                string inputBalance = GreenInput().Replace(",",".");
                if(decimal.TryParse(inputBalance, out decimal balance) == true)
                {
                    account.Balance = balance;
                    index++;
                    Console.Clear();
                    AddAccountToCustomer(customer, account, accountType, index);
                }
                else
                {
                    Console.Clear();
                    WriteLine("Invalid output", ConsoleColor.Red);
                    AddAccountToCustomer(customer, account, accountType, 0);
                }
            }

            Console.WriteLine("Mata in 'save' för att spara kontot: ");
            Console.WriteLine("Mata in 'restart' för att ångra och börja om: ");
            Console.WriteLine("Mata in RETUR för att gå tillbaka utan att spara: ");

            string input2 = GreenInput();

            if (input2.ToUpper() == "SAVE")
            {
                PersonAccount personAccount = new PersonAccount();
                _dataAccess.AddNewAccount(account);

                personAccount.CustmerId = customer.Id;
                personAccount.AccountNumber = _dataAccess.GetLatestAccountId();

                _dataAccess.AddNewPersonAccount(personAccount);
            }
            if (input2.ToUpper() == "RESTART")
                AddAccountToCustomer(customer, account, accountType, 0);

            Run();
        }
        private void AddEmployee(Employee employee, int index)
        {
            Console.Clear();

            WriteLine("Förnamn:          " + employee.FirstName, ConsoleColor.White);
            WriteLine("Efternamn:        " + employee.LastName, ConsoleColor.White);
            WriteLine("Personnummer:     " + employee.PersonNummer, ConsoleColor.White);
            WriteLine("Adress:           " + employee.Address, ConsoleColor.White);
            WriteLine("Telefonnummer:    " + employee.Phone, ConsoleColor.White);
            WriteLine("Email:            " + employee.Email, ConsoleColor.White);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Input from user
            try
            {
                //Förnamn
                if (index == 0)
                {
                    Console.Write("Mata in förnamn (not optional): ");
                    string input = GreenInput();

                    if (input.Length < 1)
                        throw new Exception();

                    employee.FirstName = input;
                    index++;
                    AddEmployee(employee, index);
                }
                //Efternamn
                if (index == 1)
                {
                    Console.Write("Mata in efternamn (not optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        throw new Exception();

                    employee.LastName = input;
                    index++;
                    AddEmployee(employee, index);
                }
                //Personnummer
                if (index == 2)
                {
                    Console.Write("Mata in personnummer (not optional): ");

                    string input = GreenInput().Replace("-", "");
                    string control = input.Replace("-", "");

                    bool parsed = true;
                    foreach (char c in control)
                    {
                        if (int.TryParse(c.ToString(), out int result) == false)
                            parsed = false;
                    }

                    if (parsed == false || input.Length < 1 || input.Length > 12)
                        throw new Exception();

                    employee.PersonNummer = input;
                    index++;
                    AddEmployee(employee, index);
                }
                //Adress
                if (index == 3)
                {
                    Console.Write("Mata in adress (optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        input = null;

                    employee.Address = input;
                    index++;
                    AddEmployee(employee, index);
                }
                //Telefonnummer
                if (index == 4)
                {
                    Console.Write("Mata in telefonnummer (optional): ");

                    string input = GreenInput().Replace(" ", "");
                    string control = input.Replace(" ", "");

                    bool parsed = true;
                    foreach (char c in control)
                    {
                        if (int.TryParse(c.ToString(), out int result) == false)
                            parsed = false;
                    }

                    if (input.Length < 1 || parsed == false)
                        input = string.Empty;

                    employee.Phone = input;
                    index++;
                    AddEmployee(employee, index);
                }
                //Email
                if (index == 5)
                {
                    Console.Write("Mata in email (optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        input = string.Empty;

                    employee.Email = input;
                    index++;
                    AddEmployee(employee, index);
                }
            }
            catch
            {
                WriteLine("Invalid input press RETUR to continue: ", ConsoleColor.Red);
                WriteLine("Enter 'exit' to go back: ", ConsoleColor.Yellow);

                if (Console.ReadLine().ToUpper() == "EXIT")
                    Run();

                AddEmployee(employee, index);
            }

            Console.WriteLine("Mata in 'save' för att spara den anställde: ");
            Console.WriteLine("Mata in 'restart' för att ångra och börja om: ");
            Console.WriteLine("Mata in RETUR för att gå tillbaka utan att spara: ");

            string input2 = GreenInput();

            if (input2.ToUpper() == "SAVE")
                _dataAccess.AddNewEmployee(employee);
            if (input2.ToUpper() == "RESTART")
                AddEmployee(new Employee(), 0);

            Run();
        }
        private void AddCustomer(Customer customer, int index)
        {
            Console.Clear();
            Employee employee = customer.EmployeeId != 0 ? employee = _dataAccess.GetEmployeeById(customer.EmployeeId) : null;
            string employeeName = employee != null ? employee.FirstName + " " + employee.LastName : string.Empty;

            WriteLine("Förnamn:          " + customer.FirstName, ConsoleColor.White);
            WriteLine("Efternamn:        " + customer.LastName, ConsoleColor.White);
            WriteLine("Personnummer:     " + customer.PersonNummer, ConsoleColor.White);
            WriteLine("Adress:           " + customer.Address, ConsoleColor.White);
            WriteLine("Telefonnummer:    " + customer.Phone, ConsoleColor.White);
            WriteLine("Email:            " + customer.Email, ConsoleColor.White);
            WriteLine("Ansvarig:         " + employeeName, ConsoleColor.White);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;

            //Input from user
            try
            {
                //Förnamn
                if (index == 0)
                {
                    Console.Write("Mata in förnamn (not optional): ");
                    string input = GreenInput();

                    if (input.Length < 1)
                        throw new Exception();

                    customer.FirstName = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Efternamn
                if (index == 1)
                {
                    Console.Write("Mata in efternamn (not optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        throw new Exception();

                    customer.LastName = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Personnummer
                if (index == 2)
                {
                    Console.Write("Mata in personnummer (not optional): ");

                    string input = GreenInput().Replace("-", "");
                    string control = input.Replace("-", "");

                    bool parsed = true;
                    foreach (char c in control)
                    {
                        if (int.TryParse(c.ToString(), out int result) == false)
                            parsed = false;
                    }

                    if (parsed == false || input.Length < 1 || input.Length > 12)
                        throw new Exception();

                    customer.PersonNummer = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Adress
                if (index == 3)
                {
                    Console.Write("Mata in adress (optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        input = string.Empty;

                    customer.Address = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Telefonnummer
                if (index == 4)
                {
                    Console.Write("Mata in telefonnummer (optional): ");

                    string input = GreenInput().Replace(" ", "");
                    string control = input.Replace(" ", "");

                    bool parsed = true;
                    foreach (char c in control)
                    {
                        if (int.TryParse(c.ToString(), out int result) == false)
                            parsed = false;
                    }

                    if (input.Length < 1 || parsed == false)
                        input = string.Empty;

                    customer.Phone = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Email
                if (index == 5)
                {
                    Console.Write("Mata in email (optional): ");

                    string input = GreenInput();

                    if (input.Length < 1)
                        input = string.Empty;

                    customer.Email = input;
                    index++;
                    AddCustomer(customer, index);
                }
                //Ansvarig
                if (index == 6)
                {
                    ListEmployees(_dataAccess.GetAllEmployees(), false);
                    Console.Write("Mata in ID:et på ansvarig anställd (optional): ");

                    string input = GreenInput();
                    
                    if(int.TryParse(input, out int result) == true)
                    {
                        Employee emp = _dataAccess.GetEmployeeById(result);
                        if (emp != null)
                            customer.EmployeeId = emp.Id;
                    }
                    index++;
                    AddCustomer(customer, index);
                }
            }
            catch
            {
                WriteLine("Invalid input press RETUR to continue: ", ConsoleColor.Red);
                WriteLine("Enter 'exit' to go back: ", ConsoleColor.Yellow);

                if (Console.ReadLine().ToUpper() == "EXIT")
                    Run();

                AddCustomer(customer, index);
            }

            Console.WriteLine("Mata in 'save' för att spara kunden: ");
            Console.WriteLine("Mata in 'restart' för att ångra och börja om: ");
            Console.WriteLine("Mata in RETUR för att gå tillbaka utan att spara: ");

            string input2 = GreenInput();

            if (input2.ToUpper() == "SAVE")
                _dataAccess.AddNewCustomer(customer);
            if (input2.ToUpper() == "RESTART")
                AddCustomer(new Customer(), 0);

            Run();
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
            WriteLine("Mata in 'account' för att lägga till ett konto: ", ConsoleColor.Yellow);
            WriteLine("Mata in kontonummer för att se utvecklingen över 40 år: ", ConsoleColor.Yellow);
            WriteLine("Mata in RETUR för att gå tillbaka: ", ConsoleColor.Yellow);

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
            if(input == "ACCOUNT")
            {
                Console.Clear();
                AddAccountToCustomer(customer, new Account(), new AccountType(), 0);
            }
            else
            {
                Run();
            }
        }
        void ListAccounts(List<Account> accounts, bool stop)
        {
            #region header
            Write("Kontonummer: ", ConsoleColor.White, 5);
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
                // NY FUNKTION HÄR ---------------------------------------------------------------------------------------------------
                Employee employee = new Employee();
                if (customer.EmployeeId != -1)
                {
                    employee = _dataAccess.GetEmployeeById(customer.EmployeeId);
                }
                else
                {
                    employee.FirstName = "Unknown";
                    employee.LastName = "Unknown";
                }
                // NY FUNKTION HÄR ---------------------------------------------------------------------------------------------------

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
        void ListAccountTypes(List<AccountType> accountTypes, bool stop)
        {
            #region header
            Write("ID: ", ConsoleColor.White, 5);
            Write("Konto typ: ", ConsoleColor.White, 25);
            Write("Ränta: ", ConsoleColor.White, 5);

            Console.WriteLine();
            #endregion
            foreach (var aT in accountTypes)
            {
                Write(aT.Id.ToString(), _colorManager.GetColor(true), 5);
                Write(aT.AccountTypeName.ToString(), _colorManager.GetColor(false), 25);
                Write(aT.Interest.ToString(), _colorManager.GetColor(false), 5);
                Console.WriteLine();
            }
            if (stop == true)
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

            string input = GreenInput().ToUpper();

            if (input == string.Empty)
                Run();

            switch (input)
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
                    customer.PersonNummer = GreenInput();
                    break;
                case "4":
                    WriteLine("Mata in ny adress: ", ConsoleColor.Yellow);
                    customer.Address = GreenInput();
                    break;
                case "5":
                    WriteLine("Mata in nytt telefonnummer: ", ConsoleColor.Yellow);
                    customer.Phone = GreenInput();
                    break;
                case "6":
                    WriteLine("Mata in ny email: ", ConsoleColor.Yellow);
                    customer.Email = GreenInput();
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
            Console.Clear();
            EditCustomer(customer);
        }
        void EditEmployee(Employee employee)
        {
            Run();
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
                Console.WriteLine("TRANSACTION COMPLETED");
            }

            Account accountFrom2 = _dataAccess.GetAccountByAccountNumber(accountNumberFrom);
            Account accountTo2 = _dataAccess.GetAccountByAccountNumber(accountNumberTo);
            ListAccounts(new List<Account> { accountFrom2, accountTo2 }, false);
            Console.WriteLine("Press ENTER to continue: ");
            Console.ReadLine();
            Run();
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
            Console.WriteLine("Press ENTER to continue: ");
            Console.ReadLine();
            Run();
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
            Console.WriteLine("Press ENTER to continue: ");
            Console.ReadLine();
            Run();
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
