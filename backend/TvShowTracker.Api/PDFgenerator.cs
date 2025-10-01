using System.ComponentModel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

class PdfGenerator{
public static byte[] GenerateTvShowPdf(TvShowDto tvShow)
{
    QuestPDF.Settings.License = LicenseType.Community;
    var pdfBytes = Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Size(PageSizes.A4);

            page.Header().Text(tvShow.Name).FontSize(24).Bold();

            page.Content()
            .PaddingVertical(25)
            .Column(stack =>
            {
                // Image
                if (!string.IsNullOrEmpty(tvShow.ImageUrl))
                {
                    var filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"+tvShow.ImageUrl);
                    //stack.Item().Text($"filepath: {filePath}");
                    stack.Item().Image(filePath);
                }

                // Basic info
                stack.Item().Text($"Release Date: {tvShow.ReleaseDate:yyyy-MM-dd}");
                stack.Item().Text($"Seasons: {tvShow.Seasons}");
                stack.Item().Text($"Origin: {tvShow.Origin}");
                stack.Item().Text($"Rating: {tvShow.Rating}");

                // Genres
                if (tvShow.Genres != null && tvShow.Genres.Any())
                {
                    stack.Item().Text("Genres: " + string.Join(", ", tvShow.Genres.Select(c=>c.Name)));
                }

                // Cast
                if (tvShow.Cast != null && tvShow.Cast.Any())
                {
                    stack.Item().Text("Cast: " + string.Join(", ", tvShow.Cast.Select(c=>c.Name)));
                }

                // Directors
                if (tvShow.Directors != null && tvShow.Directors.Any())
                {
                    stack.Item().Text("Directors: " + string.Join(", ", tvShow.Directors.Select(c=>c.Name)));
                }

                if (!string.IsNullOrEmpty(tvShow.Description))
                {
                    stack.Item().Text(tvShow.Description).FontSize(12).Italic();
                }

            });

            page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
        });
    }).GeneratePdf();

    return pdfBytes;
}}
