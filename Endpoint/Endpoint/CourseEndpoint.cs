using ApplicationServices;
using AutoMapper;
using Endpoint.Infastructure;
using Endpoint.RequestsAndResponses;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.Endpoint
{
    public static class CourseEndpoint
    {
        static string CourseImageFolder = @"Images\Course";
        static string DefaultCourseImageName = "Default.jpg";
        public static WebApplication MapCourse(this WebApplication app, string prefix)
        {
            var CourseMapGroup = app.MapGroup(prefix);
            CourseMapGroup.MapPost("/", AddCourse).DisableAntiforgery();
            CourseMapGroup.MapGet("/{pageNumber:int}/{itemPerPage:int}", GetAllCourse);
            CourseMapGroup.MapGet("/totalCount", GetTotalCount);
            CourseMapGroup.MapGet("/{id:int}", GetCourse);
            CourseMapGroup.MapPut("/{id:int}", UpdateCourse).DisableAntiforgery();
            CourseMapGroup.MapDelete("/{id:int}", DeleteCourse);
            CourseMapGroup.MapGet("/search", CourseSearch);
            return app;
        }

        static async Task<Created<TeacherResponse>> AddCourse(CourseRequest course, CourseServices courseServices, IMapper mapper, IFileAdapter fileAdapter)
        {
            var teacherForSave = mapper.Map<Course>(course);
            string fileName = DefaultCourseImageName;
            if (course.File is not null)
            {
                fileName = fileAdapter.InsertFile(course.File, CourseImageFolder);
            }
            teacherForSave.ImageUrl = fileName;
            var result = await courseServices.Insert(teacherForSave);
            var response = mapper.Map<TeacherResponse>(teacherForSave);
            return TypedResults.Created($"/teacher/{result}", response);
        }

        static async Task<Ok<int>> GetTotalCount(CourseServices courseServices)
        {
            int result = await courseServices.GetTotallCountAsync();
            return TypedResults.Ok<int>(result);
        }

        static async Task<Ok<List<CourseResponse>>> GetAllCourse(CourseServices courseServices, IMapper mapper, int pageNumber, int itemPerPage)
        {
            var result = await courseServices.GetAllCourseAsync(pageNumber, itemPerPage);
            var response = mapper.Map<List<CourseResponse>>(result);
            return TypedResults.Ok(response);
        }

        static async Task<Results<Ok<CourseResponse>, NotFound>> GetCourse(CourseServices courseServices, int id, IMapper mapper)
        {
            var isCourseExist = await courseServices.IsCourseExist(id);
            if (!isCourseExist)
            {
                return TypedResults.NotFound();
            }
            var result = await courseServices.GetCourseAsync(id);
            var response = mapper.Map<CourseResponse>(result);
            return TypedResults.Ok(response);
        }

        static async Task<Results<NoContent, NotFound>> UpdateCourse([FromForm] CourseRequest course, CourseServices courseServices, IMapper mapper, int id, IFileAdapter fileAdapter)
        {
            var prevCourse = await courseServices.GetCourseAsync(id);
            var courseToUpdate = mapper.Map<Course>(course);

            if (prevCourse is null)
            {
                return TypedResults.NotFound();
            }

            if (course.File is not null)
            {
                courseToUpdate.ImageUrl = fileAdapter.UpdateFile(prevCourse.ImageUrl, course.File, CourseImageFolder);
            }
            else
            {
                courseToUpdate.ImageUrl = prevCourse.ImageUrl;
            }

            courseToUpdate.Id = id;
            await courseServices.Update(courseToUpdate);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteCourse(CourseServices courseServices, int id, IMapper mapper)
        {
            var isCourseExist = await courseServices.IsCourseExist(id);
            if (!isCourseExist)
            {
                return TypedResults.NotFound();
            }
            await courseServices.Delete(id);
            return TypedResults.NoContent();
        }

        static async Task<Results<Ok<List<CourseResponse>>, NotFound>> CourseSearch(CourseServices courseServices, string title, IMapper mapper)
        {
            var result = await courseServices.Search(title);
            if (result == null)
            {
                return TypedResults.NotFound();
            }
            var response = mapper.Map<List<CourseResponse>>(result);
            return TypedResults.Ok(response);
        }
    }
}
