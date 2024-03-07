using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCarManagment.Data;

namespace SimpleCarManagment
{
    public partial class ExportSimpleCarManagmentDbController : ExportController
    {
        private readonly SimpleCarManagmentDbContext context;
        private readonly SimpleCarManagmentDbService service;
        public ExportSimpleCarManagmentDbController(SimpleCarManagmentDbContext context, SimpleCarManagmentDbService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/SimpleCarManagmentDb/cars/csv")]
        [HttpGet("/export/SimpleCarManagmentDb/cars/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCarsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCars(), Request.Query), fileName);
        }

        [HttpGet("/export/SimpleCarManagmentDb/cars/excel")]
        [HttpGet("/export/SimpleCarManagmentDb/cars/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCarsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCars(), Request.Query), fileName);
        }
        [HttpGet("/export/SimpleCarManagmentDb/carservices/csv")]
        [HttpGet("/export/SimpleCarManagmentDb/carservices/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCarServicesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCarServices(), Request.Query), fileName);
        }

        [HttpGet("/export/SimpleCarManagmentDb/carservices/excel")]
        [HttpGet("/export/SimpleCarManagmentDb/carservices/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportCarServicesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCarServices(), Request.Query), fileName);
        }
        [HttpGet("/export/SimpleCarManagmentDb/services/csv")]
        [HttpGet("/export/SimpleCarManagmentDb/services/csv(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportServicesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetServices(), Request.Query), fileName);
        }

        [HttpGet("/export/SimpleCarManagmentDb/services/excel")]
        [HttpGet("/export/SimpleCarManagmentDb/services/excel(fileName='{fileName}')")]
        public async System.Threading.Tasks.Task<FileStreamResult> ExportServicesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetServices(), Request.Query), fileName);
        }
    }
}
