using Microsoft.AspNetCore.Mvc;
using UPBank.Addresses.Mongo.Repositories;
using UPBank.Addresses.PostalServices.Abstract;
using UPBank.DTOs;
using UPBank.Models;

namespace UPBank.Addresses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly AddressRepository _repository;
        private readonly IPostalAddressService _service;

        public AddressesController(AddressRepository repository, IPostalAddressService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get()
        {
            return Ok(await _repository.Find());
        }

        [HttpGet("zipcode/{zipcode}")]
        public async Task<ActionResult<Address>> Get(string zipcode)
        {
            var address = await _repository.Find(zipcode);

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Post(AddressDTO addressDTO)
        {
            bool addressExists = await AddressExistsAsync(addressDTO);

            if (addressExists)
                return Conflict("Endereço já cadastrado");

            IAddressResult? result = await _service.Fetch(addressDTO.Zipcode);

            if (result == null)
                return BadRequest();

            var address = new Address
            {
                Complement = addressDTO.Complement,
                Number = addressDTO.Number,
                Zipcode = result.Zipcode,
                State = result.State,
                Street = result.Street,
                City = result.City,
                Neighborhood = result.Neighborhood,
            };

            _repository.Insert(address);

            return address;
        }

        public async Task<bool> AddressExistsAsync(AddressDTO addressDTO)
        {
            return (await _repository.Find(addressDTO.Zipcode)) != null;
        }
    }
}
