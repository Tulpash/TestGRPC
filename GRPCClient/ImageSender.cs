using Google.Protobuf;
using Grpc.Core;

namespace GRPCClient
{
    internal class ImageSender
    {
        public static async Task SendImage(IClientStreamWriter<UploadImage> writer, string path)
        {
            int portionSize = 8;
            int blockCount;
            byte[] bytes = new byte[portionSize];

            using (var stream = File.OpenRead(path))
            {
                blockCount = (int)Math.Ceiling((double)stream.Length / portionSize);
                for (int i = 0; i < blockCount; i++)
                {
                    await stream.ReadAsync(bytes, 0, portionSize);
                    var cur = ByteString.CopyFrom(bytes);
                    Console.WriteLine($"Send: {i}");
                    await writer.WriteAsync(new UploadImage() { Content = cur, Id = i });
                }
            }
        }

        public static async Task ReciveImage(IAsyncStreamReader<AckImage> reader)
        {
            await foreach (var item in reader.ReadAllAsync())
            {
                Console.WriteLine($"Ack: {item.Id}");
            }
        }
    }
}
