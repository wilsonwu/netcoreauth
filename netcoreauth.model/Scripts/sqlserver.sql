CREATE TABLE [token] (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [user_id] int NOT NULL,
    [jwt_token] nvarchar(max) NOT NULL,
    [token_type] nvarchar(max) NOT NULL
);

CREATE TABLE [user] (
    [Id] int IDENTITY(1,1) PRIMARY KEY,
    [email] nvarchar(max) NOT NULL,
    [password] nvarchar(max) NOT NULL,
    [is_activated] bit default 0 NOT NULL,
    [is_disabled] bit default 0 NOT NULL
);