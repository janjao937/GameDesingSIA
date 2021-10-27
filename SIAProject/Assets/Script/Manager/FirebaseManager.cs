using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using FullSerializer;
using TMPro;
using SimpleJSON;
using UnityEngine.Events;
using System;

public class FirebaseManager : Singleton<FirebaseManager>
{
    
    [Header("Sign IN")]
    public TMP_InputField emailSignin;
    public TMP_InputField passwordSignin;
    public TMP_Text warningSigninText;

    [Header("Sign UP")]
    public TMP_InputField usernameSignup;
    public TMP_InputField emailSignup;
    public TMP_InputField passwordSignup;
    public TMP_InputField confirmPasswordSinup;
    public TMP_Text warningSignupText;

    [Header("Firebase")]
    private static string databaseURL = "https://sia-firebase-125eb-default-rtdb.asia-southeast1.firebasedatabase.app/users";
    private string apikey = "AIzaSyA9rERnZuGm9k4gNXePzvFh_NJ4TdCFmHU";
    private static string secret = "9ddqSj28nVB0nUOf09Qe4ArqRrTbhRruDA2ALMRd";
    private static string idToken ;
    private string getLocalId;
    public static fsSerializer serializer = new fsSerializer();
    public static string localId;

    [Header("User Info")]
    public string email;
    public string username;
    public int score;

    [Space]
    public UISignScene uISignScene;
    public event Action OnSetUserRank;

    [Space]
    public UserInfo userInfo;
    public List<UserScore> userScore;
    
    private void Awake()
    {

        DontDestroyOnLoad(gameObject);
    }

    public void SignUpButton()
    {
        SignUp(usernameSignup.text,emailSignup.text, passwordSignup.text);
    }
    public void SignInButton()
    {
        SignIn(emailSignin.text, passwordSignin.text);
    }
    
    private void SignUp(string _username,string _email, string _password)
    {
        if (_username == "")
        {
            warningSignupText.text = "Username Missng";
        }
        else if(_email == "")
        {
            warningSignupText.text = "Email Missing";
        }
        else if (passwordSignup.text != confirmPasswordSinup.text)
        {
            warningSignupText.text = "Password Doesn't Macth";
        }
        else
        {
            string userData = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
            RestClient.Post<SignResponse>($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key={apikey}", userData).Then(response =>
            {
                string emailVerification = "{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"" + response.idToken + "\"}";
                RestClient.Post($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/getOobConfirmationCode?key={apikey}", emailVerification);
                localId = response.localId;
                username = _username;
                email = _email;
                PosttoDatabase(response.idToken);

            }).Catch(error =>
            {
                Debug.Log(error);
            });
        }
        
    }
    private void SignIn(string _email, string _password)
    {
        string userData = "{\"email\":\"" + _email + "\",\"password\":\"" + _password + "\",\"returnSecureToken\":true}";
        RestClient.Post<SignResponse>($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={apikey}", userData).Then(response =>
        {
            string emailVerification = "{\"idToken\":\"" + response.idToken + "\"}";
            RestClient.Post($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/getAccountInfo?key={apikey}", emailVerification).Then(
                emailResponse =>
                {
                    fsData emailVerificationData = fsJsonParser.Parse(emailResponse.Text);
                    EmailConfirm emailConfirmationInfo = new EmailConfirm();
                    serializer.TryDeserialize(emailVerificationData, ref emailConfirmationInfo).AssertSuccessWithoutWarnings();
                    if (emailConfirmationInfo.users[0].emailVerified)
                    {
                        idToken = response.idToken;
                        localId = response.localId;
                        GetUserName();
                        warningSigninText.text = "Login Complete";
                        uISignScene.SignInComplete();
                        Debug.Log("Signin Complete");
                    }
                    else
                    {
                        Debug.Log("You are stupid, you need to verify your email dumb");
                    }
                });

        }).Catch(error =>
        {
            Debug.Log("Signin Error");
        });
    }
    private void PosttoDatabase(string idTokenTemp = "")
    {
        if(idTokenTemp == "")
        {
            idTokenTemp = idToken;
        }
        UserInfo user = new UserInfo(email,username, score);
        Debug.Log($"{email} : {username} : {score}");
        RestClient.Put<UserInfo>($"{databaseURL}/{localId}.json?auth={idTokenTemp}", user).Then(response =>
        {
            Debug.Log("Put Database");
        }).Catch(error =>
        {
            Debug.Log("Put Database Error");
        });
    }
    private void RetrieveFromDatabase()
    {
        RestClient.Get<UserInfo>($"{databaseURL}/{getLocalId}.json?auth={idToken}").Then(response =>
        {
            userInfo.username = response.username;
            userInfo.score = response.score;
            RankManager.Instance.Invoke("SetUserRank", 0);
            Debug.Log(" Get Retrieve From Database");
        }).Catch(error =>
        {
            Debug.Log("Error Retrieve From Database");
        });
    }
    private void GetUserName()
    {
        RestClient.Get<UserInfo>($"{databaseURL}/{localId}.json?auth={idToken}").Then(response =>
        {
            username = response.username;
            email = response.email;
            score = response.score;
            UserSelf.getUsername = username;
            Debug.Log("Get Username");
        }).Catch(error =>
        {
            Debug.Log("Error Get User Name");
        });
    }
    public void GetLocalID()
    {
        RestClient.Get($"{databaseURL}.json?auth={idToken}").Then(response =>
        {
            var email = emailSignin.text;
            fsData userData = fsJsonParser.Parse(response.Text);
            Dictionary<string, UserInfo> emails = null;
            serializer.TryDeserialize(userData, ref emails);

            foreach (var user in emails.Values)
            {
                if (user.email == email)
                {
                    getLocalId = user.localId;
                    RetrieveFromDatabase();
                    Debug.Log("GetLocalID Complete");
                    break;
                }
            }
        }).Catch(error =>
        {
            Debug.Log("GetLocalID Error");
        });

    }
    public void GetData()
    {
        RestClient.Get($"{databaseURL}.json?auth={secret}").Then(response =>
        {
            JSONNode jsonNode = JSON.Parse(response.Text);

            userScore = new List<UserScore>();

            for (int i = 0; i < jsonNode.Count; i++)
            {
                userScore.Add(new UserScore(jsonNode[i]["username"], jsonNode[i]["score"]));
            }

            GetLocalID();
            RankManager.Instance.SetRankLeader();
            Debug.Log("Get Data");
        }).Catch(error => 
        {
            Debug.Log("Get Data Error");

        });
    }
}
