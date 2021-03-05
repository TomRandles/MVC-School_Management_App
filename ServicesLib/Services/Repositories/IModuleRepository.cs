using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface IModuleRepository : IRepository<Module> 
    {
        Task DeleteModuleAsync(string Id);
        Task<IEnumerable<Module>> AllIncludeProgramme();
        Task<IEnumerable<Module>> FindAllModulesForProgramme(string programmeID);
    }
}