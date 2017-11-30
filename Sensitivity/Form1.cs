using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sensitivity
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void RunModel_Click(object sender, EventArgs e)
        {
            Run();
        }

        [STAThread]
        static void Run()
        {
            Sensitivity.InternalModelSensitivity ms;
            try
            {
                CreateDirectory(TempDirectoryName);
                ms = new InternalModelSensitivity(DataDirectoryName, TempDirectoryName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        #region Website directory faking
        private static string DataDirectoryName
        {
            get
            {
                return @"App_Data\WaterSim_6_0\";
            }
        }

        private static string TempDirectoryName
        {
            set
            {
                string dir = value;
                string.Concat(@"WaterSim_Output\", dir);
            }
            get
            {
                // Make a common for testing
                return @"WaterSim_Output\";
                // Make the temp directory name unique for each access to avoid client clashes
                //return +System.Guid.NewGuid().ToString() + @"\";
            }
        }
        private static void CreateDirectory(string directoryName)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(directoryName);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }
        #endregion

        private void RunExternal_Click(object sender, EventArgs e)
        {
            External();
        }
        static void External()
        {
            Sensitivity.ExternalModelSensitivity ex;
            try
            {
                CreateDirectory(TempDirectoryName);
                ex = new ExternalModelSensitivity(DataDirectoryName, TempDirectoryName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
