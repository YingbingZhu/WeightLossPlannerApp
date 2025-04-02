using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WeightLossPlannerAPI.Data;
using WeightLossPlannerAPI.Models;

namespace WeightLossPlannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DailyCaloriesSummaryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DailyCaloriesSummaryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<DailyCaloriesSummary>>> GetSummary(int userId)
        {
            var result = await BuildCalorieSummaryAsync(userId);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<DailyCaloriesSummary>>> GetSummaryForCurrentUser()
        {
            var userIdStr = User.FindFirst("sub")?.Value ?? User.FindFirst("UserId")?.Value;

            if (!int.TryParse(userIdStr, out var userId))
            {
                return Unauthorized("Invalid user ID.");
            }

            var result = await BuildCalorieSummaryAsync(userId);
            return Ok(result);
        }

        private async Task<List<DailyCaloriesSummary>> BuildCalorieSummaryAsync(int userId)
        {
            var meals = await _context.MealLogs
                .Where(m => m.UserProfileId == userId)
                .GroupBy(m => m.Date)
                .Select(g => new { Date = g.Key, Calories = g.Sum(m => m.Calories) })
                .ToListAsync();

            var exercises = await _context.ExerciseLogs
                .Where(e => e.UserProfileId == userId)
                .GroupBy(e => e.Date)
                .Select(g => new { Date = g.Key, Calories = g.Sum(e => e.CaloriesBurned) })
                .ToListAsync();

            var summary = meals
                .UnionBy(exercises, x => x.Date)
                .GroupJoin(
                    exercises,
                    meal => meal.Date,
                    ex => ex.Date,
                    (meal, ex) => new DailyCaloriesSummary
                    {
                        Date = meal.Date,
                        CaloriesEaten = (int)meal.Calories,
                        CaloriesBurned = (int)(ex.FirstOrDefault()?.Calories ?? 0)
                    })
                .ToList();

            return summary;
        }
    }
}
