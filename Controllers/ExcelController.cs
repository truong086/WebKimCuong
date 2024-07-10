using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using thuongmaidientus1.AccountService;

namespace thuongmaidientus1.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly IExcelService _excelService;

        public ExcelController(IExcelService excelService)
        {
            _excelService = excelService;
        }

        [HttpPost]
        [Route(nameof(UpdateExcel))]
        public async Task<IActionResult> UpdateExcel([FromBody] List<Student> students)
        {
            if (students == null || students.Count == 0)
            {
                return BadRequest("No data received.");
            }

            var success = await _excelService.UpdateCSVData(students);

            if (success)
            {
                return Ok("Data has been added to the Excel file.");
            }
            else
            {
                return StatusCode(500, "Failed to update the Excel file.");
            }
        }
    }
}
