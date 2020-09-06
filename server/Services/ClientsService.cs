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

<<<<<<< HEAD
namespace WebApi.Services {
	public interface IClientService {
		List<Clients> GetAll();
		Clients GetById(int id);
		ClientInformationDto Create(ClientInformationDto payload);
		Task<ClientInformationDto> Update(ClientInformationDto payload);
		void Delete(int id);
		Task<Boolean> ChekcName(string criteria);
		Task<List<Addresses>> GetClientAddress(int clientId);
		Task<Boolean> ChekcSsn(string criteria);
		Task<List<Clients>> GetClientByCriteria(string criteria);
	}
	public class Rate
	{
		/*
            planName = "Current",
                    addOns = "",
                    cost = Member.CurrentCost,
                    Class = "",

             */
		public string company { get; set; }
		public string planName { get; set; }
		public string addOns { get; set; }
		public double cost { get; set; }
		public string Class { get; set; }
	}


	public class ClientService : IClientService {
		private readonly DataContext _context;
		private readonly AppSettings _appSettings;

		public ClientService(DataContext context, IOptions<AppSettings> appSettings) {
			_context = context;
			_appSettings = appSettings.Value;
		}

		public List<Clients> GetAll() {
			List<Clients> payload = null;

			try {

				payload = _context.Clients.Where(ag => ag.DeletedAt == null).ToList();
				//only show last 4 in client list
				payload.ForEach(each => {
					if (each.Ssn != null && each.Ssn.Length >= 9) {
						var filteredSsn = each.Ssn.Replace("-", "");
						each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
					};
				});
				return payload;
			} catch (Exception ex) {
				throw ex;
			}

		}

		public Clients GetById(int id) {
			var res = _context.Clients.Include(cl => cl.Tutors).FirstOrDefault(c => c.Id == id);
			res.Tutors.ToList().ForEach(tut => {
				tut.Client = null;
			});
			return res;
		}

		public ClientInformationDto Create(ClientInformationDto payload) {
			try {
				var newClient = new Clients();
				if (payload.PreRegister) {
					GeneralInformationBuilder(ref payload, ref newClient);
					newClient.PreRegister = true;
					newClient.UpdatedAt = DateTime.Now;
					newClient.CreatedAt = DateTime.Now;
					_context.Clients.Add(newClient);
					_context.SaveChanges();
					payload.Demographic.Id = newClient.Id;
					AddressBuilder(payload);
					TutorBuilder(payload);
					_context.SaveChanges();
				} else {
					GeneralInformationBuilder(ref payload, ref newClient);
					newClient.UpdatedAt = DateTime.Now;
					newClient.CreatedAt = DateTime.Now;
					_context.Clients.Add(newClient);
					_context.SaveChanges();
					payload.Demographic.Id = newClient.Id;
					AddressBuilder(payload);
					TutorBuilder(payload);
					ClientChapterAssociationBuilder(payload.Bonafides, newClient.Id);

					List<Dependents> dependentsList = new List<Dependents>();
					payload.Dependants.ForEach(D => {
						var dependat = new Dependents();
						dependat.Name = D.Name;
						dependat.Initial = D.Initial;
						dependat.LastName1 = D.LastName1;
						dependat.LastName2 = D.LastName2;
						dependat.Phone1 = D.Phone1;
						dependat.Phone2 = D.Phone2;
						dependat.Ssn = D.Ssn;
						dependat.UpdatedAt = D.UpdatedAt;
						dependat.AgencyId = D.AgencyId;
						dependat.BirthDate = D.BirthDate;
						dependat.City = D.City;
						dependat.CityId = D.CityId;
						dependat.ClientId = newClient.Id;
						dependat.ContractNumber = D.ContractNumber;
						dependat.CoverId = D.CoverId;
						dependat.CreatedAt = D.CreatedAt;
						dependat.DeletedAt = D.DeletedAt;
						dependat.EffectiveDate = D.EffectiveDate;
						dependat.Email = D.Email;
						dependat.Gender = D.Gender;
						dependat.Relationship = D.Relationship.Id;
						dependentsList.Add(dependat);
					});
					_context.Dependents.AddRange(dependentsList);

					_context.SaveChanges();
				}
				return payload;
			} catch (Exception ex) {
				throw ex;
			}
		}

