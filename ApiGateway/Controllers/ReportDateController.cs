using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using ReportDateService;

namespace ApiGateway.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ReportDateController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetReportDate([FromQuery] int dayOfMonth, [FromQuery] string? date,
        [FromQuery] bool adjust = true)
    {
        if (dayOfMonth < 1 || dayOfMonth > 31)
            return BadRequest("Invalid dayOfMonth value.");

        var dateNow = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("yyyy-MM-dd") : date;

        var grpcServiceUrl = configuration["GrpcServiceUrl"];
        if (string.IsNullOrEmpty(grpcServiceUrl))
            return BadRequest("Отсутствует или недопустимая конфигурация GrpcServiceUrl.");

        using var channel = GrpcChannel.ForAddress(grpcServiceUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }
        });

        var client = new ReportDate.ReportDateClient(channel);

        var request = new ReportDateRequest
        {
            DayOfMonth = dayOfMonth,
            Date = dateNow,
            Adjust = adjust
        };

        try
        {
            var response = await client.GetReportDateAsync(request);
            return Ok(response.ReportDate);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}