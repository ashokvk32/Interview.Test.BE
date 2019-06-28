using System.Collections.Generic;

namespace GraduationTracker
{
    public class Student
    {
        public int Id { get; set; }
        public List<Course> Courses { get; set; }
        public Standing Standing { get; set; } = Standing.None;
    }
}
