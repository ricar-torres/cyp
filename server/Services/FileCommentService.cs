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

    public interface IFileCommentService
    {
        IQueryable<FileComment> GetAll(File file);
        FileComment GetById(int id);
        FileComment Create(FileComment payload);
        FileComment Update(FileComment payload);
        FileComment Delete(int id);
    }

    public class FileCommentService : IFileCommentService
    {

        private readonly DataContext _context;

        public FileCommentService(DataContext context)
        {
            _context = context;
      
        }

        public IQueryable<FileComment> GetAll(File file)
        {

            try
            {

                IQueryable<FileComment> res = _context.FileComment;
                res = _context.FileComment.Include(x=> x.CreatedUser).Where(s => s.ApplicationId == file.ApplicationId && s.Key == file.Key);

                return res.AsNoTracking(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FileComment GetById(int id)
        {

            FileComment res;
            res = _context.FileComment.Find(id);

            return res;
        }

        public FileComment Create(FileComment payload)
        {

            _context.FileComment.Add(payload);
            _context.SaveChanges();

            return payload;

        }

        public FileComment Update(FileComment payload)
        {
            return new FileComment();
        }

        public FileComment Delete(int id)
        {
            var res = _context.FileComment.Find(id);

            if (res != null)
            {

                res.UpdDt = DateTime.Now;
                res.DelFlag = true;

                _context.FileComment.Update(res);
                _context.SaveChanges();

            }

            return res;

        }

    }

}

