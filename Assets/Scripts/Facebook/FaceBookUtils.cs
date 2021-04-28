using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FaceBookUtils: MonoBehaviour
{
    public Text FriendsText;
    public bool isLoggedIn;
    private string shareLink = "https://bilson4321.github.io/kitti_game/";
    private string shareTitle = "This is kitti game post";
    private string shareDescription = "This is test post";
    private string shareImage = "http://i.imgur.com/j4M7vCO.jpg";

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

    private void Start()
    {
        if (FB.IsLoggedIn)
        {
            isLoggedIn = true;
        }
        else
        {
            isLoggedIn = false;
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
  /*  FB.ShareLink(new System.Uri("https://www.lftechnology.com/"), "Check it out!",
        "We help businesses imagine and create the digital experiences of tomorrow. We succeed together by blending the best of entrepreneurship, startup thinking, and world-class engineering.",
        new System.Uri("https://www.lftechnology.com/wp-content/themes/Froggy/img/logo_leapfrog.svg"));
*/
        FB.ShareLink(
                        new System.Uri(this.shareLink),
                        this.shareTitle,
                        this.shareDescription,
                        new System.Uri(this.shareImage));
    }

#region Inviting
public void FacebookGameRequest()
{
    FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
}

    #endregion
    public void FbGameRequest()
    {
        FB.AppRequest("Hey! Come and play this awesome game!", title: "Kitty Card Game",callback: delegate (IAppRequestResult result) {
            Debug.Log(result.RawResult);
        });
    }


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
