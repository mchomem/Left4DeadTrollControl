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
