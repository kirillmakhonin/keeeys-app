namespace Keeeys.Common.Models
{
    public interface ICustomListItem
    {
        string ToLabelString();
        int GetId();
    }
}