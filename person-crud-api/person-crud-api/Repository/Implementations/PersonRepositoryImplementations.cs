using Microsoft.EntityFrameworkCore;
using person_crud_api.Model;
using person_crud_api.Model.Context;

namespace person_crud_api.Business.Implementations;

public class PersonRepositoryImplementations : IPersonRepository
{
    private MySqlContext _context;
    public Person? ExistingPerson(long id) => _context.People.SingleOrDefault(p => p.Id == id);

    public PersonRepositoryImplementations(MySqlContext context)
    {
        _context = context;
    }

    public List<Person> FindAll()
    {
        return _context.People.ToList();
    }

    public Person? FindById(long id)
    {
        return _context.People.SingleOrDefault(p => p.Id.Equals(id));
    }

    public Person Create(Person person)
    {
        try
        {
            _context.People.Add(person);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
        return person;
    }

    public Person Update(Person person)
    {
        var personExists = ExistingPerson(person.Id);
        if (personExists is null)
        {
            throw new KeyNotFoundException("Person not found.");
        }
        try
        {
            _context.Entry(personExists).CurrentValues.SetValues(person);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }

        return personExists;
    }

    public void Delete(long id)
    {
        var personExists = ExistingPerson(id);
        if (personExists is null)
        {
            throw new KeyNotFoundException("Person not found.");
        }
        try
        {
            _context.People.Remove(personExists);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
