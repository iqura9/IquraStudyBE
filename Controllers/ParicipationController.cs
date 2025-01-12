using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IquraStudyBE.Context;
using IquraStudyBE.Models;
using IquraStudyBE.Services;
using Microsoft.AspNetCore.Authorization;




namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParicipationController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _tokenService;

        public ParicipationController(MyDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // GET: api/Paricipation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participation>>> GetParticipations()
        {
          if (_context.Participations == null)
          {
              return NotFound();
          }
            return await _context.Participations.ToListAsync();
        }
        
        // GET: api/Participation/5
        [HttpGet("{competitionId}")]
        [Authorize]
        public async Task<ActionResult<Participation>> GetParticipationIsValid(int competitionId)
        {
            if (_context.Participations == null)
            {
                return NotFound("Participation data is not available.");
            }

            // Extract userId from the token
            var userId = _tokenService.GetUserIdFromToken();

            // Check if the user is a participant of the given competition
            var participation = await _context.Participations
                .Include(p => p.Competition)
                .Where(p => p.UserId == userId && p.CompetitionId == competitionId)
                 .Select(p => new Participation
                    {
                        Id = p.Id,
                        StartedAt = p.StartedAt,
                        EndedAt = p.EndedAt,
                        Score = p.Score,
                        Status = p.Status,
                        Competition = new Competition
                        {
                            Id = p.Competition.Id,
                            Title = p.Competition.Title,
                            Description = p.Competition.Description,
                            Format = p.Competition.Format,
                            ParticipantMode = p.Competition.ParticipantMode,
                            StartTime = p.Competition.StartTime,
                            EndTime = p.Competition.EndTime,
                            Duration = p.Competition.Duration,
                            Difficulty = p.Competition.Difficulty
                        }
                    })
                .FirstOrDefaultAsync();

            if (participation == null)
            {
                return NotFound("You are not a participant of this competition.");
            }

            return Ok(participation);
        }

        // POST: api/Participation
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateParticipation([FromBody] int competitionId)
        {
            if (_context.Participations == null || _context.Competitions == null)
            {
                return NotFound("Required data is not available.");
            }

            var userId = _tokenService.GetUserIdFromToken();


            // Check if the competition exists
            var competitionExists = await _context.Competitions.AnyAsync(c => c.Id == competitionId);
            if (!competitionExists)
            {
                return NotFound($"Competition with ID {competitionId} not found.");
            }

            // Check if the user is already a participant
            var existingParticipation = await _context.Participations
                .FirstOrDefaultAsync(p => p.UserId == userId && p.CompetitionId == competitionId);

            if (existingParticipation != null)
            {
                return Ok(existingParticipation);
            }

            // Create a new participation
            var newParticipation = new Participation
            {
                UserId = userId,
                CompetitionId = competitionId,
                StartedAt = DateTime.UtcNow,
                Status = "Active",
                Score = 0 // Default score
            };

            _context.Participations.Add(newParticipation);
            await _context.SaveChangesAsync();

            return Ok(newParticipation);
        }


        private bool ParticipationExists(int id)
        {
            return (_context.Participations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
