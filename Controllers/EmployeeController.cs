using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;

namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        


        [HttpGet]
        public IActionResult Get()
        {
            string query = @"SELECT * FROM Employee";
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

            if (table.Rows.Count != 0)
            {
                return new JsonResult(table);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("login")]
        public IActionResult LoginChecker(LoginReq req)
        {
            string query = @"SELECT employeeID, firstName,lastName,email,password, role, employeeActive
                                FROM Employee 
                                where  email = @email and password = @employeePassword";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


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
                return new JsonResult(table);
            }
            else
            {
                return BadRequest();
            }

        }

        // EE EP 1
        [HttpGet("viewEmployeeReq/{employee_id}")]
        public IActionResult ViewEmployeeRequest(int employee_id)
        {
            string query = @"SELECT R.requestID, R.requestName, R.requestSentTime, R.requestStatus, R.resumeID, E.firstName, E.lastName
                             FROM Request R, Workspace W, Employee E
                             WHERE R.workspaceID = W.workspaceID AND W.employeeID = E.employeeID AND R.employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employee_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 2
        [HttpPut("updateRequestStatus")]
        public IActionResult UpdateRequestStatus(UpdateRequestStatusReq req)
        {
            string query = @"UPDATE Request
                             SET requestStatus = 1
                             WHERE requestID = @requestID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@requestID", req.request_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return NoContent();
            }
        }

        // EE EP 3
        [HttpGet("viewReqById/{request_id}")]
        public IActionResult ViewRequestbyID(int request_id)
        {
            string query = @"SELECT * FROM Request WHERE requestID = @requestID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@requestID", request_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 4
        [HttpGet("getResumeTemplateFromRequest/{resume_id}")]
        public IActionResult GetResumeTemplateFromRequest(int resume_id)
        {
            string query = @"SELECT ST.sectorTypeID, ST.sectorTypeName, ST.sectorTypeModifiedDate, ST.sectorTypeActive
                             FROM SectorType ST, SectorTypeTemplate STT, Resume R
                             WHERE ST.sectorTypeID = STT.sectorTypeID AND STT.templateID = R.templateID AND R.resumeID = @resumeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resume_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 5
        [HttpGet("getDupSectorsByResumeIdAndSectorType/{resume_id}/{sector_type_id}")]
        public IActionResult getDupSectorsByResumeIdAndSectorType(int resume_id, int sector_type_id)
        {
            string query = @"SELECT duplicatedSectorID, duplicatedSectorName, duplicatedSectorModifiedDate, sectorTypeID
                                FROM DuplicatedSector
                                WHERE resumeID = @resumeID AND sectorTypeID = @sectorTypeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resume_id);
                    myCommand.Parameters.AddWithValue("@sectorTypeID", sector_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

                return new JsonResult(table);

                /*
                if (table.Rows.Count != 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return NotFound();
                }
                */
            }
        }

        // EE EP 5a
        [HttpGet("getDupSectorsByResumeId/{resume_id}")]
        public IActionResult getDupSectorsByResumeId(int resume_id)
        {
            string query = @"SELECT duplicatedSectorID, duplicatedSectorName, duplicatedSectorModifiedDate, sectorTypeID
                                FROM DuplicatedSector
                                WHERE resumeID = @resumeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resume_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

                return new JsonResult(table);

                /*
                if (table.Rows.Count != 0)
                {
                    return new JsonResult(table);
                }
                else
                {
                    return NotFound();
                }
                */
            }
        }

        //EE EP 6
        [HttpPut("editDuplicateSector")]
        public IActionResult EditDuplicateSector(EditDuplicateSectorReq req)
        {
            string query = @"UPDATE DuplicatedSector
                             SET duplicatedSectorName = @duplicatedSectorName, duplicatedSectorModifiedDate = GETDATE()
                             WHERE duplicatedSectorID = @duplicatedSectorID";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@duplicatedSectorName", req.dup_sector_name);
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", req.dup_sector_id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }


        // EE EP 7:
        [HttpDelete("deleteDuplicateSector/{sector_id}")]
        public IActionResult DeleteDuplicateSector(int sector_id)
        {
            string query = @"DELETE FROM DuplicatedSector WHERE duplicatedSectorID = @duplicatedSectorID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return NoContent();

            }
        }


        // EE EP 8
        [HttpGet("getSectorByTypeId/{employee_id}/{sector_type_id}")]
        public IActionResult GetSectorByTypeId(int employee_id, int sector_type_id)
        {
            string query = @"SELECT * FROM Sector WHERE employeeID = @employeeID AND sectorTypeID = @sectorTypeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employee_id);
                    myCommand.Parameters.AddWithValue("@sectorTypeID", sector_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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



        // EE EP 9a
        [HttpPost("addDupSector")]
        public IActionResult AddDupSector(AddDupSectorReq req)
        {
            string query = @"INSERT INTO DuplicatedSector(duplicatedSectorName, duplicatedSectorModifiedDate, resumeID, sectorTypeID)
                             (SELECT sectorName, GETDATE(), @resumeID, sectorTypeID FROM Sector WHERE sectorID = @sectorID)";
            string query2 = @"SELECT MAX(duplicatedSectorID) AS duplicatedSectorID FROM DuplicatedSector";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", req.resume_id);
                    myCommand.Parameters.AddWithValue("@sectorID", req.sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                using (SqlCommand myCommand = new SqlCommand(query2, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return new JsonResult(table);
            }
        }


        // EE EP 9b
        [HttpPost("addAttrToDupSector")]
        public IActionResult AddAttrToDupSector(AddAttrToDupSectorReq req)
        {
            string query = @"INSERT INTO Attribute(attributeValue, attributeTypeID, duplicatedSectorID)
                            (SELECT attributeValue, attributeTypeID, @duplicatedSectorID FROM Attribute WHERE sectorID = @sectorID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    /*
                     @"INSERT INTO Attribute(attributeValue, attributeTypeID, duplicatedSectorID)
                            (SELECT attributeValue, attributeTypeID, " + req.dup_sector_id + " FROM Attribute WHERE sectorID = @sectorID)"
                     */
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", req.dup_sector_id);
                    myCommand.Parameters.AddWithValue("@sectorID", req.sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return NoContent();
            }
        }

        // EE EP 10
        [HttpGet("getDupSectorById/{dup_sector_id}")]
        public IActionResult GetgetDupSectorById(int dup_sector_id)
        {
            string query = @"SELECT A.attributeID, A.attributeValue, A.attributeTypeID, AT.attributeTypeName, AT.required
                             FROM Attribute A, AttributeType AT
                             WHERE A.attributeTypeID = AT.attributeTypeID AND A.duplicatedSectorID = @duplicatedSectorID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", dup_sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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



        // EE EP 11
        [HttpPut("editAttributesOfDupSector")]
        public IActionResult EditAttributesOfDupSector(EditAttributesOfDupSectorReq req)
        {
            string query = @"UPDATE Attribute
                             SET attributeValue = @attributeValue
                             WHERE attributeID = @attributeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                for (int i = 0; i < req.attr.Count; i++)
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@attributeValue", req.attr[i].attributeValue);
                        myCommand.Parameters.AddWithValue("@attributeID", req.attr[i].attributeID);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
                return NoContent();
            }
        }

        // EE EP 12 
        [HttpGet("getSectorByEmployeeId/{employee_id}")]
        public IActionResult GetSectorByEmployeeId(int employee_id)
        {
            string query = @"SELECT S.sectorID, S.sectorName, S.sectorModifiedDate, S.sectorTypeID, ST.sectorTypeName
                                FROM Sector S, SectorType ST 
                                WHERE S.sectorTypeID = ST.sectorTypeID AND S.employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employee_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 13
        [HttpPut("editSectorName")]
        public IActionResult EditSectorName(EditSectorNameReq req)
        {
            string query = @"UPDATE Sector
                             SET sectorName = @sectorName, sectorModifiedDate = GETDATE()
                                 WHERE sectorID = @sectorID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorName", req.sector_name);
                    myCommand.Parameters.AddWithValue("@sectorID", req.sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return NoContent();
            }
        }

        // EE EP 14
        [HttpDelete("deleteSector/{sector_id}")]
        public IActionResult DeleteSector(int sector_id)
        {
            string query = @"DELETE FROM Sector WHERE sectorID = @sectorID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorID", sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return NoContent();
            }
        }

        // EE EP 15
        [HttpPost("addSectorToWorkspace")]
        public IActionResult AddSectorToWorkspace(AddSectorToWorkspaceReq req)
        {
            string query = @"INSERT INTO Sector(sectorName, sectorModifiedDate, employeeID, sectorTypeID)
                             VALUES(@sectorName, GETDATE(), @employeeID, @sectorTypeID);";
            string query2 = @"SELECT MAX(sectorID) AS sectorID FROM Sector";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorTypeID", req.sec_type_id);
                    myCommand.Parameters.AddWithValue("@employeeID", req.employee_id);
                    myCommand.Parameters.AddWithValue("@sectorName", req.sector_name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                using (SqlCommand myCommand = new SqlCommand(query2, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return new JsonResult(table);
            }
        }

        // EE EP 16
        [HttpGet("getAttrFromSector/{sector_id}")]
        public IActionResult GetAttrFromSector(int sector_id)
        {
            string query = @"SELECT A.attributeID, A.attributeValue, A.attributeTypeID, AT.attributeTypeName, AT.required FROM Attribute A, AttributeType AT
                                WHERE A.attributeTypeID = AT.attributeTypeID AND sectorID = @sectorID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorID", sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 17
        [HttpPut("editAttributesOfSector")]
        public IActionResult EditAttributesOfSector(EditAttributesOfSectorReq req)
        {
            string query = @"UPDATE Attribute
                             SET attributeValue = @attributeValue
                             WHERE attributeID = @attributeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                for (int i = 0; i < req.attr.Count; i++)
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@attributeValue", req.attr[i].attributeValue);
                        myCommand.Parameters.AddWithValue("@attributeID", req.attr[i].attributeID);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
                return NoContent();
            }
        }


        // EE EP 18
        [HttpGet("getAttributeTypeIdFromSectorType/{sec_type_id}")]
        public IActionResult GetAttributeTypeIdFromSectorType(int sec_type_id)
        {
            string query = @"SELECT attributeTypeID, attributeTypeName, required
                                FROM AttributeType
                                WHERE sectorTypeID = @sectorTypeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorTypeID", sec_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
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

        // EE EP 19
        [HttpPost("createDefaultAttrOfSector")]
        public IActionResult CreateDefaultAttrOfSector(CreateDefaultAttrOfSectorReq req)
        {
            string query = @"INSERT INTO Attribute(attributeValue, attributeTypeID, sectorID)
                                VALUES('', @attributeTypeID, @sectorID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                for (int i = 0; i < req.attr_type_id.Count; i++)
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@sectorID", req.sector_id);
                        myCommand.Parameters.AddWithValue("@attributeTypeID", req.attr_type_id[i]);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
                return NoContent();
            }
        }




    }
}
