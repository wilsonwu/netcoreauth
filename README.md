# netcoreauth
ASP.NET Core with JWT Authentication Demo

# Framework and SDK
- Compatibile for `Visual Studio 2017` and `Visual Studio for Mac`
- Base on `.NET Core 1.1`
- `Swashbuckle.AspNetCore` for Swagger API documentation
- `Dapper` for ORM
- `MailKit` for email sending
- `Azure SQL Database` (SQL Server standalone also fine)

# Token Policy
- **Access Token**: JWT token genrate by `POST: /api/tokens/access` the Sign In API, can be refreshed, the access token has not been stored
- **Refresh Token**: JWT token genrate by `POST: /api/tokens/access` the Sign In API, will be replaced by new one if call refresh API, the refresh token has not been stored
- **Active Token**: JWT token genrate by `POST: /api/users` the Create Account API and `GET: /api/users/sendactiveemail/{email}` the Send Account Activation Mail API, use for active account, it has been stored in Token table till finish account activation 
- **Rest Password Token**: JWT token genrate by `GET: /api/users/sendresetmail/{email}` the Send Reset Password Mail API, use for reset account password, it has been stored in Token table till finish password update. 

# Steps to Run:
1. Create your database manually and run the user and token tables create script under `netcoreauth.model` project `Scripts` folder.
2. Update database connection string in `appsettings.json`
3. If you want to use mail sending for account activation, please update the `Mail.cs` class file in `netcoreauth.model` project, suggest to use Gmail, I tested by Gmail successful in my project. 
4. After all, try `http://[localhost]:[port]/swagger`, to get API document
5. `GET: /api/tests/1` this API without Auth, `GET: /api/tests` this API with Auth
6. Call `POST: /api/users` to create account
7. Get the token in you database token table, then call `PUT: /api/users/active/{token}` to active account
8. Call `POST: /api/tokens/access` to use your email and password login and get tokens (access token and refresh token)
    ```
    POST /api/tokens/access
    {
      "email": "xxxx@xxxx.com",
      "password": "xxxxxxxxxxxxxxxxxxxxxxx"
    }
    ```
9. Call `GET: /api/tests` with header: `Authorization: Bearer {token}`, both access token and refresh token work fine
10. Get response: `["value1", "value2"]` without `401` HTTP code from `GET: /api/tests`, that means you get success.

# TODO
1. ~~Database script add~~
2. ~~More detail usage description for this demo~~
3. Send mail async implementation
4. Add Postman script samples for API calls
5. Upgrade to .NET Core 2.0
6. Add API version support
