using System.Net.Sockets;
using System.Text;


namespace WebApiFingertec3._0.Entity
{
    public class SocketClient
    {
        public string getDigitalString(int digital)
        {
            return enviarRequisição("192.168.1.33", digital);
        }

        private string enviarRequisição(string ip, int envio)
        {
            int port = 13000;
            try
            {
                TcpClient tcpClient = new TcpClient(ip, port);
                byte[] bytes = Encoding.ASCII.GetBytes(envio.ToString());
                NetworkStream stream = tcpClient.GetStream();

                stream.Write(bytes, 0, bytes.Length);

                bytes = new byte[100000];
                string Template = string.Empty;
                int count = stream.Read(bytes, 0, bytes.Length);
                Template = Encoding.ASCII.GetString(bytes, 0, count);
                tcpClient.Close();

                return Newtonsoft.Json.JsonConvert.SerializeObject(new { textFir = Template });
                //return empty;
            }
            catch (ArgumentNullException ex)
            {
                return ex.ToString();
            }
        }



    }
}