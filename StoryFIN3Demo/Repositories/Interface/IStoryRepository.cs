using System;
using System.Collections.Generic;

using DemoFIN3.Core.Models;

namespace DemoFIN3.Core.Repositories.Interface
{
    public interface IStoryRepository : IDisposable
    {
        Story FindStory(int year, int month, string urlSlug, int id);
        Story FindStory(int storyId);
        void AddStory(Story story);
        void UpdateStory(Story story);
        void DeleteStory(Story story);
        void DeleteStory(int storyId);
        void DeleteLogicStory(int storyId);
        IList<Story> GetAllStories();
        IList<Story> GetLatestStory(int size);
        IList<Story> GetPublisedStories();
        IList<Story> GetUnpublisedStories();
        IList<Story> GetStoriesByMonth(DateTime monthYear);
        int CountStoriesForCategory(string category);
        IList<Story> GetStoriesByCategory(string category);
        IList<Story> GetStoriesByAuthor(string author);
        IList<Story> GetMostViewedStory(int size);
        IList<Story> GetHighestStories(int size);
    }
}
