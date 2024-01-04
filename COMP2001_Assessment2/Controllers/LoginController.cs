using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using COMP2001_Assessment2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace COMP2001_Assessment2.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Validate email and password
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and password are required.");
            }

            // Prepare the payload for the authentication API
            var loginPayload = new
            {
                email,
                password
            };

            // Convert payload to JSON
            var jsonPayload = JsonSerializer.Serialize(loginPayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Call the authentication API
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var authUrl = "https://web.socem.plymouth.ac.uk/COMP2001/auth/api/users"; // First API to check user
                    var response = await httpClient.PostAsync(authUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var verificationStatus = JsonSerializer.Deserialize<List<string>>(responseContent);

                        if (verificationStatus != null && verificationStatus.Count == 2 && verificationStatus[0] == "Verified" && verificationStatus[1] == "True")
                        {
                            // If authentication succeeds, call the API to get all users
                            var getAllUsersUrl = "https://localhost:7037/api/Users"; // Relative path to retrieve all users
                            var getAllUsersResponse = await httpClient.GetAsync(getAllUsersUrl);
                            _logger.LogInformation($"API status Content: {getAllUsersResponse}");

                            if (getAllUsersResponse.IsSuccessStatusCode)
                            {

                                var allUsersContent = await getAllUsersResponse.Content.ReadAsStringAsync();
                                _logger.LogInformation($"API Response Content: {allUsersContent}");
                                _logger.LogInformation($"User Credential Content: {email + ' '+password}");
                                var allUsers = JsonSerializer.Deserialize<List<User>>(allUsersContent);
                                _logger.LogInformation($"API Json Content: {JsonSerializer.Serialize(allUsers)}");
                                

                                // Filter the user by email
                                var usersWithEmail = allUsers.Where(u => u.Email == email).ToList();

                                _logger.LogInformation("List of Users with Email:");
                                foreach (var user in usersWithEmail)
                                {
                                    _logger.LogInformation($"User ID: {user.UserId}, Email: {user.Email}, Password: {user.Password}");
                                }

                                if (usersWithEmail.Count > 0)
                                {
                                    // Perform additional checks if necessary to validate the correct user
                                    var user = usersWithEmail.FirstOrDefault(u => u.Password == password);
                                    _logger.LogInformation($"Password Response Content: {user}");

                                    if (user != null)
                                    {
                                        var userId = user.UserId;
                                        // Save the user ID in session
                                        HttpContext.Session.SetInt32("UserId", userId);
                                        // Save the email in session
                                        HttpContext.Session.SetString("UserEmail", email);

                                        // Redirect to the profile page
                                        return RedirectToAction("Index", "UpdateProfile");
                                    }
                                    else
                                    {
                                        _logger.LogInformation("Password did not match for any user.");
                                    }
                                }
                                else
                                {
                                    _logger.LogInformation("user email not exits");
                                }
                            }
                        }
                    }

                    // If authentication fails or user not found, return to login page
                    return RedirectToAction("Index", "Home"); // Redirect to login page
                }
                catch (HttpRequestException ex)
                {
                    string errorMessage = "An error occurred while processing your request.";

                    if (ex.StatusCode.HasValue)
                    {
                        var response = ex.Message; // If you need the entire exception message
                        errorMessage = response;
                    }

                    // Log or handle the exception and error message here

                    return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
                }
            }
        }
    }
}