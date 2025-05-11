using System.Collections.Generic;
using LibrarySystem.Domain;
using LibrarySystem.Repository.Interfaces;
using LibrarySystem.Service.Interfaces;

namespace LibrarySystem.Service.Implemented;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepo;
    public MemberService(IMemberRepository memberRepo) => _memberRepo = memberRepo;

    public Member CreateMember(string fullName, string email, string phone)
    {
        var m = new Member { FullName = fullName, Email = email , Phone = phone};
        _memberRepo.Add(m);
        return m;
    }

    public Member GetMemberById(int id) 
        => _memberRepo.GetById(id);

    public IEnumerable<Member> GetAllMembers() 
        => _memberRepo.GetAll();

    public void UpdateMember(Member member) 
        => _memberRepo.Update(member);

    public void DeleteMember(int id) 
        => _memberRepo.Delete(id);

    // ------------------------------------------------------------------
    public Member GetMemberByEmail(string email) 
        => _memberRepo.GetByEmail(email);

    public IEnumerable<Member> GetMembersByName(string namePart) 
        => _memberRepo.GetByName(namePart);
}