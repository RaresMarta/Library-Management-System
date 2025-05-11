using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using log4net;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;

namespace LibrarySystem.Repository.Implemented
{
    public class LoanRepository : ILoanRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LoanRepository));
        private readonly string _connStr = IRepository<Loan>.LoadDatabaseUrl();

        public void Add(Loan loan)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Loans (BookId,MemberId,BorrowedDate,DueDate,ReturnedDate,Status,RenewalCount) "+
                                  "VALUES ($bookId,$memberId,$bDate,$dDate,NULL,$status,$renewals);";
                cmd.Parameters.AddWithValue("$bookId", loan.BookId);
                cmd.Parameters.AddWithValue("$memberId", loan.MemberId);
                cmd.Parameters.AddWithValue("$bDate", loan.BorrowedDate.ToString("o"));
                cmd.Parameters.AddWithValue("$dDate", loan.DueDate.ToString("o"));
                cmd.Parameters.AddWithValue("$status", (int)loan.Status);
                cmd.Parameters.AddWithValue("$renewals", loan.RenewalCount);
                cmd.ExecuteNonQuery();
                Logger.Info($"Executed Add(Loan): {loan.ToString()}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Add(Loan) failed. Loan={loan.ToString()}", ex);
                throw;
            }
        }

        public Loan GetById(int id)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Loans WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    Logger.Info($"Executed GetById({id}): no record found.");
                    return null;
                }
                var loan = new Loan
                {
                    Id          = reader.GetInt32(0),
                    BookId      = reader.GetInt32(1),
                    MemberId    = reader.GetInt32(2),
                    BorrowedDate= DateTime.Parse(reader.GetString(3)),
                    DueDate     = DateTime.Parse(reader.GetString(4)),
                    ReturnedDate= reader.IsDBNull(5) ? (DateTime?)null : DateTime.Parse(reader.GetString(5)),
                    Status      = (LoanStatus)reader.GetInt32(6),
                    RenewalCount= reader.GetInt32(7)
                };
                Logger.Info($"Executed GetById({id}): retrieved Loan={loan.ToString()}");
                return loan;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetById({id}) failed", ex);
                throw;
            }
        }

        public IEnumerable<Loan> GetAll()
        {
            var list = new List<Loan>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Loans;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var loan = new Loan
                    {
                        Id          = reader.GetInt32(0),
                        BookId      = reader.GetInt32(1),
                        MemberId    = reader.GetInt32(2),
                        BorrowedDate= DateTime.Parse(reader.GetString(3)),
                        DueDate     = DateTime.Parse(reader.GetString(4)),
                        ReturnedDate= reader.IsDBNull(5) ? null : DateTime.Parse(reader.GetString(5)),
                        Status      = (LoanStatus)reader.GetInt32(6),
                        RenewalCount= reader.GetInt32(7)
                    };
                    list.Add(loan);
                }
                Logger.Info($"Executed GetAll(): retrieved {list.Count} loans");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error("GetAll() failed", ex);
                throw;
            }
        }

        public void Update(Loan loan)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Loans " +
                                  "SET BookId=$bookId,MemberId=$memberId,BorrowedDate=$bDate,DueDate=$dDate,ReturnedDate=$rDate,Status=$status,RenewalCount=$renewals " +
                                  "WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$bookId", loan.BookId);
                cmd.Parameters.AddWithValue("$memberId", loan.MemberId);
                cmd.Parameters.AddWithValue("$bDate", loan.BorrowedDate.ToString("o"));
                cmd.Parameters.AddWithValue("$dDate", loan.DueDate.ToString("o"));
                cmd.Parameters.AddWithValue(
                    "$rDate",
                    loan.ReturnedDate.HasValue
                        ? loan.ReturnedDate.Value.ToString("o")
                        : DBNull.Value
                );
                cmd.Parameters.AddWithValue("$status", (int)loan.Status);
                cmd.Parameters.AddWithValue("$renewals", loan.RenewalCount);
                cmd.Parameters.AddWithValue("$id", loan.Id);
                int rows = cmd.ExecuteNonQuery();
                Logger.Info($"Executed Update(Loan): updated {rows} row(s). Loan={loan.ToString()}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Update(Loan) failed. Loan={loan.ToString()}", ex);
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
                cmd.CommandText = "DELETE FROM Loans WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                int rows = cmd.ExecuteNonQuery();
                Logger.Info($"Executed Delete(Loan): deleted {rows} row(s). Id={id}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Delete(Loan) failed for Id={id}", ex);
                throw;
            }
        }

        // -----------------------------------------------------------------------
        public IEnumerable<Loan> GetByBook(int bookId)
        {
            var list = new List<Loan>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Loans WHERE BookId=$bookId;";
                cmd.Parameters.AddWithValue("$bookId", bookId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var loan = new Loan
                    {
                        Id          = reader.GetInt32(0),
                        BookId      = reader.GetInt32(1),
                        MemberId    = reader.GetInt32(2),
                        BorrowedDate= DateTime.Parse(reader.GetString(3)),
                        DueDate     = DateTime.Parse(reader.GetString(4)),
                        ReturnedDate= reader.IsDBNull(5) ? (DateTime?)null : DateTime.Parse(reader.GetString(5)),
                        Status      = (LoanStatus)reader.GetInt32(6),
                        RenewalCount= reader.GetInt32(7)
                    };
                    list.Add(loan);
                }
                Logger.Info($"Executed GetLoansByBook({bookId}): retrieved {list.Count} loans");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetLoansByBook({bookId}) failed", ex);
                throw;
            }
        }

        public IEnumerable<Loan> GetByMember(int memberId)
        {
            var list = new List<Loan>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Loans WHERE MemberId=$memberId;";
                cmd.Parameters.AddWithValue("$memberId", memberId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var loan = new Loan
                    {
                        Id          = reader.GetInt32(0),
                        BookId      = reader.GetInt32(1),
                        MemberId    = reader.GetInt32(2),
                        BorrowedDate= DateTime.Parse(reader.GetString(3)),
                        DueDate     = DateTime.Parse(reader.GetString(4)),
                        ReturnedDate= reader.IsDBNull(5) ? (DateTime?)null : DateTime.Parse(reader.GetString(5)),
                        Status      = (LoanStatus)reader.GetInt32(6),
                        RenewalCount= reader.GetInt32(7)
                    };
                    list.Add(loan);
                }
                Logger.Info($"Executed GetLoansByMember({memberId}): retrieved {list.Count} loans");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetLoansByMember({memberId}) failed", ex);
                throw;
            }
        }

        public IEnumerable<Loan> GetByStatus(LoanStatus status)
        {
            var list = new List<Loan>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Loans WHERE Status=$status;";
                cmd.Parameters.AddWithValue("$status", (int)status);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var loan = new Loan
                    {
                        Id          = reader.GetInt32(0),
                        BookId      = reader.GetInt32(1),
                        MemberId    = reader.GetInt32(2),
                        BorrowedDate= DateTime.Parse(reader.GetString(3)),
                        DueDate     = DateTime.Parse(reader.GetString(4)),
                        ReturnedDate= reader.IsDBNull(5) ? (DateTime?)null : DateTime.Parse(reader.GetString(5)),
                        Status      = (LoanStatus)reader.GetInt32(6),
                        RenewalCount= reader.GetInt32(7)
                    };
                    list.Add(loan);
                }
                Logger.Info($"Executed GetLoansByStatus({status}): retrieved {list.Count} loans");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetLoansByStatus({status}) failed", ex);
                throw;
            }
        }
    }
}
