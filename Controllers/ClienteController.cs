using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using CajeroAPI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;


using System;
using CajeroAPI.Controllers;
using System.Data.Common;

using System.Security.Claims;
using System.Runtime.InteropServices;



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

        [HttpPost()]
        [Microsoft.AspNetCore.Mvc.Route("repot/detail/{id}")]
        public dynamic calculateResultReport(int id)
        {
            ReportDAO reportDAO = new ReportDAO();
            String query = "SELECT * FROM REPORT WHERE ID = " + id;
            dynamic report = reportDAO.GetReportsddbb(query);

            String sqlToRun = report[0].SQL_CRITERIA1;
            dynamic data = reportDAO.GetReportResult(sqlToRun);
            dynamic result = new System.Dynamic.ExpandoObject();
            result.data = data;
            result.name = report[0].NAME_REPORT1;
            result.description = report[0].DESCRIPTION1;
            result.url_template = report[0].URL_TEMPLATE1;
            return result;
        }



    }

    

    
}