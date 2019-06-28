﻿using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Interfaces;

namespace GraduationTracker.Repository
{
    public class StudentRepository : IStudentRepository
    {
        public Student GetStudent(int id)
        {
            return GetStudents().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Courses = new List<Course>
                    {
                        new Course{Id = 1, Name = "Math", Mark = 95 },
                        new Course{Id = 2, Name = "Science", Mark = 95 },
                        new Course{Id = 3, Name = "Literature", Mark = 95 },
                        new Course{Id = 4, Name = "Physical Education", Mark = 95 }
                    }
                },
                new Student
                {
                    Id = 2,
                    Courses = new List<Course>
                    {
                        new Course{Id = 1, Name = "Math", Mark = 80 },
                        new Course{Id = 2, Name = "Science", Mark = 80 },
                        new Course{Id = 3, Name = "Literature", Mark = 80 },
                        new Course{Id = 4, Name = "Physical Education", Mark = 80 }
                    }
                },
                new Student
                {
                    Id = 3,
                    Courses = new List<Course>
                    {
                        new Course{Id = 1, Name = "Math", Mark = 50 },
                        new Course{Id = 2, Name = "Science", Mark = 50 },
                        new Course{Id = 3, Name = "Literature", Mark = 50 },
                        new Course{Id = 4, Name = "Physical Education", Mark = 50 }
                    }
                },
                new Student
                {
                    Id = 4,
                    Courses = new List<Course>
                    {
                        new Course{Id = 1, Name = "Math", Mark = 40 },
                        new Course{Id = 2, Name = "Science", Mark = 40 },
                        new Course{Id = 3, Name = "Literature", Mark = 40 },
                        new Course{Id = 4, Name = "Physical Education", Mark = 40 }
                    }
                }

            };
        }
    }
}
