namespace CajeroAPI.Models
{
    public class Users {
        private int id;
        private string name;
        private string password;
        private string email;
        private string realname;
        private int Surname;
        private int status;
        private DateTime Creation_date;
        private DateTime Last_Login;
        private DateTime Blocked_Date;
        private DateTime Expiration_Date;
        private int Login_Attempts;
        private int User_Type;
        private int Profile_Id;

        public Users(int id, string name, string password, string email, string realname, int surname, int status, DateTime creation_date, DateTime last_Login, DateTime blocked_Date, DateTime expiration_Date, int login_Attempts, int user_Type, int profile_Id)
        {
            this.Id = id;
            this.Name = name;
            this.Password = password;
            this.Email = email;
            this.Realname = realname;
            Surname1 = surname;
            this.Status = status;
            Creation_date1 = creation_date;
            Last_Login1 = last_Login;
            Blocked_Date1 = blocked_Date;
            Expiration_Date1 = expiration_Date;
            Login_Attempts1 = login_Attempts;
            User_Type1 = user_Type;
            Profile_Id1 = profile_Id;
        }

        public Users()
        {
            }
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public string Realname { get => realname; set => realname = value; }
        public int Surname1 { get => Surname; set => Surname = value; }
        public int Status { get => status; set => status = value; }
        public DateTime Creation_date1 { get => Creation_date; set => Creation_date = value; }
        public DateTime Last_Login1 { get => Last_Login; set => Last_Login = value; }
        public DateTime Blocked_Date1 { get => Blocked_Date; set => Blocked_Date = value; }
        public DateTime Expiration_Date1 { get => Expiration_Date; set => Expiration_Date = value; }
        public int Login_Attempts1 { get => Login_Attempts; set => Login_Attempts = value; }
        public int User_Type1 { get => User_Type; set => User_Type = value; }
        public int Profile_Id1 { get => Profile_Id; set => Profile_Id = value; }
    }
}
