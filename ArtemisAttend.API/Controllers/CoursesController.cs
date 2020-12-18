using ArtemisAttend.API.Helper;
using ArtemisAttend.API.Models;
using AutoMapper;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtemisAttend.API.Controllers
{
    [ApiController]
    [Route("api/Users/{userId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IArtemisAttendRepository _artemisAttendRepository;
        private readonly IMapper _mapper;

        public CoursesController(IArtemisAttendRepository artemisAttendRepository, IMapper mapper)
        {
            _artemisAttendRepository = artemisAttendRepository ??
                throw new ArgumentNullException(nameof(artemisAttendRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper)); ;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForUser(Guid userId)
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var coursesFromRepo = _artemisAttendRepository.GetCourses(userId);
            return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesFromRepo));
        }

        [HttpGet("{courseId}", Name = "GetCourseForUser")]
        public ActionResult<CourseDto> GetCourseForUser(Guid userId, Guid courseId)
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var courseFromRepo = _artemisAttendRepository.GetCourse(userId, courseId);

            if (courseFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(courseFromRepo));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForUser(Guid userId, CourseForCreationDto course) 
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var courseEntity = _mapper.Map<Entities.Course>(course);
            _artemisAttendRepository.AddCourse(userId, courseEntity);
            _artemisAttendRepository.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseForUser", new { userId, courseId = courseToReturn.Id }, courseToReturn);

        }

        
        // This route now returns 2 things, noContent or Created
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForUser(Guid userId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var courseForUserRepo = _artemisAttendRepository.GetCourse(userId, courseId);

            if (courseForUserRepo == null)
            {
                //return NotFound();

                var courseEntity = _mapper.Map<Entities.Course>(course);
                courseEntity.Id = courseId;

                _artemisAttendRepository.AddCourse(userId, courseEntity);
                _artemisAttendRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseEntity);

                return CreatedAtRoute("GetCourseForUser",
                    new { userId = userId, courseId = courseToReturn.Id },
                    courseToReturn);

            }

            _mapper.Map(course, courseForUserRepo);
            _artemisAttendRepository.UpdateCourse(courseForUserRepo);
            _artemisAttendRepository.Save();

            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForUser(
            Guid userId, 
            Guid courseId,
            JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var courseForUserFromRepo = _artemisAttendRepository.GetCourse(userId, courseId);

            if (courseForUserFromRepo == null)
            {
                //return NotFound();

                // Upsert
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);
                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }

                var courseToAdd = _mapper.Map<Entities.Course>(courseDto);
                courseToAdd.Id = courseId;

                _artemisAttendRepository.AddCourse(userId, courseToAdd);
                _artemisAttendRepository.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForUser",
                    new { userId, courseId = courseToReturn.Id },
                    courseToReturn);
            }

            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForUserFromRepo);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(courseToPatch, courseForUserFromRepo);

            _artemisAttendRepository.UpdateCourse(courseForUserFromRepo);

            _artemisAttendRepository.Save();

            return NoContent();
        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourseForUser(Guid userId, Guid courseId)
        {
            if (!_artemisAttendRepository.UserExists(userId))
            {
                return NotFound();
            }

            var courseForUserFromRepo = _artemisAttendRepository.GetCourse(userId, courseId);

            if (courseForUserFromRepo == null)
            {
                return NotFound();
            }

            _artemisAttendRepository.DeleteCourse(courseForUserFromRepo);
            _artemisAttendRepository.Save();

            return NoContent();
        }
    }
}
