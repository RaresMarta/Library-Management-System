using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Service.Interfaces;

public interface IAnalyticsService
{
    Dictionary<string, int> GetGenreDistribution();
    Dictionary<LoanStatus, int> GetLoanStatusDistribution();
    IEnumerable<Book> GetMostBorrowedBooks(int count);
    int GetBorrowCount(int bookId);
}