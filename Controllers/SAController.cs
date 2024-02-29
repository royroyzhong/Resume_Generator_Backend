using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;
using Attribute = CPSC319BackEnd.Models.Attribute;

/*
 Backend developers, please read the comments of the queries and adjust the detail of the C# code accordingly 
 to make sure that the endpoint works appropriately in every detail. 
 */

namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SAController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SAController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        // SA EP1
        // Search test, filter
        [HttpGet("searchEmployee/{filter}/{value}")]
        public IActionResult FilterEmp(string filter, string value)
        {
            /*
             * employeeID is INT in the DB, but should be returned as String. Type conversion may be needed.
             * All other columns have type VARCHAR and should be returned as String, so no need to convert type.
             * @filter is a String equal to one of the column name of the Employee table.
             * @value is a String indicating the value of @filter to search.
             * Because @filter is never "employeeID" and all columns of the Employee table are of type VARCHAR,
             *      there is no need to worry about type conversion of @value.
             */
            //string query = @"SELECT employeeID, firstName, lastName, email, role
            //                 FROM Employee
            //                 WHERE @filter = @value";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand("SELECT employeeID, firstName, lastName, email, role FROM Employee WHERE " + filter + " LIKE '%" + value +"%'", myConn))
                {
                    // myCommand.Parameters.AddWithValue("@filter", filter);
                    // myCommand.Parameters.AddWithValue("@value", value);
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
                    // return new JsonResult(query);
                }
            }
        }

        // SA EP 2
        [HttpGet("searchEmployeeById/{employee_id}")]
        public IActionResult SearchEmp(int employee_id)
        {
            /*
             * employeeID in the query result has DB type INT, but should be returned as String. Type conversion
             *      may be needed.
             * All other columns have type VARCHAR and should be returned as String, so no need to convert type.
             * @employee_id is a String, but the employeeID of the Employee table is INT. Type conversion may be 
             *      needed.
             */
            string query = @"SELECT * FROM Employee 
                             WHERE employeeID  = @employee_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employee_id", employee_id);
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

        // SA EP 3
        [HttpPut("assignRoleById")]
        public JsonResult AssignRole(AssignRoleReq req)
        {
            /*
             * @role is a String specifying the new role of the Employee.
             * @employee_id is a String, but the employeeID of the Employee table is INT. Type conversion may be 
             *      needed.
             */
            string query = @"UPDATE Employee
                             SET role = @role
                             WHERE employeeID = @employee_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employee_id", req.employee_id);
                    myCommand.Parameters.AddWithValue("@role", req.role_name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        // SA EP 4
        [HttpGet("view_sector")]
        public IActionResult ViewAllSectorType()
        {   
            /*
             * sectorTypeID in the query result has type INT in the DB, but should be returned as a String. 
             *      Type conversion may be needed.
             * sectorTypeModified in the query result has type TIMESTAMP in the DB, but should be returned as a String. 
             *      Type conversion may be needed.
             */
            string query = @"SELECT * FROM SectorType";
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

        // SA EP 5
        [HttpPost("addSectorType")]
        public IActionResult AddSectorType(AddSectorTypeReq req)
        {
            /*
             * @typeName is a String for the name of the sector type to be created.
             */
            string query1 = @"INSERT INTO SectorType(sectorTypeName, sectorTypeModifiedDate, sectorTypeActive)
                             VALUES(@typeName, GETDATE(), 1)";

            /*
             * Frontend will send a list of Attribute Type information, so this query needs to be iteratively executed.
             * @attributeTypeName is a String for the name of the attribute type to be created.
             * @require is a Boolean for whether the attribute type is required. The required column of the AttributeType
             *      table in DB has type BIT. Type conversion may be needed.
             */
            string query2 = @"INSERT INTO AttributeType(attributeTypeName, attributeTypeModifiedDate, required, sectorTypeID)
                                (SELECT @attributeTypeName, GETDATE(), @required, MAX(sectorTypeID) FROM SectorType)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query1, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@typeName", req.sector_type_name);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    for (int i = 0; i < req.attr_type.Count; i++)
                    {
                        using (SqlCommand myCommand = new SqlCommand(query2, myConn))
                        {

                            myCommand.Parameters.AddWithValue("@attributeTypeName", req.attr_type[i].name);
                            if (req.attr_type[i].required)
                            {
                                myCommand.Parameters.AddWithValue("@required", 1);
                            }
                            else 
                            {
                                myCommand.Parameters.AddWithValue("@required", 0);
                            }
                            
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();
                        }
                    }
                    myConn.Close();
                    return NoContent();

                }
            } 
            catch (Exception ex)
            {
                return StatusCode(500);
            }


        }

        // SA EP 6
        [HttpDelete("deleteSectorTypeById/{sector_type_id}")]
        public IActionResult DeleteSectorTypeByID(int sector_type_id)
        {

            // NOTE: The advanced implementation which will be left for future improvements.Ignore for now.
            /*
             * sectorTypeID is a String for the id of the sector type to be deleted. sectorTypeID is
             *      INT in the DB. Type conversion may be needed.
             */
            // string query1 = @"SELECT * FROM Sector WHERE sectorTypeID = @sectorTypeID";
            // string query2 = @"SELECT * FROM DuplicatedSector WHERE sectorTypeID = @sectorTypeID";

            // If the result of query1 and query2 are both empty, execute query 3:

            // string query3 = @"DELETE FROM SectorType WHERE sectorTypeID = @sectorTypeID";

            // else:
            // string query4 = @"UPDATE SectorType SET sectorTypeActive = 0 WHERE sectorTypeID = @sectorTypeID";
            // END NOTE

            /*
             * sectorTypeID is a String for the id of the sector type to be deleted. sectorTypeID is
             *      INT in the DB. Type conversion may be needed.
             */
            string query = @"DELETE FROM SectorType WHERE sectorTypeID = @sectorTypeID";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorTypeID", sector_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();

        }

        // SA EP 7
        [HttpGet("viewAttributeTypeById/{sector_type_id}")]
        public IActionResult ViewAttributeTypeByID(int sector_type_id)
        {   
            /*
             * @typeID is a String for the id of the sector type to be viewed. sectorTypeID is INT in DB.
             *      Type conversion may be needed.
             * Same conversion may be needed to return the response.
             */ 
            string query = @"SELECT * FROM AttributeType where sectorTypeID =@typeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@typeID", sector_type_id);
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

        // SA EP 8
        [HttpPost("addNewAttributeTypeInSectorType")]
        public IActionResult AddNewAttributeTypeInSectorType(AddNewAttrTypeInSectorTypeReq req)
        {
            /*
             * @typeName is the String name for the newly created type, as passed in as param.
             * @required is the String char with value 0 or 1, as passed in as param.
             * @secTypeID is the String typeID for the Sector to which the attribute should associate, as passed in as param.
             *      @secTypeID should be converted to INT before executing the query. 
             */
            string query = @"INSERT INTO AttributeType(attributeTypeName, attributeTypeModifiedDate, required, sectorTypeID)
                             VALUES (@typeName, GETDATE(), @required, @secTypeID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@secTypeID", req.sector_type_id);
                    myCommand.Parameters.AddWithValue("@typeName", req.attribute_type_name);
                    if (req.required) 
                    {
                        myCommand.Parameters.AddWithValue("@required", 1);
                    } 
                    else
                    {
                        myCommand.Parameters.AddWithValue("@required", 0);
                    }
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();
        }

        // SA EP 9
        [HttpDelete("deleteAttributeTypeById/{attribute_type_id}")]
        public IActionResult DeleteAttributeTypeByID(int attribute_type_id)
        {
            /*
             * @typeID is a String for the attribute type to be deleted. attributeTypeID is INT in DB.
             *      Type conversion may be needed.
             */
            string query = @"DELETE FROM AttributeType WHERE attributeTypeID =@typeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@typeID", attribute_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

            }

            return NoContent();

        }

        /* 
         * TODO: This endpoint is drastically changed due to the remove and addition of DB tables.
         * The old PossibleValues table is replace by the Division table. This endpoint is now
         * changed to ViewAllDivisions
         */

        // SA EP 10
        [HttpGet("viewAllDivisions")]
        public IActionResult ViewAllDivisions()
        {
            string query = @"SELECT * FROM Division";
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

        /*
         * TODO: This endpoint is drastically changed due to the remove and addition of DB tables.
         * The old PossibleValues table is replace by the Division table. This endpoint is now
         * changed to AddDivision
         */

        // SA EP 11
        [HttpPost("addDivision")]
        public IActionResult AddDivision(AddDivisionReq req)
        {
            /*
             @divisionName is a String for the name of the division to be created.
             */
            string query = @"INSERT INTO Division(divisionName) VALUES (@divisionName)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@divisionName", req.division);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();
        }

        /*
         * TODO: This endpoint is drastically changed due to the remove and addition of DB tables.
         * The old PossibleValues table is replace by the Division table. This endpoint is now
         * changed to DeleteDivisionByID
         */

        // SA EP 12
        [HttpDelete("deleteDivision/{division_id}")]
        public IActionResult DeleteDivision(int division_id)
        {
            /*
             * @divisionID is a String for the ID of the division to be deleted.divisionID is INT in DB.
             *      Type conversion may be needed.
             */
            string query = @"DELETE FROM Division WHERE divisionID =@divisionID ;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@divisionID", division_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();

        }

        /*
         * TODO: This endpoint is drastically changed due to the remove and addition of DB tables.
         * The old PossibleValues table is replace by the Division table. This endpoint is now
         * changed to UpdateDivision
         */

        // SA EP 13
        [HttpPut("updateDivision")]
        public IActionResult UpdateDivision(UpdateDivisionNameReq req)
        {

            /*
             * @divisionName is a String for the new name of the division to be updated.
             * @divisionID is a String for the ID of the division to be updated.divisionID is INT in DB.
             *      Type conversion may be needed.
             */
            string query = @"UPDATE Division
                                SET divisionName = @divisionName
                                WHERE divisionID = @divisionID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@divisionID", req.division_id);
                    myCommand.Parameters.AddWithValue("@divisionName", req.division_name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();

                }
            }

            return NoContent();
        }

        // SA EP 14
        [HttpGet("viewTemplate")]
        public IActionResult ViewTemplate()
        {
            /*
             * templateID is INT in the DB, but should be returned as String. Type conversion
             *      may be needed.
             */
            string query = @"SELECT * FROM Template";
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

        // SA EP 15
        [HttpGet("viewTemplateById/{template_id}")]
        public JsonResult ViewTemplateByID(int template_id)
        {   
            /*
             * sectorTypeID is INT in the DB, but should be returned as a String. Type conversion may be needed.
             * sectirTypeModifiedDate is TIMESTAMP in the DB, but should be returned as a String. Type conversion
             *      may be needed.
             * @templateID is a String for the id of the template to view. templateID is INT in the DB.
             *      Type conversion may be needed.
             */
            string query = @"SELECT ST.sectorTypeID, ST.sectorTypeName, ST.sectorTypeModifiedDate
                                FROM SectorType ST, SectorTypeTemplate STT
                                WHERE ST.sectorTypeID = STT.sectorTypeID AND STT.templateID = @templateID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@templateID", template_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        // SA EP 16
        // TODO: add query according to new spec (may need to loop & query)
        // Template name not in DB
        [HttpPost("addTemplate")]
        public IActionResult AddTemplate(AddTemplateReq req)
        {
            /*
             * @templateName is a String for the name of the template to be created.
             */
            string query1 = @"INSERT INTO Template(templateName, templateModifiedDate, templateActive)
                                VALUES(@templateName, GETDATE(), 1)";


            /*
             * Frontend will send a list of sector type information, so this query needs to be iteratively executed.
             * @sectorTypeID is a String for the id of the sector type to be added.sectorTypeID is INT in DB.
             *      Type conversion may be needed.
             */
            string query2 = @"INSERT INTO SectorTypeTemplate(sectorTypeID, templateID)
                                (SELECT @sectorTypeID, MAX(templateID) FROM Template)";
            string query3 = @"SELECT MAX(templateID) FROM Template;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query1, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@templateName", req.template_name);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    for (int i = 0; i < req.sector_type_ids.Count; i++)
                    {
                        using (SqlCommand myCommand = new SqlCommand(query2, myConn))
                        {

                            myCommand.Parameters.AddWithValue("@sectorTypeID", req.sector_type_ids[i]);
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();
                        }
                    }
                    using (SqlCommand myCommand = new SqlCommand(query3, myConn))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myConn.Close();
                    }
                    return new JsonResult(table);
                }
            } catch (Exception ex)
            {
                return StatusCode(500);
            }
            
        }

        // SA EP 17
        [HttpDelete("deleteTemplateById/{template_id}")]
        public IActionResult DeleteTempalte(int template_id)
        {
            /*
             * @templateID is a String for the id of the template to view. templateID is INT in the DB.
             *      Type conversion may be needed.
             */
            string query = @"DELETE FROM Template WHERE templateID =@templateID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@templateID", template_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();
        }

        // SA EP 18
        [HttpPost("addSectorInTemplate")]
        public IActionResult AddSectorTypeInTemplate(AddSectorTypeInTemplateReq req)
        {
            /*
             * Frontend will send a list of sector type information, so this query needs to be iteratively executed.
             * @sectorTypeID is a String for the id of the sector type to be added.sectorTypeID is INT in DB.
             *      Type conversion may be needed.
             * @templateID is a String for the id of the template to view. templateID is INT in the DB.
             *      Type conversion may be needed.
             */
            string query = @"INSERT INTO SectorTypeTemplate(sectorTypeID, templateID)
                                    VALUES(@sectorTypeID, @templateID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@templateID", req.template_id);
                    myCommand.Parameters.AddWithValue("@sectorTypeID", req.sector_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();
        }

        // SA EP 19
        // missing sector type id
        [HttpDelete("deleteSectorTypeFromTemplate")]
        public IActionResult DeleteSectorTypeFromTemplate(DeleteSectorTypeFromTemplateReq req)
        {
            /*
             @typeID is the String typeID for the sector type, as passed in as param.
             @templateID is the String templateID for the template, as passed in as param.
             */
            string query = @"DELETE FROM SectorTypeTemplate
                             WHERE sectorTypeID = @sectorTypeID AND templateID = @templateID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@templateID", req.template_id);
                    myCommand.Parameters.AddWithValue("@sectorTypeID", req.sector_type_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return NoContent();
        }

        // SA EP 20
        [HttpGet("getSectorTypeNameById/{sector_type_id}")]
        public IActionResult GetSectorTypeNameById(int sector_type_id)
        {
            string query = @"SELECT sectorTypeName FROM SectorType WHERE sectorTypeID = @sector_type_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sector_type_id", sector_type_id);
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

        // SA EP 21
        [HttpGet("getTemplateNameById/{template_id}")]
        public IActionResult GetTemplateNameById(int template_id)
        {
            string query = @"SELECT templateName FROM Template WHERE templateID = @template_id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@template_id", template_id);
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


    }
}
