using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ADO;

namespace HR.api.Controllers
{
    [RoutePrefix("api/HR")]
    public class HRController : ApiController
    {
        [Route("getAllUsers")]
        [HttpGet]
        public IHttpActionResult getAllUsers()
        {
            var result = utils.getAllUsers();
            //if (result == 0)
                //return NotFound();
                //return Exception();
            return Ok(result);
        }

        [Route("getUserById")]
        [HttpGet]
        public IHttpActionResult getUserById(int id)
        {
            var result = utils.getUserById(id);
            //if (result == 0)
                //return NotFound();
                //return Exception();
            return Ok(result);
        }

        [Route("createUser")]
        [HttpPost]
        public IHttpActionResult createUser([FromBody] user u)
        {
            var result = utils.createUser(u);
            //if (result == 0)
                //return NotFound();
                //return Exception();
                //return Unauthorized();
                //return BadRequest();
                //return Conflict();
                //return Redirect();
                //return InvalidModelState();
            return Ok(result);
        }

        [Route("updateUser")]
        [HttpPut]
        public IHttpActionResult updateUser([FromBody] user u)
        {
            Boolean isSucceeded = utils.updateUser(u);
            //if (result == 0)
                //return NotFound();
                //return Exception();
                //return Unauthorized();
                //return BadRequest();
                //return Conflict();
                //return Redirect();
                //return InvalidModelState();
            return Ok(isSucceeded);
        }

        [Route("deleteUser")]
        [HttpDelete]
        public IHttpActionResult deleteUser([FromUri] int id)
        {
            int result = utils.deleteUser(id);
            //if (result == 0)
                //return NotFound();
                //return Exception();
                //return Unauthorized();
            return Ok(result);          
        }

        [Route("version")]
        [HttpGet]
        public IHttpActionResult Version() { return Ok("HR v1.0"); }
    }
}