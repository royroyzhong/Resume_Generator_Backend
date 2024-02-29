using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;
using CPSC319BackEnd.Helper;

namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("login")]
        public IActionResult LoginChecker(LoginReq req)
        {
            string query = @"SELECT employeeID, firstName, lastName, email, password, role, employeeActive
                                FROM Employee 
                                where  email = @email and password = @employeePassword";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            JwtService _jtwService = new JwtService();

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@email", req.email);
                    myCommand.Parameters.AddWithValue("@employeePassword", req.password);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            if (table.Rows.Count != 0)
            {
                int id = (int)table.Rows[0]["employeeID"];
                var jwt = _jtwService.Generate(id);
                Response.Cookies.Append("jwt", jwt, new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    Secure = true,
                    HttpOnly = true
                });


                var jwt1 = Request.Cookies["jwt"];
                var token = _jtwService.Verify(jwt);
                var userId = int.Parse(token.Issuer);


                DataTable dt = table.Copy();
                dt.AcceptChanges();
                // dt.Columns.Remove("employeeID");
                System.Data.DataColumn dataColumn = new System.Data.DataColumn("jwt", typeof(System.String));
                dataColumn.DefaultValue = jwt;
                dt.Columns.Add(dataColumn);
                return new JsonResult(dt);
            }
            else
            {
                return BadRequest();
            }

        }



        [HttpPost]
        public IActionResult getEmployeByJwt(JwtReq req)
        {
            JwtService _jtwService = new JwtService();
            //var jwt1 = Request.Cookies["jwt"];
            var token = _jtwService.Verify(req.jwt);
            int userId = int.Parse(token.Issuer);
            string query = @"SELECT * FROM Employee where employeeID = " + userId;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@jwt", req.jwt);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            if (table.Rows.Count != 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
