using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Interfaces;

namespace GraduationTracker.Repository
{
    public class DiplomaRepository : IDiplomaRepository
    {
        public Diploma GetDiploma(int id)
        {
            return GetDiplomas().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Diploma> GetDiplomas()
        {
            return new List<Diploma>
            {
                new Diploma
                {
                    Id = 1,
                    Credits = 4,
                    Requirements = new List<int> {100, 102, 103, 104}
                }
            };
        }
    }
}
