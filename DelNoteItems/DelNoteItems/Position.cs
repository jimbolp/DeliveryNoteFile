using System;
using System.Collections;
using System.Reflection;

namespace DelNoteItems
{
    public partial class Position
    {
        //$$POS$$ Line properties        
        public int? ArticleNo { get; set; }
        public long? EAN { get; set; }
        public string ArticleName { get; set; }
        public string ArticleUnit { get; set; }
        public int? InvoicedQty { get; set; }
        public decimal? InvoicedPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? ArticleVATPercentage { get; set; }
        
        //$$POS1$$ Line properties
        public string Reference { get; set; }
        public string ArticleCertification { get; set; }
        public bool? isNZOKArticle { get; set; }
        public string NZOKArticleCode { get; set; }
        public int? OrderQty { get; set; }
        public int? DeliveryQty { get; set; }
        public int? BonusQty { get; set; }
        public decimal? PharmacyPurchasePrice { get; set; }
        public decimal? InvoicedPriceInclVATNoDiscount { get; set; }
        public decimal? InvoicedPriceExclVAT { get; set; }
        public decimal? InvoicedPriceInclVAT { get; set; }
        
        //$$POS2$$ Line properties
        public DateTime? ExpiryDate { get; set; }
        public string Batch { get; set; }
        
        //$$POS3$$ Line properties
        public string ArticleLongName { get; set; }

        //$$POS4$$ Line - still no explanation about this one!
        
        public Position(string[] lines, bool isCreditNote)
        {
            if (isCreditNote)
            {
                InitializeCreditNote(lines);
            }
            else
            {
                InitializeInvoice(lines);
            }
        }

        private void InitializeInvoice(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("$$POS$$"))
                {
                    try
                    {
                        Pos(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS1$"))
                {
                    try
                    {
                        Pos1(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS2$$"))
                {
                    try
                    {
                        Pos2(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS3$"))
                {
                    try
                    {
                        Pos3(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS4$$"))
                {
                    try
                    {
                        Pos4(line);
                    }
                    catch (NotImplementedException) { }
                }
            }
        }
        private void InitializeCreditNote(string[] lines)
        {
            foreach (string line in lines)
            {
                if (line.StartsWith("$$POS$$"))
                {
                    try
                    {
                        Pos(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS1$$"))
                {
                    string fixedLine = RemoveSymbol(line); 
                    try
                    {
                        Pos1(fixedLine);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS2$$"))
                {
                    try
                    {
                        Pos2(line);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS3$$"))
                {
                    string fixedLine = RemoveSymbol(line);
                    try
                    {
                        Pos3(fixedLine);
                    }
                    catch (NotImplementedException) { }
                }
                else if (line.StartsWith("$$POS4$$"))
                {
                    try
                    {
                        Pos4(line);
                    }
                    catch (NotImplementedException) { }
                }
            }
        }

        private string RemoveSymbol(string line)
        {
            string str = line.Substring(0, 7) + line.Substring(8);
            return str;
        }

#if DEBUG
        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!pi.GetType().IsAssignableFrom(typeof(IEnumerable)))
                {
                    toString += pi.Name + " -> ";
                    try
                    {
                        toString += pi.GetValue(this).ToString();
                    }
                    catch (Exception) { }
                    toString += Environment.NewLine;
                }
            }
            return toString;
        } 
#endif
    }
}
