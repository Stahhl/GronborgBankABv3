using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.IO;

/// <summary>
/// 'DataAccess' handels all database interactions
/// </summary>
namespace GronborgBankAB
{
    class DataAccess
    {
        public bool CheckConnection()
        {
            //Return true if it can open a connection
            //Throws an exception if not...

            bool result = false;

            try
            {
                SqlConnection connection = new SqlConnection(App.ConnectionString);
                connection.Open();

                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                    result = true;

                connection.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        // M - färdig
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customerList = new List<Customer>();

            string sqlQuery = @"SELECT * FROM Customers";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        FirstName = reader.GetSqlString(1).Value,
                        LastName = reader.GetSqlString(2).Value,
                        PersonNummer = reader.GetSqlString(3).Value,
                        Address = reader.IsDBNull(4) ? "Unknown" : reader.GetSqlString(4).Value,
                        Phone = reader.IsDBNull(5) ? "Unknown" : reader.GetSqlString(5).Value,
                        Email = reader.IsDBNull(6) ? "Unknown" : reader.GetSqlString(6).Value,
                        EmployeeId = reader.IsDBNull(7) ? -1 : reader.GetSqlInt32(7).Value
                    };

                    customerList.Add(customer);
                }

                connection.Close();
            }
            return customerList;
        }

        // M - färdig
        public Customer GetCustomerById(int customerId)
        {
            Customer customer;

            string sqlQuery = @"SELECT * FROM Customers WHERE Id=@id";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                command.Parameters.Add(new SqlParameter("id", customerId));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        FirstName = reader.GetSqlString(1).Value,
                        LastName = reader.GetSqlString(2).Value,
                        PersonNummer = reader.GetSqlString(3).Value,
                        Address = reader.IsDBNull(4) ? "Unknown" : reader.GetSqlString(4).Value,
                        Phone = reader.IsDBNull(5) ? "Unknown" : reader.GetSqlString(5).Value,
                        Email = reader.IsDBNull(6) ? "Unknown" : reader.GetSqlString(6).Value,
                        EmployeeId = reader.IsDBNull(7) ? -1 : reader.GetSqlInt32(7).Value
                    };
                    return customer;
                }
            }
            return null;
        }

        // M - färdig
        public Customer GetCustomerByAccountNumber(int accountNumber)
        {
            Customer customer;

            string sqlQuery = @"SELECT * FROM PersonAccounts WHERE AccountNumber=@AccountNumber";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                command.Parameters.Add(new SqlParameter("AccountNumber", accountNumber));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int customerId = reader.GetSqlInt32(1).Value;

                    customer = GetCustomerById(customerId);
                    return customer;
                }
            }
            return null;
        }

        // M - färdig
        public bool UpdateCustomer(Customer customer)
        {
            string sqlQuery = @"UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, PersonNummer=@PersonNummer,
                              Address=@Address, Phone=@Phone, Email=@Email, EmployeeId=@EmployeeId WHERE Id=@Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("Id", customer.Id));
                    command.Parameters.Add(new SqlParameter("FirstName", customer.FirstName));
                    command.Parameters.Add(new SqlParameter("LastName", customer.LastName));
                    command.Parameters.Add(new SqlParameter("PersonNummer", customer.PersonNummer));
                    command.Parameters.Add(new SqlParameter("Address", customer.Address));
                    command.Parameters.Add(new SqlParameter("Phone", customer.Phone));
                    command.Parameters.Add(new SqlParameter("Email", customer.Email));
                    command.Parameters.Add(new SqlParameter("EmployeeId", customer.EmployeeId));

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // M - färdig
        public bool AddNewCustomer(Customer customer)
        {
            string sqlQuery = @"INSERT INTO Customers (FirstName, LastName, PersonNummer, Address, Phone, Email, EmployeeId) 
                              VALUES 
                              (@FirstName, @LastName, @PersonNummer,@Address, @Phone, @Email, @EmployeeId)";
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("FirstName", customer.FirstName));
                    command.Parameters.Add(new SqlParameter("LastName", customer.LastName));
                    command.Parameters.Add(new SqlParameter("PersonNummer", customer.PersonNummer));
                    command.Parameters.Add(new SqlParameter("Address", customer.Address));
                    command.Parameters.Add(new SqlParameter("Phone", customer.Phone));
                    command.Parameters.Add(new SqlParameter("Email", customer.Email));
                    command.Parameters.Add(new SqlParameter("EmployeeId", customer.EmployeeId));

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw new Exception(e.Message);
            }
        }

        // M - färdig
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employeeList = new List<Employee>();

            string sqlQuery = @"SELECT * FROM Employees";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        FirstName = reader.GetSqlString(1).Value,
                        LastName = reader.GetSqlString(2).Value,
                        PersonNummer = reader.GetSqlString(3).Value,
                        Address = reader.IsDBNull(4) ? "Unknown" : reader.GetSqlString(4).Value,
                        Phone = reader.IsDBNull(5) ? "Unknown" : reader.GetSqlString(5).Value,
                        Email = reader.IsDBNull(6) ? "Unknown" : reader.GetSqlString(6).Value
                    };
                    employeeList.Add(employee);
                }
            }
            return employeeList;
        }

        // M - färdig
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee;

            string sqlQuery = @"SELECT * FROM Employees WHERE Id=@id";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                command.Parameters.Add(new SqlParameter("id", employeeId));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    employee = new Employee
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        FirstName = reader.GetSqlString(1).Value,
                        LastName = reader.GetSqlString(2).Value,
                        PersonNummer = reader.GetSqlString(3).Value,
                        Address = reader.IsDBNull(4) ? "Unknown" : reader.GetSqlString(4).Value,
                        Phone = reader.IsDBNull(5) ? "Unknown" : reader.GetSqlString(5).Value,
                        Email = reader.IsDBNull(6) ? "Unknown" : reader.GetSqlString(6).Value
                    };
                    return employee;
                }
            }
            return null;
        }

        // M - färdig
        public bool UpdateEmployee(Employee employee)
        {
            string sqlQuery = @"UPDATE Employees SET FirstName=@FirstName, LastName=@LastName, PersonNummer=@PersonNummer,
                              Address=@Address, Phone=@Phone, Email=@Email WHERE Id=@Id";
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("Id", employee.Id));
                    command.Parameters.Add(new SqlParameter("FirstName", employee.FirstName));
                    command.Parameters.Add(new SqlParameter("LastName", employee.LastName));
                    command.Parameters.Add(new SqlParameter("PersonNummer", employee.PersonNummer));
                    command.Parameters.Add(new SqlParameter("Address", employee.Address));
                    command.Parameters.Add(new SqlParameter("Phone", employee.Phone));
                    command.Parameters.Add(new SqlParameter("Email", employee.Email));
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // M - färdig
        public bool AddNewEmployee(Employee employee)
        {
            string sqlQuery = @"INSERT INTO Employees (FirstName, LastName, PersonNummer, Address, Phone, Email) 
                              VALUES 
                              (@FirstName, @LastName, @PersonNummer, @Address, @Phone, @Email)";
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("FirstName", employee.FirstName));
                    command.Parameters.Add(new SqlParameter("LastName", employee.LastName));
                    command.Parameters.Add(new SqlParameter("PersonNummer", employee.PersonNummer));
                    command.Parameters.Add(new SqlParameter("Address", employee.Address));
                    command.Parameters.Add(new SqlParameter("Phone", employee.Phone));
                    command.Parameters.Add(new SqlParameter("Email", employee.Email));

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw new Exception(e.Message);
            }
        }

        // M - färdig
        public List<Account> GetAllAccounts()
        {
            List<Account> accountList = new List<Account>();

            string sqlQuery = @"SELECT * FROM Accounts";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Account account = new Account
                    {
                        AccountNumber = reader.GetSqlInt32(0).Value,
                        AccountTypeId = reader.GetSqlInt32(1).Value,
                        Balance = reader.IsDBNull(2) ? 0 : reader.GetSqlDecimal(2).Value
                    };

                    accountList.Add(account);
                }

                connection.Close();
            }
            return accountList;
        }

        // M - färdig
        public List<Account> GetAccountsByCustomerId(int customerId)
        {
            List<Account> accountList = new List<Account>();

            string sqlQuery = @"SELECT * FROM Accounts FULL JOIN PersonAccounts ON " +
                              "Accounts.AccountNumber=PersonAccounts.AccountNumber WHERE CustomerId=@CustomerId";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                command.Parameters.Add(new SqlParameter("CustomerId", customerId));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Account account = new Account
                    {
                        AccountNumber = reader.GetSqlInt32(0).Value,
                        AccountTypeId = reader.GetSqlInt32(1).Value,
                        Balance = reader.IsDBNull(2) ? 0 : reader.GetSqlDecimal(2).Value
                    };

                    accountList.Add(account);
                }

                connection.Close();
            }
            return accountList;
        }

        public Account GetAccountByAccountNumber(int accountNumber)
        {
            Account account = new Account();

            string sqlQuery = @"SELECT * FROM Accounts WHERE AccountNumber=@AccountNumber";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                command.Parameters.Add(new SqlParameter("AccountNumber", accountNumber));

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    account = new Account
                    {
                        AccountNumber = reader.GetSqlInt32(0).Value,
                        AccountTypeId = reader.GetSqlInt32(1).Value,
                        Balance = reader.IsDBNull(2) ? 0 : reader.GetSqlDecimal(2).Value
                    };
                }
                connection.Close();
            }
            return account;
        }

        // M - färdig
        public bool UpdateAccount(Account account)
        {
            string sqlQuery = @"UPDATE Accounts SET AccountTypeId=@AccountTypeId, Balance=@Balance WHERE AccountNumber=@AccountNumber";
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.Add(new SqlParameter("AccountNumber", account.AccountNumber));
                    command.Parameters.Add(new SqlParameter("AccountTypeId", account.AccountTypeId));
                    command.Parameters.Add(new SqlParameter("Balance", account.Balance));

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // M - färdig
        public List<AccountType> GetAllAccountTypes()
        {
            List<AccountType> accountTypeList = new List<AccountType>();

            string sqlQuery = @"SELECT * FROM AccountType";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AccountType accountType = new AccountType
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        Interest = reader.GetSqlDecimal(1).Value,
                        AccountTypeName = reader.GetSqlString(2).Value
                    };

                    accountTypeList.Add(accountType);
                }

                connection.Close();
            }
            return accountTypeList;
        }

        // M - färdig
        public AccountType GetAccountTypeByAccountTypeId(int accountTypeId)
        {
            AccountType accountType = new AccountType();

            string sqlQuery = @"SELECT * FROM AccountType WHERE Id=@AccountTypeId";

            using (SqlConnection connection = new SqlConnection(App.ConnectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("AccountTypeId", accountTypeId));

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    accountType = new AccountType
                    {
                        Id = reader.GetSqlInt32(0).Value,
                        Interest = reader.GetSqlDecimal(1).Value,
                        AccountTypeName = reader.GetSqlString(2).Value
                    };

                }
                connection.Close();
            }
            return accountType;
        }

        // M - färdig
        public bool PerformTransaction(Account from, Account to, decimal amount)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                {
                    SqlCommand command;

                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    string sqlQueryFrom = @"UPDATE Accounts SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    command = new SqlCommand(sqlQueryFrom, connection);
                    command.Transaction = transaction;

                    command.Parameters.Add(new SqlParameter("AccountNumber", from.AccountNumber));
                    command.Parameters.Add(new SqlParameter("Balance", from.Balance - amount));
                    command.ExecuteNonQuery();

                    string sqlQueryTo = @"UPDATE Accounts SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    var command2 = new SqlCommand(sqlQueryTo, connection);
                    command2.Transaction = transaction;


                    command2.Parameters.Add(new SqlParameter("AccountNumber", to.AccountNumber));
                    command2.Parameters.Add(new SqlParameter("Balance", to.Balance + amount));
                    command2.ExecuteNonQuery();

                    Console.WriteLine("BEFORE COMMIT");
                    transaction.Commit();
                    Console.WriteLine("AFTER COMMIT");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // M - färdig
        public bool UpdateBalance(Account account, decimal amount)
        {
            string sqlQuery = @"UPDATE Accounts SET Balance = @Balance WHERE AccountNumber = @AccountNumber";

            try
            {
                using (SqlConnection connection = new SqlConnection(App.ConnectionString))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();

                    command.Parameters.Add(new SqlParameter("AccountNumber", account.AccountNumber));
                    command.Parameters.Add(new SqlParameter("Balance", account.Balance + amount));
                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
