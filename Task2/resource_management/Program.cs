using System;
using System.IO;

public class TextFile : IDisposable
{
    public FileStream fileStream;
    public StreamReader streamReader;
    public StreamWriter streamWriter;

    // Конструктор класса TextFile
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

    // Индексатор класса TextFile
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

    // Свойство Length класса TextFile
    public int Length
    {
        get { return (int)fileStream.Length; }
    }

    // Статический метод Create класса TextFile
    public static TextFile Create(string path, int length)
    {
        return new TextFile(path, length);
    }

    // Метод Dispose класса TextFile
    public void Dispose()
    {
        streamWriter.Close();
        streamReader.Close();        
        fileStream.Close();
    }
}

public class Program
{
    static void Main(string[] args)
    {
        TextFile textFile = null;
        try
        {
            // Создание экземпляра класса TextFile
            textFile = TextFile.Create("test.txt", 15);
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

            Console.WriteLine("Содержимое файла после записи:");
            for (int i = 0; i < textFile.Length; i++)
            {
                Console.Write(textFile[i]);
            }
            Console.WriteLine();
        }
        finally
        {
            // Освобождение ресурсов
            Console.WriteLine("Освобождение ресурсов...");
            
            textFile?.Dispose();
        }

        using (TextFile sw = new TextFile("test.txt", 15))
        {
            // Изменение содержимого файла
            sw[1] = '2';

            Console.WriteLine("Содержимое файла после изменения:");
            for (int i = 0; i < sw.Length; i++)
            {
                Console.Write(sw[i]);
            }
            Console.WriteLine();
        }
        Console.ReadLine();
    }
}