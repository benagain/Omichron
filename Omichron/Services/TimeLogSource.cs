using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omichron.Services
{
    public interface TimeLogSource
    {
        Task<List<TimeLog>> Search();
    }
}
