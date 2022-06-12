DROP TABLE Users
CREATE TABLE Users
(
	ID UNIQUEIDENTIFIER,
	Name VARCHAR(40),
	Email VARCHAR(100),
	Password VARCHAR(100),
	Authenticated BIT,
	Access BIT,
	HasOrder BIT,
	BankAccountID UNIQUEIDENTIFIER,
	BankAccountAmount DECIMAL
);
INSERT INTO Users(ID, Name, Email, Password, Authenticated, Access, HasOrder, BankAccountID, BankAccountAmount) VALUES 
('A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Admin','maximkirichenk0.06@gmail.com','1fasdfasd4752', 1, 1, 1, '216fbfbb-07a7-434e-9eff-fbeb1bd4e087', 1000),
('0273419B-FD78-4EEC-8CB9-097B549F8789', 'Maxim','hatemtbofferskin@gmail.com','root', 1, 1, 0, 'e1970973-91dc-41f8-8d25-bad9e12c123d', 1000),
('B560A785-C146-465A-9EEB-A8E588BA023E', 'matvey','matvey@gmail.com','matvey', 1, 0, 0, '9e1b00eb-202c-4b18-96d2-6aed0d7e982c', 1000),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', 'msi','msi@gmail.com','msi', 1, 0, 0, 'cc01181e-0f5e-4f99-adaa-7335b475bf2e', 10000),
('7aec92dc-fb2a-45d2-a0db-3e6f66aa9ebb', 'Lay','sl@gmail.com','sl', 1, 0, 0, '5d2636f4-1ad1-41e6-baac-cb71acfbb947', 10000)

DROP TABLE Banks
CREATE TABLE Banks
(
	ID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	BankName VARCHAR(40),
	MembersAmount INT,
	AccountAmount DECIMAL
)
INSERT INTO Banks(ID, BankID, BankName, MembersAmount, AccountAmount) VALUES
('ae59b1df-089d-4823-a32a-41a44f878b4b', 'bed62930-9356-477a-bed5-b84d59336122', 'Tinkoff', 4321, 10523400000),
('c2c4fc26-e503-4d48-8a24-ad9233e0e603', 'e4c18139-f2c8-4a4b-a8b8-cf0d230b37fa', 'SberBank', 532, 1043200000),
('335ba509-2994-4068-9a50-f703490891ba', 'b56c8051-6eee-4441-a7de-7cb4789de362', 'PochtaBank', 1235, 100650000)

DROP TABLE Operations
CREATE TABLE Operations
(
	ID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	ReceiverID UNIQUEIDENTIFIER,
	SenderID UNIQUEIDENTIFIER,
	TransferAmount DECIMAL,
	OperationStatus INT
)
INSERT INTO Operations(ID, BankID, ReceiverID, SenderID, TransferAmount, OperationStatus) VALUES
('ae734776-9cb6-464e-9adf-638a04db8e0f', 'bed62930-9356-477a-bed5-b84d59336122', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', '7aec92dc-fb2a-45d2-a0db-3e6f66aa9ebb', 120, 200),
('84840c88-d183-491c-9e6e-19b16e26fbc9', 'bed62930-9356-477a-bed5-b84d59336122', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', '7aec92dc-fb2a-45d2-a0db-3e6f66aa9ebb', 1234,  300),
('5091ef5f-973a-4e46-9428-7b8f69dcc23d', 'e4c18139-f2c8-4a4b-a8b8-cf0d230b37fa', '7aec92dc-fb2a-45d2-a0db-3e6f66aa9ebb', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 5234, 400)

DROP TABLE Orders
CREATE TABLE Orders
(
	ID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	DriverName NVARCHAR(50),
	PhoneNumber NVARCHAR(12),
	Price INT,
	Description NVARCHAR(1000),
	Address NVARCHAR(1000),
	OrderTime DATETIME
);

INSERT INTO Orders(ID, UserID, DriverName, PhoneNumber, Price, Description, Address, OrderTime) VALUES
('4bc89c1a-b818-4bbf-8905-ffaae04fb9c3', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Alex', '+79611750020', 260,  'please take some dishes', 'Baker Street 12', CURRENT_TIMESTAMP)

DROP TABLE Taxi
CREATE TABLE Taxi
(
	ID UNIQUEIDENTIFIER,
	DriverID UNIQUEIDENTIFIER,
	TaxiNumber NVARCHAR(8),
	TaxiClass INT,
	Price INT,
	SpecialName NVARCHAR(50),
)
INSERT INTO Taxi(ID, DriverID, TaxiNumber, Price, TaxiClass, SpecialName) VALUES 
('f6c6ed2d-86b4-41b4-af23-4f1f0915b665', 'bd1e063f-b343-4387-812c-e203bfaa1f65',  'À032ÊÐ36', 235, 2, 'TaxiComfortAlex')

DROP TABLE Drivers
CREATE TABLE Drivers
(
	DriverID UNIQUEIDENTIFIER,
	Name NVARCHAR(50),
	PhoneNumber NVARCHAR(12),
	Busy BIT
)
INSERT INTO Drivers(DriverID, Name, PhoneNumber, Busy) VALUES
('bd1e063f-b343-4387-812c-e203bfaa1f65', 'Alex', '+79611750020', 1),
('96583972-f4f2-4c73-99b6-0708fd53aa97', 'Nick', '+79046438918', 0)

DROP TABLE BindTaxiDriver
CREATE TABLE BindTaxiDriver
(
	ID UNIQUEIDENTIFIER,
	TaxiID UNIQUEIDENTIFIER,
	DriverID UNIQUEIDENTIFIER
)
INSERT INTO BindTaxiDriver(ID, TaxiID, DriverID) VALUES
('d78d5101-7876-477a-84ea-50e61519943d', 'f6c6ed2d-86b4-41b4-af23-4f1f0915b665', 'bd1e063f-b343-4387-812c-e203bfaa1f65')

DROP TABLE AdminHandling
CREATE TABLE AdminHandling 
(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	UserID UNIQUEIDENTIFIER,
	SelectMode INT,
	Time DATETIME
)
INSERT INTO AdminHandling(SelectMode, UserID, Time) VALUES
(1, 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', CURRENT_TIMESTAMP)