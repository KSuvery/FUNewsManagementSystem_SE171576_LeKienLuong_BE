-- ==========================================
-- CREATE DATABASE
-- ==========================================
CREATE DATABASE NewsManagementDB;
GO

USE NewsManagementDB;
GO

-- ==========================================
-- Drop existing tables (for rerun safety)
-- ==========================================
IF OBJECT_ID('dbo.NewsTag', 'U') IS NOT NULL DROP TABLE dbo.NewsTag;
IF OBJECT_ID('dbo.NewsArticle', 'U') IS NOT NULL DROP TABLE dbo.NewsArticle;
IF OBJECT_ID('dbo.Tag', 'U') IS NOT NULL DROP TABLE dbo.Tag;
IF OBJECT_ID('dbo.Category', 'U') IS NOT NULL DROP TABLE dbo.Category;
IF OBJECT_ID('dbo.SystemAccount', 'U') IS NOT NULL DROP TABLE dbo.SystemAccount;
GO

-- ==========================================
-- 1. SystemAccount Table
-- ==========================================
CREATE TABLE SystemAccount (
    AccountID INT IDENTITY(1,1) PRIMARY KEY,
    AccountName NVARCHAR(100) NOT NULL,
    AccountEmail NVARCHAR(150) NOT NULL UNIQUE,
    AccountRole INT NOT NULL,   -- 1 = Staff, 2 = Lecturer, Admin role is configured from appsettings.json
    AccountPassword NVARCHAR(200) NOT NULL
);
GO

-- ==========================================
-- 2. Category Table
-- ==========================================
CREATE TABLE Category (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryDescription NVARCHAR(255),
    ParentCategoryID INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,  -- 1 = Active, 0 = Inactive
    CONSTRAINT FK_Category_Parent FOREIGN KEY (ParentCategoryID)
        REFERENCES Category(CategoryID)
);
GO

-- ==========================================
-- 3. Tag (Tag) Table
-- ==========================================
CREATE TABLE Tag (
    TagID INT IDENTITY(1,1) PRIMARY KEY,
    TagName NVARCHAR(100) NOT NULL,
    Note NVARCHAR(255)
);
GO

-- ==========================================
-- 4. NewsArticle Table
-- ==========================================
CREATE TABLE NewsArticle (
    NewsArticleID INT IDENTITY(1,1) PRIMARY KEY,
    NewsTitle NVARCHAR(255) NOT NULL,
    Headline NVARCHAR(255),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    NewsContent NVARCHAR(MAX),
    NewsSource NVARCHAR(255),
    CategoryID INT NOT NULL,
    NewsStatus BIT NOT NULL DEFAULT 1, -- 1 = Active, 0 = Inactive
    CreatedByID INT NOT NULL,
    UpdatedByID INT NULL,
    ModifiedDate DATETIME NULL,
    CONSTRAINT FK_NewsArticle_Category FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID),
    CONSTRAINT FK_NewsArticle_CreatedBy FOREIGN KEY (CreatedByID)
        REFERENCES SystemAccount(AccountID),
    CONSTRAINT FK_NewsArticle_UpdatedBy FOREIGN KEY (UpdatedByID)
        REFERENCES SystemAccount(AccountID)
);
GO

-- ==========================================
-- 5. NewsTag (Many-to-Many Relationship)
-- ==========================================
CREATE TABLE NewsTag (
    NewsArticleID INT NOT NULL,
    TagID INT NOT NULL,
    CONSTRAINT PK_NewsTag PRIMARY KEY (NewsArticleID, TagID),
    CONSTRAINT FK_NewsTag_Article FOREIGN KEY (NewsArticleID)
        REFERENCES NewsArticle(NewsArticleID)
        ON DELETE CASCADE,
    CONSTRAINT FK_NewsTag_Tag FOREIGN KEY (TagID)
        REFERENCES Tag(TagID)
        ON DELETE CASCADE
);
GO

-- ==========================================
-- Sample Data (Optional)
-- ==========================================
INSERT INTO SystemAccount (AccountName, AccountEmail, AccountRole, AccountPassword)
VALUES ('Admin', 'admin@news.com', 3, 'hashedpassword'),
       ('John Staff', 'john@news.com', 1, 'hashedpassword'),
       ('Mary Lecturer', 'mary@news.com', 2, 'hashedpassword');

INSERT INTO Category (CategoryName, CategoryDescription, IsActive)
VALUES ('Technology', 'All about tech', 1),
       ('Education', 'Learning and teaching', 1),
       ('Sports', 'News about sports events', 1);

INSERT INTO Tag (TagName, Note)
VALUES ('AI', 'Artificial Intelligence'),
       ('Cloud', 'Cloud computing related'),
       ('Education', 'Education-related topics');

INSERT INTO NewsArticle (NewsTitle, Headline, NewsContent, NewsSource, CategoryID, CreatedByID)
VALUES ('AI Revolution', 'The rise of AI', 'Full article content...', 'TechSource', 1, 1),
       ('Teaching Online', 'Digital classrooms', 'Online learning trends...', 'EduSource', 2, 3);

INSERT INTO NewsTag (NewsArticleID, TagID)
VALUES (1, 1),
       (1, 2),
       (2, 3);
GO
