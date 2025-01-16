using API.Entities;

namespace API;

public interface IInterestReponsitory
{
    Task<Interest> CreateInterestAsync(Interest interest);
    Task<Interest?> GetInterestByIdAsync(int id);
    Task<IEnumerable<Interest>> GetAllInterestsAsync();
}