using RestApi.Controllers;

namespace RestApi.Domain;

public interface IAnimalRepository
{

    IEnumerable<Animal> FindAll();

    Animal FindById(int id);
    
    void Add(Animal animal);

    void Update(Animal animal);

    void Delete(Animal animal);

}