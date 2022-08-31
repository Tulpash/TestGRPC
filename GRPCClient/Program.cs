using Grpc.Net.Client;
using GRPCClient;

using (var channel = GrpcChannel.ForAddress("https://localhost:7141"))
{
    var client = new Image.ImageClient(channel);
    using (var response = client.Upload())
    {
        //File is in the folder (..\TestGRPC\GRPCClient\bin\Debug\net6.0)
        string filr = "test.jpg";
        Console.WriteLine("Start");
        var read = Task.Run(async () => await ImageSender.ReciveImage(response.ResponseStream));
        await ImageSender.SendImage(response.RequestStream, filr);
        await response.RequestStream.CompleteAsync();
        await read;
        Console.WriteLine("Complete");
    }
}
Console.ReadLine();