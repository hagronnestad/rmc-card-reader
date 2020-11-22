using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace RmcCardReader {
    static class Program {
        // Add your Computer Vision subscription key and endpoint to your environment variables.
        static string subscriptionKey = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");

        // An endpoint should have a format like "https://westus.api.cognitive.microsoft.com"
        static string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

        // the Batch Read method endpoint
        static string uriBase = endpoint + "/vision/v3.1/read/analyze";

        // Add a local image with text here (png or jpg is OK)
        static string imageFilePath = @"E:\HAG'S LAB\In Progress\Retro Tea Breaks Cards\Images\Cards\06-orig.png";


        static void Main(string[] args) {
            // Call the REST API method.
            Console.WriteLine("\nExtracting text...\n");
            ReadText(imageFilePath).Wait();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Gets the text from the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with text.</param>
        static async Task ReadText(string imageFilePath) {
            try {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                string url = uriBase;

                HttpResponseMessage response;

                // Two REST API methods are required to extract text.
                // One method to submit the image for processing, the other method
                // to retrieve the text found in the image.

                // operationLocation stores the URI of the second REST API method,
                // returned by the first REST API method.
                string operationLocation;

                // Reads the contents of the specified local image
                // into a byte array.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                // Adds the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData)) {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // The first REST API method, Batch Read, starts
                    // the async process to analyze the written text in the image.
                    response = await client.PostAsync(url, content);
                }

                // The response header for the Batch Read method contains the URI
                // of the second method, Read Operation Result, which
                // returns the results of the process in the response body.
                // The Batch Read operation does not return anything in the response body.
                if (response.IsSuccessStatusCode)
                    operationLocation =
                        response.Headers.GetValues("Operation-Location").FirstOrDefault();
                else {
                    // Display the JSON error data.
                    string errorString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("\n\nResponse:\n{0}\n",
                        JToken.Parse(errorString).ToString());
                    return;
                }

                // If the first REST API method completes successfully, the second 
                // REST API method retrieves the text written in the image.
                //
                // Note: The response may not be immediately available. Text
                // recognition is an asynchronous operation that can take a variable
                // amount of time depending on the length of the text.
                // You may need to wait or retry this operation.
                //
                // This example checks once per second for ten seconds.
                string contentString;
                int i = 0;
                do {
                    System.Threading.Thread.Sleep(1000);
                    response = await client.GetAsync(operationLocation);
                    contentString = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1);

                if (i == 60 && contentString.IndexOf("\"status\":\"succeeded\"") == -1) {
                    Console.WriteLine("\nTimeout error.\n");
                    return;
                }

                // Display the JSON response.
                //Console.WriteLine("\nResponse:\n\n{0}\n", JToken.Parse(contentString).ToString());

                var j = JToken.Parse(contentString);
                var lines = j.SelectToken("analyzeResult.readResults[0].lines");

                var sb = new StringBuilder();

                foreach (var line in lines) {
                    var text = line.SelectToken("text").ToString();

                    text = text.Replace('2', '0');
                    text = text.Replace('3', '0');
                    text = text.Replace('4', '0');
                    text = text.Replace('5', '0');
                    text = text.Replace('6', '0');
                    text = text.Replace('7', '0');
                    text = text.Replace('9', '0');
                    text = text.Replace('D', '0');
                    text = text.Replace('F', '0');
                    text = text.Replace('Q', '0');
                    text = text.Replace('@', '0');
                    text = text.Replace('%', '0');

                    text = text.Replace('|', '1');
                    text = text.Replace(':', '1');
                    text = text.Replace(')', '1');
                    text = text.Replace('T', '1');

                    text = text.Replace(".", "");
                    text = text.Replace("-", "");
                    text = text.Replace(",", "");
                    text = text.Replace("+", "");
                    text = text.Replace(" ", "");

                    Console.WriteLine(text);
                    Console.WriteLine("");

                    if (!ValidateCharData(text)) continue;

                    char c;
                    string s;

                    if (text.Length >= 8) {
                        s = new string(text.Take(8).ToArray());
                        if (ValidateCharData(s)) sb.Append(s);

                        if (text.Length >= 16) {
                            s = new string(text.Skip(8).Take(8).ToArray());
                            if (ValidateCharData(s)) sb.Append(s);

                            if (text.Length >= 24) {
                                s = new string(text.Skip(16).Take(8).ToArray());
                                if (ValidateCharData(s)) sb.Append(s);

                                if (text.Length >= 32) {
                                    s = new string(text.Skip(24).Take(8).ToArray());
                                    if (ValidateCharData(s)) sb.Append(s);

                                    if (text.Length >= 40) {
                                        s = new string(text.Skip(32).Take(8).ToArray());
                                        if (ValidateCharData(s)) sb.Append(s);
                                    }
                                }
                            }
                        }
                    }

                }

                Console.WriteLine(sb.ToString());
                Console.WriteLine("");
                Console.WriteLine(Encoding.UTF8.GetString(BitStringToBytes(sb.ToString())));

            } catch (Exception e) {
                Console.WriteLine("\n" + e.Message);
            }
        }

        public static bool ValidateCharData(string text) {
            //if (!text.StartsWith("0")) return false; // Would only work for ASCII, but we're actually dealing with UTF-8

            if (text.Any(x => x != '1' && x != '0')) return false;

            var bytes = BitStringToBytes(text);
            if (bytes.Any(x => x < ' ')) return false;

            return true;
        }

        public static byte[] BitStringToBytes(string input) {
            int numOfBytes = input.Length / 8;
            var bytes = new byte[numOfBytes];

            for (int i = 0; i < numOfBytes; ++i) {
                bytes[i] = Convert.ToByte(input.Substring(8 * i, 8), 2);
            }

            return bytes;
        }

        static byte[] GetImageAsByteArray(string file) {
            using FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