		private void ClientChapterAssociationBuilder(List<BonaFidesDto> payload, int clientId) {
			payload.ForEach(bn => {
				var ch = new ChapterClient();
				ch.ChapterId = bn.Chapter.ChapterId.GetValueOrDefault();
				ch.ClientId = clientId;
				ch.Primary = bn.Chapter.Primary;
				ch.NewRegistration = bn.Chapter.NewRegistration;
				ch.RegistrationDate = bn.Chapter.RegistrationDate;
				ch.UpdatedAt = DateTime.Now;
				_context.ChapterClient.Add(ch);
			});
		}

		public async Task<ClientInformationDto> Update(ClientInformationDto payload) {
			try {
				if (payload.Demographic != null) {
					var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.Id == payload.Demographic.Id);
					GeneralInformationBuilder(ref payload, ref client);
					client.UpdatedAt = DateTime.Now;
					_context.Clients.Update(client);
				}
				payload = AddressBuilder(payload);
				TutorBuilder(payload);

				await _context.SaveChangesAsync();
				return payload;
			} catch (Exception ex) {
				throw ex;
			}
		}

		private ClientInformationDto AddressBuilder(ClientInformationDto payload) {
			if (payload.Address?.PhysicalAddress != null) {
				var physicalAddress = _context.Addresses.AsNoTracking().FirstOrDefault(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 1);

				if (physicalAddress != null) {
					PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
					physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
					_context.Addresses.Update(physicalAddress);
				} else {
					physicalAddress = new Addresses();
					PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
					physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
					physicalAddress.CreatedAt = DateTime.Now;
					_context.Addresses.Add(physicalAddress);
				}

				physicalAddress.UpdatedAt = DateTime.Now;

			}

			if (payload.Address?.PostalAddress != null) {
				var postalAddress = _context.Addresses.AsNoTracking().FirstOrDefault(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 2);
				if (postalAddress != null) {
					PostalAddressInformationBuilder(ref payload, ref postalAddress);
					_context.Addresses.Update(postalAddress);
				} else {
					postalAddress = new Addresses();
					PostalAddressInformationBuilder(ref payload, ref postalAddress);
					postalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
					_context.Addresses.Add(postalAddress);
				}

				postalAddress.UpdatedAt = DateTime.Now;
			}

			return payload;
		}

		private void TutorBuilder(ClientInformationDto payload) {
			var tutors = payload.Demographic.Tutors.ToArray();
			if (tutors.Length > 0) {
				if (tutors[0].Id != null) {
					var tutor = _context.Tutors.FirstOrDefault(t => t.Id == tutors[0].Id);
					if (!String.IsNullOrEmpty(tutors[0].Phone)) {
						tutor.Name = tutors[0].Name;
						tutor.LastName = tutors[0].LastName;
						tutor.Phone = tutors[0].Phone;
						_context.Tutors.Update(tutor);
					} else {
						_context.Tutors.Remove(tutor);
					}
				} else {
					if (!String.IsNullOrEmpty(tutors[0].Phone)) {
						var newTutor = new Tutors();
						newTutor.ClientId = payload.Demographic.Id.GetValueOrDefault();
						newTutor.Name = tutors[0].Name;
						newTutor.LastName = tutors[0].LastName;
						newTutor.Phone = tutors[0].Phone;
						_context.Tutors.Add(newTutor);
					}
				}
			}
		}

		private static void PhysicalAddressInformationBuilder(ref ClientInformationDto payload, ref Addresses Address) {
			Address.Line1 = payload.Address?.PhysicalAddress.Line1;
			Address.Line2 = payload.Address?.PhysicalAddress.Line2;
			Address.State = payload.Address?.PhysicalAddress.State;
			Address.City = payload.Address?.PhysicalAddress.City;
			Address.Type = 1;
			Address.Zip4 = payload.Address?.PhysicalAddress.Zip4;
			Address.Zipcode = payload.Address?.PhysicalAddress.Zipcode;
		}

		private static void PostalAddressInformationBuilder(ref ClientInformationDto payload, ref Addresses Address) {
			Address.Line1 = payload.Address?.PostalAddress.Line1;
			Address.Line2 = payload.Address?.PostalAddress.Line2;
			Address.State = payload.Address?.PostalAddress.State;
			Address.City = payload.Address?.PostalAddress.City;
			Address.Type = 2;
			Address.Zip4 = payload.Address?.PostalAddress.Zip4;
			Address.Zipcode = payload.Address?.PostalAddress.Zipcode;
		}

