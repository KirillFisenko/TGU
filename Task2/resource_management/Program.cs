using System;
using System.IO;

public class TextFile : IDisposable
{
    private FileStream fileStream;
    private StreamReader streamReader;
    private StreamWriter streamWriter;

    // Конструктор класса TextFile
    private TextFile(string path, int length)
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
        streamReader.Close();
        streamWriter.Close();
        fileStream.Close();
    }
}

internal class Program
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

            Console.WriteLine("Содержимое файла1 после записи:");
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

        using (StreamWriter sw = new StreamWriter("test.txt", true, System.Text.Encoding.Default))              
        {            
            // Изменение содержимого файла
            textFile[1] = '2';

            Console.WriteLine("Содержимое файла2 после изменения:");
            for (int i = 0; i < textFile.Length; i++)
            {
                Console.Write(textFile[i]);
            }
            Console.WriteLine();
        }
        Console.ReadLine();
    }
}