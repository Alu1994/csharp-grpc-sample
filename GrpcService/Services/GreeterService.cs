using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly TestgRPCDI _testgRPCDI;

        public GreeterService(ILogger<GreeterService> logger, TestgRPCDI testgRPCDI)
        {
            _logger = logger;
            _testgRPCDI = testgRPCDI;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public async override Task SayHelloByStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var texts = new []
            {
                "Hello", "Matheus", "Veiga", "My", "Friend."
            };

            foreach (var text in texts)
            {
                await Task.Delay(1000);
                var message = new HelloReply { Message = text };
                await responseStream.WriteAsync(message);
            }

            await Task.Delay(1000);
            var messageAge = new HelloReply { Message = $"Age: {_testgRPCDI.Age.ToString()}" };
            await responseStream.WriteAsync(messageAge);
        }
    }
}