		private static void GeneralInformationBuilder(ref ClientInformationDto payload, ref Clients client) {
			client.Email = payload.Demographic?.Email;
			client.Gender = payload.Demographic?.Gender;
			client.Initial = payload.Demographic?.Initial;
			client.LastName1 = payload.Demographic?.LastName1;
			client.LastName2 = payload.Demographic?.LastName2;
			client.MaritalStatus = payload.Demographic?.MaritalStatus;
			client.Name = payload.Demographic?.Name;
			client.Phone1 = payload.Demographic?.Phone1;
			client.Phone2 = payload.Demographic?.Phone2;
			client.Phone2 = payload.Demographic?.Phone2;
			client.Ssn = payload.Demographic?.Ssn;
			client.EffectiveDate = payload.Demographic.EffectiveDate;
			client.BirthDate = payload.Demographic.BirthDate;
			client.MedicareA = payload.Demographic.MedicareA;
			client.MedicareB = payload.Demographic.MedicareB;

			var AgencyId = payload.Demographic.AgencyId;
			if (AgencyId != null) {
				client.AgencyId = AgencyId.GetValueOrDefault();
			} else {
				client.AgencyId = 17;
			}

			var RetirementId = payload.Demographic.RetirementId;
			if (RetirementId != null) {
				client.RetirementId = RetirementId.GetValueOrDefault();
			} else {
				client.RetirementId = 6;
			}

			var CoverId = payload.Demographic.CoverId;
			if (CoverId != null) {
				client.CoverId = CoverId.GetValueOrDefault();
			} else {
				client.CoverId = 206;
			}

			var CampaignId = payload.Demographic.CampaignId;
			if (CampaignId != null) {
				client.CampaignId = CampaignId.GetValueOrDefault();
			} else {
				client.CampaignId = 17;
			}
		}

		public void Delete(int id) {
			var item = _context.Clients.Find(id);

			if (item != null) {

				item.DeletedAt = DateTime.Now;

				_context.Clients.Update(item);
				_context.SaveChanges();
			} else
				throw new AppException("Agency not found");

		}

		public async Task<Boolean> ChekcName(string criteria) {
			if (!String.IsNullOrEmpty(criteria)) {
				criteria = criteria.ToLower().Trim();
				var payload = await _context.Clients.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
				if (payload != null) {
					return true;
				}
			}
			return false;
		}

		public async Task<List<Addresses>> GetClientAddress(int clientId) {
			try {
				var address = await _context.Addresses.Where(addr => addr.ClientId == clientId && addr.DeletedAt == null).ToListAsync();
				return address;
			} catch (Exception ex) {
				throw ex;
			}
		}

		public async Task<Boolean> ChekcSsn(string criteria) {
			if (!String.IsNullOrEmpty(criteria)) {
				criteria = criteria.ToLower().Trim();
				var payload = await _context.Clients.FirstOrDefaultAsync(ag => ag.Ssn.Replace("-", "") == criteria);
				if (payload != null) {
					return true;
				}
			}
			return false;
		}

		public async Task<List<Clients>> GetClientByCriteria(string criteria) {
			var matchingClients = await _context.Clients.Where(x =>
				x.Name.ToLower().Contains(criteria) ||
				x.Ssn.Contains(criteria) ||
				x.Phone1.Contains(criteria) ||
				x.Phone2.Contains(criteria) ||
				x.Email.Contains(criteria)
			).Take(100).OrderBy(x => x.Id).ToListAsync();
			matchingClients.ForEach(each => {
				if (each.Ssn != null && each.Ssn.Length >= 9) {
					var filteredSsn = each.Ssn.Replace("-", "");
					each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
				};
			});

			return matchingClients;
		}




