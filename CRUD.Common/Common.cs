using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
namespace CRUD.Common
{
    public class Common
    {
        public static string GenerateToken(IConfiguration config, string userId, string userEmail, string userRole)
        {
            var sercurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sid, userId),
                    new Claim(JwtRegisteredClaimNames.Email, userEmail),
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim("Date", DateTime.Now.ToString()),
                };

            var token = new JwtSecurityToken(config["JWT:Issuer"],
                config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static void WriteToLog(string methodName, string message)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyyMMdd");

                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "LogInfo\\";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = folderPath + "Log" + currentDate + ".txt";
                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {
                        fs.Close();
                    }
                }

                using (StreamWriter writer = new StreamWriter(filePath, true, UnicodeEncoding.Default))
                {
                    writer.WriteLine("-------------------- < " + DateTime.Now + " >  --------------------");
                    writer.WriteLine(methodName + " -> " + message);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static List<T> ConvertDTintoList<T>(DataTable dt)
        {
            var columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToUpper()).ToList();
            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var prop in properties)
                {
                    if (columns.Contains(prop.Name.ToUpper()))
                    {
                        try
                        {
                            prop.SetValue(objT, row[prop.Name]);
                        }
                        catch (Exception ex)
                        {
                            WriteToLog("Common -> ConvertToList", ex.Message);
                        }
                    }
                }
                return objT;
            }).ToList();
        }
    }
}
