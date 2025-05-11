using System;
using System.Collections.Generic;
using System.IO;
using LibrarySystem.Domain;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Repository.Interfaces;

/// CRUD operations repository interface
public interface IRepository<T> where T : IEntity
{
    void Add(T t);
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Update(T t);
    void Delete(int id);

    static string LoadDatabaseUrl()
    {
        try
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var dbUrl = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(dbUrl))
                throw new FileNotFoundException("Database URL not found in appsettings.json");

            return dbUrl;
        } catch (Exception ex)
        {
            throw new Exception("Failed to load database URL", ex);
        }
    }
}