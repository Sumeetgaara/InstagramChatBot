namespace ImageCaption.Services
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Microsoft.ProjectOxford.Vision;
    using Microsoft.ProjectOxford.Vision.Contract;

    /// <summary>
    /// A wrapper around the Microsoft Cognitive Computer Vision API Service.
    /// <remarks>
    /// This class makes use of the Microsoft Computer Vision SDK.
    /// SDK: https://github.com/Microsoft/ProjectOxford-ClientSDK/blob/master/Vision/Windows/ClientLibrary"
    /// </remarks>
    /// </summary>
    public class MicrosoftCognitiveCaptionService : ICaptionService
    {
        /// <summary>
        /// Microsoft Computer Vision API key.
        /// </summary>
        private static readonly string ApiKey = WebConfigurationManager.AppSettings["MicrosoftVisionApiKey"];

        /// <summary>
        /// The set of visual features we want from the Vision API.
        /// </summary>
        private static readonly VisualFeature[] VisualFeatures = { VisualFeature.Description , VisualFeature.Tags};
        private static readonly VisualFeature[] poop = { VisualFeature.Tags };

        /// <summary>
        /// Gets the caption of an image URL.
        /// <remarks>
        /// This method calls <see cref="IVisionServiceClient.AnalyzeImageAsync(string, string[])"/> and
        /// returns the first caption from the returned <see cref="AnalysisResult.Description"/>
        /// </remarks>
        /// </summary>
        /// <param name="url">The URL to an image.</param>
        /// <returns>Description if caption found, null otherwise.</returns>
        public async Task<string> GetCaptionAsync(string url)
        {
            var client = new VisionServiceClient(ApiKey);
            var result = await client.AnalyzeImageAsync(url, VisualFeatures);
            return ProcessAnalysisResult(result);
        }

        public async Task<string> GetTagsAsync(string url)
        {
            var client = new VisionServiceClient(ApiKey);
            
            var insta = await client.AnalyzeImageAsync(url, poop);
            return ProcessAnalysisInsta(insta);
        }
        /// <summary>
        /// Gets the caption of the image from an image stream.
        /// <remarks>
        /// This method calls <see cref="IVisionServiceClient.AnalyzeImageAsync(Stream, string[])"/> and
        /// returns the first caption from the returned <see cref="AnalysisResult.Description"/>
        /// </remarks>
        /// </summary>
        /// <param name="stream">The stream to an image.</param>
        /// <returns>Description if caption found, null otherwise.</returns>
        public async Task<string> GetCaptionAsync(Stream stream)
        {
            var client = new VisionServiceClient(ApiKey);
            var result = await client.AnalyzeImageAsync(stream, VisualFeatures);
            return ProcessAnalysisResult(result);
        }
        public async Task<string> GetTagsAsync(Stream stream)
        {
            var client = new VisionServiceClient(ApiKey);

            var insta = await client.AnalyzeImageAsync(stream, poop);
            return ProcessAnalysisInsta(insta);
        }

        /// <summary>
        /// Processes the analysis result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>The caption if found, error message otherwise.</returns>
        private static string ProcessAnalysisResult(AnalysisResult result)
        {
            string message = result?.Description?.Captions.FirstOrDefault()?.Text;
            string im = result?.Tags?.ElementAtOrDefault(0)?.Name;
            string im1 = result?.Tags?.ElementAtOrDefault(1)?.Name;
            string im2 = result?.Tags?.ElementAtOrDefault(2)?.Name;
            string im3 = result?.Tags?.ElementAtOrDefault(3)?.Name;
            string im4 = result?.Tags?.ElementAtOrDefault(4)?.Name;
            string im5 = " ,";
            string imf = im+im5+im1+im5+im2+im5+im3+im5+im4;


            string fm = message;

            return string.IsNullOrEmpty(fm) ?
                        "Couldn't find a caption for this one" :
                        "Your Instagram caption can be: " + message + "   " +
                        ".Most relevant tags I can think of :" + imf;
        }
        private static string ProcessAnalysisInsta(AnalysisResult insta)
        {
            string instamessage = insta?.Tags?.FirstOrDefault()?.Name;

            return string.IsNullOrEmpty(instamessage) ?
                        "Couldn't find a caption for this one" :
                        "Your Instagrams Tags will be: " + instamessage;


        }
    }
}