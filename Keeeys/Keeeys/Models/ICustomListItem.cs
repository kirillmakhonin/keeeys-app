namespace Keeeys.Models
{
    public interface ICustomListItem
    {
        string ToLabelString();
        int GetId();
    }
}