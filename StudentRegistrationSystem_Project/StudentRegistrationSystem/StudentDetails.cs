using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace StudentRegistrationSystem
{
    public partial class StudentDetails : Form
    {

        #region Fields

        /// <summary>
        /// Create an object of a DataBaseConnect class
        /// </summary>
        private DataBaseConnect dataBaseConnect;

        /// <summary>
        /// Used to store Current Student Number, which is taken from Database
        /// </summary>
        private int currentStudentID;

        /// <summary>
        /// Used to send bulk of Student details, as a xml file
        /// </summary>
        private string studentDetailCollectionXML;

        /// <summary>
        /// Used to find the next dataGrid Row number
        /// </summary>
        private int currentRowNumber = 0;

        #endregion Fields


        #region Public methods

        /// <summary>
        /// This is the Default Constructor
        /// </summary>
        public StudentDetails()
        {
            InitializeComponent();
            dataBaseConnect = new DataBaseConnect();
        }

        #endregion Public methods


        #region Private methods

        /// <summary>
        /// Used to Get new Student ID number from the database when the Form Loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentDetails_Load(object sender, EventArgs e)
        {
            try
            {
                //Call the SP and get the CurrentStudentID from the Actual DB Table
                currentStudentID = dataBaseConnect.RunSpGetNewStudentIdNumber("SpGetNewStudentIDNumber");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Constants.MessageCaptionStudentDetails, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Used to Open the 'StudentRegistration' Form and get new student details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Increase CurrentStudentID ID
                currentStudentID = currentStudentID + 1;

                //Popup the 'StudentRegistrationDetails' form to get data
                StudentRegistration studRegDetils = new StudentRegistration(currentStudentID);
                studRegDetils.ShowDialog(this);

                //Check Successfull Addtion of Data. If not CurrentStudentID reduce by 1
                if (studRegDetils.successStatus)
                {
                    //Add New Row in Datagrid
                    dataGridStudent.Rows.Add();

                    dataGridStudent.Rows[currentRowNumber].Cells[0].Value = studRegDetils.studentID;
                    dataGridStudent.Rows[currentRowNumber].Cells[1].Value = studRegDetils.nameOfStudent;
                    dataGridStudent.Rows[currentRowNumber].Cells[2].Value = studRegDetils.dob;
                    dataGridStudent.Rows[currentRowNumber].Cells[3].Value = studRegDetils.gpa;
                    dataGridStudent.Rows[currentRowNumber].Cells[4].Value = studRegDetils.active;

                    //Increment Datagrid Row Number
                    currentRowNumber = currentRowNumber + 1;
                }
                else
                {
                    //Decrease CurrentStudentID ID due to Cancel button clicked
                    currentStudentID = currentStudentID - 1;
                }

                //Clear and Close the Object
                studRegDetils.Dispose();
            }
            catch (Exception exception)
            {                
                MessageBox.Show(exception.Message, Constants.MessageCaptionStudentDetails, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Used to save data to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentRowNumber > 0)
                {
                    //Call the StudentDataSerializer() to generate XML data collection of Students from the data added to DataGridView
                    studentDetailCollectionXML = StudentDataSerializer();

                    //Call the SP to save data by providing Student details
                    dataBaseConnect.RunSpSaveNewStudentDetails("SpSaveNewStudentDetails", studentDetailCollectionXML);

                    MessageBox.Show(Constants.MessageTextSuccessfullySaved, Constants.MessageCaptionStudentDetails, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Clear datagrid and set to new record addition
                    dataGridStudent.DataSource = null;
                    dataGridStudent.Rows.Clear();
                    currentRowNumber = 0;
                }
                else
                {
                    MessageBox.Show(Constants.MessageTextStudentDataNotAdded, Constants.MessageCaptionStudentDetails, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Constants.MessageCaptionStudentDetails, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Generate XML to send database by using the student details in the datagridview
        /// </summary>
        /// <returns></returns>
        private string StudentDataSerializer()
        {
            MemoryStream msStudentDetails = null;
            try
            {
                string xmlStudDetails = null;

                //Create a List and add data to the list from DataGridview
                List<StudentDetailsXml> studentList = new List<StudentDetailsXml>();

                for (int indexValue = 0; indexValue < dataGridStudent.Rows.Count; indexValue++)
                {
                    if ((Convert.ToString(dataGridStudent.Rows[indexValue].Cells[0].Value)).Length != 0)
                    {
                        StudentDetailsXml stdXmlObj = new StudentDetailsXml();
                        stdXmlObj.studentId = Convert.ToInt32(dataGridStudent.Rows[indexValue].Cells[0].Value);
                        stdXmlObj.studentName = Convert.ToString(dataGridStudent.Rows[indexValue].Cells[1].Value);
                        stdXmlObj.dob = Convert.ToString(dataGridStudent.Rows[indexValue].Cells[2].Value);
                        stdXmlObj.gradePointAvg = Convert.ToDouble(dataGridStudent.Rows[indexValue].Cells[3].Value);
                        stdXmlObj.active = Convert.ToBoolean(dataGridStudent.Rows[indexValue].Cells[4].Value);
                        studentList.Add(stdXmlObj);
                    }
                    else
                    { }
                }

                //Serialize data which was added to the List by using a Memory Stream
                XmlSerializer serializerStudDetails = new XmlSerializer(typeof(List<StudentDetailsXml>));

                msStudentDetails = new MemoryStream();

                serializerStudDetails.Serialize(msStudentDetails, studentList);

                xmlStudDetails = System.Text.UTF8Encoding.UTF8.GetString(msStudentDetails.ToArray());

                msStudentDetails.Close();

                return xmlStudDetails;
            }
            catch (Exception exception)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(exception.Message);
            }
            finally
            {
                if (msStudentDetails != null)
                    msStudentDetails.Dispose();
            }
        }

        #endregion Private methods
    }
}
