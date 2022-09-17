using Newtonsoft.Json;

var test = "[]";

var deser = JsonConvert.DeserializeObject<List<Row>>(test);
Console.WriteLine(deser);

public sealed class Row
{
    public int Id { get; set; }
    public string Japan { get; set; }
    public string English { get; set; }
}