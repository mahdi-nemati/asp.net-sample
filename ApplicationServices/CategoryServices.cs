using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServices
{
    public class CategoryServices(CourseDBContext ctx)
    {
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await ctx.Categories.OrderBy(c => c.Id).AsNoTrackingWithIdentityResolution().ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await ctx.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> Insert(Category category)
        {
            ctx.Categories.Add(category);
            await ctx.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> IsCategoryExist(int id)
        {
            return await ctx.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task RenameCategory(Category category)
        {
            ctx.Update(category);
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var entityToRemove = new Category { Id = id };
            ctx.Categories.Remove(entityToRemove);
            await ctx.SaveChangesAsync();
        }

    }
}
