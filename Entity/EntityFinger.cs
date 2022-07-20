using NITGEN.SDK.NBioBSP;


namespace WebApiFingertec3._0.Entity
{
    public static class EntityFinger
    {

        public static NBioAPI.Export? m_Export;
        public static NBioAPI.Type.FIR_TEXTENCODE? m_textFIR;
        public static NBioAPI.Type.HFIR? m_hNewFIR;
        public static NBioAPI.Type.HFIR? NewFIR;
    


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


        public static string Identify(string Templates)
        {

            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI.IndexSearch.FP_INFO[] fpInfo;
       
            NBioAPI.Type.WINDOW_OPTION m_WinOption = new NBioAPI.Type.WINDOW_OPTION();
            m_WinOption.WindowStyle = (uint)NBioAPI.Type.WINDOW_STYLE.NO_WELCOME;

            uint ID = 1;
            m_textFIR.TextFIR = Templates.ToString();

            m_IndexSearch.AddFIR(m_textFIR, ID, out fpInfo);

            uint dataCount;
            m_IndexSearch.GetDataCount(out dataCount);

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Capture(out hCapturedFIR);

            if (ret != NBioAPI.Error.NONE)
            {
                //DisplayErrorMsg(ret);
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                m_NBioAPI.GetTextFIRFromHandle(hCapturedFIR, out m_textFIR, true);
            }

            m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);


            NBioAPI.IndexSearch.FP_INFO fpInfo2;
            NBioAPI.IndexSearch.CALLBACK_INFO_0 cbInfo0 = new NBioAPI.IndexSearch.CALLBACK_INFO_0();
            cbInfo0.CallBackFunction = new NBioAPI.IndexSearch.INDEXSEARCH_CALLBACK(myCallback);

            // Identify FIR to IndexSearch DB
            ret = m_IndexSearch.IdentifyData(hCapturedFIR, NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL, out fpInfo2, cbInfo0);
            if (ret != NBioAPI.Error.NONE)
            {
                //DisplayErrorMsg(ret);
                return fpInfo2.ID.ToString();

            }

            return "Error";
        }

        public static uint myCallback(ref NBioAPI.IndexSearch.CALLBACK_PARAM_0 cbParam0, IntPtr userParam)
        {
            return NBioAPI.IndexSearch.CALLBACK_RETURN.OK;
        }

        public static string Compare(string Digital)
        {
            uint ret;
            bool result;
            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR = new NBioAPI.Type.FIR_TEXTENCODE();
            NBioAPI.Type.FIR_PAYLOAD myPayload = new NBioAPI.Type.FIR_PAYLOAD();

            m_textFIR.TextFIR = Digital.ToString();

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            m_NBioAPI.Capture(out hCapturedFIR);

            ret = m_NBioAPI.VerifyMatch(hCapturedFIR, m_textFIR, out result, myPayload);

            if (result == true)
                return "OK";
            else
                return "";
        }

    }
}
