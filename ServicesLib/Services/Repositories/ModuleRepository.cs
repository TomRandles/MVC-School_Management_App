using ServicesLib.Services.Repository.Generic;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using ServicesLib.Domain.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServicesLib.Services.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        public ModuleRepository(SchoolDbContext dbContext) : base (dbContext)
        { 
        }
        public async Task DeleteModuleAsync(string Id)
        {
            var Module = await base.FindByIdAsync(Id);
            if (Module != null)
                base.Delete(Module);
            else
                throw new DataAccessException($"DeleteModuleAsync: student: {Id} not found");
        }

        public async Task<IEnumerable<Module>> AllIncludeProgramme()
        {
            return await base.dbContext.Modules.Include(m => m.Programme).ToListAsync();
        }

        public async Task<IEnumerable<Module>> FindAllModulesForProgramme(string programmeID)
        {
            return await base.dbContext.Modules.Where(m => m.ProgrammeID == programmeID).ToListAsync();
        }
    }
}