using FilesProj.Core.DTOs;
using FilesProj.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilesProj.Service.Services
{
    public class PyService:IPyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _pythonApiUrl;

        // HttpClient is needed to make HTTP requests to your Python API
        public PyService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Get API URL from environment variable
            _pythonApiUrl = Environment.GetEnvironmentVariable("PYTHON_API_URL");
        }

        public async Task<PyResponse> ProcessPromptAsync(PromptModel promptModel)
        {
            try
            {
                // Serialize the request model to JSON
                var jsonContent = JsonSerializer.Serialize(promptModel);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send the request to the Python API
                var response = await _httpClient.PostAsync(_pythonApiUrl, content);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read and deserialize the response
                var responseContent = await response.Content.ReadAsStringAsync();
                var pythonResponse = JsonSerializer.Deserialize<PyResponse>(responseContent);

                // Return only the status and data (not the response time)
                return new PyResponse
                {
                    Status = pythonResponse.Status,
                    Data = pythonResponse.Data
                };
            }
            catch (Exception ex)
            {
                // Simple error handling without logging
                return new PyResponse
                {
                    Status = "error",
                    Data = new Dictionary<string, object>
                    {
                        ["error"] = ex.Message
                    }
                };
            }
        }

        
    }
}
