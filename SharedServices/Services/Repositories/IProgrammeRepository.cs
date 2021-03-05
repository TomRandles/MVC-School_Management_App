using ServicesLib.Domain.Models;
using ServicesLib.Services.Repositories.Generic;
using System.Threading.Tasks;

namespace ServicesLib.Services.Repositories
{
    public interface IProgrammeRepository : IRepository<Programme> 
    {
        Task DeleteProgrammeAsync(string Id);
    }
}