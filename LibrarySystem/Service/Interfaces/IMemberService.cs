using System.Collections.Generic;
using LibrarySystem.Domain;

namespace LibrarySystem.Service.Interfaces;

public interface IMemberService
{
    Member CreateMember(string fullName, string email, string phone);
    Member GetMemberById(int id);
    IEnumerable<Member> GetAllMembers();
    void UpdateMember(Member member);
    void DeleteMember(int id);
    Member GetMemberByEmail(string email);
    IEnumerable<Member> GetMembersByName(string namePart);
}