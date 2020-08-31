using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;
using System.IO;
using OfficeOpenXml;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CoversController : BaseController
  {
    private ICoverService _itemService;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public CoversController(
        ICoverService service,
        IMapper mapper,
        IOptions<AppSettings> appSettings)
    {
            _itemService = service;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    [Authorize]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
      try
      {
        var res = await _itemService.GetAll();
        if (res == null)
        {
          return NotFound();
        }
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("plan/{coverId}")]
    public async Task<IActionResult> GetPlanByCover(int coverId)
    {
      try
      {
        var res = await _itemService.GetPlanByCover(coverId);
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }

    [Authorize]
    [HttpGet("{planId}")]
    public async Task<IActionResult> GetByPlan(int planId)
    {
      try
      {
        var res = await _itemService.GetByPlan(planId);
        if (res == null)
        {
          return NotFound();
        }
        return Ok(res);
      }
      catch (Exception ex)
      {
        return DefaultError(ex);
      }
    }


        // GET: api/Covers/5

        //[AllowAnonymous]
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    try
        //    {
        //        var item = _itemService.GetById(id);
        //        return Ok(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        return DefaultError(ex.Message);
        //    }
        //}

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Covers item)
        {
            // map dto to entity and set id
            item.Id = id;

            try
            {
                // save 
                //item.FUpdUserId = GetNameClaim();
                item.UpdatedAt = DateTime.Now;
                item = _itemService.Update(item);


                return Ok(item);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex.Message);
            }
        }

        // POST: api/Covers

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] Covers item)
        {
            // map dto to entity
            // var item = _mapper.Map<Covers>(paramDto);

            try
            {
                // save 

                //item.FCreateUserId = GetNameClaim();
                item.CreatedAt = DateTime.Now;
                _itemService.Create(item);


                return Ok(item);

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex.Message);
            }
        }

        // PUT: api/Covers/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/Covers/5

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                Covers item = new Covers
                {
                    Id = id,
                    //FUpdUserId = GetNameClaim(),
                    UpdatedAt = DateTime.Now
                };

                _itemService.Delete(item);

                return Ok();

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return DefaultError(ex.Message);
            }
        }


        //[Authorize(Roles = "MenuItem2.Permission1")]
        //[Authorize(Roles = "MenuItem2.Permission3")]

        [AllowAnonymous]
        [HttpPost("{CoverId}/BenefitType")]
        public IActionResult AddBenefit([FromBody] InsurancePlanBenefit item)
        {
            try
            {

                //CoversOptionalCover CoversOptionalCover = new CoversOptionalCover
                //{
                //    CoverId  = CoverId,
                //    OptionalCoverId  = OptionalCoverId
                //};

                _itemService.AddBenefit(item);

                return Ok(item);

            }
            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("{CoverId}/BenefitType/UploadSingleFile")]
        public async Task<IActionResult> Post(IFormFile file)
        {

            // Full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (file.Length > 0)
                using (var stream = new FileStream(filePath, FileMode.Create))
                    await file.CopyToAsync(stream);

            // Process uploaded files

            return Ok(new { count = 1, path = filePath });
        }
        //[AllowAnonymous]
        //[HttpPost("{CoverId}/BenefitType/Upload")]
        //public IActionResult AddBenefitByExcel2(IFormFile formFile, int CoverId)
        //{
        //    try
        //    {


        //        if (formFile == null || formFile.Length <= 0)
        //        {

        //            return DefaultError("formfile is empty");
        //            //return DemoResponse<List<CoversBenefit>>.GetResult(-1, "formfile is empty");
        //        }

        //        if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //        {
        //            return DefaultError("Not Support file extension");
        //        }

        //        var list = new List<CoversBenefit>();


        //        using (var stream = new MemoryStream())
        //        {
        //            formFile.CopyTo(stream);

        //            using (var package = new ExcelPackage(stream))
        //            {
        //                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //                var rowCount = worksheet.Dimension.Rows;

        //                for (int row = 2; row <= rowCount; row++)
        //                {
        //                    list.Add(new CoversBenefit
        //                    {
        //                        InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
        //                        CoverId = CoverId,
        //                        Value = worksheet.Cells[row, 4].Value.ToString().Trim()
        //                    });
        //                }
        //            }
        //        }
        //        return Ok(list);

        //        //CoversBenefit CoversBenefit = JsonConvert.DeserializeObject<CoversBenefit>(data);
        //        //return Json(data);

        //        //CoversOptionalCover CoversOptionalCover = new CoversOptionalCover
        //        //{
        //        //    CoverId  = CoverId,
        //        //    OptionalCoverId  = OptionalCoverId
        //        //};

        //        //_itemService.AddBenefitByExcel(data);

        //        //return Ok(item);

        //    }
        //    catch (AppException ex)
        //    {
        //        return DefaultError(ex.Message);
        //    }
        //}


        [AllowAnonymous]
        [HttpPost("{CoverId}/BenefitType/Upload")]

        public IActionResult AddBenefitByExcel(int CoverId, [FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {

                if (formFile == null || formFile.Length <= 0)
                {

                    return BadRequest(new { error = "file is empty" });
                }


                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    if (!Path.GetExtension(formFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        return BadRequest(new { error = "Not Support file extension" });
                    }
                }

                ICollection<InsurancePlanBenefit> list = null;

                using (var stream = new MemoryStream())
                {
                    formFile.CopyTo(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            list.Add(new InsurancePlanBenefit
                            {
                                InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                                CoverId = CoverId,
                                Value = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                                CreatedAt = DateTime.Now

                            });
                        }
                    }
                }

                // add list to db ..  
                // here just read and return  
                return Ok(list);

            }

            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }

        // GET: api/Covers/5/Rate

        [AllowAnonymous]
        [HttpGet("{id}/Rate")]
        public IActionResult GetRateList(int id)
        {
            try
            {
                var item = _itemService.GetRateList(id).OrderBy(r => r.Age);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return DefaultError(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("insuranceCompany/{InsuranceCompanyId}/Rate/{PolicyYear}/Upload")]

        public IActionResult AddPlanAndRateByExcel(int InsuranceCompanyId, int PolicyYear, [FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {

                if (formFile == null || formFile.Length <= 0)
                {

                    return BadRequest(new { error = "file is empty" });
                }


                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    if (!Path.GetExtension(formFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        return BadRequest(new { error = "Not Support file extension" });
                    }
                }


                using (var stream = new MemoryStream())
                {
                    formFile.CopyTo(stream);
                    //UploadRatesByAge(int id, int PolicyYear, MemoryStream stream, int userId)
                    var data = _itemService.UpLoadRate(InsuranceCompanyId, PolicyYear, stream);
                    //using (var package = new ExcelPackage(stream))
                    //{
                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //    var rowCount = worksheet.Dimension.Rows;

                    //    for (int row = 2; row <= rowCount; row++)
                    //    {
                    //        list.Add(new CoversBenefit
                    //        {
                    //            InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                    //            CoverId = CoverId,
                    //            Value = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                    //            CreatedAt = DateTime.Now,
                    //            FCreateUserId = GetNameClaim()

                    //        });
                    //    }
                    //}
                    return Ok(data);
                }

                // add list to db ..  
                // here just read and return  

            }

            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("{id}/Rate/{PolicyYear}/Upload")]

        public IActionResult AddRateByExcel(int id, int PolicyYear, [FromForm(Name = "file")] IFormFile formFile)
        {
            try
            {

                if (formFile == null || formFile.Length <= 0)
                {

                    return BadRequest(new { error = "file is empty" });
                }


                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    if (!Path.GetExtension(formFile.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase))
                    {
                        return BadRequest(new { error = "Not Support file extension" });
                    }
                }


                using (var stream = new MemoryStream())
                {
                    formFile.CopyTo(stream);
                    var data = _itemService.UploadRatesByAge(id, PolicyYear, stream, GetNameClaim());
                    //var data = _itemService.UpLoadRate(InsuranceCompanyId, PolicyYear, stream);
                    return Ok(data);
                }

                // add list to db ..  
                // here just read and return  

            }

            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("{CoverId}/AddOns/{InsuranceAddOnsId}")]
        public IActionResult AddPlanAddOns([FromBody] InsurancePlanAddOns item)
        {
            try
            {

                //InsuranceCompanyOptionalCover InsuranceCompanyOptionalCover = new InsuranceCompanyOptionalCover
                //{
                //    InsuranceCompanyId  = InsuranceCompanyId,
                //    OptionalCoverId  = OptionalCoverId
                //};

                _itemService.AddPlanAddOns(item);

                return Ok(item);

            }
            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpDelete("{CoverId}/AddOns/{InsuranceAddOnsId}")]
        public IActionResult DeletePlanAddOns(int CoverId, int InsuranceAddOnsId)
        {
            try
            {

                //InsuranceCompanyOptionalCover InsuranceCompanyOptionalCover = new InsuranceCompanyOptionalCover
                //{
                //    InsuranceCompanyId  = InsuranceCompanyId,
                //    OptionalCoverId  = OptionalCoverId
                //};

                _itemService.DeletePlanAddOns(CoverId, InsuranceAddOnsId);

                return Ok();

            }
            catch (AppException ex)
            {
                return DefaultError(ex.Message);
            }
        }

        //[AllowAnonymous]
        //[HttpPost("{CoverId}/BenefitType/Upload")]
        //public async Task<DemoResponse<List<CoversBenefit>>> AddBenefitByExcel(IFormFile formFile, int CoverId)
        //{
        //    if (formFile == null || formFile.Length <= 0)
        //    {
        //        return DemoResponse<List<CoversBenefit>>.GetResult(-1, "formfile is empty");
        //    }

        //    if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return DemoResponse<List<CoversBenefit>>.GetResult(-1, "Not Support file extension");
        //    }

        //    var list = new List<CoversBenefit>();

        //    using (var stream = new MemoryStream())
        //    {
        //        await formFile.CopyToAsync(stream);

        //        using (var package = new ExcelPackage(stream))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            var rowCount = worksheet.Dimension.Rows;

        //            for (int row = 2; row <= rowCount; row++)
        //            {
        //                list.Add(new CoversBenefit
        //                {
        //                    InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
        //                    CoverId  = CoverId,
        //                    Value = worksheet.Cells[row, 4].Value.ToString().Trim()
        //                });
        //            }
        //        }
        //    }

        //    // add list to db ..  
        //    // here just read and return  

        //    return DemoResponse<List<CoversBenefit>>.GetResult(0, "OK", list);
        //}

        //[Authorize(Roles = "MenuItem2.Permission4")]

        [AllowAnonymous]
        [HttpDelete("{CoverId}/BenefitType/{BenefitTypeId}")]
        public IActionResult DeleteOptionalCover(int CoverId, int BenefitTypeId)
        {
            try
            {
                _itemService.DeleteBenefit(CoverId, BenefitTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return DefaultError(ex.Message);
            }
        }


        private IActionResult DefaultError(string exceptionMessage)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                code = StatusCodes.Status500InternalServerError,
                error = exceptionMessage
            });

        }

    }
}