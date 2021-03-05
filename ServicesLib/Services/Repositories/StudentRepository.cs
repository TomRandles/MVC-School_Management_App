using ServicesLib.Services.Repository.Generic;
using ServicesLib.Domain.Models;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServicesLib.Services.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchoolDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Student> FindByIdIncludeProgrammeAsync(string Id)
        {
            return await dbContext.Students
                                  .Include(m => m.Programme)
                                  .FirstOrDefaultAsync(m => m.StudentID == Id);
        }

        public async Task<IEnumerable<Student>> AllIncludeProgrammeAsync()
        {
            return await base.dbContext
                             .Students
                             .Include(m => m.Programme)
                             .ToListAsync();
        }

        public async Task DeleteStudentAsync(string Id)
        {
            var student = await base.FindByIdAsync(Id);
            if (student != null)
                base.Delete(student);
            else
                throw new DataAccessException($"DeleteStudentAsync: student: {Id} not found");
        }
    }
}