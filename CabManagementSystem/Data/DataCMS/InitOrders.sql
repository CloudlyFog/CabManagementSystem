DROP TABLE Users
CREATE TABLE Users
(
	ID UNIQUEIDENTIFIER,
	Name VARCHAR(40) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Password VARCHAR(100) NOT NULL,
	Authenticated BIT,
	Access BIT,
	HasOrder BIT,
	BankAccountID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	BankAccountAmount DECIMAL
);
INSERT INTO Users(ID, Name, Email, Password, Authenticated, Access, HasOrder, BankAccountID, BankID, BankAccountAmount) VALUES 
('A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Admin','maximkirichenk0.06@gmail.com','erwkAsDWjzRNIZZrUawEwyd5z4r5ZGCbkTorVKuuhIw=', 1, 1, 1, '216fbfbb-07a7-434e-9eff-fbeb1bd4e087', 'bed62930-9356-477a-bed5-b84d59336122', 1000)

DROP TABLE BankAccounts
CREATE TABLE BankAccounts
(
	ID UNIQUEIDENTIFIER,
	UserBankAccountID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	BankAccountAmount DECIMAL
)
INSERT INTO BankAccounts(ID, UserBankAccountID, BankID, BankAccountAmount) VALUES
('216fbfbb-07a7-434e-9eff-fbeb1bd4e087', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'bed62930-9356-477a-bed5-b84d59336122', 1000)

DROP TABLE Banks
CREATE TABLE Banks
(
	ID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	BankName VARCHAR(40),
	AccountAmount DECIMAL
)
INSERT INTO Banks(ID, BankID, BankName, AccountAmount) VALUES
('ae59b1df-089d-4823-a32a-41a44f878b4b', 'bed62930-9356-477a-bed5-b84d59336122', 'Tinkoff', 234523450),
('c2c4fc26-e503-4d48-8a24-ad9233e0e603', 'e4c18139-f2c8-4a4b-a8b8-cf0d230b37fa', 'SberBank', 1043200000),
('335ba509-2994-4068-9a50-f703490891ba', 'b56c8051-6eee-4441-a7de-7cb4789de362', 'PochtaBank', 100650000)



DROP TABLE Operations
CREATE TABLE Operations
(
	ID UNIQUEIDENTIFIER,
	BankID UNIQUEIDENTIFIER,
	ReceiverID UNIQUEIDENTIFIER,
	SenderID UNIQUEIDENTIFIER,
	TransferAmount DECIMAL,
	OperationStatus INT,
	OperationKind INT
)
INSERT INTO Operations(ID, BankID, ReceiverID, SenderID, TransferAmount, OperationStatus, OperationKind) VALUES
('ae734776-9cb6-464e-9adf-638a04db8e0f', 'bed62930-9356-477a-bed5-b84d59336122', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'bed62930-9356-477a-bed5-b84d59336122', 120, 200, 1)
--('84840c88-d183-491c-9e6e-19b16e26fbc9', 'bed62930-9356-477a-bed5-b84d59336122', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'bed62930-9356-477a-bed5-b84d59336122', 1234,  300, 1),
--('5091ef5f-973a-4e46-9428-7b8f69dcc23d', 'e4c18139-f2c8-4a4b-a8b8-cf0d230b37fa', '7aec92dc-fb2a-45d2-a0db-3e6f66aa9ebb', 'b56c8051-6eee-4441-a7de-7cb4789de362', 5234, 400, 2)

DROP TABLE Orders
CREATE TABLE Orders
(
	ID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	TaxiID UNIQUEIDENTIFIER,
	DriverName NVARCHAR(50),
	PhoneNumber NVARCHAR(12),
	Price INT,
	Description NVARCHAR(1000),
	Address NVARCHAR(1000)
);

INSERT INTO Orders(ID, UserID, TaxiID, DriverName, PhoneNumber, Price, Description, Address) VALUES
('4bc89c1a-b818-4bbf-8905-ffaae04fb9c3', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'f6c6ed2d-86b4-41b4-af23-4f1f0915b665', 'Alexey', '+79611750020', 260,  'please take some dishes', 'Baker Street 12')