		private List<Rate> getRate(Clients Member, Alianzas _estimate)
		{
			try
			{
				var Rate = new List<Rate>();

				//var Plans = (from r in _estimate.Cover
				//			 where r.GroupNumber == Member.GroupNumber
				//			 select new { insuranceAddOns = r.InsuranceAddOns.ToList(), plan = r.InsurancePlan }).ToList();

			
				//foreach (var r in Plans)
				//{
				
					var r = _estimate.Cover;
					Rate.Add(new Rate
					{
						company = _estimate.Cover.HealthPlan.Name,
						planName = _estimate.Cover.Name,
						addOns = String.Join(",", _estimate.AlianzaAddOns.Select(h => h.InsuranceAddOn.Name).ToArray()),
						cost = CalcularCost(Member, _estimate.Cover, _estimate.AlianzaAddOns.ToList(), Convert.ToDateTime(_estimate.StartDate)),
						Class = "",

					});

				
				//var u = (from r in Plans
				//     where r.count > 1
				//     select r.member).ToList();

				return Rate;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private double CalcularCost(Clients Member, Covers Plans, List<AlianzaAddOns> AddOns, DateTime EffectiveDate)
		{
			try
			{

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
					else if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.EEOnly )
					{
						cost = r.InsuranceAddOn.IndividualRate;
					}
					else if (r.InsuranceAddOn.TypeCalculate == (int)TypeCalculate.Tier )
					{
						if (Member.Dependents.ToList().Count <= 0)
							cost = r.InsuranceAddOn.CoverageSingleRate;
						else if (Member.Dependents.ToList().Count == 1)
							cost = r.InsuranceAddOn.CoverageCoupleRate;
						else if (Member.Dependents.ToList().Count > 1)
							cost = r.InsuranceAddOn.CoverageFamilyRate;
					}


					Totalcost += cost;


				}


				return Math.Round(Totalcost, 2);
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
=======
namespace WebApi.Services
{
  public interface IClientService
  {
    List<Clients> GetAll();
    Clients GetById(int id);
    Clients Create(ClientInformationDto payload);
    Task<ClientInformationDto> Update(ClientInformationDto payload);
    void Delete(int id);
    Task<Boolean> ChekcName(string criteria);
    Task<List<Addresses>> GetClientAddress(int clientId);
    Task<Boolean> ChekcSsn(string criteria);
    Task<List<Clients>> GetClientByCriteria(string criteria);
    Task Deceased(int clientId);
  }

  public class ClientService : IClientService
  {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;

    public ClientService(DataContext context, IOptions<AppSettings> appSettings)
    {
      _context = context;
      _appSettings = appSettings.Value;
    }

    public List<Clients> GetAll()
    {
      List<Clients> payload = null;

      try
      {

        payload = _context.Clients.Where(ag => ag.DeletedAt == null).ToList();
        //only show last 4 in client list
        payload.ForEach(each =>
        {
          if (each.Ssn != null && each.Ssn.Length >= 9)
          {
            var filteredSsn = each.Ssn.Replace("-", "");
            each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
          };
        });
        return payload;
      }
      catch (Exception ex)
      {
        throw ex;
      }

    }

    public Clients GetById(int id)
    {
      var res = _context.Clients.Include(cl => cl.Tutors).FirstOrDefault(c => c.Id == id);
      res.Tutors.ToList().ForEach(tut =>
      {
        tut.Client = null;
      });
      return res;
    }

    public Clients Create(ClientInformationDto payload)
    {
      try
      {
        var newClient = new Clients();
        if (payload.PreRegister)
        {
          GeneralInformationBuilder(ref payload, ref newClient);
          newClient.PreRegister = true;
          newClient.UpdatedAt = DateTime.Now;
          newClient.CreatedAt = DateTime.Now;
          _context.Clients.Add(newClient);
          _context.SaveChanges();
          payload.Demographic.Id = newClient.Id;
          AddressBuilder(payload);
          TutorBuilder(payload);
          _context.SaveChanges();
        }
        else
        {
          GeneralInformationBuilder(ref payload, ref newClient);
          newClient.UpdatedAt = DateTime.Now;
          newClient.CreatedAt = DateTime.Now;
          _context.Clients.Add(newClient);
          _context.SaveChanges();
          payload.Demographic.Id = newClient.Id;
          AddressBuilder(payload);
          TutorBuilder(payload);
          ClientChapterAssociationBuilder(payload.Bonafides, newClient.Id);

          List<Dependents> dependentsList = new List<Dependents>();
          payload.Dependants.ForEach(D =>
          {
            var dependat = new Dependents();
            dependat.Name = D.Name;
            dependat.Initial = D.Initial;
            dependat.LastName1 = D.LastName1;
            dependat.LastName2 = D.LastName2;
            dependat.Phone1 = D.Phone1;
            dependat.Phone2 = D.Phone2;
            dependat.Ssn = D.Ssn;
            dependat.UpdatedAt = D.UpdatedAt;
            dependat.AgencyId = D.AgencyId;
            dependat.BirthDate = D.BirthDate;
            dependat.City = D.City;
            dependat.CityId = D.CityId;
            dependat.ClientId = newClient.Id;
            dependat.ContractNumber = D.ContractNumber;
            dependat.CoverId = D.CoverId;
            dependat.CreatedAt = D.CreatedAt;
            dependat.DeletedAt = D.DeletedAt;
            dependat.EffectiveDate = D.EffectiveDate;
            dependat.Email = D.Email;
            dependat.Gender = D.Gender;
            dependat.Relationship = D.Relationship.Id;
            dependentsList.Add(dependat);
          });
          _context.Dependents.AddRange(dependentsList);

          _context.SaveChanges();
        }

        foreach (var item in newClient.ChapterClient)
        {
          item.Client = null;
        }

        return newClient;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private void ClientChapterAssociationBuilder(List<BonaFidesDto> payload, int clientId)
    {
      payload.ForEach(bn =>
      {
        var ch = new ChapterClient();
        ch.ChapterId = bn.Chapter.ChapterId.GetValueOrDefault();
        ch.ClientId = clientId;
        ch.Primary = bn.Chapter.Primary;
        ch.NewRegistration = bn.Chapter.NewRegistration;
        ch.RegistrationDate = bn.Chapter.RegistrationDate;
        ch.UpdatedAt = DateTime.Now;
        _context.ChapterClient.Add(ch);
      });
    }

    public async Task<ClientInformationDto> Update(ClientInformationDto payload)
    {
      try
      {
        if (payload.Demographic != null)
        {
          var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.Id == payload.Demographic.Id);
          GeneralInformationBuilder(ref payload, ref client);
          client.UpdatedAt = DateTime.Now;
          _context.Clients.Update(client);
        }
        payload = AddressBuilder(payload);
        TutorBuilder(payload);

        await _context.SaveChangesAsync();
        return payload;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private ClientInformationDto AddressBuilder(ClientInformationDto payload)
    {
      if (payload.Address?.PhysicalAddress != null)
      {
        var physicalAddress = _context.Addresses.AsNoTracking().FirstOrDefault(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 1);

        if (physicalAddress != null)
        {
          PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
          physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
          _context.Addresses.Update(physicalAddress);
        }
        else
        {
          physicalAddress = new Addresses();
          PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
          physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
          physicalAddress.CreatedAt = DateTime.Now;
          _context.Addresses.Add(physicalAddress);
        }

        physicalAddress.UpdatedAt = DateTime.Now;

      }

      if (payload.Address?.PostalAddress != null)
      {
        var postalAddress = _context.Addresses.AsNoTracking().FirstOrDefault(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 2);
        if (postalAddress != null)
        {
          PostalAddressInformationBuilder(ref payload, ref postalAddress);
          _context.Addresses.Update(postalAddress);
        }
        else
        {
          postalAddress = new Addresses();
          PostalAddressInformationBuilder(ref payload, ref postalAddress);
          postalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
          _context.Addresses.Add(postalAddress);
        }

        postalAddress.UpdatedAt = DateTime.Now;
      }

      return payload;
    }

    private void TutorBuilder(ClientInformationDto payload)
    {
      var tutors = payload.Demographic.Tutors.ToArray();
      if (tutors.Length > 0)
      {
        if (tutors[0].Id != null)
        {
          var tutor = _context.Tutors.FirstOrDefault(t => t.Id == tutors[0].Id);
          if (!String.IsNullOrEmpty(tutors[0].Phone))
          {
            tutor.Name = tutors[0].Name;
            tutor.LastName = tutors[0].LastName;
            tutor.Phone = tutors[0].Phone;
            _context.Tutors.Update(tutor);
          }
          else
          {
            _context.Tutors.Remove(tutor);
          }
        }
        else
        {
          if (!String.IsNullOrEmpty(tutors[0].Phone))
          {
            var newTutor = new Tutors();
            newTutor.ClientId = payload.Demographic.Id.GetValueOrDefault();
            newTutor.Name = tutors[0].Name;
            newTutor.LastName = tutors[0].LastName;
            newTutor.Phone = tutors[0].Phone;
            _context.Tutors.Add(newTutor);
          }
        }
      }
    }

    private static void PhysicalAddressInformationBuilder(ref ClientInformationDto payload, ref Addresses Address)
    {
      Address.Line1 = payload.Address?.PhysicalAddress.Line1;
      Address.Line2 = payload.Address?.PhysicalAddress.Line2;
      Address.State = payload.Address?.PhysicalAddress.State;
      Address.City = payload.Address?.PhysicalAddress.City;
      Address.Type = 1;
      Address.Zip4 = payload.Address?.PhysicalAddress.Zip4;
      Address.Zipcode = payload.Address?.PhysicalAddress.Zipcode;
    }

    private static void PostalAddressInformationBuilder(ref ClientInformationDto payload, ref Addresses Address)
    {
      Address.Line1 = payload.Address?.PostalAddress.Line1;
      Address.Line2 = payload.Address?.PostalAddress.Line2;
      Address.State = payload.Address?.PostalAddress.State;
      Address.City = payload.Address?.PostalAddress.City;
      Address.Type = 2;
      Address.Zip4 = payload.Address?.PostalAddress.Zip4;
      Address.Zipcode = payload.Address?.PostalAddress.Zipcode;
    }

    private static void GeneralInformationBuilder(ref ClientInformationDto payload, ref Clients client)
    {
      client.Email = payload.Demographic?.Email;
      client.Gender = payload.Demographic?.Gender;
      client.Initial = payload.Demographic?.Initial;
      client.LastName1 = payload.Demographic?.LastName1;
      client.LastName2 = payload.Demographic?.LastName2;
      client.MaritalStatus = payload.Demographic?.MaritalStatus;
      client.Name = payload.Demographic?.Name;
      client.Phone1 = payload.Demographic?.Phone1;
      client.Phone2 = payload.Demographic?.Phone2;
      client.Phone2 = payload.Demographic?.Phone2;
      client.Ssn = payload.Demographic?.Ssn;
      client.EffectiveDate = payload.Demographic.EffectiveDate;
      client.BirthDate = payload.Demographic.BirthDate;
      client.MedicareA = payload.Demographic.MedicareA;
      client.MedicareB = payload.Demographic.MedicareB;

      var AgencyId = payload.Demographic.AgencyId;
      if (AgencyId != null)
      {
        client.AgencyId = AgencyId.GetValueOrDefault();
      }
      else
      {
        client.AgencyId = 17;
      }

      var RetirementId = payload.Demographic.RetirementId;
      if (RetirementId != null)
      {
        client.RetirementId = RetirementId.GetValueOrDefault();
      }
      else
      {
        client.RetirementId = 6;
      }

      var CoverId = payload.Demographic.CoverId;
      if (CoverId != null)
      {
        client.CoverId = CoverId.GetValueOrDefault();
      }
      else
      {
        client.CoverId = 206;
      }

      var CampaignId = payload.Demographic.CampaignId;
      if (CampaignId != null)
      {
        client.CampaignId = CampaignId.GetValueOrDefault();
      }
      else
      {
        client.CampaignId = 17;
      }
    }

    public void Delete(int id)
    {
      var item = _context.Clients.Find(id);

      if (item != null)
      {

        item.DeletedAt = DateTime.Now;

        _context.Clients.Update(item);
        _context.SaveChanges();
      }
      else
        throw new AppException("Agency not found");

    }

    public async Task<Boolean> ChekcName(string criteria)
    {
      if (!String.IsNullOrEmpty(criteria))
      {
        criteria = criteria.ToLower().Trim();
        var payload = await _context.Clients.FirstOrDefaultAsync(ag => ag.Name.ToLower().Trim() == criteria);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

    public async Task<List<Addresses>> GetClientAddress(int clientId)
    {
      try
      {
        var address = await _context.Addresses.Where(addr => addr.ClientId == clientId && addr.DeletedAt == null).ToListAsync();
        return address;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public async Task<Boolean> ChekcSsn(string criteria)
    {
      if (!String.IsNullOrEmpty(criteria))
      {
        criteria = criteria.ToLower().Trim();
        var payload = await _context.Clients.FirstOrDefaultAsync(ag => ag.Ssn.Replace("-", "") == criteria && ag.DeletedAt == null);
        if (payload != null)
        {
          return true;
        }
      }
      return false;
    }

    public async Task<List<Clients>> GetClientByCriteria(string criteria)
    {
      var matchingClients = await _context.Clients.Where(x =>
        x.Name.ToLower().Contains(criteria) ||
        x.Ssn.Contains(criteria) ||
        x.Phone1.Contains(criteria) ||
        x.Phone2.Contains(criteria) ||
        x.Email.Contains(criteria)
      ).Take(100).OrderBy(x => x.Id).ToListAsync();
      matchingClients.ForEach(each =>
      {
        if (each.Ssn != null && each.Ssn.Length >= 9)
        {
          var filteredSsn = each.Ssn.Replace("-", "");
          each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
        };
      });

      return matchingClients;
    }

    public async Task Deceased(int clientId)
    {
      var client = await _context.Clients.FirstOrDefaultAsync(x => x.Id == clientId);
      if (client == null)
        throw new Exception("No Client with provided id");
      client.Status = 3;
      _context.Clients.Update(client);
      await _context.SaveChangesAsync();
    }
  }
>>>>>>> ca88cee6fecd415628af5a50c4d5db8b71400807
}