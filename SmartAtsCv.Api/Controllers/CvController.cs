using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartAtsCv.Api.Data;
using SmartAtsCv.Api.DTOs;
using SmartAtsCv.Api.Models;
using SmartAtsCv.Api.Services;

namespace SmartAtsCv.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CvController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly CvPdfService _pdf;

    public CvController(AppDbContext db, CvPdfService pdf)
    {
        _db = db;
        _pdf = pdf;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirst("sub")!.Value);

    // GET api/cv
    [HttpGet]
    public async Task<ActionResult<List<CvDataDto>>> GetAll()
    {
        var profiles = await _db.CvProfiles
            .Where(c => c.UserId == CurrentUserId)
            .Include(c => c.Experiences)
            .Include(c => c.Educations)
            .Include(c => c.Skills)
            .Include(c => c.Languages)
            .Include(c => c.Certifications)
            .ToListAsync();

        return Ok(profiles.Select(ToDto));
    }

    // GET api/cv/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CvDataDto>> GetById(int id)
    {
        var profile = await FindOwnedProfile(id);
        if (profile is null) return NotFound();
        return Ok(ToDto(profile));
    }

    // POST api/cv
    [HttpPost]
    public async Task<ActionResult<CvDataDto>> Create(CvDataDto dto)
    {
        var profile = new CvProfile { UserId = CurrentUserId };
        ApplyDto(profile, dto);

        _db.CvProfiles.Add(profile);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, ToDto(profile));
    }

    // PUT api/cv/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CvDataDto>> Update(int id, CvDataDto dto)
    {
        var profile = await FindOwnedProfile(id);
        if (profile is null) return NotFound();

        // Replace child collections
        _db.Experiences.RemoveRange(profile.Experiences);
        _db.Educations.RemoveRange(profile.Educations);
        _db.Skills.RemoveRange(profile.Skills);
        _db.Languages.RemoveRange(profile.Languages);
        _db.Certifications.RemoveRange(profile.Certifications);

        ApplyDto(profile, dto);
        profile.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(ToDto(profile));
    }

    // DELETE api/cv/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var profile = await FindOwnedProfile(id);
        if (profile is null) return NotFound();

        _db.CvProfiles.Remove(profile);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/cv/5/pdf
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> DownloadPdf(int id)
    {
        var profile = await FindOwnedProfile(id);
        if (profile is null) return NotFound();

        var bytes = _pdf.Generate(ToDto(profile));
        var fileName = string.IsNullOrWhiteSpace(profile.FullName) ? "CV.pdf" : $"CV-{profile.FullName.Replace(' ', '-')}.pdf";
        return File(bytes, "application/pdf", fileName);
    }

    private async Task<CvProfile?> FindOwnedProfile(int id)
    {
        return await _db.CvProfiles
            .Include(c => c.Experiences)
            .Include(c => c.Educations)
            .Include(c => c.Skills)
            .Include(c => c.Languages)
            .Include(c => c.Certifications)
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == CurrentUserId);
    }

    private static DateTime? ParseDate(string? value) =>
        DateTime.TryParse(value, out var d) ? d : null;

    private static void ApplyDto(CvProfile profile, CvDataDto dto)
    {
        profile.FullName = dto.PersonalInfo.FullName;
        profile.Email = dto.PersonalInfo.Email;
        profile.Phone = dto.PersonalInfo.Phone;
        profile.Location = dto.PersonalInfo.Location;
        profile.Summary = dto.PersonalInfo.Summary;
        profile.LinkedIn = dto.PersonalInfo.LinkedIn;
        profile.Website = dto.PersonalInfo.Website;

        profile.Experiences = dto.Experience.Select(e => new Experience
        {
            Company = e.Company,
            Position = e.Position,
            StartDate = ParseDate(e.StartDate),
            EndDate = ParseDate(e.EndDate),
            Current = e.Current,
            Description = e.Description
        }).ToList();

        profile.Educations = dto.Education.Select(e => new Education
        {
            Institution = e.Institution,
            Degree = e.Degree,
            Field = e.Field,
            StartDate = ParseDate(e.StartDate),
            EndDate = ParseDate(e.EndDate),
            Grade = e.Grade
        }).ToList();

        profile.Skills = dto.Skills.Select(s => new Skill { Name = s }).ToList();

        profile.Languages = dto.Languages.Select(l => new LanguageItem
        {
            Language = l.Language,
            Level = l.Level
        }).ToList();

        profile.Certifications = dto.Certifications.Select(c => new Certification
        {
            Name = c.Name,
            Issuer = c.Issuer,
            Date = ParseDate(c.Date),
            Url = c.Url
        }).ToList();
    }

    private static CvDataDto ToDto(CvProfile p) => new()
    {
        Id = p.Id,
        PersonalInfo = new PersonalInfoDto
        {
            FullName = p.FullName,
            Email = p.Email,
            Phone = p.Phone,
            Location = p.Location,
            Summary = p.Summary,
            LinkedIn = p.LinkedIn,
            Website = p.Website
        },
        Experience = p.Experiences.Select(e => new ExperienceDto
        {
            Company = e.Company,
            Position = e.Position,
            StartDate = e.StartDate?.ToString("yyyy-MM-dd"),
            EndDate = e.EndDate?.ToString("yyyy-MM-dd"),
            Current = e.Current,
            Description = e.Description
        }).ToList(),
        Education = p.Educations.Select(e => new EducationDto
        {
            Institution = e.Institution,
            Degree = e.Degree,
            Field = e.Field,
            StartDate = e.StartDate?.ToString("yyyy-MM-dd"),
            EndDate = e.EndDate?.ToString("yyyy-MM-dd"),
            Grade = e.Grade
        }).ToList(),
        Skills = p.Skills.Select(s => s.Name).ToList(),
        Languages = p.Languages.Select(l => new LanguageDto { Language = l.Language, Level = l.Level }).ToList(),
        Certifications = p.Certifications.Select(c => new CertificationDto
        {
            Name = c.Name,
            Issuer = c.Issuer,
            Date = c.Date?.ToString("yyyy-MM-dd"),
            Url = c.Url
        }).ToList()
    };
}
