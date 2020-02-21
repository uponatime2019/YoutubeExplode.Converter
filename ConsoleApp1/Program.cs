using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Models.MediaStreams;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new YoutubeClient();
            var converter = new YoutubeConverter(client); // re-using the same client instance for efficiency, not required

            // Get media stream info set
            var mediaStreamInfoSet = await client.GetVideoMediaStreamInfosAsync("NIJHqNWMtAw");

            // Select audio stream
            var audioStreamInfo = mediaStreamInfoSet.Audio.WithHighestBitrate();

            // Select video stream
            var videoStreamInfo = mediaStreamInfoSet.Video.FirstOrDefault(s => s.VideoQualityLabel.Contains("360"));

            // Combine them into a collection
            var mediaStreamInfos = new MediaStreamInfo[] { audioStreamInfo, videoStreamInfo };

            // Download and process them into one file
            await converter.DownloadAndProcessMediaStreamsAsync(mediaStreamInfos, @"video.mp4", "mp4", new MyProgress());

            Console.WriteLine("Done");
        }
    }

    public class MyProgress : IProgress<double>
    {
        double previousValue;
        public void Report(double value)
        {
            if (value - previousValue >= 0.01)
            {
                Console.WriteLine($"Download progress: {value * 100} %");
                previousValue = value;
            }
        }
    }
}
