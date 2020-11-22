using ArtemisAttend.API.Helper;
using ArtemisAttend.API.Models;
using ArtemisAttend.API.ResourceParameters;
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
    [Route("api/Users")]
    public class UsersController: ControllerBase
    {
        private readonly IArtemisAttendRepository _artemisAttendRepository;
        private readonly IMapper _mapper;

        public UsersController(IArtemisAttendRepository artemisAttendRepository, IMapper mapper)
        {
            _artemisAttendRepository = artemisAttendRepository ??
                throw new ArgumentNullException(nameof(artemisAttendRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper)); ;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] UsersResourceParameters usersResourceParameters)
        {
            var usersFromRepo = _artemisAttendRepository.GetUsers(usersResourceParameters);
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo));
        }

        [HttpGet("{userId}", Name = "GetUser")]
        public IActionResult GetUser(Guid userId)
        {
            var userFromRepo = _artemisAttendRepository.GetUser(userId);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(userFromRepo));
        }

        [HttpPost]
        public ActionResult<UserDto> CreateUser(UserForCreationDto user)
        {
            var userEntity = _mapper.Map<Entities.User>(user);
            _artemisAttendRepository.AddUser(userEntity);
            _artemisAttendRepository.Save();

            var userToReturn = _mapper.Map<UserDto>(userEntity);
            return CreatedAtRoute("GetUser", new { userId = userToReturn.Id }, userToReturn);
        }
    }
}
