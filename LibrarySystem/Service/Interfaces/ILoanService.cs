using System;
using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Service.Interfaces;

public interface ILoanService
{
    Loan LendBook(int bookId, int memberId, DateTime dueAt);  
    void ReportReturned(int loanId);                          
    void ReportOverdue(int loanId);                            
                             
    void RenewLoan(int loanId, DateTime newDueAt);
    
    IEnumerable<Loan> GetAllLoans();
    IEnumerable<Loan> GetLoansByBook(int bookId);
    IEnumerable<Loan> GetLoansByMember(int memberId);
    IEnumerable<Loan> GetLoansByStatus(LoanStatus status);
}