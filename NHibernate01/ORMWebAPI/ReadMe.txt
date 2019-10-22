22 Oct 2019
-----------
Project Created using Visual Studio 2019
	- New Project -> ASP.NET Core Web Application -> ASP.NET Core 3.0 -> API
	- No Authentication, No HTTPS, No Docker Support
	- Create

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

Nuget Package Manager Console
PM > install-package NHibernate

