using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1;
using System.Threading.Tasks;
using static System.Console;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WriteLine("Hello World!");
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greetClient = new Greeter.GreeterClient(channel);
            var hello = new HelloRequest();
            hello.Name = "Matheus Veiga";
            var response = await greetClient.SayHelloAsync(hello);
            WriteLine($"gRPC Service Reply: {response.Message}");
            ReadLine();

            var helloStream = new HelloRequest();
            using (var call = greetClient.SayHelloByStream(helloStream))
            {
                while(await call.ResponseStream.MoveNext())
                {
                    var currentMessage = call.ResponseStream.Current;
                    WriteLine($"gRPC Service Reply: {currentMessage.Message}");
                }                
            }            
            ReadLine();
        }
    }
}