using Fiorello_PB101.Data;
using Fiorello_PB101.Models;
using Fiorello_PB101.Services.Interfaces;
using Fiorello_PB101.ViewModels.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Fiorello_PB101.Services
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        public BlogService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BlogVM>> GetAllAsync(int? take = null)
        {
            IEnumerable<Blog> blogs;

            if ( take is null) 
            {
                 blogs= await _context.Blogs.ToListAsync();
            }
            else
            {
                 blogs = await _context.Blogs.Take((int)take).ToListAsync();
            }

            return blogs.Select(m=> new BlogVM { Id=m.Id, Title=m.Title,Description=m.Description,Image=m.Image,CreatedDate=m.CreatedDate.ToString("MM.dd.yyyy")});

        }
        public async Task<Blog> GetByIdAsync(int id)
        {
            return await _context.Blogs.Where(m => m.Id == id)
                                        .Include(m => m.Description)
                                        .Include(m => m.Image)
                                        .FirstOrDefaultAsync();
        }
    }
}
