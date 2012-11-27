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
    public partial class StudentRegistrationSystemMain : Form
    {
        #region Public methods

        /// <summary>
        /// This is the default Constructor
        /// </summary>
        public StudentRegistrationSystemMain()
        {
            InitializeComponent();
        }

        #endregion Public methods


        #region Private Methods

        /// <summary>
        /// Used to Close the Main Application 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Used to show the 'StudentDetails' Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StudentDetails StudDetails = new StudentDetails();
            StudDetails.MdiParent = this;
            StudDetails.Show();
        }        

        private void StudentRegistrationSystemMain_Load(object sender, EventArgs e)
        {

        }

         #endregion Private Methods
    }
}
