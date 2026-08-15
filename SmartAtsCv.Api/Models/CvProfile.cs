using System.ComponentModel.DataAnnotations;

namespace SmartAtsCv.Api.Models;

public class CvProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? LinkedIn { get; set; }

    [MaxLength(300)]
    public string? Website { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public List<Experience> Experiences { get; set; } = new();
    public List<Education> Educations { get; set; } = new();
    public List<Skill> Skills { get; set; } = new();
    public List<LanguageItem> Languages { get; set; } = new();
    public List<Certification> Certifications { get; set; } = new();
}

public class Experience
{
    public int Id { get; set; }
    public int CvProfileId { get; set; }
    public CvProfile? CvProfile { get; set; }

    [MaxLength(200)] public string Company { get; set; } = string.Empty;
    [MaxLength(200)] public string Position { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool Current { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class Education
{
    public int Id { get; set; }
    public int CvProfileId { get; set; }
    public CvProfile? CvProfile { get; set; }

    [MaxLength(200)] public string Institution { get; set; } = string.Empty;
    [MaxLength(150)] public string Degree { get; set; } = string.Empty;
    [MaxLength(150)] public string Field { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [MaxLength(50)] public string? Grade { get; set; }
}

public class Skill
{
    public int Id { get; set; }
    public int CvProfileId { get; set; }
    public CvProfile? CvProfile { get; set; }

    [MaxLength(100)] public string Name { get; set; } = string.Empty;
}

public class LanguageItem
{
    public int Id { get; set; }
    public int CvProfileId { get; set; }
    public CvProfile? CvProfile { get; set; }

    [MaxLength(100)] public string Language { get; set; } = string.Empty;
    [MaxLength(50)] public string Level { get; set; } = string.Empty;
}

public class Certification
{
    public int Id { get; set; }
    public int CvProfileId { get; set; }
    public CvProfile? CvProfile { get; set; }

    [MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(200)] public string Issuer { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    [MaxLength(300)] public string? Url { get; set; }
}
