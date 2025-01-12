using IquraStudyBE.Context;
using IquraStudyBE.Models;

namespace IquraStudyBE.Services;

public class TaskService
{
    private readonly MyDbContext _context;

    public TaskService(MyDbContext context)
    {
        _context = context;
    }

    public async Task<string> AddQuizAndProblemTasks(
        int taskId,
        int[] quizIds,
        int[] problemIds,
        bool isCompetition
    )
    {
        if (_context.Quizzes == null)
        {
            throw new InvalidOperationException("Entity set 'MyDbContext.Quizzes' is null.");
        }

        if ((quizIds == null || quizIds.Length == 0) && (problemIds == null || problemIds.Length == 0))
        {
            throw new ArgumentException("QuizIds and ProblemIds array is null or empty.");
        }

        bool changesMade = false;

        // Handle QuizIds
        if (quizIds != null && quizIds.Length > 0)
        {
            foreach (int quizId in quizIds)
            {
                if (isCompetition)
                {
                    // For competitions
                    // if (!_context.CompetitionQuizzes.Any(cq => cq.QuizId == quizId && cq.CompetitionId == taskId))
                    // {
                    //     var competitionQuiz = new CompetitionQuiz
                    //     {
                    //         CompetitionId = taskId,
                    //         QuizId = quizId,
                    //     };
                    //     _context.CompetitionQuizzes.Add(competitionQuiz);
                    //     changesMade = true;
                    // }
                }
                else
                {
                    // For group tasks
                    if (!_context.GroupTaskQuizzes.Any(gtq => gtq.QuizId == quizId && gtq.GroupTaskId == taskId))
                    {
                        var groupTaskQuiz = new GroupTaskQuiz
                        {
                            GroupTaskId = taskId,
                            QuizId = quizId,
                        };
                        _context.GroupTaskQuizzes.Add(groupTaskQuiz);
                        changesMade = true;
                    }
                }
            }
        }

        // Handle ProblemIds
        if (problemIds != null && problemIds.Length > 0)
        {
            foreach (int problemId in problemIds)
            {
                if (isCompetition)
                {
                    // For competitions
                    if (!_context.CompetitionProblems.Any(cp => cp.ProblemId == problemId && cp.CompetitionId == taskId))
                    {
                        var competitionProblem = new CompetitionProblem
                        {
                            CompetitionId = taskId,
                            ProblemId = problemId,
                        };
                        _context.CompetitionProblems.Add(competitionProblem);
                        changesMade = true;
                    }
                }
                else
                {
                    // For group tasks
                    if (!_context.GroupTaskProblems.Any(gtp => gtp.ProblemId == problemId && gtp.GroupTaskId == taskId))
                    {
                        var groupTaskProblem = new GroupTaskProblem
                        {
                            GroupTaskId = taskId,
                            ProblemId = problemId,
                        };
                        _context.GroupTaskProblems.Add(groupTaskProblem);
                        changesMade = true;
                    }
                }
            }
        }

        if (changesMade)
        {
            await _context.SaveChangesAsync();
            return "Successfully added tasks.";
        }

        return "No changes made.";
    }
}
