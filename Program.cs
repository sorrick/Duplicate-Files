/*
AUTHOR: Richard Soria
DESCR:  Program that uses sha1 hashing to find duplicate files in folder specified.
        Outputs the specific duplicate files and how much space was wasted.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Duplicate_Files
{
    //Try using sha1 instead of adler32
    public static class StringExtensions
    {
        public static string SHA1(this string msg)
        {
            using (SHA1 sha1Hash = System.Security.Cryptography.SHA1.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(msg);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

    public class fileInfo
    {
        public string filePath { get; set; }
        public string fileHash { get; set; }
        public long size { get; set; }
        public fileInfo(string filePath, string fileHash, long size)
        {
            this.filePath = filePath;
            this.fileHash = fileHash;
            this.size = size;
        }

    }

    class Program
    {

        static void Main(string[] args)
        {   
            //Now we need to go into the folder and look through all the files
            string rootdir = @"FILEPATH";
            string[] filePaths = Directory.GetFiles(rootdir, "*", SearchOption.AllDirectories);
            
            int count = 0;

            fileInfo[] testingFile = new fileInfo[20];

            foreach (string i in filePaths)
            {

                //Console.WriteLine(adler32(File.OpenRead(i)));
                using (FileStream fs = File.OpenRead(i))
                {

                    using(SHA1 sha1Hash = SHA1.Create())
                    {
                        byte[] fileHash = sha1Hash.ComputeHash(fs);
                        string path = filePaths[count];
                        string hash = BitConverter.ToString(fileHash).Replace("-", "").ToLower();
                        long size = path.Length;
                        testingFile[count] = new fileInfo(path, hash, size);
                    }
                    
                }
               
                count++;               
            }
            
            long totalspace = 0;
            for (int i = 0;i < 4; i++)
            {
                string temphash = testingFile[i].fileHash;
                for(int j = 1; j < 5; j++)
                {
                    string temphash2 = testingFile[j].fileHash;
                    if(temphash == temphash2)
                    {
                        Console.WriteLine("Duplicates\n");
                        Console.WriteLine(testingFile[i].filePath + "\n" + testingFile[j].filePath + "\n");
                        totalspace = testingFile[i].size + testingFile[j].size;
                    }
                }
            }
            Console.WriteLine("Total wasted space is: " + totalspace);
        }
    }
}
