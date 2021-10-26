using DemoFIN3.Core.Models;
using DemoFIN3.Core.Repositories.Interface;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DemoFIN3.Core.Repositories
{
    /**
     * AuthorRepository
     * 
     * Version 1.0
     * 
     * Date: 14-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 14-09-2021     NghiaTNT     AuthorRepository
     */
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DemoFIN3Context db;

        public AuthorRepository()
        {
            db = new DemoFIN3Context();
        }

        /// <summary>
        /// Add author into db
        /// </summary>
        /// <param name="author"></param>
        public void AddAuthor(Author author)
        {
            db.Authors.Add(author);
            db.SaveChanges();
        }

        /// <summary>
        /// delete an author
        /// </summary>
        /// <param name="author"></param>
        public void DeleteAuthor(Author author)
        {
            var item = db.Authors.Find(author.Id);
            db.Authors.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete an author by AuthorId
        /// </summary>
        /// <param name="authorId"></param>
        public void DeleteAuthor(int authorId)
        {
            var item = db.Authors.Find(authorId);
            db.Authors.Remove(item);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        /// <summary>
        /// Find author by Id
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public Author Find(int authorId)
        {
            return db.Authors.Find(authorId);
        }

        /// <summary>
        /// Get list authors
        /// </summary>
        /// <returns></returns>
        public IList<Author> GetAllAuthors()
        {
            return db.Authors.ToList();
        }

        /// <summary>
        /// Update author (edit)
        /// </summary>
        /// <param name="author"></param>
        public void UpdateAuthor(Author author)
        {
            db.Entry(author).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
