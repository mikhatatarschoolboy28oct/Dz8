using System;

class Song
{
    private string name;
    private string author;
    private Song prev;

    public Song()
    {
    }

    public Song(string name, string author)
    {
        this.name = name;
        this.author = author;
        this.prev = null;
    }

    public Song(string name, string author, Song prev)
    {
        this.name = name;
        this.author = author;
        this.prev = prev;
    }

    public void SetName(string value)
    {
        name = value;
    }

    public void SetAuthor(string value)
    {
        author = value;
    }

    public void SetPrev(Song value)
    {
        prev = value;
    }

    public string Title()
    {
        return name + " - " + author;
    }

    public void Print()
    {
        Console.WriteLine("Песня: " + Title());
        if (prev != null)
        {
            Console.WriteLine("  Предыдущая: " + prev.Title());
        }
        else
        {
            Console.WriteLine("  Предыдущей песни нет");
        }
    }

    public override bool Equals(object d)
    {
        Song other = d as Song;
        if (other == null)
        {
            return false;
        }
        return name == other.name && author == other.author;
    }

    public override int GetHashCode()
    {
        return (name + author).GetHashCode();
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Song mySong = new Song();
        mySong.SetName("Группа крови");
        mySong.SetAuthor("Кино");

        Song s2 = new Song("Звезда по имени Солнце", "Кино");
        Song s3 = new Song("Знаешь ли ты", "Ария", s2);

        mySong.Print();
        s2.Print();
        s3.Print();
    }
}