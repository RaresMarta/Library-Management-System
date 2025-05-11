using System;
using Microsoft.Data.Sqlite;

namespace LibrarySystem.Data;

/// Initialize SQLite database
public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        try
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Books (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Author TEXT NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Genre TEXT NOT NULL                                                 
                );

                CREATE TABLE IF NOT EXISTS Members (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FullName TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Phone TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Loans (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BookId INTEGER NOT NULL,
                    MemberId INTEGER NOT NULL,
                    BorrowedDate TEXT    NOT NULL,
                    DueDate TEXT NOT NULL,
                    ReturnedDate TEXT,
                    Status INTEGER NOT NULL DEFAULT 1,
                    RenewalCount INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY(BookId) REFERENCES Books(Id),
                    FOREIGN KEY(MemberId) REFERENCES Members(Id)
                );
            ";
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to initialize database: {ex.Message}", ex);
        } 
        finally
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Close();
        }
    }
}