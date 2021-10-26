using System;
using System.Collections.Generic;

using DemoFIN3.Core.Models;

namespace DemoFIN3.Core.Repositories.Interface
{
    public interface IAuthorRepository : IDisposable
    {
        Author Find(int authorId);
        void AddAuthor(Author author);
        void UpdateAuthor(Author author);
        void DeleteAuthor(Author author);
        void DeleteAuthor(int authorId);
        IList<Author> GetAllAuthors();
    }
}
