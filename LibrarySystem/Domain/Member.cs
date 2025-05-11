namespace LibrarySystem.Domain;

public class Member: IEntity
{
    public int    Id       { get; set; }
    public string FullName { get; set; }
    public string Email    { get; set; }
    public string Phone    { get; set; }
    
    public Member() { }
    
    public Member(int id, string fullName, string email, string phone)
    {
        Id       = id;
        FullName = fullName;
        Email    = email;
        Phone    = phone;
    }
    
    public string ToString()
    {
        return $"ID: {Id}, FullName: {FullName}, Email: {Email}, Phone: {Phone}";
    }
}