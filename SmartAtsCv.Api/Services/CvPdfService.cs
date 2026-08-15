using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SmartAtsCv.Api.DTOs;

namespace SmartAtsCv.Api.Services;

public class CvPdfService
{
    static CvPdfService()
    {
        // QuestPDF Community license is free for small businesses / individuals.
        // See https://www.questpdf.com/license/ before using in a larger company.
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] Generate(CvDataDto cv)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header().Column(col =>
                {
                    col.Item().Text(string.IsNullOrWhiteSpace(cv.PersonalInfo.FullName) ? "الاسم الكامل" : cv.PersonalInfo.FullName)
                        .FontSize(22).Bold();

                    col.Item().Text(text =>
                    {
                        var parts = new[] { cv.PersonalInfo.Email, cv.PersonalInfo.Phone, cv.PersonalInfo.Location }
                            .Where(p => !string.IsNullOrWhiteSpace(p));
                        text.Span(string.Join("   |   ", parts)).FontSize(10).FontColor(Colors.Grey.Darken1);
                    });
                });

                page.Content().PaddingVertical(15).Column(col =>
                {
                    col.Spacing(12);

                    if (!string.IsNullOrWhiteSpace(cv.PersonalInfo.Summary))
                    {
                        col.Item().Text("نبذة مهنية").FontSize(14).Bold();
                        col.Item().Text(cv.PersonalInfo.Summary);
                    }

                    if (cv.Experience.Any())
                    {
                        col.Item().Text("الخبرات العملية").FontSize(14).Bold();
                        foreach (var exp in cv.Experience)
                        {
                            col.Item().Column(c =>
                            {
                                c.Item().Text($"{exp.Position} - {exp.Company}").Bold();
                                c.Item().Text($"{exp.StartDate} - {(exp.Current ? "الآن" : exp.EndDate)}")
                                    .FontSize(9).FontColor(Colors.Grey.Darken1);
                                if (!string.IsNullOrWhiteSpace(exp.Description))
                                    c.Item().Text(exp.Description).FontSize(10);
                            });
                        }
                    }

                    if (cv.Education.Any())
                    {
                        col.Item().Text("التعليم").FontSize(14).Bold();
                        foreach (var edu in cv.Education)
                        {
                            col.Item().Column(c =>
                            {
                                c.Item().Text($"{edu.Degree} في {edu.Field}").Bold();
                                c.Item().Text(edu.Institution).FontSize(10);
                                c.Item().Text($"{edu.StartDate} - {edu.EndDate}")
                                    .FontSize(9).FontColor(Colors.Grey.Darken1);
                            });
                        }
                    }

                    if (cv.Skills.Any())
                    {
                        col.Item().Text("المهارات").FontSize(14).Bold();
                        col.Item().Text(string.Join("  •  ", cv.Skills));
                    }

                    if (cv.Languages.Any())
                    {
                        col.Item().Text("اللغات").FontSize(14).Bold();
                        col.Item().Text(string.Join("  •  ", cv.Languages.Select(l => $"{l.Language} ({l.Level})")));
                    }

                    if (cv.Certifications.Any())
                    {
                        col.Item().Text("الشهادات والدورات").FontSize(14).Bold();
                        foreach (var cert in cv.Certifications)
                        {
                            col.Item().Text($"{cert.Name} - {cert.Issuer} ({cert.Date})").FontSize(10);
                        }
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("تم إنشاؤه بواسطة Smart ATS CV").FontSize(8).FontColor(Colors.Grey.Medium);
                });
            });
        });

        return document.GeneratePdf();
    }
}
