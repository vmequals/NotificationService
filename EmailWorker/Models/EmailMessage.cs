namespace EmailWorker.Models;

public class EmailMessage
{
    public string FromAddress { get; set; }
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
