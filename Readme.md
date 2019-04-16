# Demo 比較幾種 Entity Framework 大量新增資料時的效能

### Installation Instructions
1. 請先安裝 SQL Server Express OR LocalDB
2. 請執行下述語法建立 Database 及 Table
```sql
CREATE DATABASE [TSQL2020]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TSQL2019', FILENAME = N'C:\Users\reco\TSQL2020.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TSQL2019_log', FILENAME = N'C:\Users\reco\TSQL2020_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

CREATE TABLE [dbo].[Users](
	[UID] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NULL
) ON [PRIMARY]
GO
```


### 校能評比結果(以3000筆資料新增為例)
| 驗證模式     | 資料筆數的效能 |
|----------|---------------|
| 單筆Add，單筆SaveChange | 37562      |
| 單筆Add，一次SaveChange | 14633      |
| 批次Add，批次SaveChange | 35784      |
| 批次Add，批次SaveChange，Dispose dbContext | 13416      |


