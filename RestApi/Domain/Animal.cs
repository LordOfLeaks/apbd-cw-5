namespace RestApi.Domain;

public class Animal
{

    private int _id;
    private string _name;
    private string _category;
    private string _area;

    public Animal(int id, string name, string category, string area)
    {
        _id = id;
        _name = name;
        _category = category;
        _area = area;
    }

    public Animal(int id, string name, string category, string area, string? description)
    {
        _id = id;
        _name = name;
        _category = category;
        _area = area;
        Description = description;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string? Description { get; set; }

    public string Category
    {
        get => _category;
        set => _category = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Area
    {
        get => _area;
        set => _area = value ?? throw new ArgumentNullException(nameof(value));
    }
}