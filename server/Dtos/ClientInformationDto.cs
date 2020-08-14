using System;
using WebApi.Entities;

namespace server.Dtos
{

  public class PhysicalPostalAddresses
  {
    public AddressesDto PhysicalAddress { get; set; }
    public AddressesDto PostalAddress { get; set; }
  }


  public class ClientInformationDto
  {
    public bool PreRegister { get; set; }
    public ClientsDto Demographic { get; set; }
    public PhysicalPostalAddresses Address { get; set; }
  }
}