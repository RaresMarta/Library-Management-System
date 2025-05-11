using System;

namespace LibrarySystem.Domain;

public class Loan : IEntity
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int MemberId { get; set; }
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public LoanStatus Status { get; set; }
    public int RenewalCount { get; set; }
    
    public Loan() { }
    
    public Loan(int id, int bookId, int memberId, DateTime borrowedDate, DateTime dueDate, DateTime? returnedDate, LoanStatus status, int renewalCount)
    {
        Id = id;
        BookId = bookId;
        MemberId = memberId;
        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        ReturnedDate = returnedDate;
        Status = status;
        RenewalCount = renewalCount;
    }
    public string ToString()
    {
        return $"ID: {Id}, BookId: {BookId}, MemberId: {MemberId}, BorrowedDate: {BorrowedDate}, DueDate: {DueDate}, ReturnedDate: {ReturnedDate}, Status: {Status}, RenewalCount: {RenewalCount}";
    }
}