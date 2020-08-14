using System;
using WebApi.Entities;

namespace server.Dtos
{

  public class PhysicalPostalAddresses
  {
    public Addresses PhysicalAddress { get; set; }
    public Addresses PostalAddress { get; set; }
  }


  public class ClientInformationDto
  {
    public bool PreRegister { get; set; }
    public ClientsDto Demographic { get; set; }
    public PhysicalPostalAddresses Address { get; set; }
  }
}