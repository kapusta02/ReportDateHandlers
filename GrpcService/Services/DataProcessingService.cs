using Grpc.Core;
using GrpcService;

namespace ReportDateHandlers.Services;

public class DataProcessingService : DataProcessing.DataProcessingBase
{
    public override Task<DataResponse> ProcessData(DataRequest request, ServerCallContext context)
    {
        string processedData = $"Processed: {request.Data}";
        
        return Task.FromResult(new DataResponse {Result = processedData});
    }
}