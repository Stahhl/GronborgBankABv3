go
use master;

go
drop database BankDB;

go
create database BankDB;

go
use BankDB;

create table Employees
(
	[Id] int primary key identity not null,
	[FirstName] varchar(32) not null,
	[LastName] varchar(32) not null,
	[PersonNummer] varchar(12) not null,
	[Address] varchar(64),
	[Phone] varchar(32),
	[Email] varchar(32)
	unique(PersonNummer)
);
create table Customers
(
	[Id] int primary key identity not null,
	[FirstName] varchar(32) not null,
	[LastName] varchar(32) not null,
	[PersonNummer] varchar(12) not null,
	[Address] varchar(64),
	[Phone] varchar(32),
	[Email] varchar(32),
	[EmployeeId] int foreign key references [Employees](Id)
	unique(PersonNummer)
);
CREATE TABLE AccountType
(
    [Id] INT PRIMARY KEY IDENTITY NOT NULL,
    [Interest] DECIMAL(5,5) NOT NULL,
    [AccountTypeName] VARCHAR (64) NOT NULL
	UNIQUE ([AccountTypeName])
)

CREATE TABLE Accounts
(
    [AccountNumber] INT PRIMARY KEY IDENTITY NOT NULL,
    [AccountTypeId] INT FOREIGN KEY REFERENCES AccountType(Id) NOT NULL,
    [Balance] DECIMAL(19,5) NULL
)

CREATE TABLE PersonAccounts
(
    [AccountNumber] INT FOREIGN KEY REFERENCES Accounts(AccountNumber) NOT NULL,
    [CustomerId] INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL
)

