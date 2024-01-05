using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using COMP2001_Assessment2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.ComponentModel;

namespace COMP2001_Assessment2.Controllers
{
    public class UpdateProfile : Controller
    {
        private string userEmail = "";
        private readonly ILogger<UpdateProfile> _logger;
        
        private readonly IHttpClientFactory _clientFactory;
        public UpdateProfile(ILogger<UpdateProfile> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            // Get User Id through session
            var userId = HttpContext.Session.GetInt32("UserId");
            userEmail = HttpContext.Session.GetString("UserEmail");

            if (userId == null)
            {
                // if user id not exist then redirect back to login
                return RedirectToAction("Index", "Home");
            }

            // Get User personal information
            var profile = await GetProfileInfo(userId.Value);
            // Get follower and followed count
            var (followerCount, followedCount) = await GetFollowerFollowedCount(userId.Value);

            // Update the profile model with follower and followed counts
            profile.FollowerCount = followerCount;
            profile.FollowedCount = followedCount;


            // pass user information to view
            return View("~/Views/UpdateProfile/Index.cshtml", profile);
        }


        //Get User Profile Informaiton method
        private async Task<ProfileModel> GetProfileInfo(int userId)
        {
            var apiUrl = $"https://localhost:7037/api/Profiles/{userId}";
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringDateTimeConverter("ProfileBirthday"),
                            new JsonStringDateTimeConverter("JoinDate")
                        }
                };

                var profile = JsonSerializer.Deserialize<Profile>(content, jsonOptions);

                var profileModel = new ProfileModel
                {
                    // Show Related Data
                    ProfileName = profile.ProfileName,
                    Email = userEmail,
                    Bio = profile.Bio,
                    ImageUrl = profile.ImageUrl,
                    Birthday = profile.ProfileBirthday,
                    JoinDate = profile.JoinDate
                };

                return profileModel;
            }

            return null;
        }


        public class JsonStringDateTimeConverter : JsonConverter<DateTime>
        {
            private readonly string _propertyName;

            public JsonStringDateTimeConverter(string propertyName)
            {
                _propertyName = propertyName;
            }

            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && reader.GetString() != null && reader.GetString() != string.Empty)
                {
                    if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    {
                        return date;
                    }
                }

                return default;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd")); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileModel profile)
        {
            // Get user id
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                // if user id not exist then redirect back to login
                return RedirectToAction("Index", "Home");
            }

            // call api to update user profile
            var isSuccess = await UpdateProfileInfo(userId.Value, profile);

            if (isSuccess)
            {
                // Update Successful
                return RedirectToAction("Index", "UpdateProfile");
            }
            else
            {
                return View("Error");
            }
        }

        // Update User Information Method
        private async Task<bool> UpdateProfileInfo(int userId, ProfileModel profilemodel)
        {
            var apiUrl = $"https://localhost:7037/api/Profiles/{userId}";
            var client = _clientFactory.CreateClient();
            var Userprofile = await GetProfileInfo(userId);

            var profileData = new UpdateProfileModel
            {
                ProfileId = userId,
                UserId = userId,
                ProfileName = profilemodel.ProfileName,
                ImageUrl = Userprofile.ImageUrl,
                ProfileBirthday = profilemodel.Birthday,
                Bio = profilemodel.Bio,
                JoinDate = Userprofile.JoinDate
            };
            _logger.LogInformation($"User Credential: {profilemodel.ProfileName+' '+profilemodel.Bio+' '+profilemodel.ImageUrl+' '+profilemodel.Birthday+' '+profilemodel.JoinDate}");
            var jsonOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringDateTimeConverter("ProfileBirthday") 
                }
            };
            var jsonContent = JsonSerializer.Serialize(profileData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            // Read and log the actual content
            string actualContent = await content.ReadAsStringAsync();
            _logger.LogInformation($"API Json Content: {actualContent}");

            var response = await client.PutAsync(apiUrl, content);

            return response.IsSuccessStatusCode;
        }

        private async Task<(int, int)> GetFollowerFollowedCount(int userId)
        {
            var apiUrl = $"https://localhost:7037/api/FollowRelationships";
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                        {
                            new JsonStringDateTimeConverter("FollowDate"),
                        }
                };
                var content = await response.Content.ReadAsStringAsync();
                var followRelationships = JsonSerializer.Deserialize<List<FollowRelationship>>(content,jsonOptions);
                _logger.LogInformation($"FollowRelationships data: {JsonSerializer.Serialize(followRelationships)}");

                int followerCount = followRelationships.Count(fr => fr.FollowerProfileId == userId);
                int followedCount = followRelationships.Count(fr => fr.FollowedProfileId == userId);

                return (followerCount, followedCount);
            }

            return (0, 0); 
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 

            return RedirectToAction("Index", "Home"); 
        }

    }

}


