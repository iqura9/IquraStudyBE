using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IquraStudyBE.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IquraStudyBE.Context;
using IquraStudyBE.Migrations;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

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
    public int ProblemsCount  { get; set; }
    public List<Quiz> Quizzes { get; set; }
    public int QuizzesCount { get; set; }
}

// DTO for Problem
public class ProblemDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Score { get; set; }
}

public class QuizDto
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public string QuizTitle { get; set; }
    public int Score { get; set; }
}

public class ScoreboardDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public double TotalScore { get; set; }
    public List<ProblemScoreDto> Problems { get; set; } = new List<ProblemScoreDto>();
    public List<QuizScoreDto> Quizzes { get; set; } = new List<QuizScoreDto>();
}



public class ScoreboardResponseDto
{
    public int CompetitionId { get; set; }
    public string Title { get; set; }
    public List<UserScoreDto> UserScores { get; set; } = new List<UserScoreDto>();
}
public class UserScoreDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public double TotalScore { get; set; }
    public List<ProblemScoreDto> Problems { get; set; } = new List<ProblemScoreDto>();
    public List<QuizScoreDto> Quizzes { get; set; } = new List<QuizScoreDto>();
}
public class ProblemScoreDto
{
    public int ProblemId { get; set; }
    public string Title { get; set; }
    public double Score { get; set; }
}

public class QuizScoreDto
{
    public int QuizId { get; set; }
    public string Title { get; set; }
    public double Score { get; set; }
}

