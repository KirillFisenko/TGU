using System;
using System.IO;

class TextFile
{
    private FileStream fileStream;
    private StreamReader streamReader;
    private StreamWriter streamWriter;

    public TextFile(string path, int length)
    {
        fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        streamReader = new StreamReader(fileStream);
        streamWriter = new StreamWriter(fileStream);

        fileStream.SetLength(length);
        for (int i = 0; i < length; i++)
        {
            streamWriter.Write(' ');
        }
        streamWriter.Flush();
    }

    public char this[int index]
    {
        get
        {
            fileStream.Seek(index, SeekOrigin.Begin);
            return (char)streamReader.Read();
        }
        set
        {
            fileStream.Seek(index, SeekOrigin.Begin);
            streamWriter.Write(value);
            streamWriter.Flush();
        }
    }

    public int Length
    {
        get { return (int)fileStream.Length; }
    }

    public void Close()
    {
        streamReader.Close();
        streamWriter.Close();
        fileStream.Close();
    }

    public void Dispose()
    {
        this.Close();
    }
}

namespace resource_management
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TextFile textFile = null;
            try
            {
                textFile = new TextFile("file.txt", 15);
                textFile[0] = '[';
                textFile[1] = '0';
                textFile[2] = '1';
                textFile[3] = ']';
                textFile[4] = ' ';
                textFile[5] = 'П';
                textFile[6] = 'р';
                textFile[7] = 'и';
                textFile[8] = 'в';
                textFile[9] = 'е';
                textFile[10] = 'т';
                textFile[11] = ' ';
                textFile[12] = 'м';
                textFile[13] = 'и';
                textFile[14] = 'р';                
            }
            finally
            {
                if (textFile != null)
                {
                    textFile.Dispose();
                }
            }
            using (TextFile textFile = TextFile.Read("file.txt", 15))
            {
                textFile[1] = '2';
            }
        }
    }
}