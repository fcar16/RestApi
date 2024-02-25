using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using CajeroAPI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Oracle.ManagedDataAccess.Client;
using System;
using CajeroAPI.Controllers;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims; 



namespace CajeroAPI.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("Get Reports")]
        public dynamic getReports(String name)
        {
           
            ReportDAO reportDAO = new ReportDAO();
           int id = reportDAO.GetProfileId("SELECT PROFILE_ID FROM USERS WHERE USERNAME = '" + name + "'");
           Console.WriteLine("ID: " + id);
            String query = "SELECT * FROM REPORT WHERE ID IN (SELECT REPORT_ID FROM PROFILE_RERPOT WHERE PROFILE_ID =  " + id + ")";
            dynamic report = new ReportDAO().GetReportsddbb(query);

            return report;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("Get ReportCrit")]
        public dynamic getReportsCrit(int id)
        {
            
            String query1 = "SELECT * FROM REPORT R JOIN REPORT_CRITERIA RC ON R.ID = RC.ID_REPORT WHERE R.ID =" + id ;
            dynamic report = new ReportDAO().GetReportCritddbb(query1);

            return report;
        }
        [HttpGet()]
[Microsoft.AspNetCore.Mvc.Route("Get Report/{id}")]
public dynamic getReport(int id)
{
    ReportDAO reportDAO = new ReportDAO();
    String query = "SELECT * FROM REPORT WHERE ID = " + id;
    dynamic report = reportDAO.GetReportsddbb(query);

    return report;
}
    }

    

    
}