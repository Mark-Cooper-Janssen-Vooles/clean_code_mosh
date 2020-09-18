using System;
using System.Data;
using System.IO;

namespace CleanCode.LongMethods
{
    public class DataFileToCsvMapper
    {
        public System.IO.MemoryStream Map(DataTable dataTable)
        {
            MemoryStream ReturnStream = new MemoryStream();

            StreamWriter sw = new StreamWriter(ReturnStream);
            WriteColumnNames(dataTable, sw);
            WriteColumnRows(dataTable, sw);
            sw.Flush();
            sw.Close();

            return ReturnStream;
        }

        private static void WriteColumnRows(DataTable dt, StreamWriter sw)
        {
            foreach (DataRow dr in dt.Rows)
            {
                WriteRow(dt, sw, dr);
                sw.WriteLine();
            }
        }

        private static void WriteRow(DataTable dt, StreamWriter sw, DataRow dataRow)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                WriteCell(dataRow[i], sw);
                WriteSeparatorIfRequired(dt, i, sw);
            }
        }

        private static void WriteSeparatorIfRequired(DataTable dt, int i, StreamWriter sw)
        {
            if (i < dt.Columns.Count - 1)
            {
                sw.Write(",");
            }
        }

        private static void WriteCell(object dataRow, StreamWriter sw)
        {
            if (!Convert.IsDBNull(dataRow))
            {
                string str = String.Format("\"{0:c}\"", dataRow.ToString()).Replace("\r\n", " ");
                sw.Write(str);
            }
            else
            {
                sw.Write("");
            }
        }

        private static void WriteColumnNames(DataTable dt, StreamWriter sw)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < dt.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.WriteLine();
        }
    }
}
