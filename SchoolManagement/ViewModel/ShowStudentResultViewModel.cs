using ServicesLib.Domain.Models;
using System.Collections.Generic;

namespace SchoolManagement.ViewModel
{
    public class ShowStudentResultViewModel
    {
        public ShowStudentResultViewModel()
        {
            listOfAssessmentResult = new List<AssessmentResult>();
        }
        public string StudentName { get; set; }
        public string Programme { get; set; }
        public  byte[] StudentImage { get; set; }

        public List<AssessmentResult> listOfAssessmentResult;

    }
}
