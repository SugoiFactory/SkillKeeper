namespace SmashGGApiWrapper
{
    public class Image2
    {
        public int id { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public double ratio { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public bool isOriginal { get; set; }
        public object entity { get; set; }
        public object entityId { get; set; }
        public object uploadedBy { get; set; }
        public object createdAt { get; set; }
        public object updatedAt { get; set; }
    }
}