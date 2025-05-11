using System;
using System.Collections.Generic;
using System.Linq;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;
using LibrarySystem.Service.Interfaces;
using log4net;

namespace LibrarySystem.Service.Implemented;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepo;
    private readonly IBookRepository _bookRepo;
    private readonly IMemberRepository _memberRepo;
    private static readonly ILog Logger = LogManager.GetLogger(typeof(LoanService));

    public LoanService(ILoanRepository loanRepo, IBookRepository bookRepo, IMemberRepository memberRepo)
    {
        _loanRepo = loanRepo;
        _bookRepo = bookRepo;
        _memberRepo = memberRepo;
    }

    /// Create an active loan and decrement book stock.
    public Loan LendBook(int bookId, int memberId, DateTime dueAt)
    {
        var book = GetBookOrThrow(bookId);
        GetMemberOrThrow(memberId);
        if(LoanExists(bookId, memberId))
            throw new ArgumentException($"Member {memberId} already has an active or overdue loan for book {bookId}");
        if (book.Quantity < 1)
            throw new ArgumentException($"Book {bookId} is out of stock");
        
        book.Quantity--;
        _bookRepo.Update(book);
        
        var newLoan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowedDate = DateTime.UtcNow,
            DueDate = dueAt,
            ReturnedDate = null,
            Status = LoanStatus.Active,
            RenewalCount = 0
        };

        _loanRepo.Add(newLoan);
        Logger.Info($"Lent book {bookId} to member {memberId} with due date {dueAt}");
        return newLoan;
    }
    
    /// Mark loan as returned and increment book stock.
    public void ReportReturned(int loanId)
    {
        var loan = GetLoanOrThrow(loanId);

        if (loan.Status == LoanStatus.Returned)
            throw new ArgumentException($"Cannot return loan {loanId}; current status is {loan.Status}");

        loan.ReturnedDate = DateTime.UtcNow;
        loan.Status = LoanStatus.Returned;
        _loanRepo.Update(loan);

        var book = GetBookOrThrow(loan.BookId);
        book.Quantity++;
        _bookRepo.Update(book);

        Logger.Info($"Loan {loanId} marked as returned. Book {book.Id} stock increased.");
    }

    /// Mark loan as overdue.
    public void ReportOverdue(int loanId)
    {
        var loan = GetLoanOrThrow(loanId);
        
        if (loan.Status != LoanStatus.Active)
            throw new ArgumentException($"Cannot mark loan {loanId} as overdue; current status is {loan.Status}");

        loan.Status = LoanStatus.Overdue;
        _loanRepo.Update(loan);

        Logger.Info($"Loan {loanId} marked as overdue.");
    }

    /// Renew loan with a new due date.
    public void RenewLoan(int loanId, DateTime newDueAt)
    {
        var loan = GetLoanOrThrow(loanId);

        if (loan.Status != LoanStatus.Active)
            throw new ArgumentException($"Only active loans can be renewed; loan {loanId} has status {loan.Status}");

        loan.DueDate = newDueAt;
        loan.RenewalCount++;
        _loanRepo.Update(loan);
        
        Logger.Info($"Loan {loanId} renewed. New due date: {newDueAt}");
    }

    public void UpdateOverdueLoans()
    {
        var overdue = _loanRepo.GetAll()
            .Where(l => l.Status == LoanStatus.Active
                        && l.DueDate.Date < DateTime.Today)
            .ToList();

        foreach (var loan in overdue)
        {
            loan.Status = LoanStatus.Overdue;
            _loanRepo.Update(loan);
            Logger.Info($"Auto-marked loan #{loan.Id} as overdue");
        }
    }
    
    public IEnumerable<Loan> GetAllLoans() => _loanRepo.GetAll();

    public IEnumerable<Loan> GetLoansByBook(int id) => _loanRepo.GetByBook(id);

    public IEnumerable<Loan> GetLoansByMember(int id) => _loanRepo.GetByMember(id);

    public IEnumerable<Loan> GetLoansByStatus(LoanStatus status) => _loanRepo.GetByStatus(status);


    /// Finds an active or overdue loan for the given book and member.
    /// Throws error if no matching loan exists.
    public Loan GetLoanByBookAndMember(int bookId, int memberId)
    {
        var loan = _loanRepo.GetByBook(bookId)
            .FirstOrDefault(l => l.MemberId == memberId
                                 && (l.Status == LoanStatus.Active || l.Status == LoanStatus.Overdue));
        if (loan != null)
            return loan;
        
        throw new ArgumentException($"No active or overdue loan found for book {bookId} and member {memberId}");
    }
    
    /// Verify if a loan exists based on bookId and memberId
    public bool LoanExists(int bookId, int memberId) =>
        _loanRepo.GetByBook(bookId)
            .Any(l => l.MemberId == memberId
                      && (l.Status == LoanStatus.Active || l.Status == LoanStatus.Overdue));

    // ------------------------- Helper functions -----------------------------------------
    private Loan GetLoanOrThrow(int loanId) =>
        _loanRepo.GetById(loanId) ?? throw new ArgumentException($"Loan {loanId} not found");

    private Book GetBookOrThrow(int bookId) =>
        _bookRepo.GetById(bookId) ?? throw new ArgumentException($"Book {bookId} not found");
    
    private Member GetMemberOrThrow(int memberId) =>
        _memberRepo.GetById(memberId) ?? throw new ArgumentException($"Member {memberId} not found");
}
