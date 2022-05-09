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

SELECT * FROM Orders