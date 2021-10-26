using System;
using System.Collections.Generic;

using DemoFIN3.Core.Models;

namespace DemoFIN3.Core.Repositories.Interface
{
    public interface ICommentRepository : IDisposable
    {
        Comment Find(int commentId);
        void AddComment(Comment comment);
        void AddComment(int storyId, string accountEmail, string commentHeader, string commentText);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
        void DeleteComment(int commendId);
        IList<Comment> GetAllComments();
        IList<Comment> GetCommentsForStory(int storyId);
        IList<Comment> GetCommentsForStory(Story story);
    }
}
