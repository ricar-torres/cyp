using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Services;
using System.IO;

namespace WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HealthPlanController : BaseController
  {
    private IHealthPlanService _itemService;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;

    public HealthPlanController(
        IHealthPlanService service,
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




        // GET: api/HealthPlan/5

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var item = _itemService.GetById(id);
                return item;
            }
            catch (Exception ex)
            {
                return DefaultError(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] HealthPlans item)
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

        // POST: api/HealthPlan

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] HealthPlans item)
        {
            // map dto to entity
            // var item = _mapper.Map<HealthPlans>(paramDto);

            try
            {
                // save 

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

        // PUT: api/HealthPlan/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/HealthPlan/5

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {

                HealthPlans item = new HealthPlans
                {
                    Id = id,
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


        //addons

        // GET: api/HealthPlan/Addons/5

        [AllowAnonymous]
        [HttpGet("Addons/{id}")]
        public IActionResult GetAddOns(int id)
        {
            try
            {
                var item = _itemService.GetAddOnsById(id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return DefaultError(ex.Message);
            }
        }


        // GET: api/HealthPlan/{id}/Addons

        [AllowAnonymous]
        [HttpGet("{HealthPlansId}/addons")]
        public IActionResult GetAllAddOns(int HealthPlansId)
        {
            try
            {
                var item = _itemService.GetAllAddOns(HealthPlansId);
                return item;
            }
            catch (Exception ex)
            {
                return DefaultError(ex.Message);
            }
        }


        // PUT: api/HealthPlan/Addons/5
        [AllowAnonymous]
        [HttpPut("Addons/{id}")]
        public IActionResult UpdateAddOns(int id, [FromBody] InsuranceAddOns item)
        {
            // map dto to entity and set id
            item.Id = id;

            try
            {
                // save 
                item.UpdatedAt = DateTime.Now;
                item = _itemService.UpdateAddOns(item);


                return Ok(item);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex.Message);
            }
        }

        // POST: api/HealthPlan/{HealthPlansId}/Addons

        [AllowAnonymous]
        [HttpPost("{HealthPlansId}/Addons")]
        public IActionResult RegisterAddOns(int HealthPlansId, [FromBody] InsuranceAddOns item)
        {
            // map dto to entity
            // var item = _mapper.Map<InsuranceAddOns>(paramDto);

            try
            {
                // save 
                item.CreatedAt = DateTime.Now;
                _itemService.CreateAddOns(item);


                return Ok(item);

            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return DefaultError(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpPost("{HealthPlansId}/Addons/Upload")]

        public IActionResult AddAddOnsRateByExcel(int HealthPlansId, [FromForm(Name = "file")] IFormFile formFile)
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

                    var data = _itemService.UploadAddOnsRatesByAge(HealthPlansId, stream);
                    //using (var package = new ExcelPackage(stream))
                    //{
                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //    var rowCount = worksheet.Dimension.Rows;

                    //    for (int row = 2; row <= rowCount; row++)
                    //    {
                    //        list.Add(new InsurancePlanBenefit
                    //        {
                    //            InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                    //            InsurancePlanId = InsurancePlanId,
                    //            Value = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                    //            CreateDt = DateTime.Now,
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
        [HttpPost("{HealthPlansId}/Addons/{id}/Upload")]

        public IActionResult AddAddOnsRateByExcel(int HealthPlansId, int id, [FromForm(Name = "file")] IFormFile formFile)
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

                    var data = _itemService.UploadAddOnsRatesByAge(HealthPlansId, id, stream);
                    //using (var package = new ExcelPackage(stream))
                    //{
                    //    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //    var rowCount = worksheet.Dimension.Rows;

                    //    for (int row = 2; row <= rowCount; row++)
                    //    {
                    //        list.Add(new InsurancePlanBenefit
                    //        {
                    //            InsuranceBenefitTypeId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                    //            InsurancePlanId = InsurancePlanId,
                    //            Value = worksheet.Cells[row, 4].Value == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim(),
                    //            CreateDt = DateTime.Now,
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

        // PUT: api/HealthPlan/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/HealthPlan/Addons/5

        [AllowAnonymous]
        [HttpDelete("Addons/{id}")]
        public IActionResult DeleteAddOns(int id)
        {
            try
            {

                InsuranceAddOns item = new InsuranceAddOns
                {
                    Id = id,
                    UpdatedAt = DateTime.Now
                };

                _itemService.DeleteAddOns(item);

                return Ok();

            }
            catch (Exception ex)
            {
                // return error message if there was an exception
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