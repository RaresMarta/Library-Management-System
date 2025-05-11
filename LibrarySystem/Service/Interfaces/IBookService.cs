using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Service.Interfaces;

public interface IBookService
{
    Book AddBook(string title, string author, int quantity, string genre);
    Book GetBookById(int id);
    IEnumerable<Book> GetAllBooks();
    void UpdateBook(Book book);
    void DeleteBook(int id);
}