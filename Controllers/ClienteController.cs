﻿using System.Net.NetworkInformation;
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
        public dynamic getReports()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value;
            LoginModel loginModel = new LoginModel();
            String query1 = "SELECT PROFILE_ID FROM USERS WHERE USERNAME = '"+ username + "'"; // Modified query to include single quotes
            Console.WriteLine(username);
            ReportDAO reportDAO = new ReportDAO();
            
            int id = reportDAO.GetProfileId(query1);
            String query = "SELECT * FROM REPORT WHERE ID IN (SELECT REPORT_ID FROM PROFILE_RERPOT WHERE PROFILE_ID =  " +  id + ")";
            dynamic report = new ReportDAO().GetReportsddbb(query);

            return report;
        }
    }
}