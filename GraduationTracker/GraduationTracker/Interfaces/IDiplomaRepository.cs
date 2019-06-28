using System.Collections.Generic;

namespace GraduationTracker.Interfaces
{
    public interface IDiplomaRepository
    {
        
        Diploma GetDiploma(int id);

        IEnumerable<Diploma> GetDiplomas();
    }
}
