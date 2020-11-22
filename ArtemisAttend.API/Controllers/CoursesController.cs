using ArtemisAttend.API.Helper;
using ArtemisAttend.API.Models;
using AutoMapper;
using CourseLibrary.API.Services;
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

        [HttpGet("{courseId}")]
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
    }
}
