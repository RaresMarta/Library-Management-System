using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Repository.Interfaces;

public interface ILoanRepository: IRepository<Loan>
{
    IEnumerable<Loan> GetByBook(int bookId);
    IEnumerable<Loan> GetByMember(int memberId);
    IEnumerable<Loan> GetByStatus(LoanStatus status);
}