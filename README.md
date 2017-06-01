# netcoreauth
ASP.NET Core with JWT Authentication Demo

# Framework and SDK
1. Base on .NET Core 1.1
2. Swashbuckle for Swagger API documentation
3. Dapper for ORM
4. MailKit for email sending
5. Azure SQL Database (SQL Server standalone also fine)

# Token Policy
- Access Token: JWT token genrate by /api/tokens/access the Sign In API, can be refreshed, the access token has not been stored
- Refresh Token: JWT token genrate by /api/tokens/access the Sign In API, will be replaced by new one if call refresh API, the refresh token has not been stored
- Active Token: JWT token genrate by /api/users the Create Account API and /api/users/sendactiveemail/{token} the Send Active Account Mail API, use for active account, it has been stored in Token table till finish account activation 
- Rest Password Token: JWT token genrate by /api/users/sendresetmail/{token} the Send Reset Password Mail API, use for reset account password, it has been stored in Token table till finish password update. 

# Steps:
1. Create your database with tables user and token, you can use the classes in model project to create fields, I will update database script later.
2. Update database connection string in appsettings.json
3. If you want to use mail sending for account activation, please update the Mail class in model project, suggest to use gmail, I tested by gamil successful in my project. 
4. After all, try http://[localhost]:[port]/swagger/ui, to get API document

# TODO
I will do below update soon
1. Database script add
2. More detail usage description for this demo
3. Send mail async implementation
