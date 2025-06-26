using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// https://postman-echo.com/time/now 
/// Returns raw text as UTC time
/// </summary>
public class UtcTimeService
{
    private readonly HttpClient _client;
    private readonly string url = "https://postman-echo.com/time/now";
    public DateTime DateTime { get; private set; }

    public UtcTimeService()
    {
        _client = new HttpClient();
    }

    public async Task<DateTime?> GetUtcTimeAsync()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            DateTime serverTimeUtc = DateTime.Parse(responseBody).ToUniversalTime();

            return serverTimeUtc;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Error fetching UTC time: " + e.Message);
            return null;
        }
    }

    public async Task<TimeSpan?> GetTimeDifferenceFromNowAsync(string otherTimeStr)
    {
        DateTime otherTime = DateTime.Parse(otherTimeStr, null, DateTimeStyles.RoundtripKind);
        DateTime nowUtc = await GetUtcTimeAsync() ?? DateTime.UtcNow;

        TimeSpan difference = otherTime - nowUtc;
        return difference < TimeSpan.Zero ? TimeSpan.Zero : difference;
    }
}
