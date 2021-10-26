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
     * StoryRepository
     * 
     * Version 1.0
     * 
     * Date: 12-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 12-09-2021     NghiaTNT         StoryRepository
     */
    public class StoryRepository : IStoryRepository
    {
        private readonly DemoFIN3Context db;
        public StoryRepository()
        {
            db = new DemoFIN3Context();
        }

        public StoryRepository(DemoFIN3Context db)
        {
            this.db = db;
        }
        public void AddStory(Story story)
        {
            db.Stories.Add(story);
            db.SaveChanges();
        }

        /// <summary>
        /// Count stories for Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int CountStoriesForCategory(string category)
        {
            return db.Stories.Count(p => p.Category.Name == category);
        }

        public void DeleteStory(Story story)
        {
            var item = db.Stories.Find(story.Id);
            db.Stories.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete story by Id
        /// </summary>
        /// <param name="storyId"></param>
        public void DeleteStory(int storyId)
        {
            var item = db.Stories.Find(storyId);
            db.Stories.Remove(item);
            db.SaveChanges();
        }

        /// <summary>
        /// Delete story = set story status (isDelete == true)
        /// </summary>
        /// <param name="storyId"></param>
        public void DeleteLogicStory(int storyId)
        {
            var item = db.Stories.Find(storyId);
            item.isDelete = true;
            db.SaveChanges();
        }

        /// <summary>
        /// Find story by ...
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="urlSlug"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Story FindStory(int year, int month, string urlSlug, int id)
        {
            var story = db.Stories.Include(p => p.Author).Include(p => p.Category).Include(p => p.Source).Include(p => p.Chapters).FirstOrDefault<Story>(p => !p.isDelete && p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug == urlSlug && p.Id == id);
            story.ViewCount++;
            db.SaveChanges();
            return story;
        }

        public Story FindStory(int storyId)
        {
            return db.Stories.Find(storyId);
        }

        public IList<Story> GetAllStories()
        {
            return db.Stories.ToList();
        }

        /// <summary>
        /// List stories OrderByDescending rate
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public IList<Story> GetHighestStories(int size)
        {
            return db.Stories.Include(p => p.Author).Include(p => p.Category).Where(p => !p.isDelete).OrderByDescending(p => p.Rate).Take(size).ToList();
        }

        /// <summary>
        /// List stories OrderByDescending Update date
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public IList<Story> GetLatestStory(int size)
        {
            return db.Stories.Include(s => s.Author).Include(s => s.Category).Where(p => !p.isDelete).OrderByDescending(p => p.UpdatedOn).Take(size).ToList();
        }

        /// <summary>
        /// List stories OrderByDescending View count
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public IList<Story> GetMostViewedStory(int size)
        {
            return db.Stories.Include(p => p.Author).Include(p => p.Category).Where(p => !p.isDelete).OrderByDescending(p => p.ViewCount).Take(size).ToList();
        }

        public IList<Story> GetPublisedStories()
        {
            return db.Stories.Where(p => !p.isDelete).OrderByDescending(p => p.PostedOn).ToList();
        }

        /// <summary>
        /// List stories by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public IList<Story> GetStoriesByCategory(string category)
        {
            return db.Stories.Include(s => s.Category).Include(s => s.Author).Where(p => !p.isDelete && p.Category.Name == category).ToList();
        }

        /// <summary>
        /// List stories by author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public IList<Story> GetStoriesByAuthor(string author)
        {
            return db.Stories.Include(s => s.Author).Where(p => !p.isDelete && p.Author.Name == author).ToList();
        }

        public IList<Story> GetStoriesByMonth(DateTime monthYear)
        {
            var fromDate = new DateTime(monthYear.Year, monthYear.Month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);
            return db.Stories.Include(p => p.Author).Include(p => p.Category).Where(p => p.PostedOn >= fromDate && p.PostedOn <= toDate).ToList();
        }

        public IList<Story> GetUnpublisedStories()
        {
            return db.Stories.Where(p => p.isDelete).OrderByDescending(p => p.PostedOn).ToList();
        }

        public void UpdateStory(Story story)
        {
            db.Entry(story).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
