using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface IStudentRepository : IRepository<Student> 
    {
        Task<IEnumerable<Student>> AllIncludeProgrammeAsync();
        Task<Student> FindByIdIncludeProgrammeAsync(string Id);
        Task DeleteStudentAsync(string Id);
    }
}