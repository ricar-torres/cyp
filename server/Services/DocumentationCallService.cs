using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Entities;
using WebApi.Helpers;

namespace server.Services {
  public interface IDocumentationCallService {
    IQueryable<ClientUser> GetAll();
    IQueryable<ClientUser> GetAllByClient(int clientId);
    ClientUser GetById(int id);
    ClientUser Create(ClientUser payload);
    IQueryable<CallReasons> GetCallReasons();
    Task<string> CreateConfirmationCode();

  }
  public class DocumentationCallService : IDocumentationCallService {
    private readonly DataContext _context;
    private readonly AppSettings _appSettings;
    public DocumentationCallService(DataContext context, IOptions<AppSettings> appSettings) {
      this._appSettings = appSettings.Value;
      this._context = context;
    }

    public ClientUser Create(ClientUser payload) {
      try {

        payload.CreatedAt = DateTime.Now;
        _context.ClientUser.Add(payload);
        _context.SaveChanges();

        return payload;

      } catch (Exception ex) {

        throw ex;
      }
    }

    public IQueryable<ClientUser> GetAll() {
      IQueryable<ClientUser> result;
      try {
        result = _context.ClientUser.AsQueryable();
      } catch (Exception ex) {
        throw ex;
      }
      return result.AsNoTracking();
    }

    public IQueryable<ClientUser> GetAllByClient(int clientId) {
      IQueryable<ClientUser> result;
      try {
        result = _context.ClientUser.AsQueryable();
      } catch (Exception ex) {
        throw ex;
      }
      return result.AsNoTracking();
    }
    public ClientUser GetById(int id) {
      var res = _context.ClientUser.Find(id);
      return res;
    }

    public IQueryable<CallReasons> GetCallReasons() {
      return _context.CallReasons.AsNoTracking();
    }
    public async Task<string> CreateConfirmationCode() {
      bool exist = true;
      Random random;
      int pin;
      string code = string.Empty;
      DateTime date;

      date = DateTime.Now;
      random = new Random(date.TimeOfDay.Milliseconds);
      do {
        pin = random.Next(1000, 9999);
        code = date.ToString("yy") +
          date.ToString("MM") +
          date.ToString("dd") +
          pin;
        //Check if code exist
        exist = await _context.ClientUser.Where(cu => cu.ConfirmationNumber == code).FirstOrDefaultAsync() is object;
        System.Console.WriteLine("Exist: {0}, code: {1}", exist.ToString(), code);
      } while (exist);

      return code;
    }
  }
}