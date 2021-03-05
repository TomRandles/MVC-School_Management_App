using ServicesLib.Services.Repository.Generic;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using ServicesLib.Domain.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ServicesLib.Services.Repositories
{
    public class AssessmentResultRepository : GenericRepository<AssessmentResult>, IAssessmentResultRepository
    {
        public AssessmentResultRepository(SchoolDbContext dbContext) : base(dbContext)
        {
        }
        public async Task DeleteAssessmentResultAsync(string Id)
        {
            var AssessmentResult = await base.FindByIdAsync(Id);
            if (AssessmentResult != null)
                base.Delete(AssessmentResult);
            else
                throw new DataAccessException($"DeleteAssessmentResultAsync: assessment result {Id} not found");
        }
        public async virtual Task<IEnumerable<AssessmentResult>> AllIncludeModuleAsync()
        {
            return await base.dbContext.AssessmentResults.Include(m => m.Module).ToListAsync();
        }

        public async virtual Task<IEnumerable<AssessmentResult>> GetAllAssessmentResults()
        {
            var assessmentResultList = await base.dbContext.AssessmentResults
                .Include(a => a.Module)
                .Include(b => b.Programme)
                .Include(c => c.Student)
                .Include(d => d.Assessment)
                .ToListAsync();
            return assessmentResultList;
        }
        public async Task<IEnumerable<AssessmentResult>> AllResultsForProgrammeAsync(string programmmeID)
        {
            var ars = await base.dbContext.AssessmentResults.Where(m => m.ProgrammeID == programmmeID)
                                       .Include(a => a.Module)
                                       .Include(b => b.Programme)
                                       .Include(c => c.Student)
                                       .Include(d => d.Assessment)
                                       .ToListAsync();
            return ars;
        }

        public async Task<AssessmentResult> GetAssessmentResultDetailsAsync(string assessmentResultID)
        {
            var ar = await base.dbContext
                               .AssessmentResults
                               .Include(a => a.Module)
                               .Include(b => b.Programme)
                               .Include(c => c.Student)
                               .Include(d => d.Assessment)
                               .FirstOrDefaultAsync(m => m.AssessmentResultID == assessmentResultID);
            return ar;
        }
        public async Task<AssessmentResult> GetAssessmentResultAsync(string assessmentResultID)
        {
            var assessmentResult = await base.dbContext
                                             .AssessmentResults
                                             .FirstOrDefaultAsync(m => m.AssessmentResultID == assessmentResultID);
            return assessmentResult;
        }

        public bool AssessmentResultExists(string assessmentResultID)
        {
                      
            return base.dbContext.AssessmentResults.Any(e => e.AssessmentResultID == assessmentResultID);

        }

        public async virtual Task<IEnumerable<AssessmentResult>> GetStudentAssessmentResults(string studentID)
        {
            return await base.dbContext.AssessmentResults.Where(m => m.StudentID == studentID)
                                                         .Include(a => a.Module)
                                                         .Include(b => b.Programme)
                                                         .Include(c => c.Student)
                                                         .Include(d => d.Assessment)
                                                         .ToListAsync();
        }
    }
}