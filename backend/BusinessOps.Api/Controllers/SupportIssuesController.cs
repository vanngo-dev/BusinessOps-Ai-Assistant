using BusinessOps.Api.Data;
using BusinessOps.Api.Dtos;
using BusinessOps.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessOps.Api.Controllers;

[ApiController]
[Route("api/support-issues")]
public class SupportIssuesController : ControllerBase
{
    private readonly AppDbContext _db;

    public SupportIssuesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SupportIssueDto>>> GetSupportIssues()
    {
        var issues = await _db.SupportIssues
            .Include(si => si.RelatedOrder)
            .OrderByDescending(si => si.CreatedAt)
            .Select(si => ToDto(si))
            .ToListAsync();

        return Ok(issues);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupportIssueDto>> GetSupportIssue(int id)
    {
        var issue = await _db.SupportIssues
            .Include(si => si.RelatedOrder)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (issue is null)
        {
            return NotFound(new { message = $"Support issue with ID {id} was not found." });
        }

        return Ok(ToDto(issue));
    }

    [HttpPost]
    public async Task<ActionResult<SupportIssueDto>> CreateSupportIssue(CreateSupportIssueRequest request)
    {
        if (request.RelatedOrderId.HasValue)
        {
            var orderExists = await _db.Orders.AnyAsync(o => o.Id == request.RelatedOrderId.Value);

            if (!orderExists)
            {
                return BadRequest(new { message = $"Order with ID {request.RelatedOrderId.Value} does not exist." });
            }
        }

        if (string.IsNullOrWhiteSpace(request.IssueType))
        {
            return BadRequest(new { message = "Issue type is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Priority))
        {
            return BadRequest(new { message = "Priority is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(new { message = "Status is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(new { message = "Description is required." });
        }

        var issue = new SupportIssue
        {
            RelatedOrderId = request.RelatedOrderId,
            IssueType = request.IssueType.Trim(),
            Priority = request.Priority.Trim(),
            Status = request.Status.Trim(),
            Description = request.Description.Trim(),
            CreatedAt = DateTime.UtcNow,
            ResolvedAt = null
        };

        _db.SupportIssues.Add(issue);
        await _db.SaveChangesAsync();

        var createdIssue = await _db.SupportIssues
            .Include(si => si.RelatedOrder)
            .FirstAsync(si => si.Id == issue.Id);

        return CreatedAtAction(nameof(GetSupportIssue), new { id = issue.Id }, ToDto(createdIssue));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SupportIssueDto>> UpdateSupportIssue(int id, UpdateSupportIssueRequest request)
    {
        var issue = await _db.SupportIssues
            .Include(si => si.RelatedOrder)
            .FirstOrDefaultAsync(si => si.Id == id);

        if (issue is null)
        {
            return NotFound(new { message = $"Support issue with ID {id} was not found." });
        }

        if (request.RelatedOrderId.HasValue)
        {
            var orderExists = await _db.Orders.AnyAsync(o => o.Id == request.RelatedOrderId.Value);

            if (!orderExists)
            {
                return BadRequest(new { message = $"Order with ID {request.RelatedOrderId.Value} does not exist." });
            }
        }

        if (string.IsNullOrWhiteSpace(request.IssueType))
        {
            return BadRequest(new { message = "Issue type is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Priority))
        {
            return BadRequest(new { message = "Priority is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Status))
        {
            return BadRequest(new { message = "Status is required." });
        }

        if (string.IsNullOrWhiteSpace(request.Description))
        {
            return BadRequest(new { message = "Description is required." });
        }

        issue.RelatedOrderId = request.RelatedOrderId;
        issue.IssueType = request.IssueType.Trim();
        issue.Priority = request.Priority.Trim();
        issue.Status = request.Status.Trim();
        issue.Description = request.Description.Trim();
        issue.ResolvedAt = request.ResolvedAt;

        await _db.SaveChangesAsync();

        var updatedIssue = await _db.SupportIssues
            .Include(si => si.RelatedOrder)
            .FirstAsync(si => si.Id == issue.Id);

        return Ok(ToDto(updatedIssue));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupportIssue(int id)
    {
        var issue = await _db.SupportIssues.FindAsync(id);

        if (issue is null)
        {
            return NotFound(new { message = $"Support issue with ID {id} was not found." });
        }

        _db.SupportIssues.Remove(issue);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static SupportIssueDto ToDto(SupportIssue issue)
    {
        return new SupportIssueDto
        {
            Id = issue.Id,
            RelatedOrderId = issue.RelatedOrderId,
            RelatedOrderNumber = issue.RelatedOrder?.OrderNumber,
            IssueType = issue.IssueType,
            Priority = issue.Priority,
            Status = issue.Status,
            Description = issue.Description,
            CreatedAt = issue.CreatedAt,
            ResolvedAt = issue.ResolvedAt
        };
    }
}