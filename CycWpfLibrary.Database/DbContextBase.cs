using System.Data.Entity;
using System.Data.Entity.Validation;

namespace CycWpfLibrary.Database
{
  public class DbContextBase : DbContext
  {
    public DbContextBase(string nameOrConnectionString)
        : base(nameOrConnectionString)
    {
    }

    public override int SaveChanges()
    {
      try
      {
        return base.SaveChanges();
      }
      catch (DbEntityValidationException e)
      {
        var newException = new FormattedDbEntityValidationException(e);
        throw newException;
      }
    }
  }
}