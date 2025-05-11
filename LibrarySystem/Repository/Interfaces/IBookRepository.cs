using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Repository.Interfaces;

public interface IBookRepository: IRepository<Book>
{
    IEnumerable<Book> GetByTitle(string title);
    IEnumerable<Book> GetByAuthor(string author);
}