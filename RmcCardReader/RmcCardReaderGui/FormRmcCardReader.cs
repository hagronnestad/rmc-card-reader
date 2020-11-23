using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RmcCardReaderGui {
    public partial class FormRmcCardReader : Form {

        // These are taken from the command line arguments and
        // populated by the Main method in Program.cs
        public static string azureKey = "";
        public static string azureUrl = "";

        private string uriBase = azureUrl + "/vision/v3.1/read/analyze";

        private Image _image = null;
        private Image _imageWithOverlay = null;

        private FormProcessing _fp = new FormProcessing();

        public FormRmcCardReader() {
            InitializeComponent();
        }

        private void PictureBoxCard_DragDrop(object sender, DragEventArgs e) {
            if (GetFilename(out string filename, e)) {
                _image = Image.FromFile(filename);
                _imageWithOverlay = Image.FromFile(filename);
                PictureBoxCard.Image = _imageWithOverlay;
            }
        }

        private void FormRmcCardReader_Load(object sender, EventArgs e) {
            PictureBoxCard.AllowDrop = true;
        }

        private void PictureBoxCard_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Copy;
        }

        private async void ButtonGo_Click(object sender, EventArgs e) {
            if (_image == null) {
                MessageBox.Show("You need to drag and drop a card into the card area!", "Missing card!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            ReadText(ImageToByteArray(_image));
            _fp.ShowDialog();
        }

        private void TextBoxBitString_TextChanged(object sender, EventArgs e) {
            var s = new string(TextBoxBitString.Text.Where(x => x == '0' || x == '1').ToArray());
            var t = Encoding.UTF8.GetString(BitStringToBytes(s));
            TextBoxBitStringText.Text = FormatBitStringText(t);
            TextBoxResult.Text = t;
        }


        /// <summary>
        /// Gets the text from the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="byteData">The image file with text.</param>
        private async Task ReadText(byte[] byteData) {
            try {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureKey);

                string url = uriBase;

                HttpResponseMessage response;

                // Two REST API methods are required to extract text.
                // One method to submit the image for processing, the other method
                // to retrieve the text found in the image.

                // operationLocation stores the URI of the second REST API method,
                // returned by the first REST API method.
                string operationLocation;

                // Adds the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(byteData)) {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // The first REST API method, Batch Read, starts
                    // the async process to analyze the written text in the image.
                    response = await client.PostAsync(url, content);
                }

                // The response header for the Batch Read method contains the URI
                // of the second method, Read Operation Result, which
                // returns the results of the process in the response body.
                // The Batch Read operation does not return anything in the response body.
                if (response.IsSuccessStatusCode) {
                    operationLocation = response.Headers.GetValues("Operation-Location").FirstOrDefault();

                } else {

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

                // Parse JSON result from Azure and get the lines array
                var j = JToken.Parse(contentString);
                var lines = j.SelectToken("analyzeResult.readResults[0].lines");

                var sb = new StringBuilder();

                var p = new Pen(Color.Red, 1.5f);
                var f = new Font("Courier New", 18, FontStyle.Bold);
                var b = new SolidBrush(Color.Yellow);
                var g = Graphics.FromImage(_imageWithOverlay);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Iterate through all detected lines
                foreach (var line in lines) {
                    var text = line.SelectToken("text").ToString();
                    var boundingBox = line.SelectToken("boundingBox").ToArray();

                    // Draw a bounding box around the detected text
                    g.DrawLine(p, (int)boundingBox[0], (int)boundingBox[1], (int)boundingBox[2], (int)boundingBox[3]);
                    g.DrawLine(p, (int)boundingBox[2], (int)boundingBox[3], (int)boundingBox[4], (int)boundingBox[5]);
                    g.DrawLine(p, (int)boundingBox[4], (int)boundingBox[5], (int)boundingBox[6], (int)boundingBox[7]);
                    g.DrawLine(p, (int)boundingBox[6], (int)boundingBox[7], (int)boundingBox[0], (int)boundingBox[1]);

                    // Do some rough cleanup of characters that are sometimes
                    // recognized incorrectly
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

                    if (!ValidateCharData(text)) continue;

                    string s;

                    var lb = new StringBuilder();

                    if (text.Length >= 8) {
                        s = new string(text.Take(8).ToArray());
                        if (ValidateCharData(s)) lb.Append(s);

                        if (text.Length >= 16) {
                            s = new string(text.Skip(8).Take(8).ToArray());
                            if (ValidateCharData(s)) lb.Append(s);

                            if (text.Length >= 24) {
                                s = new string(text.Skip(16).Take(8).ToArray());
                                if (ValidateCharData(s)) lb.Append(s);

                                if (text.Length >= 32) {
                                    s = new string(text.Skip(24).Take(8).ToArray());
                                    if (ValidateCharData(s)) lb.Append(s);

                                    if (text.Length >= 40) {
                                        s = new string(text.Skip(32).Take(8).ToArray());
                                        if (ValidateCharData(s)) lb.Append(s);
                                    }
                                }
                            }
                        }
                    }

                    // Draw the detected text inside the bounding box
                    g.DrawString(Encoding.UTF8.GetString(BitStringToBytes(lb.ToString())), f, b, (int)boundingBox[0], (int)boundingBox[1]);
                    sb.Append(lb);
                }

                // Update image with drawn overlay and other controls
                PictureBoxCard.Image = _imageWithOverlay;

                var textResult = Encoding.UTF8.GetString(BitStringToBytes(sb.ToString()));
                TextBoxBitString.Text = FormatBitString(sb.ToString());
                TextBoxBitStringText.Text = FormatBitStringText(textResult);
                TextBoxResult.Text = textResult;

            } catch (Exception e) {
                Console.WriteLine("\n" + e.Message);
                MessageBox.Show(e.Message, "Error During Text Extraction", MessageBoxButtons.OK, MessageBoxIcon.Error);

            } finally {
                _fp.Close();
            }
        }


        public byte[] ImageToByteArray(Image imageIn) {
            using (var ms = new MemoryStream()) {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public string FormatBitString(string s) {
            var sb = new StringBuilder();

            for (int i = 0; i < s.Length; i += 8) {
                if (i > 0 && (i % 40 == 0)) sb.AppendLine();
                sb.Append(s.Skip(i).Take(8).ToArray());
                sb.Append(" ");
            }

            return sb.ToString();
        }

        public string FormatBitStringText(string s) {
            var sb = new StringBuilder();

            for (int i = 0; i < s.Length; i++) {
                if (i > 0 && (i % 5 == 0)) sb.AppendLine();
                sb.Append(s[i]);
            }

            return sb.ToString();
        }

        protected bool GetFilename(out string filename, DragEventArgs e) {
            bool ret = false;
            filename = string.Empty;

            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy) {
                if (e.Data.GetData("FileName") is Array data) {
                    if ((data.Length == 1) && (data.GetValue(0) is String)) {
                        filename = ((string[])data)[0];
                        var ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp")) {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
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

    }
}
