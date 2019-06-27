using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationTracker.Interfaces
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);

        IEnumerable<Student> GetStudents();
    }
}
