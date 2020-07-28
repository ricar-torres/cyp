using System;
namespace WebApi.Dtos
{
    public class RoleFilterDto
    {

        public int[] Id { get; set; }
        public string Name { get; set; }
        public bool? DelFlag { get; set; }

    }
}
