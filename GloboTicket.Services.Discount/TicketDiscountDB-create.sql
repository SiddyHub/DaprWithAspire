IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Coupons] (
    [CouponId] uniqueidentifier NOT NULL,
    [Code] nvarchar(max) NULL,
    [Amount] int NOT NULL,
    [AlreadyUsed] bit NOT NULL,
    CONSTRAINT [PK_Coupons] PRIMARY KEY ([CouponId])
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] ON;
INSERT INTO [Coupons] ([CouponId], [AlreadyUsed], [Amount], [Code])
VALUES ('3416eeca-e569-44fe-a06e-b0eb0d70a855', CAST(0 AS bit), 10, N'BeNice');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] ON;
INSERT INTO [Coupons] ([CouponId], [AlreadyUsed], [Amount], [Code])
VALUES ('819200b3-f05b-4416-a846-534228c26195', CAST(0 AS bit), 20, N'Awesome');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] ON;
INSERT INTO [Coupons] ([CouponId], [AlreadyUsed], [Amount], [Code])
VALUES ('aed65b30-071f-4058-b42b-6ac0955ca3b9', CAST(0 AS bit), 100, N'AlmostFree');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'CouponId', N'AlreadyUsed', N'Amount', N'Code') AND [object_id] = OBJECT_ID(N'[Coupons]'))
    SET IDENTITY_INSERT [Coupons] OFF;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200808123113_Initial', N'3.1.5');

GO

