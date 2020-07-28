using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApi.Dtos;
using WebApi.Entities.Documents;
using WebApi.Entities.Files;
using WebApi.Entities.Identity;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IDocumentService
    {
        //IQueryable<FileComment> GetAll(File file);
        Document GetById(int id);
        Document CreateAsync(string key, Document document);
        //Task<Document> CreateAsync(IFormFile file);
        //FileComment Update(FileComment payload);
        //FileComment Delete(int id);
        IQueryable<DocumentType> GetTypes(int applicationId);
        DocumentType GetTypeById(int id);
        DocumentType CreateType(DocumentType payload);
        DocumentType UpdateType(int id, DocumentType payload);
    }

    public class DocumentService : IDocumentService
    {
        private readonly DataContext _context;
        private readonly AppSettings _appSettings;

        public DocumentService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public Document GetById(int id)
        {

            Document res;
            res = _context.Document.Find(id);

            return res;
        }

        public Document CreateAsync(string key,Document document)
        {

            if (document.File.Length > 0)
            {
                var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{Path.GetExtension(document.File.FileName)}";
                var directory = $"{_appSettings.DocumentsPath}//FILE-{key}//";
                var filePath = $"{directory}//{fileName}";
                Directory.CreateDirectory(directory);

                using (var stream = System.IO.File.Create(filePath))
                {
                    document.File.CopyTo(stream);
                }

                document.Path = filePath;
                document.Ext = document.File.ContentType;
                document.Name = document.File.FileName;

                _context.Document.Add(document);
                _context.SaveChanges();
                //_context.Dispose();

                return document;

            }

            return null;
        }

        //public async Task<Document> CreateAsync(,List<IFormFile> files)
        //{


        //    long size = files.Sum(f => f.Length);

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = Path.GetTempFileName();

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    if (file.Length > 0)
        //    {
        //        var filePath = $"//Users/gux//Develop//Git/innovasyst//crm//docs//{file.FileName}"; //Path.GetTempFileName();

        //        using (var stream = System.IO.File.Create(filePath))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }

        //    // Process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    //return Ok(new { count = files.Count, size });

        //    //_context.Document.Add(payload);
        //    //_context.SaveChanges();

        //    return new Document();

        //}

        public IQueryable<DocumentType> GetTypes(int applicationId)
        {

            try
            {

                IQueryable<DocumentType> res = _context.DocumentType;
                res = _context.DocumentType.Where(s => s.ApplicationId == applicationId ); //&& s.DelFlag == false

                return res.AsNoTracking();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DocumentType GetTypeById(int id)
        {

            DocumentType res;
            res = _context.DocumentType.Find(id);

            return res;
        }

        public DocumentType CreateType(DocumentType payload)
        {

            _context.DocumentType.Add(payload);
            _context.SaveChanges();

            return payload;

        }

        public DocumentType UpdateType(int id, DocumentType payload)
        {
            var res = _context.DocumentType.Find(id);

            if (res != null)
            {

                res.Name = payload.Name;
                res.UpdDt = DateTime.Now;
                res.DelFlag = payload.DelFlag;
                
                _context.DocumentType.Update(res);
                _context.SaveChanges();

            }

            return res;

        }



    }
}
