using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;

namespace SPTvP.UserNicknamePlugin;

internal static class LaunchIdentityReader {
    private const int MaximumFileSize = 64 * 1024;

    internal static bool TryRead(
        string handoffPath,
        [NotNullWhen(true)] out LaunchIdentity? identity,
        out string error
    )
    {
        identity = null;
        error = string.Empty;

        if(string.IsNullOrWhiteSpace(handoffPath)){
            error = "The launch session path is missing.";
            return false;
        }

        try{
            string fullPath = Path.GetFullPath(handoffPath);

            if(!File.Exists(fullPath)){
                error = "The launch session file doesn't exist.";
                return false;
            }

            var file = new FileInfo(fullPath);
            
            if(file.Length == 0){
                error = "The launch session file is empty.";
                return false;
            }

            if(file.Length > MaximumFileSize){
                error = "The launch session file exceeds 64 KiB.";
                return false;
            }

            string json = File.ReadAllText(fullPath);

            identity = JsonConvert.DeserializeObject<LaunchIdentity>(json);

            if(identity == null){
                error = "The launch session file couldn't be read.";
                return false;
            }

            if(string.IsNullOrWhiteSpace(identity.LaunchId)){
                error = "The launch ID is missing.";
                return false;
            }

            if(string.IsNullOrWhiteSpace(identity.AccountId)){
                error = "The account ID is missing.";
                return false;
            }

            if(string.IsNullOrWhiteSpace(identity.Username)){
                error = "The username is missing.";
                return false;
            }

            if(identity.Username != identity.Username.Trim()){
                error = "The username contains surrounding whitespace.";
                return false;
            }

            if(string.IsNullOrWhiteSpace(identity.CommunityId)){
                error = "The community ID is missing.";
                return false;
            }

            if(identity.ExpiresAtUtc <= DateTimeOffset.UtcNow){
                error = "The launch session has expired.";
                return false;
            }

            return true;
        }
        catch(JsonException){
            error = "The launch session file contains invalid JSON.";
            identity = null;
            return false;
        }
        catch(IOException){
            error = "The launch session file could not be opened.";
            identity = null;
            return false;
        }
        catch(UnauthorizedAccessException){
            error = "Access to the launch session file was denied.";
            identity = null;
            return false;
        }
        catch(ArgumentException){
            error = "The launch session path is invalid.";
            identity = null;
            return false;
        }
    }
}
