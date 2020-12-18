using ArtemisAttend.API.Helper;
using ArtemisAttend.API.Models;
using AutoMapper;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtemisAttend.API.Controllers
{
    [ApiController]
    [Route("api/usercollections")]
    public class UserCollectionsController: ControllerBase
    {
        private readonly IArtemisAttendRepository _artemisAttendRepository;
        private readonly IMapper _mapper;
        public UserCollectionsController(IArtemisAttendRepository artemisAttendRepository, IMapper mapper)
        {
            _artemisAttendRepository = artemisAttendRepository ??
                throw new ArgumentNullException(nameof(artemisAttendRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetUserCollection")]
        public ActionResult<IEnumerable<UserDto>> GetUserCollection(
            [FromRoute] [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids) 
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var userEntities = _artemisAttendRepository.GetUsers(ids);
            if (ids.Count() != userEntities.Count())
            {
                return NotFound();
            }

            var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);
            return Ok(usersToReturn);

        }

        [HttpPost]
        public ActionResult<IEnumerable<UserDto>> CreateUserCollection(IEnumerable<UserForCreationDto> userCollection)
        {
            var userEntities = _mapper.Map<IEnumerable<Entities.User>>(userCollection);
            foreach (var user in userEntities)
            {
                _artemisAttendRepository.AddUser(user);
            }
            _artemisAttendRepository.Save();

            var authorCollectionToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);
            var idsString = string.Join(",", authorCollectionToReturn.Select(u => u.Id));

            return CreatedAtRoute("GetUserCollection", new { ids = idsString }, authorCollectionToReturn);
        }
    }
}
