using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Dtos;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Enums;


namespace WebApi.Services
{
  public interface IAllianceService
  {
    Task<List<AllianceDto>> GetAll(int? clientId);
    Alianzas GetById(int id);
    Alianzas Create(Alianzas payload);
    Alianzas Update(Alianzas payload);
		Alianzas UpdateCost(int id);

	void Delete(int id);
  }

  public class AllianceService : IAllianceService
  {

    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public AllianceService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public Alianzas Create(Alianzas payload)
    {
      throw new NotImplementedException();
    }

    public void Delete(int id)
    {
      throw new NotImplementedException();
    }

    public async Task<List<AllianceDto>> GetAll(int? clientId)
    {
      if (clientId == null)
      {
        var allAlliances = await _context.Alianzas.Select(x => new AllianceDto(x)).ToListAsync();
        return allAlliances;
      }
      var clientAlliances =
      await (from pr in _context.ClientProduct
             join al in _context.Alianzas on pr.Id equals al.ClientProductId
             join aff in _context.AffType on al.AffType.ToString() equals aff.Id.ToString()
             join cov in _context.Covers on al.CoverId equals cov.Id
             join qu in _context.QualifyingEvents on al.QualifyingEventId equals qu.Id
             where pr.ClientId == clientId.GetValueOrDefault()
             select new { alliance = al, AffType = aff, quallifyingEvent = qu, cover = cov })
             .Select(x => new AllianceDto(x.alliance) { Cover = x.cover, QualifyingEvent = x.quallifyingEvent, AffTypeDescription = x.AffType }).ToListAsync();

      clientAlliances.ForEach(x =>
      {
        x.Cover.Alianza = null;
        x.Cover.Alianzas = null;
        x.QualifyingEvent.Alianzas = null;
      });
      return clientAlliances;
    }

    public Alianzas GetById(int id)
    {
      throw new NotImplementedException();
    }

    public Alianzas Update(Alianzas payload)
    {
      throw new NotImplementedException();
    }



		public Alianzas UpdateCost(int Alianzaid)
		{

			try
			{
				Alianzas item = null;

				item = _context.Alianzas
					.Include(u => u.ClientProduct).ThenInclude(u => u.Client)
					.Include(u => u.Cover)
					.Include(u => u.AlianzaAddOns)
					.ThenInclude(u => u.InsuranceAddOn).Where(u => u.Id == Alianzaid).FirstOrDefault();

				Clients Member = item.ClientProduct.Client;
				Covers Plans = item.Cover;
				List<AlianzaAddOns> AddOns = item.AlianzaAddOns.ToList();
				DateTime EffectiveDate = Convert.ToDateTime(item.StartDate);

				float cost = 0;
				float Totalcost = 0;

				int age = CalcularEdad(Convert.ToDateTime(Member.BirthDate), EffectiveDate);

				//if (Member.Identifier >= 4)
				//	return cost;

				if (age > 100)
					age = 100;
				else if (age < 1)
					age = 1;
				//else
				//	age = Member.Age;


				if (Plans.TypeCalculate == (int)TypeCalculate.AllMember)
				{
					cost = Plans.IndividualRate;
				}
				else if (Plans.TypeCalculate == (int)TypeCalculate.AllMemberAndAge)
				{

					var RateByAge = _context.InsuranceRate.Where(r => r.CoverId == Plans.Id && r.Age == age && r.PolicyYear == EffectiveDate.Year).FirstOrDefault();
					cost = RateByAge.IndividualRate;

				}
				else if (Plans.TypeCalculate == (int)TypeCalculate.EEOnly)
				{

					cost = Plans.IndividualRate;

				}
				else if (Plans.TypeCalculate == (int)TypeCalculate.Tier)
				{

					if (Member.Dependents.ToList().Count <= 0)
						cost = Plans.CoverageSingleRate;
					else if (Member.Dependents.ToList().Count == 1)
						cost = Plans.CoverageCoupleRate;
					else if (Member.Dependents.ToList().Count > 1)
						cost = Plans.CoverageFamilyRate;

				}
				else if (Plans.TypeCalculate == (int)TypeCalculate.TierAndAge)
				{

					var RateByAge = _context.InsuranceRate.Where(r => r.CoverId == Plans.Id && r.Age == age && r.PolicyYear == EffectiveDate.Year).FirstOrDefault();

					if (Member.Dependents.ToList().Count <= 0)
						cost = RateByAge.CoverageSingleRate;
					else if (Member.Dependents.ToList().Count == 1)
						cost = RateByAge.CoverageCoupleRate;
					else if (Member.Dependents.ToList().Count > 1)
						cost = RateByAge.CoverageFamilyRate;

				}

				item.CoverAmount = cost;

				Totalcost += cost;

				//AddOns
				for (int p = 0; p <= AddOns.Count - 1; p++)
				{
					var r = AddOns[p];
					if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.AllMember)
					{
						cost = r.InsuranceAddOn.IndividualRate;

					}
					else if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.AllMemberAndAge)
					{
						var RateByAge = _context.InsuranceAddOnsRateAge.Where(h => h.InsuranceAddOnsId == r.InsuranceAddOn.Id && h.Age == age).FirstOrDefault();
						cost = RateByAge.Rate;
					}
					else if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.EEOnly)
					{
						cost = r.InsuranceAddOn.IndividualRate;
					}
					else if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.Tier)
					{
						if (Member.Dependents.ToList().Count <= 0)
							cost = r.InsuranceAddOn.CoverageSingleRate;
						else if (Member.Dependents.ToList().Count == 1)
							cost = r.InsuranceAddOn.CoverageCoupleRate;
						else if (Member.Dependents.ToList().Count > 1)
							cost = r.InsuranceAddOn.CoverageFamilyRate;
					}


					r.Cost = cost;
					_context.AlianzaAddOns.Update(r);
					_context.SaveChanges();

					Totalcost += cost;


				}

				item.SubTotal = Totalcost;

				_context.Alianzas.Update(item);
				_context.SaveChanges();

				return item;

			}

			catch (Exception ex)
			{
				throw ex;
			}

		}



		private int CalcularEdad(DateTime birthDate, DateTime now)
		{
			int age = now.Year - birthDate.Year;
			if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
				age--;
			return age;
		}



	}
}