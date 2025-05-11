using System.Collections.Generic;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;
using LibrarySystem.Service.Interfaces;

namespace LibrarySystem.Service.Implemented;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    
    public BookService(IBookRepository bookRepo) => _bookRepo = bookRepo;
    
    public Book AddBook(string title, string author, int quantity, string genre)
    {
        var book = new Book(0, title, author, quantity, genre);
        _bookRepo.Add(book);
        return book;
    }

    public Book GetBookById(int id) => _bookRepo.GetById(id);

    public IEnumerable<Book> GetAllBooks() => _bookRepo.GetAll();

    public void UpdateBook(Book book) => _bookRepo.Update(book);

    public void DeleteBook(int id) => _bookRepo.Delete(id);

    // ------------------------------------------------------------------
    public IEnumerable<Book> GetBooksByTitle(string title) 
        => _bookRepo.GetByTitle(title);
    
    public IEnumerable<Book> GetBooksByAuthor(string author)
        => _bookRepo.GetByAuthor(author);

}