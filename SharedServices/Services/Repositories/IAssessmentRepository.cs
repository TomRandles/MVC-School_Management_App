using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface IAssessmentRepository : IRepository<Assessment> 
    {
        Task DeleteAssessmentAsync(string Id);
        Task<IEnumerable<Assessment>> AllIncludeModuleAsync();
        Task<IEnumerable<Assessment>> AllAssessmentsForModule(string moduleID);
    }
}