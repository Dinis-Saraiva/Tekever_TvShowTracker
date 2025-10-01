using System.ComponentModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

/// <summary>
/// Provides functionality to generate PDF documents for TV shows.
/// </summary>
public class PdfGenerator
{
    /// <summary>
    /// Generates a PDF document for a given TV show containing its details, image, and metadata.
    /// </summary>
    /// <param name="tvShow">The <see cref="TvShowDto"/> object containing information about the TV show.</param>
    /// <returns>A <see cref="byte[]"/> array representing the generated PDF document.</returns>
    /// <remarks>
    /// The generated PDF includes:
    /// <list type="bullet">
    /// <item><description>TV show name as header.</description></item>
    /// <item><description>TV show image if available.</description></item>
    /// <item><description>Release date, number of seasons, origin, and rating.</description></item>
    /// <item><description>Genres, cast, and directors if available.</description></item>
    /// <item><description>Description in italic font.</description></item>
    /// <item><description>Page numbers in the footer.</description></item>
    /// </list>
    /// </remarks>
    public static byte[] GenerateTvShowPdf(TvShowDto tvShow)
    {
        // Set the QuestPDF license type
        QuestPDF.Settings.License = LicenseType.Community;

        // Create the PDF document
        var pdfBytes = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);

                // Header with TV show name
                page.Header().Text(tvShow.Name).FontSize(24).Bold();

                // Main content
                page.Content()
                    .PaddingVertical(25)
                    .Column(stack =>
                    {
                        // TV show image
                        if (!string.IsNullOrEmpty(tvShow.ImageUrl))
                        {
                            var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + tvShow.ImageUrl);
                            stack.Item().Image(filePath);
                        }

                        // TV show metadata
                        stack.Item().Text($"Release Date: {tvShow.ReleaseDate:yyyy-MM-dd}");
                        stack.Item().Text($"Seasons: {tvShow.Seasons}");
                        stack.Item().Text($"Origin: {tvShow.Origin}");
                        stack.Item().Text($"Rating: {tvShow.Rating}");

                        // Genres
                        if (tvShow.Genres != null && tvShow.Genres.Any())
                        {
                            stack.Item().Text("Genres: " + string.Join(", ", tvShow.Genres.Select(c => c.Name)));
                        }

                        // Cast
                        if (tvShow.Cast != null && tvShow.Cast.Any())
                        {
                            stack.Item().Text("Cast: " + string.Join(", ", tvShow.Cast.Select(c => c.Name)));
                        }

                        // Directors
                        if (tvShow.Directors != null && tvShow.Directors.Any())
                        {
                            stack.Item().Text("Directors: " + string.Join(", ", tvShow.Directors.Select(c => c.Name)));
                        }

                        // Description
                        if (!string.IsNullOrEmpty(tvShow.Description))
                        {
                            stack.Item().Text(tvShow.Description).FontSize(12).Italic();
                        }
                    });

                // Footer with page number
                page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
            });
        }).GeneratePdf();

        return pdfBytes;
    }
}
