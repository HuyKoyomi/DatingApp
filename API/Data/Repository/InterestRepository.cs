using API.Data;
using API.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API;

public class InterestRepository : IInterestReponsitory
{
    private readonly DataContext _context;
    public InterestRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<Interest> CreateInterestAsync(Interest interest)
    {
        _context.Interest.Add(interest);
        await _context.SaveChangesAsync();
        return interest;
    }

    // Lấy sở thích theo ID
    public async Task<Interest?> GetInterestByIdAsync(int id)
    {
        return await _context.Interest.FindAsync(id);
    }

    // Lấy tất cả sở thích
    public async Task<IEnumerable<Interest>> GetAllInterestsAsync()
    {
        return await _context.Interest.ToListAsync();
    }

}