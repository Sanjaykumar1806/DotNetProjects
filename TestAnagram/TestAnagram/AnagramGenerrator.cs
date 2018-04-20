using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnagramTest
{
    class AnagramGenerrator
    {
        public static List<string> dictionaryWordList;
        public static bool isAnagram = true;
        public static string localPath = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName;
        public AnagramGenerrator()
        {
            dictionaryWordList = new List<string>();
            dictionaryWordList = LoadDictionary(Path.Combine(localPath, @"Dictionary\DictionaryWords.txt"));
        }
        static void Main(string[] args)
        {
            List<string> inputList = new List<string>();
            try
            {
                AnagramGenerrator obj = new AnagramGenerrator();
                inputList = obj.ReadFileData(Path.Combine(localPath, @"ImportFiles\InputWords.txt"));

                foreach (string word in inputList)
                {
                    List<string> anagramList = new List<string>();
                    //Create list of words can be made with the string letters
                    permute(word.ToCharArray(), 0, word.Length - 1, anagramList);

                    StringBuilder line = new StringBuilder();
                    string fileWord = string.Empty;
                    List<string> fileWords = new List<string>();
                    int count = 0;
                    foreach (string anagramWord in anagramList)
                    {
                        if (dictionaryWordList.BinarySearch(anagramWord) > -1)
                        {
                            if (count == 0)
                            {
                                line.Append(anagramWord + " ");
                                fileWords.Add(anagramWord);
                                count++;
                            }
                            else
                            {
                                if (fileWords.BinarySearch(anagramWord) < 0)
                                {
                                    line.Append(anagramWord + " ");
                                    fileWords.Add(anagramWord);
                                }
                            }
                        }
                    }
                    isAnagram = obj.GenerateOutPutFile(line.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Text : " + ex.InnerException);
            }
            if (isAnagram)
            {
                Console.WriteLine("Anagram words added in the file successfully.");
            }
            else
            {
                Console.WriteLine("Error Occure while adding anagram words");
            }
            Console.ReadLine();
        }

        static void permute(char[] word, int start, int end, List<string> lstAnagrams)
        {
            if (start == end)
                lstAnagrams.Add(string.Join("", word));
            else
            {
                for (int position = start; position <= end; position++)
                {
                    swap(ref word[start], ref word[position]);
                    permute(word, start + 1, end, lstAnagrams);
                    swap(ref word[start], ref word[position]);
                }
            }
        }

        static void swap(ref char a, ref char b)
        {
            char tmp;
            tmp = a;
            a = b;
            b = tmp;
        }

        public List<string> ReadFileData(string location)
        {
            var wordList = new List<string>();
            try
            {
                var fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string fileline;
                    while ((fileline = streamReader.ReadLine()) != null)
                    {
                        wordList.Add(fileline);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Text : " + ex.InnerException);
                isAnagram = false;
                throw;
            }
            return wordList;
        }

        public List<string> LoadDictionary(string location)
        {
            var dictionaryList = new List<string>();
            try
            {
                var fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string fileline;
                    while ((fileline = streamReader.ReadLine()) != null)
                    {
                        dictionaryList.Add(fileline);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Text : " + ex.InnerException);
                isAnagram = false;
                throw;
            }
            dictionaryList.Sort();
            return dictionaryList;
        }

        public bool GenerateOutPutFile(string word)
        {
            string fileName = "Result" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string outPath = Path.Combine(localPath, @"OutputFiles\" + fileName);
            try
            {
                if (!File.Exists(outPath))
                {
                    // Create a file to write to.
                    using (StreamWriter swOut = File.CreateText(outPath))
                    {
                        swOut.WriteLine(word);
                    }
                }
                else
                {
                    using (StreamWriter swOut = File.AppendText(outPath))
                    {
                        swOut.WriteLine("\n");
                        swOut.WriteLine(word);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Text : " + ex.InnerException);
                isAnagram = false;
                return false;
            }
            return true;
        }
    }
}