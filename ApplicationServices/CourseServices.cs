using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServices
{
    public class CourseServices(CourseDBContext ctx)
    {
        #region Queries
        // get all course
        public async Task<List<Course>> GetAllCourseAsync(int pageNumber, int itemPerPage)
        {
            int skipCount = (pageNumber - 1) * itemPerPage;
            return await ctx.Course.AsNoTracking().OrderBy(c => c.Tilte).Skip(skipCount).Take(itemPerPage).ToListAsync();
        }
        // course count
        public async Task<int> GetTotallCountAsync()
        {
            return await ctx.Course.CountAsync();
        }
        // get one course
        public async Task<Course?> GetCourseAsync(int id)
        {
            return await ctx.Course.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        // is exist
        public async Task<bool> IsCourseExist(int id)
        {
            return await ctx.Course.AnyAsync(c => c.Id == id);
        }

        // search
        public async Task<List<Course>> Search(string title)
        {
            return await ctx.Course.Where(c => c.Tilte.Contains(title)).ToListAsync();
        }
        #endregion

        #region Commands
        public async Task<int> Insert(Course course)
        {
            await ctx.Course.AddAsync(course);
            await ctx.SaveChangesAsync();
            return course.Id;
        }

        public async Task Update(Course course)
        {
            ctx.Course.Update(course);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entityToRemove = new Course { Id = id };
            ctx.Course.Remove(entityToRemove);
            await ctx.SaveChangesAsync();
        }
        #endregion
    }
}
