using System.Linq;

namespace Project.UserControls
{
    public class PostRepository
    {
        private readonly PostDbContext _dbContext;

        public PostRepository(PostDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Post GetPost(int postId)
        {
            return _dbContext.Posts.SingleOrDefault(p => p.Id == postId);
        }

        public void SavePost(Post post)
        {
            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();
        }
    }

}