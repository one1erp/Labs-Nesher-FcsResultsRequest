using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LSEXT;
using LSExtensionControlLib;
using LSExtensionWindowLib;
using LSSERVICEPROVIDERLib;
using System.Reflection;
using System.IO;
using System.Xml;
using DAL;
using Oracle.DataAccess.Client;
using Common;
//using MisradHabriutService;

namespace FcsResultsRequest
{
    [ComVisible(true)]
    [ProgId("FcsResultsRequest.FcsResultsRequest")]

    public partial class FcsResultsRequest : UserControl, IExtensionWindow
    {

        public IDataLayer dal = null;

        private IExtensionWindowSite2 _ntlsSite;
        private NautilusServiceProvider _sp;
        private NautilusProcessXML _processXml;
        private NautilusUser _ntlsUser;
        private INautilusDBConnection _ntlsCon;
        private OracleCommand cmd;

        public bool DEBUG;
        private U_FCS_MSG fcsmsg = null;


        public FcsResultsRequest()
        {
            InitializeComponent();
        }

        private void txtSdgId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var report_name = (txtSdgId.Text);
                COA_Report c = dal.GetCoaReportByName(report_name);
                if (c != null)
                {                    
                    sendTo_MisradHabriut(c);
                }
            }
        }

        public void sendTo_MisradHabriut(COA_Report coaReport)
        {
            try
            {
                SendToMSB SendToMSB = new SendToMSB();
                string url = dal.GetPhraseByName("UrlService_FCS").PhraseEntries.Where(p => p.PhraseName == "FcsResultRequest").FirstOrDefault().PhraseDescription;
         //////170423 zmani       SendToMSB.SendRequest(url + coaReport.COAReportId.ToString());

                
            }
            catch (Exception EXP)
            {
                MessageBox.Show(EXP.Message);
                Common.Logger.WriteLogFile(EXP.Message);
                fcsmsg.U_ERROR += EXP.Message;
                dal.SaveChanges();
            }
        }



        #region Page Extension Interface

        public void SetSite(object site)
        {
            _ntlsSite = (IExtensionWindowSite2)site;
            _ntlsSite.SetWindowInternalName("NautExtExample Event");
            _ntlsSite.SetWindowRegistryName("NautExtExample Event");
            _ntlsSite.SetWindowTitle("NautExtExample Event");
        }

        public void SetServiceProvider(object serviceProvider)
        {
            _sp = serviceProvider as NautilusServiceProvider;

        }

        public WindowButtonsType GetButtons()
        {
            return LSExtensionWindowLib.WindowButtonsType.windowButtonsNone;
        }

        public bool SaveData()
        {
            return false;
        }

        public bool CloseQuery()
        {
            return true;
        }

        public void Internationalise()
        {
            //   throw new NotImplementedException();
        }

        public void PreDisplay()
        {
            if (!DEBUG)
            {
                _processXml = _sp.QueryServiceProvider("ProcessXML") as NautilusProcessXML;
                _ntlsUser = _sp.QueryServiceProvider("User") as NautilusUser;
                _ntlsCon = _sp.QueryServiceProvider("DBConnection") as NautilusDBConnection;

                Utils.CreateConstring(_ntlsCon);
                dal = new DataLayer();
                dal.Connect();
            }
            else
            {
                _processXml = null;

                dal = new MockDataLayer();
                dal.Connect();
            }

            //  throw new NotImplementedException();
        }


        public void SetParameters(string parameters)
        {
            //  throw new NotImplementedException();
        }


        public void Setup()
        {
        }


        public WindowRefreshType DataChange()
        {
            return LSExtensionWindowLib.WindowRefreshType.windowRefreshNone;
        }

        public WindowRefreshType ViewRefresh()
        {
            return LSExtensionWindowLib.WindowRefreshType.windowRefreshNone;
        }

        public void refresh()
        {
            //    throw new NotImplementedException();
        }

        public void SaveSettings(int hKey)
        {
            //  throw new NotImplementedException();
        }

        public void RestoreSettings(int hKey)
        {
            //    throw new NotImplementedException();
        }

        public OracleConnection GetConnection(INautilusDBConnection ntlsCon)
        {

            OracleConnection connection = null;

            if (ntlsCon != null)
            {


                // Initialize variables
                String roleCommand;
                // Try/Catch block
                try
                {



                    var _connectionString = ntlsCon.GetADOConnectionString();

                    var splited = _connectionString.Split(';');

                    var cs = "";

                    for (int i = 1; i < splited.Count(); i++)
                    {
                        cs += splited[i] + ';';
                    }

                    var username = ntlsCon.GetUsername();
                    if (string.IsNullOrEmpty(username))
                    {
                        var serverDetails = ntlsCon.GetServerDetails();
                        cs = "User Id=/;Data Source=" + serverDetails + ";";
                    }


                    //Create the connection
                    connection = new OracleConnection(cs);

                    // Open the connection
                    connection.Open();

                    // Get lims user password
                    string limsUserPassword = ntlsCon.GetLimsUserPwd();

                    // Set role lims user
                    if (limsUserPassword == "")
                    {
                        // LIMS_USER is not password protected
                        roleCommand = "set role lims_user";
                    }
                    else
                    {
                        // LIMS_USER is password protected.
                        roleCommand = "set role lims_user identified by " + limsUserPassword;
                    }

                    // set the Oracle user for this connecition
                    cmd = new OracleCommand(roleCommand, connection);

                    // Try/Catch block
                    try
                    {
                        // Execute the command
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception f)
                    {
                        // Throw the exception
                        throw new Exception("Inconsistent role Security : " + f.Message);
                    }

                    // Get the session id
                    var sessionId = _ntlsCon.GetSessionId();

                    // Connect to the same session
                    string sSql = string.Format("call lims.lims_env.connect_same_session({0})", sessionId);

                    // Build the command
                    cmd = new OracleCommand(sSql, connection);

                    // Execute the command
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    // Throw the exception
                    throw e;
                }

                // Return the connection
            }

            return connection;

        }
        #endregion Page Extension Interface

        public void Execute(ref LSExtensionParameters Parameters)
        {

            INautilusServiceProvider _sp;
            OracleCommand cmd = null;
            //ReportDocument cr = null;
            //Debugger.Launch();
            try
            {
                _sp = Parameters["SERVICE_PROVIDER"];
                string tableName = Parameters["TABLE_NAME"];

                int workstation = Parameters["WORKSTATION_ID"];
                //  Debugger.Launch();
                long wnid = Parameters["WORKFLOW_NODE_ID"];
                INautilusDBConnection _ntlsCon = null;
                if (_sp != null)
                {
                    _ntlsCon = _sp.QueryServiceProvider("DBConnection") as NautilusDBConnection;
                }
                else
                {
                    _ntlsCon = null;
                }
                if (_ntlsCon != null)
                {
                    // _username= dbConnection.GetUsername();

                }

                var records = Parameters["RECORDS"];
                var recordId = records.Fields[tableName + "_ID"].Value;
            }
            catch (Exception EXP)
            {
                MessageBox.Show(EXP.Message);

            }
        }

        private void txtSdgId_TextChanged(object sender, EventArgs e)
        {

        }

        //private COA_Report getData()
        //{
        //    try
        //    {
        //        var SdgCoa_Report = dal.GetCoaReportByName(report_name);

        //        if (SdgCoa_Report != null)
        //        {
        //            var U_FCS_MSG_ID = SdgCoa_Report.Sdg.U_FCS_MSG_ID.Value;

        //            var fcsmsg = SdgCoa_Report.Sdg.U_FCS_MSG;

        //            fcsmsg.U_ERROR = null;//איפוס שגיאות







        //        }
        //        else
        //        {
        //            MessageBox.Show("לא נמצאה תעודה לדרישה זו");
        //            _error += "לא נמצאה תעודה לדרישה זו";
        //            _return = true;
        //            return null;

        //        }

        //        return SdgCoa_Report;
        //        //else
        //        //{
        //        //    MessageBox.Show("לא נמצאה בקשה ממשרד הבריאות לדרישה זו");
        //        //    _error += "לא נמצאה בקשה ממשרד הבריאות לדרישה זו";
        //        //    _return = true;
        //        //    return;

        //        //}
        //    }
        //    catch (Exception EXP)
        //    {
        //        MessageBox.Show(EXP.Message);
        //        _error += EXP.Message;
        //        _return = true;
        //        return null;

        //    }
        //}


    }
}


/*
            catch (Exception e)
            {
                //throw the exeption
                MessageBox.Show("Err At GetConnection: " + e.Message);
            }
*/
