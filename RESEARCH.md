# Nickname plugin research

## Reference versions

- SPT: 4.1.3
- EFT: 0.16.9.40743
- Fika Client: 2.4.2
- ILSpy: 11.0.0.9375

## Initial Ideas

**BACKUP**
- Assembly: Assembly-CSharp
- Location: EFT.ClientBackendSession.CreateProfile
- Note: Patch the method's *profileNickname* with verified, launcher-provided username.
- Note2: Would still allow user's to write their own name, only on profile submission would it be changed.

==========================================

#   Profile Creation Changes

- Assembly: Assembly-CSharp
- Location: Arena.UI.NicknameField
- _statusLabel is the status of the username in the textbox, we must set it to "SPTvP Username"
- _inputField is what's in the textbox, we must set that to the validated user's username, with correct capitalisation
- _inputField.interactable needs to be set to false -> does not allow editing
- Testing of 'interactable' seemed to not work, both mentions of it in the init of NicknameField were set to false,
  yet it still seemed interactable, but it couldn't be edited. Leaving it as is.

#   General Settings Changes

- Assembly: Assembly-CSharp
- Location: EFT.UI.Settings.GameSettingsTab
- User's profile is fetched from _backEndSession.Profile, and the corresponding object is initialised
  and set to obj = profile.Nickname. (Show)
- The username gets initialised under _changedNickname (Show)
- Nickname get's displayed by (RefreshNickname) -> _nicknameInput.SetTextWithoutNotify(_changedNickname)
- Lock the UI with postfix on (Show), by setting the _nicknameInput.interactable to be false
- Lock the button to change nickname by setting the boolean _changeNicknameButton.Interactable 
  (UpdateChangeNicknameButton) to false