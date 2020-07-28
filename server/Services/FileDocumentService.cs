using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities.Documents;
using WebApi.Entities.Files;
using WebApi.Helpers;

namespace WebApi.Services
{

    public interface IFileDocumentService
    {
        IQueryable<FileDocument> GetAll(File file, bool onlyActive);
        FileDocument GetById(int id);
        //FileDocument Create(FileDocument payload);
        List<FileDocument> Create(FileDocument fileDocument, List<IFormFile> files);
        FileDocument Update(int id, FileDocument payload);
        FileDocument Delete(int id, int userId);
    }

    public class FileDocumentService : IFileDocumentService
    {

        private IDocumentService _documentService;
        private readonly DataContext _context;

        public FileDocumentService(DataContext context, IDocumentService documentService)
        {
            _context = context;
            _documentService = documentService;

        }

        public IQueryable<FileDocument> GetAll(File file, bool onlyActive)
        {

            try
            {

                IQueryable<FileDocument> res = _context.FileDocument.Include(x => x.Document).Include(x => x.CreatedUser).Include(x => x.UpdateUser).Include(x => x.DocumentType)
                    .Where(s => s.ApplicationId == file.ApplicationId && s.Key == file.Key);

                 //search by #DelFlag
                if (onlyActive)
                {
                    res = res.Where(s => s.DelFlag == !onlyActive);
                }

                 res = res.OrderByDescending(x => x.UpdDt);

                return res.AsNoTracking();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileDocument GetById(int id)
        {

            FileDocument res;
            res = _context.FileDocument.Include(x=> x.Document).Include(x=> x.DocumentType).Where(x => x.Id == id).FirstOrDefault();

            if (res != null) {
                res.Document = _documentService.GetById(res.DocumentId);
            }

            return res;
        }

        //public FileDocument Create(FileDocument payload)
        //{


        //    payload.Document.ApplicationId = payload.ApplicationId;
        //    payload.Document.FCreateUserId = payload.FCreateUserId;
        //    payload.Document.FUpdUserId = payload.FCreateUserId;

        //    var document = _documentService.Create(payload.Document);

        //    payload.DocumentId = document.Id;

        //    _context.FileDocument.Add(payload);
        //    _context.SaveChanges();

        //    return payload;

        //}

        public List<FileDocument> Create(FileDocument fileDocument, List<IFormFile> files)
        {

            List<FileDocument> list = new List<FileDocument>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {

                    var fd = new FileDocument
                    {
                        ApplicationId = fileDocument.ApplicationId,
                        Key = fileDocument.Key,
                        Origin = fileDocument.Origin,
                        FCreateUserId = fileDocument.FCreateUserId,
                        FUpdUserId = fileDocument.FUpdUserId
                    };
                    var document = new Document
                    {
                        ApplicationId = fileDocument.ApplicationId,
                        FCreateUserId = fileDocument.FCreateUserId,
                        FUpdUserId = fileDocument.FCreateUserId,
                        File = file
                    };

                    var res = _documentService.CreateAsync(fileDocument.Key, document);

                    if (res != null)
                    {
                        fd.DocumentId = res.Id;
                        list.Add(fd);
                    }

                }

            }

            _context.FileDocument.AddRange(list);
            _context.SaveChanges();

            return list;

        }

        public FileDocument Update(int id, FileDocument payload)
        {
            var res = _context.FileDocument.Find(id);

            if (res != null)
            {

                res.DocumentTypeId = payload.DocumentTypeId;
                res.DelFlag = payload.DelFlag;
                res.UpdDt = DateTime.Now;
                res.FUpdUserId = payload.FUpdUserId;

                _context.FileDocument.Update(res);
                _context.SaveChanges();

            }

            return res;
        }

        public FileDocument Delete(int id, int userId)
        {
            var res = _context.FileDocument.Find(id);

            if (res != null)
            {

                res.UpdateUser.FUpdUserId = userId;
                res.UpdDt = DateTime.Now;
                res.DelFlag = true;

                _context.FileDocument.Update(res);
                _context.SaveChanges();

            }

            return res;

        }

    }
}
