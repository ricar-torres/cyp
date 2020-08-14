using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using server.Dtos;
using WebApi.Entities;
using WebApi.Helpers;

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
				payload.ForEach(each => {
					if (each.Ssn != null && each.Ssn.Length >= 9) {
						var filteredSsn = each.Ssn.Replace("-", "");
						each.Ssn = "XXX-XX-" + filteredSsn.Substring(5, 4);
					};

				});

			} catch (Exception ex) {
				throw ex;
			}

			return payload;
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
					TutorBuilder(payload);
					_context.SaveChanges();
				} else {

				}
				return payload;
			} catch (Exception ex) {
				throw ex;
			}
		}

		public async Task<ClientInformationDto> Update(ClientInformationDto payload) {
			try {
				if (payload.Demographic != null) {
					var client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(cl => cl.Id == payload.Demographic.Id);
					GeneralInformationBuilder(ref payload, ref client);
					client.UpdatedAt = DateTime.Now;
					_context.Clients.Update(client);
				}

				if (payload.Address?.PhysicalAddress != null) {
					var physicalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 1);

					if (physicalAddress != null) {
						PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
						physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
						_context.Addresses.Update(physicalAddress);
					} else {
						physicalAddress = new Addresses();
						PhysicalAddressInformationBuilder(ref payload, ref physicalAddress);
						physicalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
						physicalAddress.CreatedAt = DateTime.Now;
						await _context.Addresses.AddAsync(physicalAddress);
					}

					physicalAddress.UpdatedAt = DateTime.Now;

				}

				if (payload.Address?.PostalAddress != null) {
					var postalAddress = await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(cl => cl.ClientId == payload.Demographic.Id && cl.Type == 2);
					if (postalAddress != null) {
						PostalAddressInformationBuilder(ref payload, ref postalAddress);
						_context.Addresses.Update(postalAddress);
					} else {
						postalAddress = new Addresses();
						PostalAddressInformationBuilder(ref payload, ref postalAddress);
						postalAddress.ClientId = payload.Demographic.Id.GetValueOrDefault();
						await _context.Addresses.AddAsync(postalAddress);
					}

					postalAddress.UpdatedAt = DateTime.Now;
				}

				TutorBuilder(payload);

				await _context.SaveChangesAsync();
				return payload;
			} catch (Exception ex) {
				throw ex;
			}
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
			Address.Type = payload.Address?.PhysicalAddress.Type;
			Address.Zip4 = payload.Address?.PhysicalAddress.Zip4;
			Address.Zipcode = payload.Address?.PhysicalAddress.Zipcode;
		}

		private static void PostalAddressInformationBuilder(ref ClientInformationDto payload, ref Addresses Address) {
			Address.Line1 = payload.Address?.PostalAddress.Line1;
			Address.Line2 = payload.Address?.PostalAddress.Line2;
			Address.State = payload.Address?.PostalAddress.State;
			Address.City = payload.Address?.PostalAddress.City;
			Address.Type = payload.Address?.PostalAddress.Type;
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
			client.CoverId = payload.Demographic.CoverId;
			client.AgencyId = payload.Demographic.AgencyId;
			client.EffectiveDate = payload.Demographic.EffectiveDate;
			client.MedicareA = payload.Demographic.MedicareA;
			client.MedicareB = payload.Demographic.MedicareB;

			var AgencyId = payload.Demographic.AgencyId;
			if (AgencyId != 0) {
				client.AgencyId = AgencyId;
			} else {
				client.AgencyId = 17;
			}

			var RetirementId = payload.Demographic.RetirementId;
			if (RetirementId != 0) {
				client.RetirementId = RetirementId;
			} else {
				client.RetirementId = 6;
			}

			var CoverId = payload.Demographic.CoverId;
			if (CoverId != 0) {
				client.CoverId = CoverId;
			} else {
				client.CoverId = 206;
			}

			var CampaignId = payload.Demographic.CampaignId;
			if (CampaignId != 0) {
				client.CampaignId = CampaignId;
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
	}

}