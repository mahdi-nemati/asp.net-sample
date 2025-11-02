using ApplicationServices;
using AutoMapper;
using Endpoint.Infastructure;
using Endpoint.RequestsAndResponses;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Endpoint.CategoryEndpoint
{
    public static class TeacherEndpoint
    {
        static string TeachersImageFolder = @"Images\Teachers";
        static string DefaultTeachersImageName = "Default.jpg";
        public static WebApplication MapTeacher(this WebApplication app, string prefix)
        {
            var TeacherMapGroup = app.MapGroup(prefix);
            TeacherMapGroup.MapPost("/", AddTeacher).DisableAntiforgery();
            TeacherMapGroup.MapGet("/{pageNumber:int}/{itemPerPage:int}", GetAllTeachers);
            TeacherMapGroup.MapGet("/totalCount", GetTotalCount);
            TeacherMapGroup.MapGet("/{id:int}", GetTeacher);
            TeacherMapGroup.MapPut("/{id:int}", UpdateTeacher).DisableAntiforgery();
            TeacherMapGroup.MapDelete("/{id:int}", DeleteTeacher);
            TeacherMapGroup.MapGet("/search", TeacherSearch);
            return app;
        }

        static async Task<Created<TeacherResponse>> AddTeacher(TeacherRequest teacher, TeacherService teacherService, IMapper mapper, IFileAdapter fileAdapter)
        {
            var teacherForSave = mapper.Map<Teacher>(teacher);
            string fileName = DefaultTeachersImageName;
            if (teacher.File is not null)
            {
                fileName = fileAdapter.InsertFile(teacher.File, TeachersImageFolder);
            }
            teacherForSave.ImageUrl = fileName;
            var result = await teacherService.Insert(teacherForSave);
            var response = mapper.Map<TeacherResponse>(teacherForSave);
            return TypedResults.Created($"/teacher/{result}", response);
        }

        static async Task<Ok<int>> GetTotalCount(TeacherService teacherService)
        {
            int result = await teacherService.GetTotallCountAsync();
            return TypedResults.Ok<int>(result);
        }

        static async Task<Ok<List<TeacherResponse>>> GetAllTeachers(TeacherService teacherService, IMapper mapper, int pageNumber, int itemPerPage)
        {
            var result = await teacherService.GetAllTeachersAsync(pageNumber, itemPerPage);
            var response = mapper.Map<List<TeacherResponse>>(result);
            return TypedResults.Ok(response);
        }

        static async Task<Results<Ok<TeacherResponse>, NotFound>> GetTeacher(TeacherService teacherService, int id, IMapper mapper)
        {
            var isTeacherExist = await teacherService.IsTecherExist(id);
            if (!isTeacherExist)
            {
                return TypedResults.NotFound();
            }
            var result = await teacherService.GetTeacherAsync(id);
            var response = mapper.Map<TeacherResponse>(result);
            return TypedResults.Ok(response);
        }

        static async Task<Results<NoContent, NotFound>> UpdateTeacher([FromForm] TeacherRequest teacher, TeacherService teacherService, IMapper mapper, int id, IFileAdapter fileAdapter)
        {
            var oldTeacher = await teacherService.GetTeacherAsync(id);
            var teacherToUpdate = mapper.Map<Teacher>(teacher);

            if (oldTeacher is null)
            {
                return TypedResults.NotFound();
            }

            if (teacher.File is not null)
            {
                teacherToUpdate.ImageUrl = fileAdapter.UpdateFile(oldTeacher.ImageUrl, teacher.File, TeachersImageFolder);
            }
            else
            {
                teacherToUpdate.ImageUrl = oldTeacher.ImageUrl;
            }

            teacherToUpdate.Id = id;
            await teacherService.Update(teacherToUpdate);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> DeleteTeacher(TeacherService teacherService, int id, IMapper mapper)
        {
            var isTeacherExist = await teacherService.IsTecherExist(id);
            if (!isTeacherExist)
            {
                return TypedResults.NotFound();
            }
            await teacherService.Delete(id);
            return TypedResults.NoContent();
        }

        static async Task<Results<Ok<List<TeacherResponse>>, NotFound>> TeacherSearch(TeacherService teacherService, string fname, string lname, IMapper mapper)
        {
            var result = await teacherService.SearchAsync(fname, lname);
            if (result == null)
            {
                return TypedResults.NotFound();
            }
            var response = mapper.Map<List<TeacherResponse>>(result);
            return TypedResults.Ok(response);
        }
    }
}
