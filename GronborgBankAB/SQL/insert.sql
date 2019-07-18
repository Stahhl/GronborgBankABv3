GO
USE [master];

GO
USE [BankDB];

GO
INSERT INTO [Employees]([FirstName], [LastName], [PersonNummer], [Address], [Phone], [Email])
VALUES
('Test', 'McTestsson', '201010101010', 'Testgatan 1','777 777 77 77', 'test@testmail.test'),
('Arvid', 'Ståhl Strömberg', '001224-25', 'Testgatan 2','888 888 88 88', 'arvid@gronborgbank.test'),
('Mattias', 'Berglund', '190708-1337', 'Testgatan 3','999 999 99 99', 'mattias@gronborgbank.test'),
('Victoria', 'Asprou', '010101-1010', 'Testgatan 4','010 777 99 88', 'victoria@gronborgbank.test');

GO
INSERT INTO [Customers]([FirstName], [LastName], [PersonNummer], [Address], [Phone], [Email], [EmployeeId])
VALUES
('David', 'Davidsson', '991231-0000', NULL,'777 777 77 00', 'david@coolmail.com', 1),
('Oscar', 'Linros', '991231-0001', 'testgatan 01','777 777 77 01', 'oscar@coolmail.com', 2),
('Linda', 'Lindadottir', '991231-0002', 'testgatan 01','777 777 77 02', 'linda@coolmail.com', 3),
('Ali', 'Testsson', '991231-0003', 'testgatan 01','777 777 77 03', 'ali@coolmail.com', 4),
('Hulda', 'Huldasson', '991231-0004', 'testgatan 01','777 777 77 04', 'hulda@coolmail.com', 4),
('Erik', 'Eriksson', '991231-0005', 'testgatan 01',NULL, 'erik@coolmail.com', 1),
('Vladimir', 'Putin', '991231-0006', 'testgatan 01','777 777 77 06', 'vladimir@coolmail.ru', 1),
('Xi', 'Jingping', '991231-0007', 'testgatan 01','777 777 77 07', 'xi@winniepoohfan.cn', 2),
('Ursula', 'Martinsson', '991231-0008', 'testgatan 01','777 777 77 08', 'ursula@coolmail.com', 4),
('David', 'Davidzon', '991231-0009', 'testgatan 01','777 777 77 09', NULL, 4);

GO
INSERT INTO [AccountType]([Interest], [AccountTypeName])
VALUES
(0,'Lönekonto'),
(0.005, 'Pensionskonto'),
(0.001, 'Sparkonto'),
(0.002, 'Fond A'),
(0.003, 'Fond B'),
(0.004, 'Fond C'),
(0.005, 'Fond D'),
(0.1337, 'e-zPorts investments'),
(0.5, 'VIP konto');

GO
INSERT INTO [Accounts]([AccountTypeId], [Balance])
VALUES
(1, 4245),
(1, 2.424),
(1, 2525),
(1, 423424),
(1, 62346.36),
(1, 3434626),
(1, 6245646),
(1, 26245645.645),
(1, 25624562456),
(1, 262456456),
(2, 13113),
(2, 31233.13),
(2, 412414),
(2, 11444),
(3, 36173617),
(3, 41414),
(3, 36173615252.7),
(3, 76531),
(4, 174671),
(5, 78417481),
(6, 73737),
(7, 8783178371873),
(8, 1337),
(9, 417647164.444),
(9, 761784617461.78);

GO
INSERT INTO [PersonAccounts]([AccountNumber], [CustomerId])
VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 4),
(5, 5),
(6, 6),
(7, 7),
(8, 8),
(9, 9),
(10, 10),
(11, 6),
(12, 7),
(13, 3),
(14, 4),
(15, 8),
(16, 9),
(17, 7),
(18, 4),
(19, 4),
(20, 3),
(21, 3),
(21, 2),
(23, 1),
(24, 9),
(25, 10);




