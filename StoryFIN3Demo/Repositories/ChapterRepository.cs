using DemoFIN3.Core.Models;
using DemoFIN3.Core.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFIN3.Core.Repositories
{
    /**
     * ChapterRepository
     * 
     * Version 1.0
     * 
     * Date: 15-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 15-09-2021     NghiaTNT         ChapterRepository
     */
    public class ChapterRepository : IChapterRepository
    {
        private readonly DemoFIN3Context db;

        public ChapterRepository()
        {
            db = new DemoFIN3Context();
        }
        public void AddChapter(Chapter chapter)
        {
            throw new NotImplementedException();
        }

        public void DeleteChapter(Chapter chapter)
        {
            throw new NotImplementedException();
        }

        public void DeleteChapter(int chapterId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Chapter Find(int chapterId)
        {
            return db.Chapters.Find(chapterId);
        }

        public IList<Chapter> GetAllChapters()
        {
            return db.Chapters.ToList();
        }

        /// <summary>
        /// List chapters for story
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="urlSlug"></param>
        /// <returns></returns>
        public IList<Chapter> GetChaptersForStory(int year, int month, string urlSlug, int id)
        {
            var story = db.Stories.FirstOrDefault<Story>(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug == urlSlug && p.Id == id);
            return story.Chapters.OrderBy(s => s.ChapterNumber).ToList();
        }

        public IList<Chapter> GetChaptersForStory(Story story)
        {
            return story.Chapters.ToList();
        }

        public void UpdateChapter(Chapter chapter)
        {
            throw new NotImplementedException();
        }
    }
}
