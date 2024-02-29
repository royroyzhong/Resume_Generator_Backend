using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;

namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ResumeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT * FROM Resume";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        //Add data to db for test
        [HttpPost]
        public string Post(Resume resume)
        {
            string query = @"insert into Resume 
            (resumeName,employeeID,projectID,templateID) 
            VALUES
            (@resumeName,@employeeID,@projectID,@templateID)";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeName", resume.resumeName);

                    myCommand.Parameters.AddWithValue("@employeeID", resume.employeeID);
                    myCommand.Parameters.AddWithValue("@projectID", resume.projectID);
                    myCommand.Parameters.AddWithValue("@templateID ", resume.templateID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return "Added sucessfully";
        }
    }
}
