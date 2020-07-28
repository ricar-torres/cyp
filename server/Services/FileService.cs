using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Dtos;
using WebApi.Entities.Files;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IFileService
    {
        IQueryable<File> GetAll(FileFilterDto filter, string sort, string order);
        File GetById(int id);
        File GetByKey(int applicationId, string key);
        File Create(File payload);
        File Update(int id, File payload);
        File Delete(int id);
    }

    public class FileService : IFileService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;
        private IMapper _mapper;

        public FileService(DataContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _mapper = mapper;

        }

        public IQueryable<File> GetAll(FileFilterDto filter, string sort, string order)
        {

            try
            {

                IQueryable<File> res = _context.File;

                res = _context.File.Where(s => s.ApplicationId == filter.ApplicationId);

                //search by #DelFlag
                if (filter.OnlyActive)
                {
                    res = res.Where(s => s.DelFlag == !filter.OnlyActive);
                }

                //search by #Key
                if (!string.IsNullOrWhiteSpace(filter.Key))
                {
                    res = res.Where(s => s.Key == filter.Key);
                }

                //search by #FirstName
                if (!string.IsNullOrWhiteSpace(filter.FirstName))
                {
                    res = res.Where(s => s.FirstName.ToLower().Contains(filter.FirstName.ToLower()));
                }

                //search by #LastName
                if (!string.IsNullOrWhiteSpace(filter.LastName))
                {
                    res = res.Where(s => s.LastName.ToLower().Contains(filter.LastName.ToLower()));
                }

                //search by #Reference
                if (!string.IsNullOrWhiteSpace(filter.Reference))
                {
                    res = res.Where(s => s.Reference1.ToLower().Contains(filter.Reference.ToLower()) || s.Reference2.Contains(filter.Reference.ToLower()));
                }

                switch ((sort ?? "").ToLower())
                {

                    case "id":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.Id);
                        else
                            res = res.OrderBy(s => s.Id);
                        break;
                    case "key":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.Key);
                        else
                            res = res.OrderBy(s => s.Id);
                        break;
                    case "fullname":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.LastName);
                        else
                            res = res.OrderBy(s => s.LastName);
                        break;
                    case "reference1":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.Reference1);
                        else
                            res = res.OrderBy(s => s.Reference1);
                        break;
                    case "reference2":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.Reference2);
                        else
                            res = res.OrderBy(s => s.Reference2);
                        break;
                    case "type":
                        if (order.ToLower() == "desc")
                            res = res.OrderByDescending(s => s.Type);
                        else
                            res = res.OrderBy(s => s.Type);
                        break;
                    default:
                        res = res.OrderBy(s => s.Id);
                        break;
                }

                return res.AsNoTracking();

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public File GetById(int id)
        {

            File res;

            res = _context.File.Find(id);

            if (res != null)
            {

                res.Relations = _context.FileRelation
                    .Where(b => b.ApplicationId == res.ApplicationId && b.Key == res.Key)
                    .ToList();

                for (int i = 0; i < res.Relations.Count(); i++)
                {
                    var appId = res.Relations[i].ApplicationId;
                    var relKey = res.Relations[i].RelationKey;
                    var RelFile = _context.File.Where(x => x.ApplicationId == appId && x.Key == relKey).FirstOrDefault();

                    res.Relations[i].RelationFile = _mapper.Map<FileDto>(RelFile);

                }

            }

            return res;
        }

        public File GetByKey(int applicationId,string key)
        {

            var res = _context.File.Where(x=> x.ApplicationId == applicationId && x.Key == key).FirstOrDefault();
            return res;
        }

        public File Create(File payload)
        {
            return new File();
        }

        public File Update(int id,File payload)
        {
            var res = _context.File.Find(id);

            if (res != null)
            {

                res.Email = payload.Email;

                _context.File.Update(res);
                _context.SaveChanges();

            }

            return res;
        }

        public File Delete(int id)
        {
            var file = _context.File.Find(id);

            if (file != null)
            {

                file.UpdDt = DateTime.Now;
                file.DelFlag = true;

                _context.File.Update(file);
                _context.SaveChanges();

            }

            return file;

        }

    }
}
