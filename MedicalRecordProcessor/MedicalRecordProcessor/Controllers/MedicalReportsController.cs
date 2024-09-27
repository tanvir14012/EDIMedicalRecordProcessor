using MedicalRecordProcessor.Factories;
using MedicalRecordProcessor.Models;
using MedicalRecordProcessor.Models.Dto;
using MedicalRecordProcessor.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalRecordProcessor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicalReportsController : ControllerBase
    {
        private readonly IMedicalReportFactory medicalReportFactory;
        private readonly IMedicalReportService medicalReportService;
        private readonly IReportDetailsService reportDetailsService;

        public MedicalReportsController(IMedicalReportFactory medicalReportFactory,
            IMedicalReportService medicalReportService,
            IReportDetailsService reportDetailsService)
        {
            this.medicalReportFactory = medicalReportFactory;
            this.medicalReportService = medicalReportService;
            this.reportDetailsService = reportDetailsService;
        }

        // GET: api/MedicalReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalReport>>> GetMedicalReports(int pageIndex, int pageSize)
        {
            try
            {
                var model = await medicalReportService.GetMedicalReports(pageIndex, pageSize);
                return Ok(new
                {
                    total = model.Item1,
                    data = model.Item2
                });
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<MedicalReport>> PostMedicalReport([FromForm] FileUpload model)
        {
            try
            {
                var parseResult = await medicalReportFactory.PrepareMedicalReport(model);
                return Ok(parseResult);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        public async Task<ActionResult<IEnumerable<ReportDetails>>> GetMedicalReportDetails(int id, string searchText,
            int pageIndex, int pageSize)
        {
            try
            {
                var model = await reportDetailsService.GetReportDetails(id, searchText, pageIndex, pageSize);
                return Ok(new
                {
                    total = model.Item1,
                    data = model.Item2
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
