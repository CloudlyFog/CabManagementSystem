using BankSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CabManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanksController : ControllerBase
    {
        private readonly BankSystem.Services.Interfaces.IBankAccountRepository<BankAccountModel> bankAccountRepository;
        public BanksController()
        {
            bankAccountRepository = new BankSystem.Services.Repositories.BankAccountRepository();
        }
        // GET: api/<BanksController>
        [HttpGet]
        public ActionResult<IEnumerable<BankAccountModel>> Get() => bankAccountRepository.Get().ToList();

        // GET api/<BanksController>/5
        [HttpGet("{id}")]
        public ActionResult<BankAccountModel> Get(Guid id) => bankAccountRepository.Get(id);

        // POST api/<BanksController>
        [HttpPost]
        public ActionResult Post([FromBody] BankAccountModel value) => bankAccountRepository.Create(value) != ExceptionModel.Successfull ? BadRequest() : Ok();

        // PUT api/<BanksController>/5
        [HttpPut("{id}")]
        public ActionResult Put([FromBody] BankAccountModel value) => bankAccountRepository.Update(value) != ExceptionModel.Successfull ? BadRequest() : Ok();

        // DELETE api/<BanksController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id) => bankAccountRepository.Delete(bankAccountRepository.Get(id)) != ExceptionModel.Successfull ? BadRequest() : Ok();
    }
}
