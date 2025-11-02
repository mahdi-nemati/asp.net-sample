using ApplicationServices;
using AutoMapper;
using Endpoint.RequestsAndResponses;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;

namespace Endpoint.CategoryEndpoint
{
    public static class CategoryEndpoint
    {

        public static WebApplication MapCategories(this WebApplication app, string prefix)
        {
            var CategoryGroup = app.MapGroup(prefix);
            CategoryGroup.MapGet("", GetAllCategories).CacheOutput(c => c.Expire(TimeSpan.FromMinutes(15)).Tag("allCategory"));
            CategoryGroup.MapGet("/{id:int}", GetCategory);
            CategoryGroup.MapPost("", AddCategory);
            CategoryGroup.MapPut("/{id:int}", RenameCategory);
            CategoryGroup.MapDelete("/{id:int}", DeleteCategory);
            return app;
        }

        static async Task<Ok<List<CategoryResponse>>> GetAllCategories(CategoryServices categoryService, IMapper mapper)
        {
            var result = await categoryService.GetCategoriesAsync();
            var response = mapper.Map<List<CategoryResponse>>(result);
            return TypedResults.Ok(response);
        }

        static async Task<Results<Ok<CategoryResponse>, NotFound>> GetCategory(CategoryServices categoryService, int id, IMapper mapper)
        {
            var result = await categoryService.GetCategoryAsync(id);

            if (result == null)
            {
                return TypedResults.NotFound();
            }

            var response = mapper.Map<CategoryResponse>(result);
            return TypedResults.Ok<CategoryResponse>(response);
        }

        static async Task<Created<CategoryResponse>> AddCategory(CategoryServices categoryService, CategoryRequest category, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var categoryForSave = mapper.Map<Category>(category);
            var result = await categoryService.Insert(categoryForSave);
            var response = mapper.Map<CategoryResponse>(categoryForSave);
            await outputCacheStore.EvictByTagAsync("allCategory", default);
            return TypedResults.Created($"/categories/{result}", response);
        }

        static async Task<Results<NoContent, NotFound>> RenameCategory(CategoryServices categoryService, IOutputCacheStore outputCacheStore, int id, CategoryRequest category, IMapper mapper)
        {
            var isExist = await categoryService.IsCategoryExist(id);
            if (!isExist)
            {
                return TypedResults.NotFound();
            }
            var categoryForSave = mapper.Map<Category>(category);
            categoryForSave.Id = id;
            await categoryService.RenameCategory(categoryForSave);
            await outputCacheStore.EvictByTagAsync("allCategory", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteCategory(CategoryServices categoryService, IOutputCacheStore outputCacheStore, int id)
        {
            var isExist = await categoryService.IsCategoryExist(id);
            if (!isExist)
            {
                return TypedResults.NotFound();
            }
            await categoryService.DeleteCategory(id);
            await outputCacheStore.EvictByTagAsync("allCategory", default);
            return TypedResults.NoContent();
        }
    }
}
