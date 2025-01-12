using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IquraStudyBE.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IquraStudyBE.Context;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using Microsoft.AspNetCore.Authorization;

public class CompetitionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Format { get; set; }
    public string ParticipantMode { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int? Duration { get; set; }
    public string Difficulty { get; set; }
    public List<ProblemDto> Problems { get; set; }
}

// DTO for Problem
public class ProblemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly TaskService _taskService;

        public CompetitionController(MyDbContext context, TaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        // GET: api/Competition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
          if (_context.Competitions == null)
          {
              return NotFound();
          }
          return await _context.Competitions.ToListAsync();
        }

        // GET: api/Competition/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetitionDto>> GetCompetition(int id)
        {
          if (_context.Competitions == null)
          {
              return NotFound();
          }
          var competition = await _context.Competitions
              .Include(c => c.CompetitionProblems)
              .ThenInclude(cp => cp.Problem)
              .Where(c => c.Id == id)
              .Select(c => new CompetitionDto
              {
                  Id = c.Id,
                  Title = c.Title,
                  Description = c.Description,
                  Format = c.Format,
                  ParticipantMode = c.ParticipantMode,
                  StartTime = c.StartTime,
                  EndTime = c.EndTime,
                  Duration = c.Duration,
                  Difficulty = c.Difficulty,
                  Problems = c.CompetitionProblems
                      .Where(cp => cp.Problem != null)
                      .Select(cp => new ProblemDto
                      {
                          Id = cp.Problem.Id,
                          Title = cp.Problem.Title,
                          Description = cp.Problem.Description
                      })
                      .ToList()
              })
              .FirstOrDefaultAsync();
          
            if (competition == null)
            {
                return NotFound();
            }

            return competition;
        }

        // PUT: api/Competition/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetition(int id, Competition competition)
        {
            if (id != competition.Id)
            {
                return BadRequest();
            }

            _context.Entry(competition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Competition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
          if (_context.Competitions == null)
          {
              return Problem("Entity set 'MyDbContext.Competitions'  is null.");
          }
            _context.Competitions.Add(competition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompetition", new { id = competition.Id }, competition);
        }
        
        // POST: api/Competition/Problems
        [HttpPost("Problems")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult> PostCompetitionProblems([FromBody] CreateCompetitionProblemsDto data)
        {
            try
            {
                var result = await _taskService.AddQuizAndProblemTasks(data.CompetitionId, data.QuizIds, data.ProblemIds, isCompetition: true);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Competition/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            if (_context.Competitions == null)
            {
                return NotFound();
            }
            var competition = await _context.Competitions.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }

            _context.Competitions.Remove(competition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompetitionExists(int id)
        {
            return (_context.Competitions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
