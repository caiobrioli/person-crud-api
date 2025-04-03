using person_crud_api.Bussiness;
using person_crud_api.Model;
using person_crud_api.Model.Context;

namespace person_crud_api.Business.Implementations;

public class PersonBussinesImplementations : IPersonBusiness
{
    private readonly IPersonRepository _repository;

    public PersonBussinesImplementations(IPersonRepository repository)
    {
        _repository = repository;
    }

    public List<Person> FindAll()
    {
        return _repository.FindAll();
    }

    public Person? FindById(long id)
    {
        return _repository.FindById(id);
    }

    public Person Create(Person person)
    {
      return _repository.Create(person);
    }

    public Person? Update(Person person)
    {
        var personExists = _repository.ExistingPerson(person.Id);
        if(personExists == null) return null;
        return _repository.Update(person);
    }

    public void Delete(long id)
    {
        _repository.Delete(id);
    }
}
