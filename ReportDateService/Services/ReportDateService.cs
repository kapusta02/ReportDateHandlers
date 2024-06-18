using Grpc.Core;
using ReportDateService;

namespace ReportDateHandlers.Services;

public class ReportDateService : ReportDate.ReportDateBase
{
    public override Task<ReportDateResponse> GetReportDate(ReportDateRequest request, ServerCallContext context)
    {
        var date = string.IsNullOrEmpty(request.Date) ? DateTime.Now : DateTime.Parse(request.Date);
        var dayOfMonth = request.DayOfMonth;
        var adjust = request.Adjust;

        var nextMonthDate = date.AddMonths(1);
        var daysInNextMonth = DateTime.DaysInMonth(nextMonthDate.Year, nextMonthDate.Month);

        DateTime resultDate;
        if (dayOfMonth <= daysInNextMonth)
            resultDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, dayOfMonth);
        else if (adjust)
            resultDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, daysInNextMonth);
        else
            resultDate = new DateTime(nextMonthDate.AddMonths(1).Year, nextMonthDate.AddMonths(1).Month, dayOfMonth);

        return Task.FromResult(new ReportDateResponse
        {
            ReportDate = resultDate.ToString("yyyy-MM-dd")
        });
    }
}