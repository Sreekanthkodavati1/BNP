using BnPBaseFramework.Web;
using System;
using System.Configuration;
using System.Linq;

namespace Bnp.Core.WebPages.Models
{
    public class DB
    {


        public string Name
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "DBName");

            }
        }


        public string DataBasesType
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "DataBasesType");

            }
        }
        public string UserID
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "DBUserID");

            }
        }
        public string Password
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "DBPassword");

            }
        }
        public string DefaultSchema
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "DBDefaultSchema");

            }
        }
        public string Server {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "Server");

            }
        }
        public string Database
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "Database");

            }
        }
        public string ConnectionProps
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "ConnectionProps");

            }
        }
        public string Version
        {
            get
            {
                return ProjectBasePage.GetNavigatorNodesInfo("DBSettings", "Version");

            }
        }
    }

}

    
    
    
    

