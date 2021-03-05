using ServicesLib.Services.Repository.Generic;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using ServicesLib.Domain.Models;

namespace ServicesLib.Services.Repositories
{
    public class ProgrammeRepository : GenericRepository<Programme>, IProgrammeRepository 
    {
        public ProgrammeRepository(SchoolDbContext dbContext) : base (dbContext)
        { 
        }
        public async Task DeleteProgrammeAsync(string Id)
        {
            var programme = await base.FindByIdAsync(Id);
            if (programme != null)
                base.Delete(programme);
            else
                throw new DataAccessException($"DeleteProgrammeAsync: student: {Id} not found");
        }
    }
}