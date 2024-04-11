# Library API

## Description
API for the library, responsible for manage books.
Architecture explain:</br>
I Have 4 layers based on DDD:
</br>
WebAPI -> Layer with Api swagger, controller, configurations.</br>
Applicaton - > Bussiness Logic Layer.</br>
Core -> Models, Dto, Entities and Interfaces of Business, Services, WebServices and Repositorys</br>
Infraestructure -> Persistence, Services and Connected Services.</br>

## 🚀 Installation/Implemetation
To run project in local you should have instaled the SDK for .net core 8 and a database in SQL SERVER with this table not related to any other:
```sql
USE [GenericEnterprise]
GO

/****** Object:  Table [dbo].[books]    Script Date: 11/04/2024 11:27:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[books](
    [book_id] [int] IDENTITY(1,1) NOT NULL,
      NOT NULL,
      NOT NULL,
      NOT NULL,
    [total_copies] [int] NOT NULL,
    [copies_in_use] [int] NOT NULL,
      NULL,
      NULL,
      NULL,
PRIMARY KEY CLUSTERED 
(
    [book_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[books] ADD  DEFAULT ((0)) FOR [total_copies]
GO

ALTER TABLE [dbo].[books] ADD  DEFAULT ((0)) FOR [copies_in_use]
GO
```
To publish:</br>
The application is ready to be deployed on Kubernetes. Follow these steps:</br>
Dockerize the application using a Dockerfile.</br>
Publish the Docker image on Kubernetes.</br>
Apply the Kubernetes manifests.</br>
In minutes, the application will be ready and running on Kubernetes.</br>

## 📋 Usage
The Swagger is ready to use, if you run the project as Debug mode it will be not necessary to authenticate to call other methods, but if you dont, you have to authenticate with a fixed login and password, adjust the login and passwrdo if you need in appsettings.json (the way the system is developted the application is ready to put a login with database or auth with a fixed token, or even AD).

## 🤝 TODO list
Unit Test Layer with NUnit.

## 🖥️ Used Technologies
NET CORE 8;</br>
SQL SERVER EXPRESS;</br>
Data Annotations;</br>
AutoMapper;</br>
Entity Framework;</br>
Swagger;</br>
JWT;</br>

## 📝 License
Only for study and non-distributable.
