using System.Collections.Generic;

namespace GraduationTracker.Interfaces
{
    public interface IRequirementRepository
    {
        Requirement GetRequirement(int id);

        IEnumerable<Requirement> GetRequirements();
    }
}
