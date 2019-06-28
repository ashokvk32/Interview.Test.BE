using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Interfaces;

namespace GraduationTracker.Repository
{
    public class RequirementRepository : IRequirementRepository
    {
        public Requirement GetRequirement(int id)
        {
            return GetRequirements().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Requirement> GetRequirements()
        {
            return new List<Requirement>
            {
                new Requirement{Id = 100, Name = "Math", MinimumMark = 50, Courses = new List<int> {1}, Credits = 1 },
                new Requirement{Id = 102, Name = "Science", MinimumMark = 50, Courses = new List<int> {2}, Credits = 1 },
                new Requirement{Id = 103, Name = "Literature", MinimumMark = 50, Courses = new List<int> {3}, Credits = 1},
                new Requirement{Id = 104, Name = "Physical Education", MinimumMark = 50, Courses = new List<int> {4}, Credits = 1 }
            };
        }
    }
}
