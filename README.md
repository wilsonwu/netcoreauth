# netcoreauth
ASP.NET Core with JWT Authentication Demo

# Framework and SDK
Base on .NET Core 1.1
Swashbuckle for Swagger
Dapper for ORM
MailKit for email sending
Azure SQL Database for my project

# Steps:
1. Create your database with tables user and token, you can use the classes in model project to create fields, I will update database script later.
2. Update database connection string in appsettings.json
3. If you want to use mail sending for account activation, please update the Mail class in model project, suggest to use gmail, I tested by gamil successful in my project. 
4. After all, try http://[localhost]:[port]/swagger/ui, to get API document

# TODO
I will do below update soon
1. Database script add
2. More detail usage description for this demo
