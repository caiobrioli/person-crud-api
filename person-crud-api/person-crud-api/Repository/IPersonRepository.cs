using Microsoft.EntityFrameworkCore;
using person_crud_api.Model;

namespace person_crud_api.Business;

public interface IPersonRepository
{
    Person Create(Person person);
    Person? FindById(long id);
    List<Person> FindAll();
    Person Update(Person person);
    void Delete(long id);
    Person? ExistingPerson(long id);
}
