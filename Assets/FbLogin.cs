﻿using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FbLogin : MonoBehaviour
{


   public Text FriendsText;

private void Awake()
{
    if (!FB.IsInitialized)
    {
        // Initialize the Facebook SDK
        FB.Init(InitCallback, OnHideUnity);
    }
    else
    {
        // Already initialized, signal an app activation App Event
        FB.ActivateApp();
    }
}

private void InitCallback()
{
    if (FB.IsInitialized)
    {
        // Signal an app activation App Event
        FB.ActivateApp();
        // Continue with Facebook SDK
        // ...
    }
    else
    {
        Debug.Log("Failed to Initialize the Facebook SDK");
    }
}

private void OnHideUnity(bool isGameShown)
{
    if (!isGameShown)
    {
        // Pause the game - we will need to hide
        Time.timeScale = 0;
    }
    else
    {
        // Resume the game - we're getting focus again
        Time.timeScale = 1;
    }
}


#region Login / Logout
public void FacebookLogin()
{
    var permissions = new List<string>() { "public_profile", "email", "user_friends" };
    FB.LogInWithReadPermissions(permissions, AuthCallback);
}

private void AuthCallback(ILoginResult result)
{
    if (FB.IsLoggedIn)
    {
        // AccessToken class will have session details
        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        // Print current access token's User ID
        Debug.Log(aToken.UserId);
        // Print current access token's granted permissions
        foreach (string perm in aToken.Permissions)
        {
            Debug.Log(perm);
        }
    }
    else
    {
        Debug.Log("User cancelled login");
    }
}

public void FacebookLogout()
{
    FB.LogOut();
}
#endregion

public void FacebookShare()
{
    FB.ShareLink(new System.Uri("https://www.lftechnology.com/"), "Check it out!",
        "We help businesses imagine and create the digital experiences of tomorrow. We succeed together by blending the best of entrepreneurship, startup thinking, and world-class engineering.",
        new System.Uri("https://www.lftechnology.com/wp-content/themes/Froggy/img/logo_leapfrog.svg"));
}

#region Inviting
public void FacebookGameRequest()
{
    FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
}

#endregion

public void GetFriendsPlayingThisGame()
{
    string query = "/me/friends";
    FB.API(query, HttpMethod.GET, result =>
    {
        var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
        var friendsList = (List<object>)dictionary["data"];
        FriendsText.text = string.Empty;
        foreach (var dict in friendsList)
            FriendsText.text += ((Dictionary<string, object>)dict)["name"];
    });
}
}
}
