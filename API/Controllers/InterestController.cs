using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class InterestController : BaseApiController
{
    private readonly IInterestReponsitory _interestRepository;

    public InterestController(IInterestReponsitory interestRepository)
    {
        _interestRepository = interestRepository;
    }

    // POST api/interest
    [HttpPost]
    public async Task<IActionResult> CreateInterest([FromBody] Interest interest)
    {
        if (interest == null)
        {
            return BadRequest("Interest cannot be null.");
        }

        var createdInterest = await _interestRepository.CreateInterestAsync(interest);
        return CreatedAtAction(nameof(GetInterest), new { id = createdInterest.Id }, createdInterest);
    }

    // GET api/interest/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInterest(int id)
    {
        var interest = await _interestRepository.GetInterestByIdAsync(id);

        if (interest == null)
        {
            return NotFound();
        }

        return Ok(interest);
    }

    // GET api/interest
    [HttpGet]
    public async Task<IActionResult> GetAllInterests()
    {
        var interests = await _interestRepository.GetAllInterestsAsync();
        return Ok(interests);
    }
}