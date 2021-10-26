using DemoFIN3.Core.Models;
using DemoFIN3.Core.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFIN3.Core.Repositories
{
    /**
     * CategoryRepository
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
     * 14-09-2021     NghiaTNT         CategoryRepository
     */
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DemoFIN3Context db;

        public CategoryRepository()
        {
            db = new DemoFIN3Context();
        }

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="category"></param>
        public void DeleteCategory(Category category)
        {
            var item = db.Categories.Find(category.Id);
            db.Categories.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a category by Id
        /// </summary>
        /// <param name="categoryId"></param>
        public void DeleteCategory(int categoryId)
        {
            var item = db.Categories.Find(categoryId);
            db.Categories.Remove(item);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public Category Find(int categoryId)
        {
            return db.Categories.Find(categoryId);
        }

        /// <summary>
        /// Get list categories
        /// </summary>
        /// <returns></returns>
        public IList<Category> GetAllCategories()
        {
            return db.Categories.ToList();
        }

        /// <summary>
        /// Update category - Edit
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}
