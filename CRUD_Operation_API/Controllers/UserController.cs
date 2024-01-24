using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using CRUD.Model;
using CRUD.Contract;
using System.Text.Json;

namespace CRUD_Operation_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserContract _userContract;

        public UserController(IUserContract userContract)
        {
            _userContract = userContract;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginData data)
        {
            LoggedInUser userDetails = await _userContract.AuthenticateUser(data);

            return StatusCode(200, userDetails);
        }

        [HttpPost]
        [Authorize]
        [Route("GetUserDetailsByID/{userID}")]
        public async Task<IActionResult> GetUserDetailsByID(int userID)
        {
            Dictionary<string, object> userDetails = await _userContract.GetUserDetailsByID(userID);

            if (userDetails != null)
            {
                return StatusCode(200, userDetails);
            }
            else
            {
                return StatusCode(404, null);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("GetUserDetailsByID/{userID}")]
        public async Task<IActionResult> DeleteUser(int userID)
        {
            ResultResponse res = await _userContract.DeleteUser(userID);

            if (res != null)
            {
                return StatusCode(200, res);
            }
            else
            {
                return StatusCode(404, null);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            UserList result = await _userContract.GetUserList();

            if (result != null)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(404, null);
            }

        }

        [HttpPost]
        [Authorize]
        [Route("SaveUserDetails")]
        public async Task<IActionResult> SaveUserDetails([FromBody] User data)
        {
            ResultResponse result = await _userContract.SaveUserDetails(data);

            if (result != null)
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(404, null);
            }

        }
    }
}
