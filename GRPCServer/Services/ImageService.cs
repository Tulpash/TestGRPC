using Grpc.Core;

namespace TestGRPC.Services
{
    public class ImageService : Image.ImageBase
    {
        public async override Task Upload(IAsyncStreamReader<UploadImage> requestStream, IServerStreamWriter<AckImage> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Start");
            //File is in the folder (..\TestGRPC\GRPCServer)
            string pathToFile = "test.jpg";       
            using (var stream = File.OpenWrite(pathToFile))
            {
                await foreach (var item in requestStream.ReadAllAsync())
                {
                    Console.WriteLine($"Recive: {item.Id} {string.Join(',', item.Content)}");
                    await stream.WriteAsync(item.Content.ToArray());
                    await responseStream.WriteAsync(new AckImage() { Id = item.Id });
                }
            }
            Console.WriteLine("Completed");
        }
    }
}