DROP TABLE Taxi
CREATE TABLE Taxi
(
	ID UNIQUEIDENTIFIER,
	DriverID UNIQUEIDENTIFIER,
	TaxiNumber NVARCHAR(8),
	TaxiClass INT,
	Busy BIT
)
INSERT INTO Taxi(ID, DriverID, TaxiNumber, Busy, TaxiClass) VALUES 
('f6c6ed2d-86b4-41b4-af23-4f1f0915b665', 'bd1e063f-b343-4387-812c-e203bfaa1f65',  'А032КР36', 1, 1),
('c2e2d9d0-7d57-42ed-9418-4ae1d4e5e60e', '96583972-f4f2-4c73-99b6-0708fd53aa97',  'М019СА36', 1, 2),
('36aa4c66-fafb-44f4-911c-d8c8168b2fee', 'b7083c2f-2ed4-40d4-93ec-6092c87e28d2',  'А880АН36', 1, 3),
('6b3ab5ec-ea13-4fa2-b5c8-57e5307a5de7', '34dcdd7f-7e0a-43a0-9322-ceb6b19ed8c6',  'Р005РС36', 0, 4),
('bbabe9b8-34bc-48e5-bf53-557b88dac13d', '994b4561-3595-4210-9ff6-49cfb9c2b973',  'П432АС36', 1, 1),
('a841638e-2df9-4eb5-a7af-66ab63cd6b39', 'ea28d536-3d80-4377-84dc-eb46df64745c',  'Р929РК36', 0, 2),
('0a4ea3da-d46e-4bd6-9dce-b1b10f365076', 'b4f13237-b95a-495b-816d-07b30832ca4d',  'А161ВН36', 1, 3),
('99b6801d-a0ab-41e9-9a2b-42ecbdb4dddc', 'a46080a4-64e1-475e-bfc5-040e6cff3613',  'Т895ТТ36', 0, 4)

DROP TABLE Drivers
CREATE TABLE Drivers
(
	DriverID UNIQUEIDENTIFIER,
	TaxiID UNIQUEIDENTIFIER,
	Name NVARCHAR(50),
	PhoneNumber NVARCHAR(12),
	Busy BIT,
	TaxiPrice INT
)
INSERT INTO Drivers(DriverID, TaxiID, Name, PhoneNumber, Busy, TaxiPrice) VALUES
('bd1e063f-b343-4387-812c-e203bfaa1f65', 'f6c6ed2d-86b4-41b4-af23-4f1f0915b665', 'Alexey', '+79611750020', 1, 260),
('96583972-f4f2-4c73-99b6-0708fd53aa97', 'c2e2d9d0-7d57-42ed-9418-4ae1d4e5e60e', 'Evgeny', '+79846438918', 1, 235),
('b7083c2f-2ed4-40d4-93ec-6092c87e28d2', '36aa4c66-fafb-44f4-911c-d8c8168b2fee', 'Alexander', '+79311753020', 1, 180),
('34dcdd7f-7e0a-43a0-9322-ceb6b19ed8c6', '6b3ab5ec-ea13-4fa2-b5c8-57e5307a5de7', 'Dmitriy', '+79656175080', 0, 150),
('994b4561-3595-4210-9ff6-49cfb9c2b973', 'bbabe9b8-34bc-48e5-bf53-557b88dac13d', 'Evgeny', '+79613855772', 0, 260),
('ea28d536-3d80-4377-84dc-eb46df64745c', 'a841638e-2df9-4eb5-a7af-66ab63cd6b39', 'Maxim', '+79411750020', 0, 235),
('b4f13237-b95a-495b-816d-07b30832ca4d', '0a4ea3da-d46e-4bd6-9dce-b1b10f365076', 'Denis', '+79233750020', 1, 180),
('a46080a4-64e1-475e-bfc5-040e6cff3613', '99b6801d-a0ab-41e9-9a2b-42ecbdb4dddc', 'Alexandr', '+79186750020', 0, 150)

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