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
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;

namespace IquraStudyBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionContoller : ControllerBase
    {
        private readonly MyDbContext _context;

        public QuestionContoller(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/QuestionContoller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
          if (_context.Questions == null)
          {
              return NotFound();
          }
            return await _context.Questions.ToListAsync();
        }

        // GET: api/QuestionContoller/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
          if (_context.Questions == null)
          {
              return NotFound();
          }

          var question = await _context.Questions
              .Include(q => q.Answers)
              .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }
        
        // PUT: api/QuestionContoller/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question updatedQuestion)
        {
            if (id != updatedQuestion.Id)
            {
                return BadRequest();
            }

            var existingQuestion = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existingQuestion == null)
            {
                return NotFound();
            }

            // Update properties of the existing question
            existingQuestion.Title = updatedQuestion.Title;

            // Update or add new answers
            foreach (var updatedAnswer in updatedQuestion.Answers)
            {
                var existingAnswer = existingQuestion.Answers.FirstOrDefault(a => a.Id == updatedAnswer.Id);

                if (existingAnswer != null)
                {
                    // Update existing answer
                    existingAnswer.Title = updatedAnswer.Title;
                    existingAnswer.IsCorrect = updatedAnswer.IsCorrect;
                }
                else
                {
                    // Add new answer
                    existingQuestion.Answers.Add(new Answer
                    {
                        Title = updatedAnswer.Title,
                        IsCorrect = updatedAnswer.IsCorrect
                    });
                }
            }

            // Delete answers not present in the updated question
            var answersToDelete = existingQuestion.Answers
                .Where(existingAnswer => !updatedQuestion.Answers.Any(updatedAnswer => updatedAnswer.Id == existingAnswer.Id))
                .ToList();

            foreach (var answerToDelete in answersToDelete)
            {
                _context.Answers.Remove(answerToDelete);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(existingQuestion);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        // POST: api/QuestionContoller/{quizId}
        [HttpPost("{QuizId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Question>> PostQuestionWithAnswers(int QuizId, [FromBody] CreateQuestionWithAnswersDto data)
        {
            try
            {
                if (_context.Questions == null)
                {
                    return Problem("Entity set 'MyDbContext.Questions' is null.");
                }

                var question = new Question
                {
                    QuizId = QuizId,
                    CreatedAt = DateTime.UtcNow,
                    Title = data.Title,
                    Answers = data.Answers.Select(answerDto => new Answer
                    {
                        Title = answerDto.Title,
                        IsCorrect = answerDto.IsCorrect
                    }).ToList()
                };

                _context.Questions.Add(question);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/QuestionContoller/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (_context.Questions == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // PATCH: api/QuestionContoller/{questionId}
        [HttpPatch("{QuestionId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<Question>> PatchQuestionWithAnswers(int QuestionId, [FromBody] CreateQuestionWithAnswersDto data)
        {
            try
            {
                var question = await _context.Questions.FindAsync(QuestionId);

                if (question == null)
                {
                    return NotFound($"Question with ID {QuestionId} not found.");
                }

                question.UpdatedAt = DateTime.UtcNow;

                // Update properties that can be modified
                if (!string.IsNullOrEmpty(data.Title))
                {
                    question.Title = data.Title;
                }

                // Update answers
                if (data.Answers != null && data.Answers.Any())
                {
                    // Assuming you want to completely replace existing answers
                    question.Answers = data.Answers.Select(answerDto => new Answer
                    {
                        Title = answerDto.Title,
                        IsCorrect = answerDto.IsCorrect
                    }).ToList();
                }

                // Save changes
                await _context.SaveChangesAsync();

                return Ok(question);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        // PUT: api/QuestionContoller/{questionId}
        // [HttpPut("{QuestionId}")]
        // [Authorize(Roles = "Teacher")]
        // public async Task<ActionResult<Question>> PutQuestionWithAnswers(int QuestionId, [FromBody] UpdateQuestionWithAnswersDto data)
        // {
        //     try
        //     {
        //         var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == QuestionId);
        //
        //         if (question == null)
        //         {
        //             return NotFound($"Question with ID {QuestionId} not found.");
        //         }
        //
        //         question.UpdatedAt = DateTime.UtcNow;
        //
        //         // Update properties that can be modified
        //         if (!string.IsNullOrEmpty(data.Title))
        //         {
        //             question.Title = data.Title;
        //         }
        //
        //         // Update existing answers and delete those not in data.Answers
        //         if (data.Answers != null && data.Answers.Any())
        //         {
        //             var answerIdsToUpdate = data.Answers.Select(answerDto => answerDto.Id).ToList();
        //
        //             // Update existing answers
        //             foreach (var existingAnswer in question.Answers.ToList())
        //             {
        //                 if (answerIdsToUpdate.Contains(existingAnswer.Id))
        //                 {
        //                     var answerDto = data.Answers.First(a => a.Id == existingAnswer.Id);
        //                     existingAnswer.Title = answerDto.Title;
        //                     existingAnswer.IsCorrect = answerDto.IsCorrect;
        //                 }
        //                 else
        //                 {
        //                     // Delete the answer not in data.Answers
        //                     _context.Answers.Remove(existingAnswer);
        //                 }
        //             }
        //
        //             // Add new answers that are not in the existing answers
        //             var newAnswers = data.Answers.Where(a => !answerIdsToUpdate.Contains(a.Id)).Select(answerDto => new Answer
        //             {
        //                 Title = answerDto.Title,
        //                 IsCorrect = answerDto.IsCorrect
        //             }).ToList();
        //
        //             question.Answers.AddRange(newAnswers);
        //         }
        //
        //         // Save changes
        //         await _context.SaveChangesAsync();
        //
        //         return Ok(question);
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle exceptions as needed
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }
        //
        //


        private bool QuestionExists(int id)
        {
            return (_context.Questions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
