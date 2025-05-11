using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Repository.Interfaces;


public interface IMemberRepository: IRepository<Member>
{
    Member GetByEmail(string email);
    IEnumerable<Member> GetByName(string namePart);
}