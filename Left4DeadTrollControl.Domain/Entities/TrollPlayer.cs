namespace Left4DeadTrollControl.Domain.Entities;

public class TrollPlayer
{
    public TrollPlayer(string steamId, string profileUrl, string nickname, string notes)
    {
        Id = Guid.NewGuid();
        SteamId = steamId;
        ProfileUrl = profileUrl;
        Nickname = nickname;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string SteamId { get; private set; }
    public string ProfileUrl { get; private set; }
    public string Nickname { get; private set; }
    public string Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public void Update(string steamId, string profileUrl, string nickname, string notes)
    {
        SteamId = steamId;
        ProfileUrl = profileUrl;
        Nickname = nickname;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
        
        ExecuteValidations();
    }

    public void ExecuteValidations()
    {
        if (string.IsNullOrWhiteSpace(SteamId))
        {
            throw new ArgumentException("SteamId is required");
        }

        if (SteamId.Length > 8)
        {
            throw new ArgumentException("SteamId must be 8 characters long");
        }

        if (ProfileUrl.Length > 300)
        {
            throw new ArgumentException("Profile Url must be at most 300 characters long");
        }

        if (Notes.Length > 2000)
        {
            throw new ArgumentException("Notes must be at most 2000 characters long");
        }
    }
}
