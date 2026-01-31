
using LibplctagWrapper;
using System;
using System.Threading;

namespace SoftBetaMxLogix
{
    /// <summary>
    ///  This class performs an important function.
    /// </summary>
    public class logix
    {
        private string ip;
        private int timeout;
        public enum dataTimer : int
        {
            PRE,
            ACC,
        }
        /// <summary>
        /// Tiempo de Respuesta PLC Milisegundos
        /// <list type="bullet">
        /// <seealso cref="int"/>
        /// </list>
        /// <list type="bullet">
        /// <para>Default: <strong>5000</strong></para>
        /// </list>
        /// </summary>
        public int Timeout { get => timeout; set => timeout = value; }
        /// <summary>
        ///<c>Creando conexion a PLC</c>
        ///Eg:<code>logix client  = new logix("192.168.0.100");</code>
        /// </summary>
        /// <param name="ip">direccion ip del PLC</param>
        /// <returns>Retornara Instancia <see cref="logix"/></returns>
        public logix(string ip,int timeout)
        {
            this.ip = ip;
            this.timeout = timeout;
        }
        private void wirte(string tagName, string value = "")
        {
            Tag tagSend;

            char type = (tagName.ToUpper().ToCharArray()[0]);
            using (var client = new Libplctag())
            {
                switch (type)
                {
                    case 'O':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'I':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'S':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'B':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
                        System.Threading.Thread.Sleep(1);
                        client.AddTag(tagSend, timeout);
                        while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                        {
                            Thread.Sleep(1);
                        }
                        client.SetInt16Value(tagSend, 1 * tagSend.ElementSize, short.Parse(value));
                        System.Threading.Thread.Sleep(1);
                        client.WriteTag(tagSend, Timeout);
                        break;
                    case 'T':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
                        System.Threading.Thread.Sleep(1);
                        client.AddTag(tagSend);
                        while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                        {
                            Thread.Sleep(1);
                        }
                        client.SetInt16Value(tagSend, 1 * tagSend.ElementSize, short.Parse(value));
                        System.Threading.Thread.Sleep(1);
                        client.WriteTag(tagSend, Timeout);
                        break;
                    case 'C':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'R':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'N':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;
                    case 'F':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);

                        break;

                    default:
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
                        break;
                }


            }

        }

