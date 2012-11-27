using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StudentRegistrationSystem
{
    /// <summary>
    /// This class is used to create xml string which pass to database
    /// </summary>
    [XmlRoot]
    public class StudentDetailsXml
    {
        #region Fields

        /// <summary>
        /// This variable holds the new ID number of the student 
        /// </summary>
        public int studentId;

        /// <summary>
        /// This variable holds the student name
        /// </summary>
        public string studentName;

        /// <summary>
        /// This variable holds the student's Date of Birth
        /// </summary>
        public string dob;

        /// <summary>
        /// This variable holds the Grade Point Average of the student
        /// </summary>
        public double gradePointAvg;
        
        /// <summary>
        /// This varible holds whether the student is currently enrolled or not
        /// </summary>
        public bool active;

        #endregion Fields
    }
}
