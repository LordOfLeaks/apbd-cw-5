namespace RestApi.Controllers;

public record AnimalView(
    int Id,
    string Name,
    string? Description,
    string Category,
    string Area)
{
    
}