22 Oct 2019
-----------
Project Created using Visual Studio 2019
	- New Project -> ASP.NET Core Web Application -> ASP.NET Core 3.0 -> API
	- No Authentication, No HTTPS, No Docker Support
	- Create

DB
	- Using (localdb)\MSSQLLocalDB

Objective
	- Try nHibernate
	- Mimic 1P to understand how 1P uses nHibernate

References:

# In 1P, 
	we use Database first approach. 
	We have .hbm.xml files. 
	C# Classes do not have attributes.
	hbml.xml and C# classes are generated from some tool (CodeSmith)
	[?] There is some huge file - OnePlannerMapping.json
	[?] There is a class called OnePlannerMapping 
		- OnepPlannerMapping creates a dictionary between class and table, collection
		and properties from SessionFactory and provides support to map all OnePlanner
		classes and collections to tables
	[?] There is some concept of tracking and batching configs
# Another approach could be to not use xml files and use this instead:
	NHibernate.Mapping.Attributes: With NHibernate.Mapping.Attributes, you can use .NET attributes to decorate your entities and these attributes will be used to generate the mapping information
	https://github.com/nhibernate/NHibernate.Mapping.Attributes
	 - What is NHibernate.Mapping.Attributes?
		NHibernate requires mapping information to bind your domain model to your database. 
		Usually, it is written (and maintained) in separated hbm.xml files.
		With NHibernate.Mapping.Attributes, you can use .NET attributes to decorate your entities 
		and these attributes will be used to generate the mapping information. 
		So you will no longer have to bother with these nasty XML files ;).
		Licensed under the terms of the GNU Lesser General Public License.
# Another approach: Fluent NHibernate: Fluent, XML-less, compile safe, automated, convention-based mappings for NHibernate
# In addition to the above two addons, NHibernate have built-in support for "Mapping-By-Code". No need for XML
# (*) https://www.tutorialspoint.com/nhibernate/index.htm
	Uses .hbm.xml files, but in the examples, the xml files are hand-created.
	- Start with this. Resembles closest to 1P
# https://archive.codeplex.com/?p=nmg
	This is a nHibernate mapping generator (We are moving to GitHub. Please follow us at https://github.com/rvrn22/nmg)
	A simple utility to generate NHibernate mapping files and corresponding domain classes from existing DB tables.
	Features:
	1. Supports Oracle, SqlServer, PostgreSQL, MySQL, SQLite, Sybase, Ingres, CUBRID
	2. Can generate hbm.xml, Fluent NHibernate and NH 3.3 Fluent style of mapping files.
	3. Has lots of preferences to control the property naming conventions.
	4. Can generate Domain Entity and WCF Data Contracts too.
	5. Can generate one table at a time or script entire DB in one go. (It can generate mapping for around 800 tables in under 3 minutes on my moderately powered laptop)
	6. Supports ActiveRecord code generation.
	7. Its super fast and free. No licensing restrictions.
	8. Option to generate NHibernate or MS validators

=============
Start here:
https://www.tutorialspoint.com/nhibernate/index.htm

---------
Things that we will have to deal with
	- Object-Object mappings - Unidirectional/bi-directional
	- Inheritance
---------
Add Project -> Class Library (.NET Core)
- Create new Class Student

Visual Studio -> View -> SQL Server Object Explorer
	-> SQL Server -> (localdb)\MSSQLLocalDB -> Databases
	-> Right Click -> Add new database -> NHibernate01 (Path = C:\GIT\NHibernate01\NHibernate01\ORM_NHibernate\)
	->
	-> Tables -> Add new table -> Student (Use designer window to create table fields) .. with SQL like this:
	CREATE TABLE [dbo].[Student] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Firstname] NVARCHAR (MAX) NULL,
    [Lastname]  NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
	);

Nuget Package Manager Console
PM > install-package NHibernate

Add new class to represent DBSession (NHibernate session)

Note on MDF file:
	Even though I have specified the mdf/ldf files to be created in this project folder, they are gitignored

// Note: Troubleshooting
// At some point here, I was getting error which got resolved due to this:
// https://stackoverflow.com/questions/35444487/how-to-use-sqlclient-in-asp-net-core
//            Instead of referencing System.Data and System.Data.SqlClient you need to grab from Nuget:
//            System.Data.Common and System.Data.SqlClient

// Troubleshooting:
// Make sure that all the fields in C# class and hbm.xml file match case-by-case

// Troubleshooting:
// In your DB, the Id column should be defined like so
// [Id]        INT            IDENTITY (1, 1) NOT NULL
// In the hbm file, it should be
// <generator class = "identity"/>

PS: Please note the "hbm" part of the file name. 
This is a convention used by NHibernate to automatically recognize the file as a mapping file.


Able to Save record to Student
-----------

[#] How to show the SQL queries that nHibernate executes:
[Ans] Add this to the hibernate.cfg.xml file
    <property name="show_sql">true</property>
    <property name="format_sql">true</property>

This outputs the SQL query to the console, eg
NHibernate:
    SELECT
        this_.Id as id1_0_0_,
        this_.Lastname as lastname2_0_0_,
        this_.Firstname as firstname3_0_0_
    FROM
        Student this_

Another answer here: https://stackoverflow.com/questions/129133/how-do-i-view-the-sql-that-is-generated-by-nhibernate
NHibernate.EmptyInterceptor
------------
30 Oct 2019
Try with MySQL
 - Add nuget MySql.Data
 - Create a new Database named 'nhibernate01' in MySql workbench, and create a table named 'student'
 - By default, the database/table/column names need to be in small case
 - Some modifications required in cfg and hbm file
 - id column was giving some problem. I'll try setting its class to 'hilo' to mimin 1P, and that requires me to create a new table in MySql
 - CREATE TABLE `hibernate_unique_key` (
  `next_hi` int(11) DEFAULT NULL
 ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
 - Which I copied as query from 1P's table
 .. let's see
 - Troubleshooting:
 [#]: NHibernate.Id.IdentifierGenerationException: 'could not read a hi value - you need to populate the table: hibernate_unique_key'
	Ans: From stackoverflow
	NHibernate expects to find a value that stores the current hi value in that table, ie it first runs something like:

	current_hi = [SELECT max(next_hi) FROM hibernate_unique_key].
	So all you need to do is seed that table with an initial number, ie:

	INSERT INTO hibernate_unique_key(next_hi) VALUES (0)
 - And cool .. we are able to insert/get using Postman
-------------
31 Oct 2019
Troubleshooting
(#) Column names which are SQL kywords
	- In 1P, there are certain tables with column names that are SQL keywords, and the insert query that nHibernate issues fails.
	Now, we could go about and rename all those columns, but you could add this setting to hibernate.cfg.xml
	<property name="hbm2ddl.keywords">auto-quote</property>
	What it would do is, instead of writing rank as column name, nH would write `rank` in the SQL query, and that is acceptable to SQL.
	One caveat though, it probably works only if we have the mappings defined as .hbm.xml files (doesn't work with Fluent NH)
	Anyways, we are using .hbm.xml files only, so this works for us