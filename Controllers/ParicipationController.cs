using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IquraStudyBE.Classes;
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
                .Include(p => p.Submissions)
                .Include(p => p.Competition)
                .Where(p => p.UserId == userId && p.CompetitionId == competitionId)
                .Include(p => p.Competition.CompetitionProblems)
                    .ThenInclude(p => p.Problem)
                .Include(p => p.Competition.CompetitionQuizzes)
                    .ThenInclude(p => p.Quiz)
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
                            Difficulty = p.Competition.Difficulty,
                            CompetitionProblems = p.Competition.CompetitionProblems
                                .Select(cp => new CompetitionProblem
                                {
                                    Id = cp.Id,
                                    ProblemId = cp.ProblemId,
                                    CompetitionId = cp.CompetitionId,
                                    Problem = cp.Problem,
                                    MaxScore = p.Submissions
                                        .Where(s => s.ProblemId == cp.ProblemId)
                                        .Select(s => s.Score)
                                        .OrderByDescending(s => s)
                                        .FirstOrDefault(),
                                    SubmittedAt = p.Submissions
                                        .Where(s => s.ProblemId == cp.ProblemId)
                                        .OrderByDescending(s => s.Score)
                                        .Select(s => s.SubmittedAt)
                                        .FirstOrDefault(),
                                }).ToList(),
                            CompetitionQuizzes = p.Competition.CompetitionQuizzes
                                .Select(cq => new CompetitionQuiz
                                {
                                    Id = cq.Id,
                                    QuizId = cq.QuizId,
                                    Quiz = cq.Quiz,
                                    MaxScore = p.Submissions
                                        .Where(s => s.QuizId == cq.QuizId)
                                        .Select(s => s.Score)
                                        .OrderByDescending(s => s)
                                        .FirstOrDefault(),
                                    SubmittedAt = p.Submissions
                                        .Where(s => s.QuizId == cq.QuizId)
                                        .OrderByDescending(s => s.Score)
                                        .Select(s => s.SubmittedAt)
                                        .FirstOrDefault(),
                                })
                                .ToList()
                            
                        },
                        Submissions=p.Submissions,
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
        public async Task<ActionResult> CreateParticipation([FromBody] int competitionId, int groupId)
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
                Score = 0, // Default score
                GroupId = groupId
            };

            _context.Participations.Add(newParticipation);
            await _context.SaveChangesAsync();

            return Ok(newParticipation);
        }
        // GET: api/Participation/{participationId}/Submissions
        [HttpGet("{participationId}/Submissions")]
        public async Task<ActionResult<IEnumerable<Submission>>> GetSubmissionsForParticipation(int participationId)
        {
            var participation = await _context.Participations
                .Include(p => p.Submissions)
                .FirstOrDefaultAsync(p => p.Id == participationId);

            if (participation == null)
                return NotFound($"Participation with ID {participationId} not found.");

            return Ok(participation.Submissions);
        }
        
        // GET: api/Participation/{participationId}/Submissions/{submissionId}
        [HttpGet("{participationId}/Submissions/{submissionId}")]
        public async Task<ActionResult<Submission>> GetSubmissionById(int participationId, int submissionId)
        {
            var submission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.Id == submissionId && s.ParticipationId == participationId);

            if (submission == null)
                return NotFound($"Submission with ID {submissionId} not found for Participation {participationId}.");

            return Ok(submission);
        }
        
        // POST: api/Participation/{participationId}/Submissions
        [HttpPost("{participationId}/Submissions")]
        public async Task<ActionResult<Submission>> CreateSubmission(int participationId, [FromBody] Submission submission)
        {
            var participation = await _context.Participations.FindAsync(participationId);
            if (participation == null)
                return NotFound($"Participation with ID {participationId} not found.");

            // Ensure the submission is linked correctly
            submission.ParticipationId = participationId;
            submission.SubmittedAt = DateTime.UtcNow;

            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            // Return a 201 Created response
            return CreatedAtAction(
                nameof(GetSubmissionById), 
                new { participationId = participationId, submissionId = submission.Id },
                submission
            );
        }
        
        // PUT: api/Participation/{participationId}/Submissions/{submissionId}
        [HttpPut("{participationId}/Submissions/{submissionId}")]
        public async Task<IActionResult> UpdateSubmission(int participationId, int submissionId, [FromBody] Submission updatedSubmission)
        {
            var submission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.Id == submissionId && s.ParticipationId == participationId);

            if (submission == null)
                return NotFound($"Submission with ID {submissionId} not found for Participation {participationId}.");

            // Update fields (this is simplistic; consider using AutoMapper or manual mapping)
            submission.Score = updatedSubmission.Score;
            submission.Type = updatedSubmission.Type;
            submission.QuizId = updatedSubmission.QuizId;
            submission.ProblemId = updatedSubmission.ProblemId;
            // ... other fields

            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        // DELETE: api/Participation/{participationId}/Submissions/{submissionId}
        [HttpDelete("{participationId}/Submissions/{submissionId}")]
        public async Task<IActionResult> DeleteSubmission(int participationId, int submissionId)
        {
            var submission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.Id == submissionId && s.ParticipationId == participationId);

            if (submission == null)
                return NotFound($"Submission with ID {submissionId} not found for Participation {participationId}.");

            _context.Submissions.Remove(submission);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool ParticipationExists(int id)
        {
            return (_context.Participations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
