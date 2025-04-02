using person_crud_api.Model;

namespace person_crud_api.Services;

public interface IPersonService
{
    Person Create(Person person);
    Person? FindById(long id);
    List<Person> FindAll();
    Person Update(Person person);
    void Delete(long id);
}
