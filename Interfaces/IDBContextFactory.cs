using IquraStudyBE.Context;

namespace IquraStudyBE.Classes;

public interface IDBContextFactory
{ 
    MyDbContext CreateDbContext(string connectionString);
}
