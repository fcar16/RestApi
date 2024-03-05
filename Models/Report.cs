namespace CajeroAPI.Models
{
    public class Report
    {
        private int ID;
        private String NAME_REPORT;
        private String DESCRIPTION;
        private String URL_TEMPLATE;
        public String SQL_CRITERIA;

        public Report(int iD, string nAME_REPORT, string dESCRIPTION, string uRL_TEMPLATE, string sQL_CRITERIA)
        {
            ID = iD;
            NAME_REPORT = nAME_REPORT;
            DESCRIPTION = dESCRIPTION;
            URL_TEMPLATE = uRL_TEMPLATE;
            SQL_CRITERIA = sQL_CRITERIA;
        }

        public int ID1 { get => ID; set => ID = value; }
        public string NAME_REPORT1 { get => NAME_REPORT; set => NAME_REPORT = value; }
        public string DESCRIPTION1 { get => DESCRIPTION; set => DESCRIPTION = value; }
        public string URL_TEMPLATE1 { get => URL_TEMPLATE; set => URL_TEMPLATE = value; }
        public string SQL_CRITERIA1 { get => SQL_CRITERIA; set => SQL_CRITERIA = value; }
    }
}
