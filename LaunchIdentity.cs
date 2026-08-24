using System;
using Newtonsoft.Json;

namespace SPTvP.UserNicknamePlugin;

internal sealed class LaunchIdentity {
    [JsonProperty("launchId")]
    public string LaunchId { get; set; } = string.Empty;

    [JsonProperty("accountId")]
    public string AccountId { get; set; } = string.Empty;

    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;

    [JsonProperty("communityId")]
    public string CommunityId { get; set; } = string.Empty;

    [JsonProperty("expiresAtUtc")]
    public DateTimeOffset ExpiresAtUtc { get; set; }
}
