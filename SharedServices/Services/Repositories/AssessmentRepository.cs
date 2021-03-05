using ServicesLib.Services.Repository.Generic;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using ServicesLib.Domain.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServicesLib.Services.Repositories
{
    public class AssessmentRepository : GenericRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(SchoolDbContext dbContext) : base (dbContext)
        { 
        }
        public async Task DeleteAssessmentAsync(string Id)
        {
            var assessment = await base.FindByIdAsync(Id);
            if (assessment != null)
                base.Delete(assessment);
            else
                throw new DataAccessException($"DeleteAssessmentAsync: assessment {Id} not found");
        }
        public async virtual Task<IEnumerable<Assessment>> AllIncludeModuleAsync()
        {
            return await base.dbContext.Assessments.Include(m => m.Module).ToListAsync();
        }

        public virtual async Task<IEnumerable<Assessment>> AllAssessmentsForModule(string moduleID)
        {
            return await base.dbContext.Assessments.Where(m => m.ModuleID == moduleID).ToListAsync();
        }
    }
}