using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface ITeacherRepository : IRepository<Teacher> 
    {
        Task DeleteTeacherAsync(string Id);
        Task<IEnumerable<Teacher>>  AllIncludeProgrammeAsync();
        Task<Teacher> FindByIdIncludeProgrammeAsync(string Id);
    }
}