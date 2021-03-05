using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface IAssessmentResultRepository : IRepository<AssessmentResult> 
    {
        Task DeleteAssessmentResultAsync(string assessmentResultID);
        Task<IEnumerable<AssessmentResult>> AllResultsForProgrammeAsync(string programmmeID);
        Task<AssessmentResult> GetAssessmentResultDetailsAsync(string assessmentResultID);
        Task<IEnumerable<AssessmentResult>> GetAllAssessmentResults();
        Task<AssessmentResult> GetAssessmentResultAsync(string assessmentResultID);
        bool AssessmentResultExists(string assessmentResultID);
        Task<IEnumerable<AssessmentResult>> GetStudentAssessmentResults(string studentID);
    }
}