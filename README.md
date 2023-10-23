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

## Running the Web Ranking Application

### Prerequisites:
1. Ensure you have [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) installed.
2. Ensure you have [Node.js](https://nodejs.org/) installed.
3. Ensure you have both [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) and [Visual Studio Code](https://code.visualstudio.com/) installed.

### Running the Backend (.NET project):
1. Open the solution file (`*.sln`) in Visual Studio 2022.
2. Restore any NuGet packages if prompted.
3. Build the solution by right-clicking the solution in the Solution Explorer and selecting `Build Solution`.
4. Press `F5` or click the `Run` button to start the .NET application. This will start the backend server, and you should be able to access it at the specified URL (usually `http://localhost:5148` or another port if configured differently).

### Running the Frontend (Vue.js project):
1. Open Visual Studio Code.
2. Navigate to the directory containing your Vue.js project (typically a folder named `frontend` or similar).
3. Open the integrated terminal in VS Code (`View` > `Terminal`).
4. Install any required npm packages by running:
    ```bash
    npm install
    ```
5. Start the Vue.js development server by running:
    ```bash
    npm run serve
    ```
6. Once the server starts, you should be able to access the Vue.js frontend at `http://localhost:8080`.

### Accessing the Application:
With both the backend and frontend running, you can navigate to `http://localhost:8080` in your browser to access the full application. The Vue.js frontend will communicate with the .NET backend as needed.


