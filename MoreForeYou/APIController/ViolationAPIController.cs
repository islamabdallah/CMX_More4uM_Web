////using Data.Repository;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.Extensions.Logging;
////using MoreForYou.Models.Models.MasterModels;
////using MoreForYou.Services.Implementation;
////using MoreForYou.Services.Models.API;
////using MoreForYou.Services.Models.Message;
////using System.Collections.Generic;
////using System.Threading.Tasks;

////// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

////namespace MoreForYou.APIController
////{

////    [Route("api/[controller]")]
////    [ApiController]
////    public class ViolationAPIController : ControllerBase
////    {
////        private readonly IRepository<Violation, long> _repository;

////        public ViolationAPIController(IRepository<Violation, long> repository)
////        {
////            _repository= repository;
////        }
////        // GET: api/<ViolationAPIController>
////        [HttpGet]
////        public IEnumerable<string> Get()
////        {
////            return new string[] { "value1", "value2" };
////        }

////        // GET api/<ViolationAPIController>/5
////        [HttpGet("{id}")]
////        public string Get(int id)
////        {
////            return "value";
////        }

////        // POST api/<ViolationAPIController>
////        [HttpPost("AddViolation")]
////        public async Task<ActionResult> Post(Violation violation)
////        {
////            if(violation != null)
////            {
////                var result = _repository.Add(violation);
////                if(result != null)
////                {
////                    return Ok(new { Message = "Successful process", Data = true });
////                }
////                else
////                {
////                    return BadRequest(new { Message = "Failed Process", Data = false });
////                }
////            }
////            else
////            {
////                return BadRequest(new { Message = "Failed Process", Data = false });
////            }
////        }

////        // PUT api/<ViolationAPIController>/5
////        [HttpPut("{id}")]
////        public void Put(int id, [FromBody] string value)
////        {
////        }

////        // DELETE api/<ViolationAPIController>/5
////        [HttpDelete("{id}")]
////        public void Delete(int id)
////        {
////        }
////    }
////}
