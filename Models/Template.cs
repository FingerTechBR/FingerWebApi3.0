﻿namespace WebApiFingertec3._0.Models
{

    public class ContactCollection
    {
        public Template[] templates { get; set; }
    }

    public class Template
    {

        public int userId { get; set; }
        public string? template { get; set; }
    }


}
