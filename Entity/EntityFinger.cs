using Newtonsoft.Json.Linq;
using NITGEN.SDK.NBioBSP;
using System.Data;
using WebApiFingertec3._0.Models;

namespace WebApiFingertec3._0.Entity
{
    public static class EntityFinger
    {

        public static NBioAPI.Export? m_Export;
        public static NBioAPI.Type.FIR_TEXTENCODE? m_textFIR;
        public static NBioAPI.Type.HFIR? m_hNewFIR;
        public static NBioAPI.Type.HFIR? NewFIR;
        public static uint userID;


        public static string? EnrrowCapture(int id)
        {
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();
            NBioAPI m_NBioAPI = new NBioAPI();
            m_hNewFIR = null;
            myPayload.Data = id.ToString();
            string Retorno = "Error";


            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.WINDOW_OPTION m_WinOption = new NBioAPI.Type.WINDOW_OPTION();
            m_WinOption.WindowStyle = (uint)NBioAPI.Type.WINDOW_STYLE.NO_WELCOME;

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Enroll(ref m_hNewFIR, out NewFIR, myPayload, NBioAPI.Type.TIMEOUT.DEFAULT, null, null);

            if (ret != NBioAPI.Error.NONE)
            {
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            }

            if (NewFIR != null)
            {
                m_NBioAPI.GetTextFIRFromHandle(NewFIR, out m_textFIR, true);

                if (m_textFIR.TextFIR != null)
                {
                    m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { user_id = myPayload.Data, textFir = m_textFIR.TextFIR.ToString() });
                }
            }
            return Retorno;
        }

        public static string? Capture(int id)
        {                  
            // objFPImage = (NBioBSPCOMLib.IFPImage)objNBioBSP.FPImage;
            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.Type.INIT_INFO_0 initInfo0;
            uint ret = m_NBioAPI.GetInitInfo(out initInfo0);
            if (ret == NBioAPI.Error.NONE)
            {
                initInfo0.EnrollImageQuality = Convert.ToUInt32(50);
                initInfo0.VerifyImageQuality = Convert.ToUInt32(30);
                initInfo0.DefaultTimeout = Convert.ToUInt32(10000);
                initInfo0.SecurityLevel = (int)NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL - 1;
            }

            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR;
            // Get FIR data
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            m_NBioAPI.Capture(out hCapturedFIR);

            try
            {
                if (hCapturedFIR != null)
                {
                    m_NBioAPI.GetTextFIRFromHandle(hCapturedFIR, out m_textFIR, true);

                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { user_id = id, textFir = m_textFIR.TextFIR.ToString() });
                }

                return "Error";

            }
            catch (Exception)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { Msg = "Erro no cadastro de digital!" });
            }
            
        }


        public static string VerifyMatch(string Templates)
        {
            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.FIR_TEXTENCODE templatefromDB = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.Type.HFIR hCapturedFIR;
     
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Capture(out hCapturedFIR);

            if (ret != NBioAPI.Error.NONE)
            {
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                return "Erro na Captura da Biometria";
            }

            m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            templatefromDB.TextFIR = Templates.ToString();

            if (templatefromDB.TextFIR.Length == 0)
            {
                return "nenhuma digital encontrada!";
            }
   
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();

            uint ret1 = m_NBioAPI.VerifyMatch(hCapturedFIR, templatefromDB, out bool result, myPayload);

            if (result)
            {
                return "true";
            }
            return "false"; 
        }


        public static  uint myCallback(ref NBioAPI.IndexSearch.CALLBACK_PARAM_0 cbParam0, IntPtr userParam)
        {
            Convert.ToInt32(cbParam0.ProgressPos);
            return NBioAPI.IndexSearch.CALLBACK_RETURN.OK;
        }


        public static string Identify(object template)
        {
            uint ID;

            NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.IndexSearch.FP_INFO[] fpInfo;

            uint ret = m_IndexSearch.InitEngine();
            uint securityLevel = 7;

            NBioAPI.Type.FIR_TEXTENCODE templatefromDB = new();
            NBioAPI.Type.HFIR retorno_para_exportar = new NBioAPI.Type.HFIR();
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret1 = m_NBioAPI.Capture(NBioAPI.Type.FIR_PURPOSE.VERIFY, out hCapturedFIR, 5000, retorno_para_exportar, null);

            /* Busca no Banco de Dados ID, Template */
            DataTable dataTable = new DataTable();
            dataTable = Entity.EntityFinger.ConvertToDataTable(template);

            foreach (DataRow dtrow in dataTable.Rows)
            {

                ID = uint.Parse(dtrow["id"].ToString());
                templatefromDB.TextFIR = dtrow["templates"].ToString();

                m_IndexSearch.AddFIR(templatefromDB, ID, out fpInfo);
                NBioAPI.IndexSearch.CALLBACK_INFO_0 cbInfo0 = new NBioAPI.IndexSearch.CALLBACK_INFO_0();
                cbInfo0.CallBackFunction = new NBioAPI.IndexSearch.INDEXSEARCH_CALLBACK(myCallback);
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                NBioAPI.IndexSearch.FP_INFO fpInfo2;

                uint ret3 = m_IndexSearch.IdentifyData(hCapturedFIR, securityLevel, out fpInfo2, cbInfo0);
                userID = fpInfo2.ID;

                if (fpInfo2.ID != 0)
                {
                    return userID.ToString();
                }
            }
            return "Usuário não cadastrado.";
        }
        public static DataTable ConvertToDataTable(Object template)
        {
            JArray obj = JArray.Parse(template.ToString());
         
            DataTable dt = new DataTable();
            DataColumn column;
            DataRow row;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "templates";
            dt.Columns.Add(column);


            for (int i = 0; i < obj.Count; i++)
            {
                string id = obj[i]["userId"].ToString();
                string temp = obj[i]["Templates"].ToString();

                row = dt.NewRow();
                row["id"] = id;
                row["templates"] = temp;
                dt.Rows.Add(row);
            }
            return dt;
        }
    

    }
}
