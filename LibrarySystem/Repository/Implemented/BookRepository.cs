using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using log4net;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;

namespace LibrarySystem.Repository.Implemented
{
    public class BookRepository : IBookRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(BookRepository));
        private readonly string _connStr = IRepository<Book>.LoadDatabaseUrl();
        
        public void Add(Book book)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Books (Title, Author, Quantity, Genre) VALUES ($title, $author, $qty, $genre);";
                cmd.Parameters.AddWithValue("$title", book.Title);
                cmd.Parameters.AddWithValue("$author", book.Author);
                cmd.Parameters.AddWithValue("$qty", book.Quantity);
                cmd.Parameters.AddWithValue("$genre", book.Genre);
                cmd.ExecuteNonQuery();
                Logger.Info($"Executed Add(Book): {book.ToString()}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Add(Book) failed. Book={book.ToString()}", ex);
                throw;
            }
        }

        public Book GetById(int id)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Books WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    Logger.Info($"Executed GetById({id}): no record found.");
                    return null;
                }
                var book = new Book(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetString(4)
                );
                Logger.Info($"Executed GetById({id}): retrieved Book={book.ToString()}");
                return book;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetById({id}) failed.", ex);
                throw;
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var list = new List<Book>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Books;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var book = new Book(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3),
                        reader.GetString(4)
                    );
                    list.Add(book);
                }
                Logger.Info($"Executed GetAll(): retrieved {list.Count} book(s).");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error("GetAll() failed.", ex);
                throw;
            }
        }

        public void Update(Book book)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Books SET Title=$title, Author=$author, Quantity=$qty, Genre=$genre WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$title", book.Title);
                cmd.Parameters.AddWithValue("$author", book.Author);
                cmd.Parameters.AddWithValue("$qty", book.Quantity);
                cmd.Parameters.AddWithValue("$genre", book.Genre);
                cmd.Parameters.AddWithValue("$id", book.Id);
                int rows = cmd.ExecuteNonQuery();
                Logger.Info($"Executed Update: Book={book.ToString()}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Update(Book) failed. Book={book.ToString()}", ex);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Books WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                int rows = cmd.ExecuteNonQuery();
                Logger.Info($"Executed Delete(Book): deleted {rows} row(s). Id={id}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Delete(Book) failed for Id={id}.", ex);
                throw;
            }
        }

        public IEnumerable<Book> GetByTitle(string title)
        {
            var list = new List<Book>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Books WHERE Title LIKE $pattern;";
                cmd.Parameters.AddWithValue("$pattern", $"%{title}%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var book = new Book(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3),
                        reader.GetString(4)
                    );
                    list.Add(book);
                }
                Logger.Info($"Executed FindByTitle('{title}'): retrieved {list.Count} book(s).");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"FindByTitle('{title}') failed.", ex);
                throw;
            }
        }

        public IEnumerable<Book> GetByAuthor(string author)
        {
            var list = new List<Book>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Books WHERE Author LIKE $pattern;";
                cmd.Parameters.AddWithValue("$pattern", $"%{author}%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var book = new Book(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3),
                        reader.GetString(4)
                    );
                    list.Add(book);
                }
                Logger.Info($"Executed FindByAuthor('{author}'): retrieved {list.Count} book(s).");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"FindByAuthor('{author}') failed.", ex);
                throw;
            }
        }

        public IEnumerable<Book> GetByGenre(string genre)
        {
            var list = new List<Book>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Books WHERE Genre LIKE $pattern;";
                cmd.Parameters.AddWithValue("$pattern", $"%{genre}%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var book = new Book(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetInt32(3),
                        reader.GetString(4)
                    );
                    list.Add(book);
                }
                Logger.Info($"Executed GetByGenre('{genre}'): retrieved {list.Count} book(s).");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetByGenre('{genre}') failed.", ex);
                throw;
            }
        }
    }
}