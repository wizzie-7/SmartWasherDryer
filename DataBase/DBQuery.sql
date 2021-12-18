create database SMART_WASHER_DRYER;

Create table UserInfo
(
	UserID int IDENTITY(1,1) Primary Key ,
	Fname VARCHAR(15) NOT NULL,
	Lname VARCHAR(15) NOT NULL,
	Email Varchar(50) NOT NULL
)

drop table UserInfo;

drop table Payment;

select * from UserInfo;

Create Table Payment
(
	Payment_ID int IDENTITY(1,1) Primary Key,
	UserID INT NOT NULL,
	
	Payment_Amt INT NOT NULL,
	Payment_Sts Varchar(20),
	Payment_DT Varchar(20),
	FOREIGN KEY(UserID) REFERENCES UserInfo(UserID),
)


select * from Payment


SELECT UserInfo.Fname, COUNT(UserInfo.UserID) AS NumberOfOrders FROM Orders
Inner JOIN Payment ON UserInfo.UserID = Payment.UserID
GROUP BY ShipperName;
