using BankSystem.Models;
using Microsoft.AspNetCore.Mvc;
using RestApiCMS.Services.Interfaces;
using RestApiCMS.Services.Repositories;

namespace RestApiCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public BanksController()
        {
            bankAccountRepository = new BankAccountRepository();
        }
        // GET: api/<BanksController>
        [HttpGet]
        public ActionResult<IEnumerable<BankAccountModel>> Get() => bankAccountRepository.Get().ToList();

        // GET api/<BanksController>/5
        [HttpGet("{id}")]
        public ActionResult<BankAccountModel> Get(Guid id) => bankAccountRepository.Get(id);

        // POST api/<BanksController>
        [HttpPost]
        public ActionResult Post([FromBody] BankAccountModel value)
        {
            if (bankAccountRepository.Create(value) != CabManagementSystem.Models.ExceptionModel.Successfull)
                return BadRequest();
            return Ok();
        }

        // PUT api/<BanksController>/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] BankAccountModel value)
        {
            if (bankAccountRepository.Update(value) != CabManagementSystem.Models.ExceptionModel.Successfull)
                return BadRequest();
            return Ok();
        }

        // DELETE api/<BanksController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            if (bankAccountRepository.Delete(bankAccountRepository.Get(id)) != CabManagementSystem.Models.ExceptionModel.Successfull)
                return BadRequest();
            return Ok();
        }
    }
}
