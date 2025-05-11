namespace LibrarySystem.Domain;

public class Book: IEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Quantity { get; set; }
    public string Genre    { get; set; }
    
    public Book() { }
    
    public Book(int id, string title, string author, int quantity, string genre)
    {
        Id = id;
        Title = title;
        Author = author;
        Quantity = quantity;
        Genre = genre;
    }
    
    public string ToString()
    {
        return $"ID: {Id}, Title: {Title}, Author: {Author}, Quantity: {Quantity}, Genre: {Genre}";
    }
}