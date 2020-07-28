using System.Linq;
using Microsoft.Extensions.Options;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IApplicationService
    {
        Application GetById(int id);
        Application GetByKey(string key);
    }

    public class ApplicationService : IApplicationService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;

        public ApplicationService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public Application GetById(int id)
        {
            var res = _context.Application.Find(id);
            return res;
        }

        public Application GetByKey(string key)
        {
            var res = _context.Application.Where(x=> x.Key == key && x.DelFlag == false).FirstOrDefault();
            return res;
        }
    }
}
