using Microsoft.AspNetCore.Mvc;
using RestApi.Domain;
using RestApi.Repositories;

namespace RestApi.Controllers;

[ApiController]
public class AnimalsResource : ControllerBase
{

    private readonly IAnimalRepository _repository;

    public AnimalsResource()
    {
        _repository = new AnimalRepository("server=db-mssql16.pjwstk.edu.pl;database=animals;trusted_connection=true;TrustServerCertificate=True");
    }

    [HttpPost("/api/animals")]
    public ActionResult<AnimalView> AddAnimal([FromBody] AnimalView animalView)
    {
        _repository.Add(new Animal(
            animalView.Id, 
            animalView.Name, 
            animalView.Category,
            animalView.Area,
            animalView.Description));

        return new ActionResult<AnimalView>(animalView);
    }
    
    [HttpPut("/api/animals/{id}")]
    public ActionResult<AnimalView> UpdateAnimal(int id, [FromBody] AnimalView updated)
    {
        Animal animal = _repository.FindById(id);

        animal.Name = updated.Name;
        animal.Description = updated.Description;
        animal.Category = updated.Category;
        animal.Area = updated.Area;

        return new ActionResult<AnimalView>(new AnimalView(
            animal.Id,
            animal.Name,
            animal.Description,
            animal.Category,
            animal.Area));
    }
    
    [HttpGet("/api/animals")]
    public ActionResult<IEnumerable<AnimalView>> GetAllAnimals([FromQuery(Name = "orderBy")] string? orderBy)
    {
        var result = _repository.FindAll();

        if (orderBy != null)
        {
            result = result.OrderBy(a =>
            {
                switch (orderBy)
                {
                    case "name":
                        return a.Name;
                    case "area":
                        return a.Area;
                    case "description":
                        return a.Description;
                    case "category":
                        return a.Category;
                    default:
                        throw new ApplicationException("Invalid sort field " + orderBy);
                }
            });
        }
         
        return new ActionResult<IEnumerable<AnimalView>>(
            result.Select(animal => new AnimalView(
                animal.Id,
                animal.Name,
                animal.Description,
                animal.Category,
                animal.Area)));
    }
}