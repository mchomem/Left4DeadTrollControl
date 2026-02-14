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
    }

    public void ExecuteValidations()
    {
        if (string.IsNullOrWhiteSpace(SteamId))
            throw new ArgumentException("SteamId is required");

        if (SteamId.Length > 9)
            throw new ArgumentException("SteamId must be 9 characters long");

        if(string.IsNullOrEmpty(Nickname))
            throw new ArgumentException("Nickname is required");

        if (Nickname.Length > 100)
            throw new ArgumentException("Nickname must be at most 100 characters long");

        if (ProfileUrl?.Length > 300)
            throw new ArgumentException("Profile Url must be at most 300 characters long");

        if (Notes?.Length > 2000)
            throw new ArgumentException("Notes must be at most 2000 characters long");

        if (!string.IsNullOrEmpty(ProfileUrl))
        {
            var urlRegexPattern = @"^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b(?:[-a-zA-Z0-9()@:%_\+.~#?&//=]*)$";
            bool isMatch = Regex.IsMatch(ProfileUrl, urlRegexPattern, RegexOptions.IgnoreCase);
            bool isValidUri = Uri.TryCreate(ProfileUrl, UriKind.Absolute, out Uri resultUri)
                              && (resultUri.Scheme == Uri.UriSchemeHttp || resultUri.Scheme == Uri.UriSchemeHttps);

            if (!isMatch || !isValidUri)
                throw new ArgumentException($"The profile url '{ProfileUrl}' is not valid.");
        }
    }
}
