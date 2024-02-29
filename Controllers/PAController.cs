using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using CPSC319BackEnd.Models;
using Attribute = CPSC319BackEnd.Models.Attribute;

namespace CPSC319BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PAController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PAController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // PA EP 1
        [HttpGet("getWorkspace/{employee_id}")]
        public IActionResult GetWorkSpace(int employee_id)
        {
            string query = @"SELECT W.workspaceID, W.proposalNumber, W.workspaceName, W.creationDate, D.divisionName
                             FROM Workspace W, Division D
                             WHERE W.divisionID = D.divisionID AND W.employeeID = @employeeID";
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

        // PA EP 2
        [HttpPut("editWorkspace")]
        public IActionResult EditWorkspace(EditWorkspaceReq req)
        {
            string query = @"UPDATE Workspace
                             SET workspaceName = @workspaceName, comment = @workspaceComment
                             WHERE workspaceID = @workspaceID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", req.workspace_id);
                    myCommand.Parameters.AddWithValue("@workspaceName", req.workspace_name);
                    myCommand.Parameters.AddWithValue("@workspaceComment", req.workspace_comment);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }

                return NoContent();

            }

        }

        // PA EP 3
        [HttpDelete("deleteWorkspaceById/{workspace_id}")]
        public IActionResult DeleteWorkspaceById(int workspace_id)
        {
            string query = @"DELETE FROM Workspace WHERE workspaceID = @workspaceID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", workspace_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 4
        [HttpPost("createWorkspace")]
        public IActionResult CreateWorkspace(CreateWorkspaceReq req)
        {
            string query = @"INSERT INTO Workspace(proposalNumber, workspaceName, creationDate, comment, employeeID, divisionID) 
                             VALUES (@proposalNumber, @workspaceName, GETDATE(), @comment, @employeeID, @divisionID); ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@proposalNumber", req.proposal_number);
                    myCommand.Parameters.AddWithValue("@workspaceName", req.workspace_name);
                    myCommand.Parameters.AddWithValue("@employeeID", req.employee_id);
                    myCommand.Parameters.AddWithValue("@divisionID", req.division_id);
                    myCommand.Parameters.AddWithValue("@comment", req.workspace_comment);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 5  
        [HttpGet("getAllDivisions")]
        public IActionResult GetAllDivisions()
        {
            string query = @"SELECT * FROM Division;";
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

        // PA EP 6
        [HttpGet("getResumesInWorkspace/{workspace_id}")]
        public IActionResult GetResumesInWorkspace(int workspace_id)
        {
            string query = @"SELECT R.resumeID, R.resumeName, R.resumeModifiedDate, E.firstName, E.lastName
                             FROM Resume R, Employee E
                             WHERE R.employeeID = E.employeeID AND R.workspaceID = @workspaceID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", workspace_id);
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

        // PA EP 7
        [HttpPut("updateResume")]
        public IActionResult UpdateResume(UpdateResumeReq req)
        {
            string query = @"UPDATE Resume
                             SET resumeName = @resumeName, resumeModifiedDate = GETDATE()
                             WHERE resumeID = @resumeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", req.resume_id);
                    myCommand.Parameters.AddWithValue("@resumeName", req.resume_name);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 8
        [HttpDelete("deleteResumeById/{resume_id}")]
        public IActionResult DeleteResumeById(int resume_id)
        {
            string query = @"DELETE FROM Resume WHERE resumeID = @resumeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            try
            {
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
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(428, "PreCondition Required");
            }
        }

        // PA EP 9
        [HttpPost("createResume")]
        public IActionResult CreateResume(CreateResumeReq req)
        {
            string query = @"INSERT INTO Resume(resumeName, resumeModifiedDate, employeeID, workspaceID, templateID)
                             VALUES (@resumeName, GETDATE(), @employeeID, @workspaceID, @templateID);";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeName", req.resume_name);
                    myCommand.Parameters.AddWithValue("@workspaceID", req.workspace_id);
                    myCommand.Parameters.AddWithValue("@employeeID", req.employee_id);
                    myCommand.Parameters.AddWithValue("@templateID", req.template_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 10 - see GET /api/Employee

        // PA EP 11 
        [HttpGet("getTemplates")]
        public IActionResult GetTemplates()
        {
            string query = @"SELECT templateID, templateName FROM Template;";
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


        // PA EP 12
        [HttpGet("getResumeByEmployeeId/{employee_id}")]
        public IActionResult GetResumeByEmployeeId(int employee_id)
        {
            string query = @"SELECT resumeID, resumeName, resumeModifiedDate
                             FROM Resume
                             WHERE employeeID = @employeeID";
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

        // PA EP 13a
        [HttpPost("duplicateResume")]
        public IActionResult DuplicateResume(DuplicateResumeReq req)
        {
            string query = @"INSERT INTO Resume(resumeName, resumeModifiedDate, employeeID, workspaceID, templateID)
                            (SELECT resumeName, GETDATE(), employeeID, @workspaceID, templateID FROM Resume WHERE resumeID = @resumeID);";
            string query2 = @"SELECT MAX(resumeID) FROM Resume;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", req.resume_id);
                    myCommand.Parameters.AddWithValue("@workspaceID", req.workspace_id);
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
            }
            return new JsonResult(table);
        }

        // PA EP 13b
        [HttpPost("duplicateSectors")]
        public IActionResult DuplicateSectors(DuplicateSectorsReq req)
        {
            string query = @"INSERT INTO DuplicatedSector(duplicatedSectorName, duplicatedSectorModifiedDate, resumeID, sectorTypeID)
                             (SELECT duplicatedSectorName, duplicatedSectorModifiedDate, @newResumeID, sectorTypeID 
                             FROM DuplicatedSector
                             WHERE resumeID = @oldResumeID)";
            string query2 = @"SELECT duplicatedSectorID FROM DuplicatedSector WHERE resumeID = @newResumeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@oldResumeID", req.old_resume_id);
                    myCommand.Parameters.AddWithValue("@newResumeID", req.new_resume_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                using (SqlCommand myCommand = new SqlCommand(query2, myConn))
                {
                    myCommand.Parameters.AddWithValue("@newResumeID", req.new_resume_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return new JsonResult(table);
        }

        // PA EP 13c
        [HttpGet("getDuplicateSectorId/{old_resume_id}")]
        public IActionResult GetDuplicateSectorId(int old_resume_id)
        {
            string query = @"SELECT duplicatedSectorID FROM DuplicatedSector WHERE resumeID = @oldResumeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@oldResumeID", old_resume_id);
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


        // PA EP 13d
        [HttpPost("duplicateAttributes")]
        public IActionResult DuplicateAttributes(DuplicateAttributesReq req)
        {
            string query = @"INSERT INTO Attribute(attributeValue, attributeTypeID, duplicatedSectorID)
                             (SELECT attributeValue, attributeTypeID, @newDuplicatedSectorID
                              FROM Attribute
                              WHERE duplicatedSectorID = @oldDuplicatedSectorID)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                for (int i = 0; i < req.old_dup_sec_id.Count; i++)
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@oldDuplicatedSectorID", req.old_dup_sec_id[i].duplicatedSectorID);
                        myCommand.Parameters.AddWithValue("@newDuplicatedSectorID", req.new_dup_sec_id[i].duplicatedSectorID);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
            }
            return NoContent();
        }

        // PA EP 14
        [HttpGet("getResumeById/{resume_id}")]
        public IActionResult GetResumeId(int resume_id)
        {
            string query = @"SELECT DS.duplicatedSectorID, DS.duplicatedSectorName, DS.duplicatedSectorModifiedDate, DS.sectorTypeID, ST.sectorTypeName
                             FROM DuplicatedSector DS, SectorType ST, Resume R
                             WHERE DS.sectorTypeID = ST.sectorTypeID AND 
                             DS.resumeID = R.resumeID AND
                             R.resumeID = @resumeID";
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


        // PA EP 15
        [HttpPut("editDuplicateSectorName")]
        public IActionResult EditDuplicateSectorName(EditDuplicateSectorReq req)
        {
            string query = @"UPDATE DuplicatedSector
                             SET duplicatedSectorName = @duplicatedSectorName, duplicatedSectorModifiedDate = GETDATE()
                             WHERE duplicatedSectorID = @duplicatedSectorID;";
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

        // PA EP 16
        [HttpDelete("deleteDupSectorById/{dup_sector_id}")]
        public IActionResult DeleteDupSectorById(int dup_sector_id)
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
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", dup_sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }


        // PA EP 17a
        [HttpGet("getSectorsByEmployeeId/{employee_id}")]
        public IActionResult GetSectorsByEmployeeId(int employee_id)
        {
            string query = @"SELECT S.sectorID, S.sectorName, S.sectorModifiedDate, 'sector'
                             FROM Sector S
                             WHERE employeeID = @employeeID;";
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

        // PA EP 17b
        [HttpGet("getDupSectorsByEmployeeId/{employee_id}")]
        public IActionResult GetDupSectorsByEmployeeId(int employee_id)
        {
            string query = @"SELECT DS.duplicatedSectorID, DS.duplicatedSectorName, DS.duplicatedSectorModifiedDate, 'duplicate'
                             FROM DuplicatedSector DS, Resume R
                             WHERE DS.resumeID = R.resumeID AND R.employeeID = @employeeID;";
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

        // PA EP 18a
        [HttpGet("getSectorById/{sector_id}")]
        public IActionResult GetSectorById(int sector_id)
        {
            string query = @"SELECT AT.attributeTypeID, AT.attributeTypeName, AT.required, A.attributeID, A.attributeValue 
                                FROM Attribute A, AttributeType AT, Sector S 
                                WHERE A.attributeTypeID = AT.attributeTypeID 
                                AND A.sectorID = @sectorID
                                AND A.sectorID = S.sectorID";
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


        // PA EP 18b
        [HttpGet("getDupSectorById/{dup_sector_id}")]
        public IActionResult GetDupSectorById(int dup_sector_id)
        {
            string query = @"SELECT AT.attributeTypeID, AT.attributeTypeName, AT.required, A.attributeID, A.attributeValue 
                                FROM Attribute A, AttributeType AT, DuplicatedSector DS 
                                WHERE A.attributeTypeID = AT.attributeTypeID 
                                AND A.duplicatedSectorID = @duplicatedSectorID
                                AND A.duplicatedSectorID = DS.duplicatedSectorID";
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

        // PA EP 19a1
        [HttpPost("addSectorsToResume")]
        public IActionResult AddSectorsToResume(AddSectorsToResumeReq req)
        {
            string query = @"INSERT INTO DuplicatedSector(duplicatedSectorName, duplicatedSectorModifiedDate, resumeID, sectorTypeID)
                            (SELECT sectorName, GETDATE(), @resumeID, sectorTypeID 
                            FROM Sector 
                            WHERE sectorID = @sectorID)";
            string query2 = @"SELECT MAX(duplicatedSectorID) FROM DuplicatedSector";
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


        // PA EP 19a2
        [HttpPost("copyAttributesToDupSector")]
        public IActionResult CopyAttributesToDupSector(CopyAttributesToDupSectorReq req)
        {
            string query = @"INSERT INTO Attribute(attributeValue, attributeTypeID, duplicatedSectorID)
                             (SELECT attributeValue, attributeTypeID, @duplicatedSectorID
                             FROM Attribute
                             WHERE sectorID = @sectorID);";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@sectorID", req.sector_id);
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", req.dup_sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 19b1
        [HttpPost("addDupSectorsToResume")]
        public IActionResult AddDupSectorsToResume(AddDupSectorsToResumeReq req)
        {
            string query = @"INSERT INTO DuplicatedSector(duplicatedSectorName, duplicatedSectorModifiedDate, resumeID, sectorTypeID)
                             (SELECT duplicatedSectorName, GETDATE(), @resumeID, sectorTypeID 
                             FROM DuplicatedSector 
                             WHERE duplicatedSectorID = @duplicatedSectorID);";
            string query2 = @"SELECT MAX(duplicatedSectorID) FROM DuplicatedSector";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@duplicatedSectorID", req.dup_sector_id);
                    myCommand.Parameters.AddWithValue("@resumeID", req.resume_id);
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

        // PA EP 19b2
        [HttpPost("copyAttributesToNewDupSector")]
        public IActionResult CopyAttributesToNewDupSector(CopyAttributesToNewDupSectorReq req)
        {
            string query = @"INSERT INTO Attribute(attributeValue, attributeTypeID, duplicatedSectorID)
                            (SELECT attributeValue, attributeTypeID, @newDuplicatedSectorID
                            FROM Attribute
                            WHERE duplicatedSectorID = @oldDuplicatedSectorID);";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@oldDuplicatedSectorID", req.old_dup_sector_id);
                    myCommand.Parameters.AddWithValue("@newDuplicatedSectorID", req.new_dup_sector_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }


        // PA EP 20
        [HttpPut("editAttrOfDupSector")]
        public IActionResult EditAttrOfDupSector(EditAttrOfDupSectorReq req)
        {
            string query = @"UPDATE Attribute
                             SET attributeValue = @attributeValue
                             WHERE attributeID = @attributeID;";
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
                        myCommand.Parameters.AddWithValue("@attributeID", req.attr[i].attributeID);
                        myCommand.Parameters.AddWithValue("@attributeValue", req.attr[i].attributeValue);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
            }
            return NoContent();
        }

        // PA EP 21
        [HttpGet("getWorkspaceById/{workspace_id}")]
        public IActionResult GetAllWorkspace(int workspace_id)
        {
            string query = @"SELECT *
                             FROM Workspace W, Resume R, DuplicatedSector DS, Attribute A
                             WHERE W.workspaceID = R.workspaceID AND 
                             R.resumeID = DS.resumeID AND 
                             DS.duplicatedSectorID = A.duplicatedSectorID
			                 AND W.workspaceID = @workspaceID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", workspace_id);
		    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
                return new JsonResult(table);

            }
        }

        // PA EP 22
        [HttpGet("getReqByEmployeeId/{employee_id}")]
        public IActionResult GetReqByEmployeeId(int employee_id)
        {
            string query = @"SELECT Q.requestID, Q.requestName, Q.requestSentTime, Q.requestStatus, R.resumeName, R. resumeID, E.firstName, E.lastName, W.workSpaceID
                             FROM Request Q, Resume R, Employee E, Workspace W
                             WHERE Q.resumeID = R.resumeID AND
                             Q.workspaceID = W.workspaceID AND
                             R.employeeID = E.employeeID AND
                             W.employeeID = @employeeID;";
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

        // PA EP 23
        [HttpDelete("deleteReqById/{req_id}")]
        public IActionResult DeleteReqById(int req_id)
        {
            string query = @"DELETE FROM Request WHERE requestID = @requestID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@requestID", req_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 24
        [HttpPost("createRequest")]
        public IActionResult CreateRequest(CreateReqReq req)
        {
            string query = @"INSERT INTO Request(requestName, requestSentTime, requestNote, requestStatus, workspaceID, resumeID, employeeID)
                             (SELECT @requestName, GETDATE(), @requestNote, 0, @workspaceID, @resumeID, employeeID
                             FROM Resume
                             WHERE resumeID = @resumeID);";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;

            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@requestName", req.req_name);
                    myCommand.Parameters.AddWithValue("@requestNote", req.req_note);
                    myCommand.Parameters.AddWithValue("@resumeID", req.resume_id);
                    myCommand.Parameters.AddWithValue("@workspaceID", req.workspace_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }
            return NoContent();
        }

        // PA EP 25
        [HttpGet("getEmployeeById/{employee_id}")]
        public IActionResult GetEmployeeById(int employee_id)
        {
            string query = @"SELECT employeeID, firstName, lastName, email, password, role, employeeActive
                             FROM Employee
                             WHERE employeeID = @employeeID;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employee_id) ;
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

        // PA EP 26
        [HttpPut("updateProfile")]
        public IActionResult UpdateProfileReq(UpdateProfileReq req)
        {
            // string query = @"UPDATE Employee SET @fieldName = @value WHERE employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                for (int i = 0; i < req.profile.Count; i++)
                {
                    using (SqlCommand myCommand = new SqlCommand("UPDATE Employee SET "+ req.profile[i].field_name +" = @value WHERE employeeID = @employeeID", myConn))
                    {
                        myCommand.Parameters.AddWithValue("@value", req.profile[i].value);
                        myCommand.Parameters.AddWithValue("@employeeID", req.employee_id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                }
                myConn.Close();
            }
            return NoContent();
        }

        // PA EP 27
        [HttpPut("updateRequestNote")]
        public IActionResult UpdateRequestNote(UpdateRequestNote req)
        {
            string query = @"UPDATE Request
                                SET requestNote = @requestNote, requestStatus = 0
                                WHERE requestID = @requestID";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;


            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();

                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@requestID", req.requestID);
                    myCommand.Parameters.AddWithValue("@requestNote", req.requestNote);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
            }
            return NoContent();
        }

        
        // PA EP 28
        [HttpGet("getRequestByID/{request_id}")]
        public IActionResult GetRequestByID(int request_id)
        {
            string query = @"SELECT Q.requestID, Q.requestName, Q.requestNote, W.workspaceName, R.resumeName, E.firstName, E.lastName
                                FROM Request Q, Resume R, Workspace W, Employee E
                                WHERE Q.requestID = @requestID AND
                                      Q.resumeID = R.resumeID AND
                                      R.workspaceID = W.workspaceID AND
                                      R.employeeID = E.employeeID";
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

        // PA EP 29
        [HttpGet("getWorkspaceNameByID/{workspaceID}")]
        public IActionResult GetWorkspaceNameByID(int workspaceID)
        {
            string query = @"SELECT workspaceName FROM Workspace WHERE workspaceID = @workspaceID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", workspaceID);
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

        // PA EP 30
        [HttpGet("getResumeNameByID/{resumeID}")]
        public IActionResult getResumeNameByID(int resumeID)
        {
            string query = @"SELECT resumeName FROM Resume WHERE resumeID = @resumeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resumeID);
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

        // PA EP 31
        [HttpGet("getEmployeeNameByResumeID/{resumeID}")]
        public IActionResult getEmployeeNameByResumeID(int resumeID)
        {
            string query = @"SELECT firstName, lastName 
                                FROM Resume R, Employee E
                                WHERE R.employeeID = E.employeeID AND resumeID = @resumeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resumeID);
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

        // PA EP 32a
        [HttpGet("getSectorByEmployeeAndSectorType/{employeeID}/{sectorTypeID}")]
        public IActionResult getSectorByEmployeeAndSectorType(int employeeID, int sectorTypeID)
        {
            string query = @"SELECT S.sectorID, S.sectorName, S.sectorModifiedDate, 'sector'
                                FROM Sector S
                                WHERE S.sectorTypeID = @sectorTypeID AND
                                      S.employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employeeID);
                    myCommand.Parameters.AddWithValue("@sectorTypeID", sectorTypeID);
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

        // PA EP 32b
        [HttpGet("getDupSectorByEmployeeAndSectorType/{employeeID}/{dupSectorTypeID}")]
        public IActionResult getDupSectorByEmployeeAndSectorType(int employeeID, int dupSectorTypeID)
        {
            string query = @"SELECT DS.duplicatedSectorID, DS.duplicatedSectorName, DS.duplicatedSectorModifiedDate, 'duplicated'
                                FROM DuplicatedSector DS, Resume R
                                WHERE DS.resumeID = R.resumeID AND
								sectorTypeID = @dupSectorTypeID AND
                                      R.employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employeeID);
                    myCommand.Parameters.AddWithValue("@dupSectorTypeID", dupSectorTypeID);
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

        // PA EP 33
        [HttpGet("getEmployeeIDByResumeID/{resumeID}")]
        public IActionResult getEmployeeIDByResumeID(int resumeID)
        {
            string query = @"SELECT employeeID FROM Resume WHERE resumeID = @resumeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@resumeID", resumeID);
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

        // PA EP 34
        [HttpGet("getAllResumeOfSelectedEmployeeInWorkspace/{employee_id}/{workspace_id}")]
        public IActionResult GetAllResumeOfSelectedEmployeeInWorkspace(int employee_id, int workspace_id)
        {
            string query = @"SELECT resumeID, resumeName, resumeModifiedDate
                             FROM Resume
                             WHERE workspaceID = @workspaceID AND employeeID = @employeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@employeeID", employee_id);
                    myCommand.Parameters.AddWithValue("@workspaceID", workspace_id);
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

        // PA EP 35
        [HttpGet("getWorkspaceCommentById/{workspace_id}")]
        public IActionResult GetWorkspaceCommentById(int workspace_id)
        {
            string query = @"SELECT comment FROM Workspace WHERE workspaceID = @workspaceID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Dbconn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@workspaceID", workspace_id);
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
