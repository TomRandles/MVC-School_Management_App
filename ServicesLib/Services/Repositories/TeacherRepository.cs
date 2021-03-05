using ServicesLib.Services.Repository.Generic;
using ServicesLib.Domain.Models;
using ServicesLib.Services.Database;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServicesLib.Services.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(SchoolDbContext dbContext) : base(dbContext)
        {
        }
        public async Task DeleteTeacherAsync(string Id)
        {
            var Teacher = await base.FindByIdAsync(Id);
            if (Teacher != null)
                base.Delete(Teacher);
            else
                throw new DataAccessException($"DeleteTeacherAsync: Teacher: {Id} not found");
        }
        public async Task<IEnumerable<Teacher>> AllIncludeProgrammeAsync()
        {
            return await base.dbContext.Teachers
                                       .Include(t => t.Programme)
                                       .ToListAsync();
        }

        public async Task<Teacher> FindByIdIncludeProgrammeAsync(string Id)
        {
            return await dbContext.Teachers
                                  .Include(m => m.Programme)
                                  .FirstOrDefaultAsync(m => m.TeacherID == Id);
        }
    }
}