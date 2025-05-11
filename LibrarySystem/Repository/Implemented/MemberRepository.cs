using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using log4net;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;

namespace LibrarySystem.Repository.Implemented
{
    public class MemberRepository : IMemberRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MemberRepository));
        private readonly string _connStr = IRepository<Member>.LoadDatabaseUrl();

        public void Add(Member member)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Members (FullName, Email, Phone) VALUES ($name, $email, $phone);";
                cmd.Parameters.AddWithValue("$name",  member.FullName);
                cmd.Parameters.AddWithValue("$email", member.Email);
                cmd.Parameters.AddWithValue("$phone", member.Phone);
                cmd.ExecuteNonQuery();
                Logger.Info($"Executed Add(Member): {member}");
            }
            catch (SqliteException ex)
            {
                Logger.Error("Add(Member) failed", ex);
                throw;
            }
        }

        public Member GetById(int id)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Members WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    Logger.Info($"Executed GetById({id}): no record found");
                    return null;
                }
                var member = new Member
                {
                    Id       = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email    = reader.GetString(2),
                    Phone    = reader.GetString(3)
                };
                Logger.Info($"Executed GetById({id}): {member}");
                return member;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"GetById({id}) failed", ex);
                throw;
            }
        }

        public IEnumerable<Member> GetAll()
        {
            var list = new List<Member>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Members;";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Member
                    {
                        Id       = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        Email    = reader.GetString(2),
                        Phone    = reader.GetString(3)
                    });
                }
                Logger.Info($"Executed GetAll(): retrieved {list.Count} members");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error("GetAll() failed", ex);
                throw;
            }
        }

        public void Update(Member member)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Members SET FullName=$name, Email=$email, Phone=$phone WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$name",  member.FullName);
                cmd.Parameters.AddWithValue("$email", member.Email);
                cmd.Parameters.AddWithValue("$phone", member.Phone);
                cmd.Parameters.AddWithValue("$id",    member.Id);
                cmd.ExecuteNonQuery();
                Logger.Info($"Executed Update(Member): {member}");
            }
            catch (SqliteException ex)
            {
                Logger.Error("Update(Member) failed", ex);
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
                cmd.CommandText = "DELETE FROM Members WHERE Id=$id;";
                cmd.Parameters.AddWithValue("$id", id);
                cmd.ExecuteNonQuery();
                Logger.Info($"Executed Delete(Member): Id={id}");
            }
            catch (SqliteException ex)
            {
                Logger.Error($"Delete(Member) failed for Id={id}", ex);
                throw;
            }
        }

        public Member GetByEmail(string email)
        {
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Members WHERE Email=$email;";
                cmd.Parameters.AddWithValue("$email", email);
                using var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    Logger.Info($"Executed FindByEmail('{email}'): no record found");
                    return null;
                }
                var member = new Member
                {
                    Id       = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email    = reader.GetString(2),
                    Phone    = reader.GetString(3)
                };
                Logger.Info($"Executed FindByEmail('{email}'): {member}");
                return member;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"FindByEmail('{email}') failed", ex);
                throw;
            }
        }

        public IEnumerable<Member> GetByName(string namePart)
        {
            var list = new List<Member>();
            try
            {
                using var conn = new SqliteConnection(_connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Members WHERE FullName LIKE $pattern;";
                cmd.Parameters.AddWithValue("$pattern", $"%{namePart}%");
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Member
                    {
                        Id       = reader.GetInt32(0),
                        FullName = reader.GetString(1),
                        Email    = reader.GetString(2),
                        Phone    = reader.GetString(3)
                    });
                }
                Logger.Info($"Executed FindByName('{namePart}'): retrieved {list.Count} members");
                return list;
            }
            catch (SqliteException ex)
            {
                Logger.Error($"FindByName('{namePart}') failed", ex);
                throw;
            }
        }
    }
}
