# WebRankingDB Setup Guide

## Prerequisites:

- Ensure **Microsoft SQL Server (MSSQL)** is installed and running.
- Have the necessary privileges to create and manage databases, tables, and users.

## Setup:

### 1. **Create and Use the Database**:

```sql
USE master;
GO

CREATE DATABASE WebRankingDB;
GO

USE WebRankingDB;
GO
```

### 2. **Create Tables**:

#### SearchHistory Table:

```sql
CREATE TABLE SearchHistory
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Keywords NVARCHAR(255),
    Url NVARCHAR(255),
    ResultPositions NVARCHAR(255),
    SearchDate DATETIME DEFAULT GETDATE()
);
```

#### SearchEngines Table:

```sql
CREATE TABLE SearchEngines
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    BaseUrl NVARCHAR(500) NOT NULL,
    SearchUrl NVARCHAR(500) NOT NULL,
    ResultExtractionExpression NVARCHAR(500) NOT NULL
);
```

### 3. **Create User and Assign Permissions**:

```sql
USE master;
GO

CREATE LOGIN infoUser WITH PASSWORD = '12345';
GO

USE WebRankingDB;
GO

CREATE USER infoUser FOR LOGIN infoUser;
GO

GRANT SELECT, INSERT, UPDATE, DELETE ON DATABASE::WebRankingDB TO infoUser;
GO
```

### 4. **Insert Default Data**:

#### SearchEngines Table:

```sql
INSERT INTO SearchEngines (Title, BaseUrl, SearchUrl, ResultExtractionExpression)
VALUES 
('Google', 'https://www.google.com', '/search?num=100&q=#SearchText#', '//div[@class=''MjjYud'']'),
('Bing', 'https://www.bing.com', '/search?num=100&q=#SearchText#', '//li[@class=''b_algo'']'),
('DuckDuckGo', 'https://duckduckgo.com', '/?t=ffab&q=#SearchText#&ia=web?num=100', '//li[@data-layout=''organic'']/div');
```

