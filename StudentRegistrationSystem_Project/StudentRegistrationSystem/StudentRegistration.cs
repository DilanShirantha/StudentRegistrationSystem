using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StudentRegistrationSystem
{
    /// <summary>
    /// This Class is used to Enter new Student details
    /// </summary>
    public partial class StudentRegistration : Form
    {
        #region Properties

        /// <summary>
        /// This is used to store next Student Number who is going to register and this is a readonly field
        /// </summary>
        private readonly int nextStudentID;

        /// <summary>
        /// This is used to store and Pass Student Id of the newly registering Student. this is same as nextStudentID value
        /// </summary>
        public string studentID { get; set; }

        /// <summary>
        /// This is used to store and Pass Student Name of the newly registering Student
        /// </summary>
        public string nameOfStudent { get; set; }

        /// <summary>
        /// This is used to store and Pass Student Date of Birth of the newly registering Student
        /// </summary>
        public string dob { get; set; }

        /// <summary>
        /// This is used to store and Pass whether the newly registering Student is currently enrolled or not
        /// </summary>
        public bool active { get; set; }

        /// <summary>
        /// This is used to store and Pass Student GPA of the newly registering Student
        /// </summary>
        public double gpa { get; set; }

        /// <summary>
        /// This is used to check whether the new student registering is successfully done or cancel. By using this,
        /// next Student ID generation process changed in StudentDetails form
        /// </summary>
        public bool successStatus { get; set; }

        #endregion Properties


        #region Public methods

        /// <summary>
        /// This is the default constuctor
        /// </summary>
        public StudentRegistration()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This is the constructor which used to assign nextStudentID number
        /// </summary>
        /// <param name="CurrentStudentNumber"></param>
        public StudentRegistration(int currentStudentNumber)
        {
            InitializeComponent();
            this.nextStudentID = currentStudentNumber;

            this.txtGPA.KeyPress += new KeyPressEventHandler(GPA_KeyPress); 
        }

        #endregion Public methods


        #region Private methods

        /// <summary>
        /// Used to enter StudentID number in the StudentID Textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentRegistration_Load(object sender, EventArgs e)
        {
            try
            {
                txtStudentID.Text = nextStudentID.ToString();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, Constants.MessageCaptionStudentRegistration, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Used to check typing text on GPA text box. Allow only one '.' sign and Digits 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GPA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != Constants.Dot)
            {
                e.Handled = true;
            }

            // allow only one decimal point
            if (e.KeyChar == Constants.Dot && (txtGPA.Text.IndexOf(Constants.Dot) > -1))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// When user press Cancel button, it sets the successStatus to Zero and close the form.
        /// According to the successStatus, StudentDetails form sets the next StudentId number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            successStatus = false;
            this.Close();
        }

        /// <summary>
        /// When user press Ok button, First it checks the fields and validate. Then if required 
        /// details are filled it pass to the StudentDetails form and add data to datagridvied
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFormValid())
                {
                    studentID = txtStudentID.Text.Trim();
                    nameOfStudent = txtStudentName.Text.Trim();
                    dob = dtpDateOfBirth.Text.ToString();

                    if (chkActiveStatus.Checked == true)
                    {
                        active = true;
                    }
                    else
                    {
                        active = false;
                    }

                    gpa = Math.Round(Convert.ToDouble(txtGPA.Text.Trim()), 2);

                    successStatus = true;

                    this.Close();
                }
                else
                {
                    MessageBox.Show(Constants.MessageTextEnterRequiredDetails, Constants.MessageCaptionStudentRegistration, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(exception.Message);
            }
        }
        

        /// <summary>
        /// This is used to validate the Form, whether the required fields are entered
        /// </summary>
        /// <returns></returns>
        private bool IsFormValid()
        {
            bool result = txtStudentName.Text.Trim() != string.Empty
                          && txtStudentName.Text.Length != 0
                          && txtGPA.Text.Trim() != string.Empty
                          && txtGPA.Text.Length != 0;

            return result;
        }

        #endregion Private methods
    }
}
