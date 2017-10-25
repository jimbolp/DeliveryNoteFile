using System;
using Settings = DelNoteItems.Properties.Settings;

namespace DelNoteItems
{
    public class Tour : DelNoteItems
    {
        public string PharmacyStreet { get; set; }
        public string PharmacyName { get; set; }
        public int? PharmacyCIP { get; set; }     //Post Code
        public string PharmacyCity { get; set; }
        public int? TourID { get; set; }
        public DateTime? TourDate { get; set; }

        public Tour(string line, bool isCreditNote)
        {
            if (isCreditNote)
            {
                InitializeCreditNote(line);
            }
            else
            {
                InitializeInvoice(line);
            }
        }

        private void InitializeInvoice(string line)
        {
            int intVal = 0;
            try
            {
                //PharmacyStreet
                if (line.Length >= Settings.Default.PharmacyStreetStart + Settings.Default.PharmacyStreetLength)
                {
                    PharmacyStreet = line.Substring(Settings.Default.PharmacyStreetStart, Settings.Default.PharmacyStreetLength).Trim();
                }
                else if (line.Length >= Settings.Default.PharmacyStreetStart)
                {
                    PharmacyStreet = line.Substring(Settings.Default.PharmacyStreetStart).Trim();
                }

                //PharmacyName
                if (line.Length >= Settings.Default.PharmacyNameStart + Settings.Default.PharmacyNameLength)
                {
                    PharmacyName = line.Substring(Settings.Default.PharmacyNameStart, Settings.Default.PharmacyNameLength).Trim();
                }
                else if (line.Length >= Settings.Default.PharmacyNameStart)
                {
                    PharmacyName = line.Substring(Settings.Default.PharmacyNameStart).Trim();
                }

                //PharmacyCIP
                if (line.Length >= Settings.Default.PharmacyCIPStart + Settings.Default.PharmacyCIPLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PharmacyCIPStart, Settings.Default.PharmacyCIPLength).Trim(), out intVal))
                    {
                        PharmacyCIP = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.PharmacyCIPStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.PharmacyCIPStart).Trim(), out intVal))
                    {
                        PharmacyCIP = intVal;
                    }
                }

                //PharmacyCity
                if (line.Length >= Settings.Default.PharmacyCityStart + Settings.Default.PharmacyCityLength)
                {
                    PharmacyCity = line.Substring(Settings.Default.PharmacyCityStart, Settings.Default.PharmacyCityLength).Trim();
                }
                else if (line.Length >= Settings.Default.PharmacyCityStart)
                {
                    PharmacyCity = line.Substring(Settings.Default.PharmacyCityStart).Trim();
                }

                //TourID
                if (line.Length >= Settings.Default.TourIDStart + Settings.Default.TourIDLength)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.TourIDStart, Settings.Default.TourIDLength).Trim(), out intVal))
                    {
                        TourID = intVal;
                    }
                }
                else if (line.Length >= Settings.Default.TourIDStart)
                {
                    if (Int32.TryParse(line.Substring(Settings.Default.TourIDStart).Trim(), out intVal))
                    {
                        TourID = intVal;
                    }
                }

                //TourDate
                if (line.Length >= Settings.Default.TourDateStart + Settings.Default.TourDateLength)
                {
                    TourDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.TourDateStart, Settings.Default.TourDateLength).Trim());
                }
                else if (line.Length >= Settings.Default.TourDateStart)
                {
                    TourDate = Parse.ConvertToDateTime(line.Substring(Settings.Default.TourDateStart).Trim());
                }
            }
            catch (Exception) { throw; }
        }
        private void InitializeCreditNote(string line)
        {
            //FixLine();
            InitializeInvoice(line);
        }
    }
}
