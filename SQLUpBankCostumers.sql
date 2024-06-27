CREATE DATABASE DBCustumersAPI;

USE DBCustumersAPI;

CREATE TABLE Customer (
    Cpf VARCHAR(11) PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    Gender CHAR(1) NOT NULL,
    Salary DECIMAL(14, 2) NOT NULL,  
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    [Address] VARCHAR(9) NOT NULL,
    Restriction bit NOT NULL
);

CREATE TABLE DeletedCustomer (
    Cpf VARCHAR(11) PRIMARY KEY,
    [Name] VARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    Gender CHAR(1) NOT NULL,
    Salary DECIMAL(14, 2) NOT NULL,  
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(200) NOT NULL,
    [Address] VARCHAR(9) NOT NULL,
    Restriction bit NOT NULL
);

