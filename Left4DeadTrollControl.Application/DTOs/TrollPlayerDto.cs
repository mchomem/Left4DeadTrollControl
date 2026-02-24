using System.Globalization;

namespace Left4DeadTrollControl.Application.DTOs;

public class TrollPlayerDto
{
    public Guid Id { get; set; }
    public string SteamId { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string FormattedCreatedAt
    {
        get
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            return CreatedAt.ToString("G", cultureInfo);
        }
    }
    public string FormattedUpdatedAt
    {
        get
        {
            var cultureInfo = CultureInfo.CurrentCulture;
            return UpdatedAt?.ToString("G", cultureInfo) ?? string.Empty;
        }
    }
}

public class TrollPlayerInsertDto
{
    public string SteamId { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class TrollPlayerUpdateDto
{
    public Guid Id { get; set; }
    public string SteamId { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
