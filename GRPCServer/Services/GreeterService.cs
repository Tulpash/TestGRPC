using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace TestGRPC.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine("[X] Get message");
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<Names> GetNames(People request, ServerCallContext context)
        {
            return base.GetNames(request, context);
        }

        public async override Task GetMessages(Int32Value request, IServerStreamWriter<StringValue> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"[X] Recive: {request.Value}");
            Random rnd = new Random();
            for (int i = 0; i < request.Value; i++)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Request cancelled (Iteration: {i})");
                    break;
                }
                Console.WriteLine($"\tIterration: {i}");
                await responseStream.WriteAsync(new StringValue() { Value = "oottoo" });
                await Task.Delay(3000);
            }
        }
    }
}