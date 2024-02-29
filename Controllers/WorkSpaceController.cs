using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;
using Attribute = CPSC319BackEnd.Models.Attribute;
using Newtonsoft.Json.Linq;



namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkSpaceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public WorkSpaceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[HttpPut]
        //public JsonResult Put(Sector sector,Attribute attr)
        //{
        //    // TODO
        //    string query = @"";
        //    string queryReuslt = @"";


        //    DataTable table = new DataTable();
        //    DataTable tableResult = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("Dbconn");
        //    SqlDataReader myReader;

        //    using (SqlConnection myConn = new SqlConnection(sqlDataSource))
        //    {
        //        myConn.Open();

        //        using (SqlCommand myCommand = new SqlCommand(query, myConn))
        //        {
        //            myCommand.Parameters.AddWithValue("@sectorID", sector.sectorID);
        //            myCommand.Parameters.AddWithValue("@attributeID", attr.attributeID);
        //            myCommand.Parameters.AddWithValue("@attributeValue", attr.attributeValue);
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myConn.Close();
        //        }

        //        // show result 
        //        using (SqlCommand myCommand = new SqlCommand(queryReuslt, myConn))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            tableResult.Load(myReader);
        //            myReader.Close();
        //            myConn.Close();
        //        }

        //    }
        //    return new JsonResult(table);


        //}


        [HttpPut]
        public JsonResult Put([FromBody] JObject data)
        {
            // TODO
            string query = @"";
            string queryReuslt = @"";
            //JArray blogPostArray = JArray.Parse(data);
            //Sector sector = data["sector"].ToObject<Sector>();
            //Attribute attr = data["attribute"].ToObject<Attribute>();

            DataTable table = new DataTable();
            DataTable tableResult = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    //myCommand.Parameters.AddWithValue("@sectorID", sector.sectorID);
                    //myCommand.Parameters.AddWithValue("@attributeID", attr.attributeID);
                    //myCommand.Parameters.AddWithValue("@attributeValue", attr.attributeValue);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

                // show result 
                using (SqlCommand myCommand = new SqlCommand(queryReuslt, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    tableResult.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

            }
            return new JsonResult(data);


        }



    }
}
