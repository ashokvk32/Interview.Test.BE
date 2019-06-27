using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Interfaces;
using GraduationTracker.Repository;

namespace GraduationTracker.Tests.Unit
{
    [TestClass]
    public class GraduationTrackerTests
    {

        private Diploma _diploma;
        private List<Student> _students;

        private IRequirementRepository _requirementRepository;

        [TestInitialize]
        public void InitializeTest()
        {
            // Ideally we would use a mocking framework like Moq to mock this object and have our implementation to test.
            // This is because the repository generally connects to an external source like a database and testing the database connectivity
            // is not part of a unit test.
            _requirementRepository = new RequirementRepository();

            _diploma = new Diploma
            {
                Id = 1,
                Credits = 4,
                Requirements = new List<int> {100, 102, 103, 104}
            };

            _students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    Courses = new List<Course>
                    {
                        new Course {Id = 1, Name = "Math", Mark = 95},
                        new Course {Id = 2, Name = "Science", Mark = 95},
                        new Course {Id = 3, Name = "Literature", Mark = 95},
                        new Course {Id = 4, Name = "Physical Education", Mark = 95}
                    }
                },
                new Student
                {
                    Id = 2,
                    Courses = new List<Course>
                    {
                        new Course {Id = 1, Name = "Math", Mark = 80},
                        new Course {Id = 2, Name = "Science", Mark = 80},
                        new Course {Id = 3, Name = "Literature", Mark = 80},
                        new Course {Id = 4, Name = "Physical Education", Mark = 80}
                    }
                },
                new Student
                {
                    Id = 3,
                    Courses = new List<Course>
                    {
                        new Course {Id = 1, Name = "Math", Mark = 50},
                        new Course {Id = 2, Name = "Science", Mark = 50},
                        new Course {Id = 3, Name = "Literature", Mark = 50},
                        new Course {Id = 4, Name = "Physical Education", Mark = 50}
                    }
                },
                new Student
                {
                    Id = 4,
                    Courses = new List<Course>
                    {
                        new Course {Id = 1, Name = "Math", Mark = 40},
                        new Course {Id = 2, Name = "Science", Mark = 40},
                        new Course {Id = 3, Name = "Literature", Mark = 40},
                        new Course {Id = 4, Name = "Physical Education", Mark = 40}
                    }
                }
            };
        }



        [TestMethod]
        public void TestHasCredits()
        {
            var tracker = new GraduationTracker(_requirementRepository);

            var graduated = new List<StudentStatus>();

            foreach(var student in _students)
            {
                graduated.Add(tracker.HasGraduated(_diploma, student));      
            }

            // Changing it to true makes the test pass
            // This test just checks that the HasGraduated method executed successfully (without throwing an exception) for the input
            // There is no check related to the credits as the test name alludes to
            Assert.IsTrue(graduated.Any());

            // Checks if any of the students has enough credits to graduate
            Assert.IsTrue(graduated.Any(x => x.HasGraduated));
        }

        [TestMethod]
        public void TestHasGraduated_WhenStudentHasInsufficentCredits()
        {
            var tracker = new GraduationTracker(_requirementRepository);
            var student = new Student
            {
                Id = 1,
                Courses = new List<Course>
                {
                    new Course {Id = 1, Name = "Math", Mark = 95},
                    new Course {Id = 2, Name = "Science", Mark = 95},
                    new Course {Id = 3, Name = "Literature", Mark = 95}
                }
            };

            var result = tracker.HasGraduated(_diploma, student);

            Assert.IsFalse(result.HasGraduated);
            Assert.AreEqual(Standing.SumaCumLaude, result.CurrentStanding);
        }

        [TestMethod]
        public void TestHasGraduated_WhenStudentHasPoorStanding()
        {
            var tracker = new GraduationTracker(_requirementRepository);
            var student = new Student
            {
                Id = 1,
                Courses = new List<Course>
                {
                    new Course {Id = 1, Name = "Math", Mark = 40},
                    new Course {Id = 2, Name = "Science", Mark = 40},
                    new Course {Id = 3, Name = "Literature", Mark = 40},
                    new Course {Id = 4, Name = "Physical Education", Mark = 40}
                }
            };

            var result = tracker.HasGraduated(_diploma, student);

            Assert.IsFalse(result.HasGraduated);
            Assert.AreEqual(Standing.Remedial, result.CurrentStanding);
        }

        [TestMethod]
        public void TestHasGraduated_WhenStudentHasGraduateWithAvgStanding()
        {
            var tracker = new GraduationTracker(_requirementRepository);
            var student = new Student
            {
                Id = 1,
                Courses = new List<Course>
                {
                    new Course {Id = 1, Name = "Math", Mark = 75},
                    new Course {Id = 2, Name = "Science", Mark = 75},
                    new Course {Id = 3, Name = "Literature", Mark = 75},
                    new Course {Id = 4, Name = "Physical Education", Mark = 75}
                }
            };

            var result = tracker.HasGraduated(_diploma, student);

            Assert.IsTrue(result.HasGraduated);
            Assert.AreEqual(Standing.Average, result.CurrentStanding);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestHasGraduatedWithNullInput()
        {
            var tracker = new GraduationTracker(_requirementRepository);
            tracker.HasGraduated(null, _students[1]);
        }

        [TestMethod]
        public void TestGetRequirementsWhenDiplomaIsNull()
        {
            var tracker = new GraduationTracker(_requirementRepository);

            var diploma = new Diploma
            {
                Id = 1,
                Credits = 5
            };

            var result = tracker.GetRequirementsForDiploma(diploma);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetRequirementsForDiploma()
        {
            var tracker = new GraduationTracker(_requirementRepository);

            var result = tracker.GetRequirementsForDiploma(_diploma);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void TestStudentGradeWhenRequirementHasNoCourses()
        {
            var tracker = new GraduationTracker(_requirementRepository);

            var requirement = new Requirement
            {
                Courses = new List<int>()
            };
            

            tracker.GetStudentGradeForRequirement(_students[0], requirement, out int marks, out int credits);

            Assert.AreEqual(0, marks);
            Assert.AreEqual(0, credits);
        }

        [TestMethod]
        public void TestStudentGradeForRequirement()
        {
            var tracker = new GraduationTracker(_requirementRepository);

            var requirement = new Requirement
            {
                Courses = new List<int> { 1, 2, 5},
                Credits = 1,
                MinimumMark = 50
            };


            tracker.GetStudentGradeForRequirement(_students[0], requirement, out int marks, out int credits);

            Assert.AreEqual(190, marks);
            Assert.AreEqual(2, credits);
        }


    }
}
