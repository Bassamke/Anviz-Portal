using AnvizWeb.Models;
using System.Data;
using System.Data.SqlClient;

namespace AnvizWeb.Repository
{
    public class AnvizRepository
    {
        private readonly string conString;

        public AnvizRepository(IConfiguration configuration)
        {
            conString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddUser(Login user)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "INSERT INTO Users (Email, Password, FirstLogin) VALUES (@Email, @Password, 1)";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);

            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public async Task<string> SignInUser(Login user)
        {
            string message = "";
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select * from users where Email=@email AND Password=@password";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            using var reader = command.ExecuteReader();
            if (await reader.ReadAsync())
            {
                bool firstLogin = reader.GetBoolean("FirstLogin");
                message= firstLogin? "FIRSTLOGIN": "SUCCESS";
                return message;
;
            }
            return message;
        }
        public void UpdateUserPassword(ChangePassword user)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "UPDATE Users SET Password=@Password, FirstLogin=0 where Email=@Email and Password=@Oldpassword";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Oldpassword", user.OldPassword);
            command.Parameters.AddWithValue("@Email", user.Email);


            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public async Task<IEnumerable<BiometricDevices>> GetAllBiometricDevicesAsync()
        {
            List<BiometricDevices> devices = new List<BiometricDevices>();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select * from BiometricDevices";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var device = new BiometricDevices
                {
                    Id= reader.GetInt32("Id"),
                    IPAddress = reader["IPAddress"].ToString(),
                    Location = reader["Location"].ToString(),
                    LastFailDateTime=reader.GetDateTime("LastFailDateTime")
                };
                devices.Add(device);
            }
            return devices;
        }
        public async Task<BiometricDevices> GetBiometricDeviceAsync(int Id)
        {
            BiometricDevices device = new BiometricDevices();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select * from BiometricDevices where Id=@id";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", Id);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                device = new BiometricDevices
                {
                    Id = reader.GetInt32("Id"),
                    IPAddress = reader["IPAddress"].ToString(),
                    Location = reader["Location"].ToString()
                };
            }
            return device;
        }
        public void UpdateDevice(BiometricDevices device)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "UPDATE BiometricDevices SET IPAddress=@IPAddress, LOcation=@Location where Id=@Id";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@IPAddress", device.IPAddress);
            command.Parameters.AddWithValue("@Location", device.Location);
            command.Parameters.AddWithValue("@Id", device.Id);


            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public void AddDevice(BiometricDevices device)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "INSERT INTO BiometricDevices (IPAddress, Location) VALUES (@IPAddress, @Location)";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@IPAddress", device.IPAddress);
            command.Parameters.AddWithValue("@Location", device.Location);

            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public void DeleteDevice(BiometricDevices device)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "Delete from BiometricDevices where Id=@Id";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Id", device.Id);

            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public async Task<EmailSettings> getEmailSettings()
        {
            EmailSettings emailSettings = new EmailSettings();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select * from EmailSettings";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                emailSettings = new EmailSettings
                {
                    Id = reader.GetInt32("Id"),
                    Emailaccount = reader["Emailaccount"].ToString(),
                    Smtp = reader["Smtp"].ToString(),
                    Port = reader.GetInt32("Port"),
                    Password = reader["Password"].ToString(),
                    ReceiverEmail = reader["ReceiverEmail"].ToString(),
                    UseSSL = reader.GetBoolean("UseSSL")
                };
            }
            return emailSettings;
        }
        public void UpdateEmailSettings(EmailSettings emailSettings)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "UPDATE EmailSettings SET Emailaccount=@Emailaccount, Smtp=@Smtp, Port=@Port, Password=@Password, ReceiverEmail=@ReceiverEmail, UseSSL=@UseSSL where Id=@Id";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Emailaccount", emailSettings.Emailaccount);
            command.Parameters.AddWithValue("@Smtp", emailSettings.Smtp);
            command.Parameters.AddWithValue("@Port", emailSettings.Port);
            command.Parameters.AddWithValue("@Password", emailSettings.Password);
            command.Parameters.AddWithValue("@ReceiverEmail", emailSettings.ReceiverEmail);
            command.Parameters.AddWithValue("@UseSSL", emailSettings.UseSSL);
            command.Parameters.AddWithValue("@Id", emailSettings.Id);


            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public async Task<IEnumerable<EmployeePunches>> getAllEmployeesPunches()
        {
            List<EmployeePunches> employeePunches = new List<EmployeePunches>();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select top 100 * from EmployeePunches order by 1 desc";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var employeePunch = new EmployeePunches
                {
                    Id = reader.GetInt32("Id"),
                    RawPunchTime = reader.GetDateTime("RawPunchTime"),
                    PunchType = reader["PunchType"].ToString(),
                    PunchDevice = reader["PunchDevice"].ToString(),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    DayForceResponse = reader["DayForceResponse"].ToString(),
                    UploadedToDayforce = reader.GetBoolean("UploadedToDayforce")
                };
                employeePunches.Add(employeePunch);
            }
            return employeePunches;
        }
        public async Task<IEnumerable<EmployeePunches>> getPunchesByEmployee(string EmployeeNumber)
        {
            List<EmployeePunches> employeePunches = new List<EmployeePunches>();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select top 1000 * from EmployeePunches where EmployeeNumber=@employeenumber";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeenumber", EmployeeNumber);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var employeePunch = new EmployeePunches
                {
                    Id = reader.GetInt32("Id"),
                    RawPunchTime = reader.GetDateTime("RawPunchTime"),
                    PunchType = reader["PunchType"].ToString(),
                    PunchDevice = reader["PunchDevice"].ToString(),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    DayForceResponse = reader["DayForceResponse"].ToString(),
                    UploadedToDayforce = reader.GetBoolean("UploadedToDayforce")
                };
                employeePunches.Add(employeePunch);
            }
            return employeePunches;
        }
        public async Task<IEnumerable<EmployeeData>> getEmployees()
        {
            List<EmployeeData> employees = new List<EmployeeData>();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select top 100 * from EmployeeData order by 1 desc";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            
            {
                var employee = new EmployeeData
                {
                    Id = reader.GetInt32("Id"),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    DisplayName = reader["DisplayName"].ToString(),
                    Location = reader["Location"].ToString(),
                    Uploaded = (reader["Uploaded"])== DBNull.Value?false:Convert.ToBoolean(reader["Uploaded"]),
                    Active = (reader["Active"]) == DBNull.Value ? false:Convert.ToBoolean(reader["Active"]),
                    OldLocation = reader["OldLocation"].ToString(),
                    DeletedFromOldLocation = (reader["DeletedFromOldLocation"]) == DBNull.Value ? false:Convert.ToBoolean(reader["DeletedFromOldLocation"]),
                };
                employees.Add(employee);
            }
            return employees;
        }
        public async Task<EmployeeData> getEmployeeByNumber(string EmployeeNumber)
        {
            EmployeeData employee = new EmployeeData();
            using var connection = new SqlConnection(conString);
            connection.Open();
            var sql = "Select top 100 * from EmployeeData Where EmployeeNumber=@employeenumber order by 1 desc";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeenumber", EmployeeNumber);
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                employee = new EmployeeData
                {
                    Id = reader.GetInt32("Id"),
                    EmployeeNumber = reader["EmployeeNumber"].ToString(),
                    DisplayName = reader["DisplayName"].ToString(),
                    Location = reader["Location"].ToString(),
                    Uploaded = (reader["Uploaded"]) == DBNull.Value ? false : Convert.ToBoolean(reader["Uploaded"]),
                    Active = (reader["Active"]) == DBNull.Value ? false : Convert.ToBoolean(reader["Active"]),
                    OldLocation = reader["OldLocation"].ToString(),
                    DeletedFromOldLocation = (reader["DeletedFromOldLocation"]) == DBNull.Value ? false : Convert.ToBoolean(reader["DeletedFromOldLocation"]),
                };
            }
            return employee;
        }
        public void UpdateEmployeeStatus(EmployeeData employee)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "UPDATE EmployeeData SET Uploaded=@Uploaded, Active=@Active where EmployeeNumber=@EmployeeNumber";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@EmployeeNumber", employee.EmployeeNumber);
            command.Parameters.AddWithValue("@Active", employee.Active);
            command.Parameters.AddWithValue("@Uploaded", employee.Uploaded);


            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
        public void LogActivity(string username, string IPAdress, string activity)
        {
            var connection = new SqlConnection(conString);

            connection.Open();

            var sql = "INSERT INTO Logs (UserName, IPAddress,Activity, Time) VALUES (@UserName, @IPAddress, @Activity, @Time)";
            var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@UserName", username);
            command.Parameters.AddWithValue("@IPAddress",IPAdress);
            command.Parameters.AddWithValue("@Activity", activity);
            command.Parameters.AddWithValue("@Time", DateTime.Now);

            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }
    }
}
