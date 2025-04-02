using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var meals = await _context.MealLogs
                .Where(m => m.UserProfileId == userId)
                .GroupBy(m => m.Date)
                .Select(g => new {Date = g.Key, Calories = g.Sum( m => m.Calories ) })
                .ToListAsync();
            
            var exercises = await _context.ExerciseLogs
                .Where(e => e.UserProfileId == userId)
                .GroupBy(e => e.Date)
                .Select(g => new {Date = g.Key, Calories = g.Sum( m => m.CaloriesBurned ) })
                .ToListAsync();
            
            var joined = meals 
                .UnionBy(exercises, m => m.Date)
                .GroupJoin(
                    exercises,
                    m => m.Date, // base table key selector
                    e => e.Date, // join table key selector
                    (meal, ex) => new DailyCaloriesSummary // meal is item from meals
                    {
                        Date = meal.Date,
                        CaloriesEaten = (int) meal.Calories,
                        CaloriesBurned = (int)(ex.FirstOrDefault()?.Calories ?? 0)
                    })
                .ToList();
            
            return Ok(joined);
        }

    }
}