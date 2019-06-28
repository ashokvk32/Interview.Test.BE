using System.Collections.Generic;

namespace GraduationTracker.Interfaces
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);

        IEnumerable<Student> GetStudents();
    }
}
