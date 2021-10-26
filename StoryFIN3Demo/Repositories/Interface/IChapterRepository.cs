using DemoFIN3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFIN3.Core.Repositories.Interface
{
    public interface IChapterRepository : IDisposable
    {
        Chapter Find(int chapterId);
        void AddChapter(Chapter chapter);
        void UpdateChapter(Chapter chapter);
        void DeleteChapter(Chapter chapter);
        void DeleteChapter(int chapterId);
        IList<Chapter> GetAllChapters();
        IList<Chapter> GetChaptersForStory(int year, int month, string urlSlug, int id);
        IList<Chapter> GetChaptersForStory(Story story);
    }
}
