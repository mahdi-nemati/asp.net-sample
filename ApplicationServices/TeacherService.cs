using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServices
{
    public class TeacherService(CourseDBContext ctx)
    {
        #region Queries
        // get all
        public async Task<List<Teacher>> GetAllTeachersAsync(int pageNumber, int itemPerPage)
        {
            int skipCount = (pageNumber - 1) * itemPerPage;
            return await ctx.Teachers.AsNoTracking().OrderBy(t => t.LastName).Skip(skipCount).Take(itemPerPage).ToListAsync();
        }
        // teachers count
        public async Task<int> GetTotallCountAsync()
        {
            return await ctx.Teachers.CountAsync();
        }

        // search
        public async Task<List<Teacher>> SearchAsync(string firstName, string LastName)
        {
            var teacherQuery = ctx.Teachers.AsQueryable();
            if (!string.IsNullOrEmpty(firstName))
            {
                teacherQuery = teacherQuery.Where(t => t.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(LastName))
            {
                teacherQuery = teacherQuery.Where(t => t.LastName.Contains(LastName));
            }
            return await teacherQuery.AsNoTracking().ToListAsync();
        }

        // get one teacher
        public async Task<Teacher?> GetTeacherAsync(int id)
        {
            return await ctx.Teachers.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        }

        // is exist
        public async Task<bool> IsTecherExist(int id)
        {
            return await ctx.Teachers.AnyAsync(t => t.Id == id);
        }
        #endregion

        #region commands
        public async Task<int> Insert(Teacher teacher)
        {
            await ctx.Teachers.AddAsync(teacher);
            await ctx.SaveChangesAsync();
            return teacher.Id;
        }

        public async Task Update(Teacher teacher)
        {
            ctx.Teachers.Update(teacher);
            await ctx.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entityToRemove = new Teacher { Id = id };
            ctx.Teachers.Remove(entityToRemove);
            await ctx.SaveChangesAsync();
        }
        #endregion
    }
}
