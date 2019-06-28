using System.Collections.Generic;

namespace GraduationTracker
{
    public class Diploma
    {
        public int Id { get; set; }
        public int Credits { get; set; }
        public List<int> Requirements { get; set; }
    }
}
