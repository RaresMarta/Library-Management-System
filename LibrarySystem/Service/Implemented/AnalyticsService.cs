using System.Collections.Generic;
using System.Linq;
using LibrarySystem.Domain;
using LibrarySystem.Service.Interfaces;

namespace LibrarySystem.Service.Implemented;

public class AnalyticsService : IAnalyticsService
{
    private readonly ILoanService _loanService;
    private readonly IBookService _bookService;

    public AnalyticsService(ILoanService loanService, IBookService bookService)
    {
        _loanService = loanService;
        _bookService = bookService;
    }

    public Dictionary<string, int> GetGenreDistribution()
    {
        return _bookService.GetAllBooks()
            .GroupBy(b => b.Genre)
            .ToDictionary(g => g.Key ?? "Unknown", g => g.Count());
    }       

    public Dictionary<LoanStatus, int> GetLoanStatusDistribution()
    {
        return _loanService.GetAllLoans()
            .GroupBy(l => l.Status)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public IEnumerable<Book> GetMostBorrowedBooks(int count)
    {
        return _loanService.GetAllLoans()
            .GroupBy(l => l.BookId)
            .OrderByDescending(g => g.Count())
            .Take(count)
            .Select(g => _bookService.GetBookById(g.Key));
    }

    public int GetBorrowCount(int bookId)
    {
        return _loanService.GetAllLoans().Count(l => l.BookId == bookId);
    }
}