public class AddGroupCompetitionsDto
{
    public int GroupId { get; set; }
    public List<int> CompetitionIds { get; set; } = new List<int>();
}

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly TaskService _taskService;
        private readonly ITokenService _tokenService;
        public CompetitionController(MyDbContext context, TaskService taskService,ITokenService tokenService)
        {
            _context = context;
            _taskService = taskService;
            _tokenService = tokenService;
        }
        
        // GET: api/Competition/Group/{groupId}
        [HttpGet("Group/{groupId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitionsByGroup(int groupId)
        {
            if (_context.Competitions == null || _context.Groups == null)
            {
                return NotFound("Competitions or Groups data is not available.");
            }

            // Check if the group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
            {
                return NotFound($"Group with ID {groupId} does not exist.");
            }

            // Fetch competitions associated with the group
            var competitions = await _context.GroupCompetitions
                .Where(gc => gc.GroupId == groupId)
                .Select(gc => gc.Competition)
                .ToListAsync();

            return Ok(competitions);
        }
        
        // GET: api/Competition/Teacher
        [HttpGet("Teacher")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<IEnumerable<Competition>>> GetTeacherCompetitions()
        {
            if (_context.Competitions == null)
            {
                return NotFound("Competitions data is not available.");
            }
            
            var teacherId = _tokenService.GetUserIdFromToken();

            var competitions = await _context.Competitions
                .Where(c => c.UserId == teacherId)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                })
                .ToListAsync();

            return Ok(competitions);
        }
        
  // POST: api/Competition/Group
[HttpPost("Group")]
[Authorize]
public async Task<ActionResult> AddCompetitionsToGroup([FromBody] AddGroupCompetitionsDto dto)
{
    if (_context.Competitions == null || _context.Groups == null || _context.GroupCompetitions == null)
    {
        return NotFound("Required data sets are not available.");
    }

    // Validate that the group exists
    var groupExists = await _context.Groups.AnyAsync(g => g.Id == dto.GroupId);
    if (!groupExists)
    {
        return NotFound($"Group with ID {dto.GroupId} does not exist.");
    }

    // Materialize valid competition IDs to avoid IQueryable issues
    var validCompetitionIds = await _context.Competitions
        .Where(c => dto.CompetitionIds.Contains(c.Id))
        .Select(c => c.Id)
        .ToListAsync();

    // Check for invalid competition IDs
    var invalidCompetitionIds = dto.CompetitionIds.Except(validCompetitionIds).ToList();
    if (invalidCompetitionIds.Any())
    {
        return NotFound($"Competitions with IDs {string.Join(", ", invalidCompetitionIds)} do not exist.");
    }

    // Fetch existing group-competition relationships and materialize them
    var existingCompetitionIds = await _context.GroupCompetitions
        .Where(gc => gc.GroupId == dto.GroupId)
        .Select(gc => gc.CompetitionId)
        .ToListAsync();

    // Filter out competitions already associated with the group
    var newCompetitionIds = validCompetitionIds
        .Cast<int>() // Ensure validCompetitionIds is explicitly cast to IEnumerable<int>
        .Except(existingCompetitionIds.Cast<int>()) // Ensure existingCompetitionIds is also explicitly cast
        .ToList();

    if (!newCompetitionIds.Any())
    {
        return BadRequest("All provided competitions are already associated with the group.");
    }

    // Add new group-competition relationships
    var groupCompetitions = newCompetitionIds.Select(competitionId => new GroupCompetition
    {
        GroupId = dto.GroupId,
        CompetitionId = competitionId,
        CreateByUserId = User.FindFirst("sub")?.Value, // Assuming you want to track who added this
        CreatedAt = DateTime.UtcNow
    }).ToList();

    await _context.GroupCompetitions.AddRangeAsync(groupCompetitions);
    await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetCompetitionsByGroup), new { groupId = dto.GroupId }, groupCompetitions);
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

        // GET: api/Competition/Scoreboard/{competitionId}
        [HttpGet("Scoreboard/{competitionId}")]
        [Authorize]
        public async Task<ActionResult<ScoreboardResponseDto>> GetScoreboard(int competitionId)
        {
            if (_context.Competitions == null || _context.Participations == null)
            {
                return NotFound("Competitions or Participations data is not available.");
            }

            // Validate competition existence
            var competition = await _context.Competitions
                .Include(c => c.CompetitionProblems)
                    .ThenInclude(cp => cp.Problem)
                .Include(c => c.CompetitionQuizzes)
                    .ThenInclude(cq => cq.Quiz)
                .FirstOrDefaultAsync(c => c.Id == competitionId);

            if (competition == null)
            {
                return NotFound("Competition not found.");
            }

            // Get participations and calculate scores
            var participations = await _context.Participations
                .Where(p => p.CompetitionId == competitionId)
                .Include(p => p.User)
                .ToListAsync();

            // Map participations to userScores
            var userScores = participations.Select(p => new UserScoreDto
            {
                UserId = p.UserId,
                UserName = p.User.UserName, // Assuming User has a UserName property
                TotalScore = p.Score,
                Problems = competition.CompetitionProblems.Select(cp => new ProblemScoreDto
                {
                    ProblemId = cp.Id,
                    Title = cp.Problem?.Title ?? $"Problem {cp.Id}",
                    Score = cp.Score // Assuming each CompetitionProblem has a score field
                }).ToList(),
                Quizzes = competition.CompetitionQuizzes.Select(cq => new QuizScoreDto
                {
                    QuizId = cq.Id,
                    Title = cq.Quiz?.Title ?? $"Quiz {cq.Id}",
                    Score = cq.Score // Assuming each CompetitionQuiz has a score field
                }).ToList()
            })
            .OrderByDescending(us => us.TotalScore) // Sort by TotalScore descending
            .ToList();

            // Prepare the response object
            var response = new ScoreboardResponseDto
            {
                CompetitionId = competition.Id,
                Title = competition.Title,
                UserScores = userScores
            };

            return Ok(response);
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
                  Quizzes = c.CompetitionQuizzes.Select(cq =>cq.Quiz).ToList(),
                  Problems = c.CompetitionProblems
                      .Where(cp => cp.Problem != null)
                      .Select(cp => new ProblemDto
                      {
                          Id = cp.Problem.Id,
                          Title = cp.Problem.Title,
                          Description = cp.Problem.Description
                      })
                      .ToList(),
                  ProblemsCount = c.CompetitionProblems.Count(),
                  QuizzesCount = c.CompetitionQuizzes.Count(),
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
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
          if (_context.Competitions == null)
          {
              return Problem("Entity set 'MyDbContext.Competitions'  is null.");
          }
          var teacherId = _tokenService.GetUserIdFromToken();
          competition.UserId = teacherId;
            
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
