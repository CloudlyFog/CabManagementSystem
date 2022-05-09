DROP TABLE Users
CREATE TABLE Users
(
	ID UNIQUEIDENTIFIER,
	Name VARCHAR(40),
	Email VARCHAR(100),
	Password VARCHAR(100),
	Authenticated BIT,
	Access BIT
);
INSERT INTO Users(ID, Name, Email, Password, Authenticated, Access) VALUES 
('A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Admin','maximkirichenk0.06@gmail.com','1fasdfasd4752', 1, 1),
('0273419B-FD78-4EEC-8CB9-097B549F8789', 'Maxim','hatemtbofferskin@gmail.com','root', 1, 1),
('B560A785-C146-465A-9EEB-A8E588BA023E', 'matvey','matvey@gmail.com','matvey', 1, 0),
('BABF30BF-B436-46C0-B452-39FCC16E27EC', 'msi','msi@gmail.com','msi', 1, 0)

DROP TABLE Orders
CREATE TABLE Orders
(
	ID UNIQUEIDENTIFIER,
	UserID UNIQUEIDENTIFIER,
	DriverName NVARCHAR(50),
	PhoneNumber NVARCHAR(12),
	Description NVARCHAR(1000),
	Address NVARCHAR(1000)
);

INSERT INTO Orders(ID, UserID, DriverName, PhoneNumber, Description, Address) VALUES
('4bc89c1a-b818-4bbf-8905-ffaae04fb9c3', 'A08AB3E5-E3EC-47CD-84EF-C0EB75045A70', 'Alex', '+79611750020', 'please take some dishes', 'Baker Street 12')

DROP TABLE Taxi
CREATE TABLE Taxi
(
	ID INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	TaxiNumber NVARCHAR(8),
	TaxiClass INT
)
INSERT INTO Taxi(TaxiNumber, TaxiClass) VALUES 
('À032ÊÐ36', 2)