using System;
using System.IO;
using System.Text;

class MyFile : IDisposable
{
    private FileStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public int Length { get; }

    private MyFile(FileStream stream, int length, Encoding encoding)
    {
        this.stream = stream;
        writer = new StreamWriter(this.stream, encoding);
        reader = new StreamReader(this.stream, encoding);
        Length = length;
    }

    public static MyFile Create(string path, int length, Encoding encoding)
    {
        var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        var writer = new StreamWriter(stream, encoding);
        writer.Write(new string(' ', length));
        writer.Flush();
        return new MyFile(stream, length, encoding);
    }

    public static MyFile Read(string path, Encoding encoding)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        var reader = new StreamReader(stream, encoding);
        var length = (int)stream.Length;
        return new MyFile(stream, length, encoding);
    }

    public char this[int index]
    {
        get
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            stream.Seek(index, SeekOrigin.Begin);
            return (char)reader.Read();
        }
        set
        {
            if (index < 0 || index >= Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            stream.Seek(index, SeekOrigin.Begin);
            writer.Write(value);
            writer.Flush();
        }
    }

    public void Dispose()
    {
        writer.Dispose();
        reader.Dispose();
        stream.Dispose();
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создание нового файла
        var path = "test.txt";
        var length = 15;
        var encoding = Encoding.Default;
        var file = MyFile.Create(path, length, encoding);

        try
        {
            // Сохранение в файл строки вида «[01] Привет мир!»
            file[0] = '[';
            file[1] = '0';
            file[2] = '1';
            file[3] = ']';
            file[4] = ' ';
            file[5] = 'П';
            file[6] = 'р';
            file[7] = 'и';
            file[8] = 'в';
            file[9] = 'е';
            file[10] = 'т';
            file[11] = ' ';
            file[12] = 'м';
            file[13] = 'и';
            file[14] = 'р';

            Console.WriteLine("Содержимое файла после записи:");
            for (int i = 0; i < file.Length; i++)
            {
                Console.Write(file[i]);
            }
            Console.WriteLine();
            Console.WriteLine();

            // Открытие созданного файла и изменение в нём одного символа, чтобы в файле получилось «[02] Привет мир!»
            using (var reader = MyFile.Read(path, encoding))
            {
                reader[2] = '2';

                Console.WriteLine("Содержимое файла после изменения:");
                for (int i = 0; i < reader.Length; i++)
                {
                    Console.Write(reader[i]);
                }
                Console.WriteLine();
            }
        }
        finally
        {
            // Освобождение ресурсов
            file.Dispose();
        }
        Console.ReadLine();
    }
}