namespace SmartAtsCv.Api.DTOs;

public class PersonalInfoDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? LinkedIn { get; set; }
    public string? Website { get; set; }
}

public class ExperienceDto
{
    public string Company { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public bool Current { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class EducationDto
{
    public string Institution { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Grade { get; set; }
}

public class LanguageDto
{
    public string Language { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
}

public class CertificationDto
{
    public string Name { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string? Date { get; set; }
    public string? Url { get; set; }
}

public class CvDataDto
{
    public int Id { get; set; }
    public PersonalInfoDto PersonalInfo { get; set; } = new();
    public List<ExperienceDto> Experience { get; set; } = new();
    public List<EducationDto> Education { get; set; } = new();
    public List<string> Skills { get; set; } = new();
    public List<LanguageDto> Languages { get; set; } = new();
    public List<CertificationDto> Certifications { get; set; } = new();
}
