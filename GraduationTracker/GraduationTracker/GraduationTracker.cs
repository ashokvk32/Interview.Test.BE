using System;
using System.Collections.Generic;
using System.Linq;
using GraduationTracker.Interfaces;

namespace GraduationTracker
{
    public class GraduationTracker
    {
        private readonly IRequirementRepository _requirementRepo;
        

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="requirementRepo"></param>
        public GraduationTracker(IRequirementRepository requirementRepo)
        {
           _requirementRepo = requirementRepo;
        }

        public StudentStatus HasGraduated(Diploma diploma, Student student)
        {
            var credits = 0;
            var marks = 0;

            if (diploma == null)
            {
                throw new Exception("Diploma cannot be null");
            }

            if (student == null)
            {
                throw new Exception("Student cannot be null");
            }

            //Refactored the logic to avoid having multiple nested loops and to put some of the logic in their own methods so that it can be unit tested
            var requirements = GetRequirementsForDiploma(diploma);
            foreach (var requirement in requirements)
            {
                GetStudentGradeForRequirement(student, requirement, out int courseMarks, out int courseCredits);
                marks += courseMarks;
                credits += courseCredits;
            }
            // make sure we don't divide by zero
            var average = student.Courses.Count > 0 ? marks / student.Courses.Count : 0;
            // The number of credits has not be used in the code. It needs to be taken into account in the hasGraduated calculation
            // Even if the student is in good standing, not having sufficient credits needed for the diploma means he/ she hasn't graduated
            var hasEnoughCreditsToGraduate = credits >= diploma.Credits;
            // Based on existing logic, an average of < 50 returned false
            var hasGoodAverageToGraduate = average < 50 ? false : true;
            // Student has graduated only if both conditions are true. Has enough credits and has average >= 50
            var hasGraduated = hasGoodAverageToGraduate && hasEnoughCreditsToGraduate;
            // Get the standing
            var standing = GetStandingForAverageMarks(average);

            // Return as before
            return new StudentStatus
            {
                HasGraduated = hasGraduated,
                CurrentStanding = standing
            };

        }

        /// <summary>
        /// Returns the standing information for a given score
        /// </summary>
        /// <param name="average">Marks value</param>
        /// <returns>Enum indicating student standing</returns>
        public Standing GetStandingForAverageMarks(int average)
        {
            // Both cases: average < 95 and >= 95 where given MagnaCumLaude. One of them should be SumaCumLaude
            // < 95 is MagnaCumLaude and >= 95 is SumaCumLaude
            // Based on https://www.academicapparel.com/caps/cum-laude.html
            if (average < 50)
            {
                return Standing.Remedial;
            }
            else if (average < 80)
            {
                return Standing.Average;
            }
            else if (average < 95)
            {
                return Standing.MagnaCumLaude;
            }
            else return Standing.SumaCumLaude;
        }

        /// <summary>
        /// Gets the list of requirements for a diploma
        /// </summary>
        /// <param name="diploma">Diploma instance</param>
        /// <returns>List of requirement instances</returns>
        public IEnumerable<Requirement> GetRequirementsForDiploma(Diploma diploma)
        {
            var requirementList = new List<Requirement>();
            if (diploma?.Requirements == null)
            {
                return requirementList;
            }
            var requirementIds = diploma.Requirements;
            foreach (var reqId in requirementIds)
            {
                var requirement = _requirementRepo.GetRequirement(reqId);
                if (requirement != null)
                {
                    requirementList.Add(requirement);
                }
            }
            return requirementList;
        }

        /// <summary>
        /// Given a student and requirements, returns the marks and credits satisfied for the requirement 
        /// </summary>
        /// <param name="student">Student information</param>
        /// <param name="requirement">Requirement data</param>
        /// <param name="marks">marks obtained in the courses for this requirement</param>
        /// <param name="credits">Number of credits gained for this requirement</param>
        public void GetStudentGradeForRequirement(Student student, Requirement requirement, out int marks, out int credits)
        {
            marks = 0;
            credits = 0;

            if (student == null || requirement == null)
            {
                return;
            }

            var coursesInReq = requirement.Courses;
            foreach (var id in coursesInReq)
            {
                var studentCourse = student.Courses.FirstOrDefault(x => x.Id == id);
                if (studentCourse != null)
                {
                    marks += studentCourse.Mark;
                    if (studentCourse.Mark > requirement.MinimumMark)
                    {
                        credits += requirement.Credits;
                    }
                }
            }
        }
    }
}
