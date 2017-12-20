using System;
using System.Collections;
using System.IO;
using System.Reflection;
using DelNoteItems.Properties;

namespace DelNoteItems
{
    public abstract class DelNoteItems
    {
        /*public override string ToString()
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
        }//*/

        public void WriteExceptionToLog(Exception e)
        {
            File.AppendAllText(Settings.Default.LogFilePath,
                DateTime.Now + Environment.NewLine + "Message: " + e.Message + Environment.NewLine + e.StackTrace + Environment.NewLine);
        }

        public override string ToString()
        {
            string toString = GetType().Name + ":" + Environment.NewLine;
            foreach (PropertyInfo pi in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!typeof(IEnumerable).IsAssignableFrom(pi.PropertyType) || pi.PropertyType == typeof(string))
                {
                    toString += pi.Name + " -> ";
                    try
                    {
                        toString += pi.GetValue(this).ToString();
                    }
                    catch (Exception) { }
                    toString += Environment.NewLine;
                }
                else
                {
                    if (pi.GetValue(this) != null)
                    {
                        foreach (var m in pi.GetValue(this) as IEnumerable)
                        {
                            try
                            {
                                toString += m.ToString();
                            }
                            catch (Exception) { }
                        }
                    }
                }
            }
            return toString + Environment.NewLine;
        }
    }
}