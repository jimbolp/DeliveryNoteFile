using DelNoteItems;

namespace DeliveryNoteFiles
{
    class DeliveryNoteFile
    {
        public Supplier Supplier { get; set; }
        public Header Header { get; set; }
        public Customer Customer { get; set; }
        public Position Position { get; set; }
    }
}
