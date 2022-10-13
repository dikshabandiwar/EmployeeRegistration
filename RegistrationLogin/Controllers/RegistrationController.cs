using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationLogin.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace RegistrationLogin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        public List<Registration> Get()
        {
            SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Empconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("select * from Registration",cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Registration> FirstUser = new List<Registration>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Registration emp = new Registration();
                    emp.UserName = dt.Rows[i]["UserName"].ToString();
                    emp.Id = Convert.ToInt32(dt.Rows[i]["Id"]);
                    emp.Password = dt.Rows[i]["Password"].ToString();
                    emp.Email = dt.Rows[i]["Email"].ToString();
                    emp.MobileNo = Convert.ToInt32(dt.Rows[i]["MobileNo"]);
                    emp.IsActive = Convert.ToInt32(dt.Rows[i]["IsActive"]);
                    FirstUser.Add(emp);

                }
            }
            if (FirstUser.Count > 0)
            {
                return FirstUser;
            }
            else
            {
                return null;
            }
        }

       

        [HttpPost]
        [Route("registration")]
        public string registration(Registration registration)
        {
            SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Empconn").ToString());
            SqlCommand cm = new SqlCommand("Insert into Registration(UserName,Password,Email,MobileNo,IsActive)values('" + registration.UserName + "','" + registration.Password + "','" + registration.Email + "','" + registration.MobileNo + "','"+registration.IsActive+"')", cn);
            cn.Open();
            int i = cm.ExecuteNonQuery();
            cn.Close();
            if (i > 0)
            {
                return "Data Inserted";
            }
            else
            {
                return "Error";
            }


        }
        [HttpPost]
        [Route("login")]
        public string login(Registration registration)
        {
            SqlConnection cn = new SqlConnection(_configuration.GetConnectionString("Empconn").ToString());
            SqlDataAdapter da = new SqlDataAdapter("select * from Registration where Email='"+registration.Email+"' ANd Password='"+registration.Password+"' AND IsActive=1", cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                return "valid User";
            }
            else
            {
                return "Invalid User";
            }
     
        }
    }
}
