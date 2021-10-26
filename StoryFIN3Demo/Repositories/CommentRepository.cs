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
     * CommentRepository
     * 
     * Version 1.0
     * 
     * Date: 18-09-2021
     * 
     * Copyright
     * 
     * Modification Logs:
     * DATE         AUTHOR          DESCRIPTION
     * -----------------------------------------
     * 18-09-2021     NghiaTNT         CommentRepository
     */
    public class CommentRepository : ICommentRepository
    {
        private readonly DemoFIN3Context db = new DemoFIN3Context();

        public void AddComment(Comment comment)
        {
            if (comment.CommentTime == null)
            {
                comment.CommentTime = DateTime.Now;
            }

            db.Comments.Add(comment);
            db.Entry(comment).State = EntityState.Added;
            db.SaveChanges();
        }

        /// <summary>
        /// Add a comment
        /// </summary>
        /// <param name="storyId"></param>
        /// <param name="accountEmail"></param>
        /// <param name="commentHeader"></param>
        /// <param name="commentText"></param>
        public void AddComment(int storyId, string accountEmail, string commentHeader, string commentText)
        {
            var story = db.Stories.Find(storyId);
            var account = db.Users.Where(u => u.Email == accountEmail);
            Comment comment = new Comment();
            comment.CommentHeader = commentHeader;
            comment.CommentText = commentText;
            comment.Account = (Account)account;
            story.Comments.Add(comment);
            comment.CommentTime = DateTime.Now;
            db.Comments.Add(comment);
            db.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void DeleteComment(int commendId)
        {
            throw new NotImplementedException();
        }

        public Comment Find(int commentId)
        {
            return db.Comments.Find(commentId);
        }

        public IList<Comment> GetAllComments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get list comments approved to show
        /// </summary>
        /// <param name="storyId"></param>
        /// <returns></returns>
        public IList<Comment> GetCommentsForStory(int storyId)
        {
            return db.Comments.Include(c => c.Account).Include(c => c.Story).Where(c => c.StoryId == storyId).Where(c => c.CommentStatus == Enum.CommentStatus.Approved).ToList();
        }

        public IList<Comment> GetCommentsForStory(Story story)
        {
            return story.Comments.ToList();
        }

        public void UpdateComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void AddComment(int storyId, string commentTitle, string commentBody)
        {
            throw new NotImplementedException();
        }

        void ICommentRepository.AddComment(int storyId, string accountEmail, string commentHeader, string commentText)
        {
            throw new NotImplementedException();
        }
    }
}