        private string read(string tagName)
        {
            Tag tagSend;
            string datareturn = string.Empty;
            char type = (tagName.ToUpper().ToCharArray()[0]);
            using (var client = new Libplctag())
            {
                int result = -1;
                switch (type)
                {
                    case 'O':
                        return null;
                        break;
                    case 'I':
                        return null;
                        break;
                    case 'S':
                        return null;
                        break;
                    case 'B':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 2);
                        System.Threading.Thread.Sleep(1);
                        client.AddTag(tagSend);
                        while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                        {
                            Thread.Sleep(1);
                        }

                        result = client.ReadTag(tagSend, Timeout);
                        if (result != Libplctag.PLCTAG_STATUS_OK)
                        {

                            return $"$ERROR:Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n";
                        }

                        for (int i = 0; i < tagSend.ElementCount / 2; i++)
                        {
                            datareturn += client.GetInt16Value(tagSend, (i * tagSend.ElementSize));
                        }
                        return ToBinary(datareturn);


                        break;
                    case 'T':
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
                        System.Threading.Thread.Sleep(1);
                        client.AddTag(tagSend);
                        while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                        {
                            Thread.Sleep(1);
                        }

                        result = client.ReadTag(tagSend, Timeout);
                        if (result != Libplctag.PLCTAG_STATUS_OK)
                        {

                            return $"$ERROR:Unable to read the data! Got error code {result}: {client.DecodeError(result)}\n";
                        }
                        return client.GetInt16Value(tagSend, 2 * tagSend.ElementSize).ToString();
                        break;
                    case 'C':
                        return null;
                        break;
                    case 'R':
                        return null;
                        break;
                    case 'N':
                        return null;
                        break;
                    case 'F':
                        return null;
                        break;

                    default:
                        tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
                        return null;
                        break;
                }
            }
        }
        /// <summary>
        /// This method returning some data
        /// </summary>
        /// <param name="tagName">Direccion de Tag Eg:B3:0</param>
        /// <returns>Retornara Array Bool[16]</returns>
        public bool[] readBinary(string tagName)
        {
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend,timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return null;
                }
                return ToBinary(client.GetInt16Value(tagSend, (0 * tagSend.ElementSize)));
            }
        }
        /*tagName = "B3:0",values = bool[16]*/


        /// <summary>
        /// <c>WriteBinary</c> Escribe Falso o verdadero  (Tag Eg:B3:0)
        /// </summary>
        /// <param name="tagName">Tag a escribir Eg:B3:0</param>
        /// <param name="values">Parametro Array Bool[16] falso o true dependiendo </param>
        /// <returns> retorna true si se realizo correctamente</returns>

        public bool WriteBinary(string tagName, bool[] values)
        {
            string bin = string.Empty;
            Array.Reverse(values);
            foreach (bool item in values)
            {
                bin += item ? '1' : '0';
            }

            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                string res = Convert.ToInt16(bin, 2).ToString();
                client.SetInt16Value(tagSend, (0 * tagSend.ElementSize), short.Parse(Convert.ToInt16(bin, 2).ToString()));
                result = client.WriteTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }
                return true;
            }
        }
        /*tagName = "B3:0/1",values = true?false*/
        /// <summary>
        /// <c>WriteSingleBinary</c> Esribe en la direccion de Tag Eg: <code> bool result =  WriteSingleBinary("B3:0/0", True) </code>
        /// </summary>
        /// <param name="tagName">Tag a escribir Eg:B3:0/0</param>
        /// <param name="value">False O True</param>
        /// <inheritdoc cref="Boolean"/>
        /// <returns><strong><seealso cref="Boolean"/></strong></returns>
        public bool WriteSingleBinary(string tagName, bool value)
        {
            string address = tagName.Split('/')[0];
            int intByte = int.Parse(tagName.Split('/')[1]);

            //B3:0/0
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, address, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                client.SetBitValue(tagSend, intByte, value, timeout);
                result = client.WriteTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }
                return true;
            }



        }
        /*tagName = "B3:0"*/
        /// <summary>
        /// <c>WriteOneShotBinary</c> Enviara un dato <strong>True</strong> a la direccion de <paramref name="tagName"/>
        /// en 10 ms cambia a <strong>False</strong>
        /// <code> bool boolReturn = WriteOneShotBinary("B3:0/0");</code>
        /// </summary>
        /// <param name="tagName">Eg:B3:0/0</param>
        /// <returns><strong><seealso cref="Boolean"/></strong></returns>
        public bool WriteOneShotBinary(string tagName)
        {
            string address = tagName.Split('/')[0];
            int intByte = int.Parse(tagName.Split('/')[1]);
            //B3:0/0
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, address, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }

                client.SetBitValue(tagSend, intByte, true, timeout);
                result = client.WriteTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }

                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }
                bool resp = client.GetBitValue(tagSend, intByte, timeout);
                /*Tiempo de OneShot*/
                //          Thread.Sleep(100);
                client.SetBitValue(tagSend, intByte, false, timeout);
                result = client.WriteTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }
                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }

                return true;
            }

        }
        /*tagName = "N7:0"*/
        /// <summary>
        /// <c>readSingleInteger</c> Lee dato <see cref="int"/> de la direcion "N".
        /// <completionlist cref="dataTimer"/>
        /// </summary>
        /// 
        /// <param name="tagName"></param>
        /// <returns><strong><seealso cref="int"/></strong></returns>
        public int readSingleInteger(string tagName)
        {
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return 0;
                }
                return client.GetInt16Value(tagSend, (0 * tagSend.ElementSize));
            }

        }
        /*tagName = "N7:0",qty = Cantidad de datos a leer*/
        /// <summary>
        /// <c>readSingleInteger</c> Lee dato <see cref="int"/> de la direcion "N" <paramref name="tagName"/>
        /// <list type="bullet"><paramref name="tagName"/> = Direccion N
        /// </list>
        /// <list type="bullet"><paramref name="qty"/> = Cantidad(<see cref="int"/>) a leer siguientes a partir de la direccion <paramref name="tagName"/> </list>
        /// </summary>
        /// <param name="tagName">Eg:"N7:0"</param>
        /// <param name="qty">Cantidad <see cref="int"/> </param>
        /// <returns>Array <see cref="int"/>[16]</returns>
            public int[] readSingleInteger(string tagName, int qty)
        {
           
            qty = qty > 20 ? 20 : qty;
            int[] dataIntReturn = new int[qty];
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, qty);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return null;
                }
                for (int i = 0; i < tagSend.ElementCount; i++)
                {
                    dataIntReturn[i] = client.GetInt16Value(tagSend, (i * tagSend.ElementSize));
                }

                return dataIntReturn;

                //        return client.GetInt16Value(tagSend, (0 * tagSend.ElementSize));
            }

        }

        /// <summary>
        /// <c>readFloat(<see cref="string"/> tagName)</c>
        /// <code> <see cref="float"/> ft = readFloat("F8:0")</code>
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns> <see cref="float"/></returns>
        public float readFloat(string tagName)
        {
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Float32, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                result = client.ReadTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return 0;
                }
                return client.GetFloat32Value(tagSend, (0 * tagSend.ElementSize));
            }
        }

        

        /*tagName = "N7:0", value = -32767 to 32767*/
        /// <summary>
        /// <c>writeSingleInteger</c> Escribe datos <see cref="int"/> en Direccion
        /// <code>writeSingleInteger("N7:0", 1234);</code>
        /// <list type="bullet">
        /// <param name="tagName">Eg:"N7:0"</param>
        /// </list>
        /// <list type="bullet">
        /// <param name="value">-32767 to 32767</param>
        ///</list>
        /// </summary>
        /// <returns><see cref="Boolean"/></returns>
        public bool writeSingleInteger(string tagName, int value)
        {
            int result;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 2);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                client.SetInt16Value(tagSend, (0 * tagSend.ElementSize), (short)value);
                result = client.WriteTag(tagSend, timeout);
                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return false;
                }
                return true;
            }

        }
        /*tagName = "T4:0", dataTimer*/
        /// <summary>
        /// Lee datos de contadores(Timer).
        ///
        /// <see href="https:\\www.SoftbetaMx.com">SoftBetaMX.com</see>
        /// <para>
        /// Example usage:
        /// <c>readTimer("T4:0",dataTimer.ACC)</c>
        /// </para>
        /// <list type="bullet">
        /// <param name="tagName"><see cref="String"/></param>
        /// </list>
        /// <list type="bullet">
        /// <param name="data"><see cref="dataTimer"/></param>
        ///</list>
        /// </summary>
        /// <returns><see cref="int"/></returns>
        public int readTimer(string tagName, dataTimer data)
        {
            int result = 0;
            short dataResult = 0;
            Tag tagSend = new Tag(ip, CpuType.SLC, tagName, DataType.Int16, 1);
            System.Threading.Thread.Sleep(1);
            using (var client = new Libplctag())
            {
                client.AddTag(tagSend, timeout);
                while (client.GetStatus(tagSend) == Libplctag.PLCTAG_STATUS_PENDING)
                {
                    Thread.Sleep(1);
                }
                result = client.ReadTag(tagSend, timeout);

                if (result != Libplctag.PLCTAG_STATUS_OK)
                {
                    return result;
                }
                switch (data)
                {
                    case dataTimer.PRE:
                        dataResult = client.GetInt16Value(tagSend, (1 * tagSend.ElementSize));
                        break;
                    case dataTimer.ACC:
                        dataResult = client.GetInt16Value(tagSend, (2 * tagSend.ElementSize));
                        break;
                    default:
                        break;
                }
                return dataResult;
            }

        }

        #region Utilerias
        private string ToBinary(string x)
        {

            char[] buff = new char[16];

            for (int i = 15; i >= 0; i--)
            {
                int mask = 1 << i;
                buff[15 - i] = (int.Parse(x) & mask) != 0 ? '1' : '0';
            }

            Array.Reverse(buff);
            return new string(buff);
        }
        private bool[] ToBinary(int x)
        {
            bool[] buff = new bool[16];

            for (int i = 15; i >= 0; i--)
            {
                int mask = 1 << i;
                buff[15 - i] = (x & mask) != 0 ? true : false;
            }
            Array.Reverse(buff);
            return buff;
        }
        #endregion

    }

}