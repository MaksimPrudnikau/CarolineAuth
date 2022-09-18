using Newtonsoft.Json;

var test = File.ReadAllLines(@"/home/maksim/MyWorld/Symbol.csv");
var index = 1;
var rows = test.Select(line =>
{
    var row = line.Trim().Split(',');
    return new Row() { Id = index++, English = row[0], Japanese = row[1] };
}).ToList();

using var sw = File.CreateText(@"/home/maksim/MyWorld/json.txt");
sw.AutoFlush = true;
sw.Write(JsonConvert.SerializeObject(rows));

public sealed class Row
{
    public int Id { get; set; }
    public string Japanese { get; set; }
    public string English { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, English: {English}, Japan: {Japanese}";
    }
}